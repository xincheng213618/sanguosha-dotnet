#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNianChallengeHostModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreNianChallengeHostKernelCoreGameEngineNianChallengeHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.NianChallengeHost CreateNianChallengeHost();
}
