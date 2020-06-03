using AElf.Contracts.TestKit;

namespace AElf.Contracts.Vote
{
    public class ResetBlockTimeProvider : IResetBlockTimeProvider
    {
        public bool Enabled => false;
        public int StepMilliseconds => 0;
    }
}