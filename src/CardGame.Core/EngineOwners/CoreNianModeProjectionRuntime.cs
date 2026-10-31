#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNianModeProjectionRuntime
{
    private readonly ICoreNianModeProjectionCoreNianChallengeHostOperations _coreNianChallengeHostOperations;
    internal CoreNianModeProjectionRuntime(ICoreNianModeProjectionCoreNianChallengeHostOperations coreNianChallengeHostOperations)
    {
        this._coreNianChallengeHostOperations = coreNianChallengeHostOperations;
    }
}
