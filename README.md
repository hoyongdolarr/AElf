# aelf - A Decentralized Cloud Computing Blockchain Network 

[![GitHub closed issues](https://img.shields.io/github/issues-closed/aelfproject/aelf.svg)](https://app.gitkraken.com/glo/board/XKsOZJarBgAPseno)
[![lisence](https://img.shields.io/github/license/AElfProject/AElf.svg)](https://github.com/AElfProject/AElf/blob/dev/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/AElf.OS.svg)](https://www.nuget.org/packages?q=aelf)
[![MyGet (with prereleases)](https://img.shields.io/myget/aelf-project/vpre/aelf.os.svg?label=myget)](https://www.myget.org/gallery/aelf-project)
[![Twitter Follow](https://img.shields.io/twitter/follow/aelfblockchain.svg?label=%40aelfblockchain&style=social)](https://twitter.com/aelfblockchain)
[![Telegram](https://badgen.net/badge/telegram/join/blue?icon=telegram)](https://t.me/aelfdeveloper)

BRANCH | GITHUB ACTIONS BUILD                                                                                                     | TESTS                                                                                                                                                              | CODE COVERAGE                                                
------ |--------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------| ------------------------------------------------------------ 
MASTER | ![GitHub Workflow Build Status](https://github.com/AElfProject/AElf/actions/workflows/build.yml/badge.svg?branch=master) | ![GitHub Workflow Test Status](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/AElfProject/AElf/feature/badge-json/master-test-results.json) | [![codecov](https://codecov.io/gh/AElfProject/AElf/branch/master/graph/badge.svg)](https://codecov.io/gh/AElfProject/AElf) 
DEV    | ![GitHub Workflow Build Status](https://github.com/AElfProject/AElf/actions/workflows/build.yml/badge.svg?branch=dev)        | ![GitHub Workflow Test Status](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/AElfProject/AElf/feature/badge-json/dev-test-results.json)        | [![codecov](https://codecov.io/gh/AElfProject/AElf/branch/dev/graph/badge.svg)](https://codecov.io/gh/AElfProject/AElf) 


Welcome to aelf's official GitHub repo ! 

aelf is a blockchain system aiming to achieve scalability and extensibility through the use of side-chains and a flexible design. To support multiple use cases aelf makes it as easy as possible to extend/customize the system by providing easy to use tools and frameworks in order to customize the chains and write smart contracts. aelf will eventually support various languages that will let developers choose the one they are the most comfortable with.

For more information you can follow these links:
* [Official website](https://aelf.com/)
* [Documentation](https://docs.aelf.com/)
* [White Paper](https://docs.aelf.com/resources/whitepaper-2/)

This repository contains the code that runs an aelf node, you'll find below other important repositories in the aelf 
ecosystem:

TOOL/LIBRARY | description
-------------|-------------
[aelf-sdk.js](https://docs.aelf.com/tools/chain-sdk/javascript-sdk/) | Javascript development kit for interacting with an aelf node, useful for dApp developers. 
[aelf-command](https://docs.aelf.com/tools/aelf-cli/introduction/) | CLI tool for interacting with an aelf node and wallet.
[aelf-contract-templates](https://docs.aelf.com/tools/smart-contract-templates/developing-smart-contracts/) | framework for smart contract and dApp development.

## Getting Started

### This repository

This repo is where you will find the code that can use to run an aelf node. It also contains a **tests** folder that centralizes all the unit tests.

### Documentation

We strongly recommend you follow [official documentation](https://docs.aelf.com/) that will guide you through installing dependencies and running the node.

## Contributing

If you have a reasonable understanding of blockchain technology and at least some notions of C# you can of course contribute. We also appreciate other types of contributions such as documentation improvements or even correcting typos in the code if you spot any.

We expect every contributor to be respectful and constructive so that everyone has a positive experience, you can find out more in our [code of conduct](https://github.com/AElfProject/AElf/blob/dev/CODE_OF_CONDUCT.md).

### Reporting an issue

We currently only use GitHub for tracking issues, feature request and pull requests. If you're not familiar with these tools have a look at the [GitHub](https://help.github.com/en) documentation.

#### Bug report

If you think you have found a bug in our system feel free to open a GitHub issue, but first:
- check with GitHub's search engine that the bug doesn't already exist.
- in the request give as much information as possible such as: the OS, the version of aelf, how to reproduce...

#### Missing feature

We also use the GitHub issue tracker for features. If you think that some piece of functionality is missing in aelf, you can open an issue with the following in mind:
- check for similare feature requests already open.
- provide as much detail and context as possible.
- be as convincing as possible as to why we need this feature and how everybody can benefit from it.

### Pull request

For any non trivial modification of the code, the pull requests should be associated with an issue that was previously discussed. During the time you implement and are not yet ready for review, prefix the PR's title with ```[WIP]``` and also don't forget to do the following:
- add a description in the pull request saying which issue you are fixing/implementing. 
- be as explicit as possible about the changes in the description.
- add the tests corresponding to your modifications.
- pull requests should be made against the **dev** branch.

When you are ready for a review by the core team, just remove ```[WIP]``` from your PR's title and others will review. This will either lead to a discussion or to a refactor of the code. The Azure Pipelines CI system makes sure that every pull request is built for Windows, Linux, and macOS, and that unit tests are run automatically. The CI passing is a pre-condition for the PR to be merged as well as the approval from the core team.

## Versioning

We use Semantic Versioning (SemVer) for versioning, if you're intereted in closely following aelf's developement please check out the [SemVer docs](https://semver.org/).

## License

aelf is licenced under [MIT](https://github.com/AElfProject/AElf/blob/dev/LICENSE)
