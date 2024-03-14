﻿using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using AElf.Cryptography.Core;
using AElf.Cryptography.ECDSA;
using AElf.Cryptography.ECVRF;
using AElf.Cryptography.Exceptions;
using Secp256k1Net;
using Virgil.Crypto;
using ECParameters = AElf.Cryptography.ECDSA.ECParameters;

namespace AElf.Cryptography
{

    public static class CryptoHelper
    {
        private static readonly Secp256k1 Secp256K1 = new Secp256k1();

        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();  

        private static readonly Vrf<Secp256k1Curve, Sha256HasherFactory> Vrf =
            new Vrf<Secp256k1Curve, Sha256HasherFactory>(new VrfConfig(0xfe, ECParameters.Curve));

        static CryptoHelper()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, arg) => { Secp256K1.Dispose(); };
        }

        public static ECKeyPair FromPrivateKey(byte[] privateKey)
        {
            if (privateKey == null || privateKey.Length != 32)
                throw new InvalidPrivateKeyException(
                    $"Private key has to have length of 32. Current length is {privateKey?.Length}.");

            try
            {
                Lock.EnterWriteLock();
                var secp256K1PubKey = new byte[64];

                if (!Secp256K1.PublicKeyCreate(secp256K1PubKey, privateKey))
                    throw new InvalidPrivateKeyException("Create public key failed.");
                var pubKey = new byte[Secp256k1.SERIALIZED_UNCOMPRESSED_PUBKEY_LENGTH];
                if (!Secp256K1.PublicKeySerialize(pubKey, secp256K1PubKey))
                    throw new PublicKeyOperationException("Serialize public key failed.");
                return new ECKeyPair(privateKey, pubKey);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static ECKeyPair GenerateKeyPair()
        {
            try
            {
                Lock.EnterWriteLock();
                var privateKey = new byte[32];
                var secp256K1PubKey = new byte[64];

                // Generate a private key.
                var rnd = RandomNumberGenerator.Create();
                do
                {
                    rnd.GetBytes(privateKey);
                } while (!Secp256K1.SecretKeyVerify(privateKey));

                if (!Secp256K1.PublicKeyCreate(secp256K1PubKey, privateKey))
                    throw new InvalidPrivateKeyException("Create public key failed.");
                var pubKey = new byte[Secp256k1.SERIALIZED_UNCOMPRESSED_PUBKEY_LENGTH];
                if (!Secp256K1.PublicKeySerialize(pubKey, secp256K1PubKey))
                    throw new PublicKeyOperationException("Serialize public key failed.");
                return new ECKeyPair(privateKey, pubKey);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static byte[] SignWithPrivateKey(byte[] privateKey, byte[] hash)
        {
            try
            {
                Lock.EnterWriteLock();
                var recSig = new byte[65];
                var compactSig = new byte[65];
                if (!Secp256K1.SignRecoverable(recSig, hash, privateKey))
                    throw new SignatureOperationException("Create a recoverable ECDSA signature failed.");
                if (!Secp256K1.RecoverableSignatureSerializeCompact(compactSig, out var recoverId, recSig))
                    throw new SignatureOperationException("Serialize an ECDSA signature failed.");
                compactSig[64] = (byte)recoverId; // put recover id at the last slot
                return compactSig;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static bool VerifySignature(byte[] signature, byte[] data, byte[] publicKey)
        {
            var recoverResult = RecoverPublicKey(signature, data, out var recoverPublicKey);
            return recoverResult && publicKey.BytesEqual(recoverPublicKey);
        }

        public static bool RecoverPublicKey(byte[] signature, byte[] hash, out byte[] pubkey)
        {
            pubkey = null;
            try
            {
                Lock.EnterWriteLock();
                // Recover id should be greater than or equal to 0 and less than 4
                if (signature.Length != Secp256k1.SERIALIZED_UNCOMPRESSED_PUBKEY_LENGTH || signature.Last() >= 4)
                    return false;
                var pubKey = new byte[Secp256k1.SERIALIZED_UNCOMPRESSED_PUBKEY_LENGTH];
                var recoveredPubKey = new byte[Secp256k1.PUBKEY_LENGTH];
                var recSig = new byte[65];
                if (!Secp256K1.RecoverableSignatureParseCompact(recSig, signature, signature.Last()))
                    return false;
                if (!Secp256K1.Recover(recoveredPubKey, recSig, hash))
                    return false;
                if (!Secp256K1.PublicKeySerialize(pubKey, recoveredPubKey))
                    return false;
                pubkey = pubKey;
                return true;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static byte[] EncryptMessage(byte[] senderPrivateKey, byte[] receiverPublicKey, byte[] plainText)
        {
            var crypto = new VirgilCrypto(KeyPairType.EC_SECP256K1);
            var ecdhKey = Ecdh(senderPrivateKey, receiverPublicKey);
            var newKeyPair = crypto.GenerateKeys(KeyPairType.EC_SECP256K1, ecdhKey);
            return crypto.Encrypt(plainText, newKeyPair.PublicKey);
        }

        public static byte[] DecryptMessage(byte[] senderPublicKey, byte[] receiverPrivateKey, byte[] cipherText)
        {
            var crypto = new VirgilCrypto(KeyPairType.EC_SECP256K1);
            var ecdhKey = Ecdh(receiverPrivateKey, senderPublicKey);
            var newKeyPair = crypto.GenerateKeys(KeyPairType.EC_SECP256K1, ecdhKey);
            return crypto.Decrypt(cipherText, newKeyPair.PrivateKey);
        }

        public static byte[] Ecdh(byte[] privateKey, byte[] publicKey)
        {
            try
            {
                Lock.EnterWriteLock();
                var usablePublicKey = new byte[Secp256k1.SERIALIZED_UNCOMPRESSED_PUBKEY_LENGTH];
                if (!Secp256K1.PublicKeyParse(usablePublicKey, publicKey))
                    throw new PublicKeyOperationException("Parse public key failed.");
                var ecdhKey = new byte[Secp256k1.SERIALIZED_COMPRESSED_PUBKEY_LENGTH];
                if (!Secp256K1.Ecdh(ecdhKey, usablePublicKey, privateKey))
                    throw new EcdhOperationException("Compute EC Diffie- secret failed.");
                return ecdhKey;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static byte[] ECVrfProve(ECKeyPair keyPair, byte[] alpha)
        {
            try
            {
                Lock.EnterWriteLock();
                return Vrf.Prove(keyPair, alpha);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static byte[] ECVrfVerify(byte[] publicKey, byte[] alpha, byte[] pi)
        {
            try
            {
                Lock.EnterWriteLock();
                return Vrf.Verify(publicKey, alpha, pi);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
    }
}