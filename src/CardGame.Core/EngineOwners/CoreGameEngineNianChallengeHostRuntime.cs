#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNianChallengeHostRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateNianChallengeHostOperations _kernelCoreGameEngineNianChallengeHostOperations;
    internal CoreGameEngineNianChallengeHostRuntime(global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelCreateNianChallengeHostOperations kernelCoreGameEngineNianChallengeHostOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._kernelCoreGameEngineNianChallengeHostOperations = kernelCoreGameEngineNianChallengeHostOperations;
    }
}
