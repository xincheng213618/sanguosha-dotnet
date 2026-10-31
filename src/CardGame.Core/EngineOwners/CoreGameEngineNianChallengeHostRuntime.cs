#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNianChallengeHostRuntime
{
    private readonly ICoreNianChallengeHostModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreNianChallengeHostKernelCoreGameEngineNianChallengeHostOperations _kernelCoreGameEngineNianChallengeHostOperations;
    internal CoreGameEngineNianChallengeHostRuntime(ICoreNianChallengeHostModeRuntimeOperations modeRuntimeOperations, ICoreNianChallengeHostKernelCoreGameEngineNianChallengeHostOperations kernelCoreGameEngineNianChallengeHostOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._kernelCoreGameEngineNianChallengeHostOperations = kernelCoreGameEngineNianChallengeHostOperations;
    }
}
