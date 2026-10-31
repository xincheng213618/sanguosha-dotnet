#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGameStartingDrawOriginRuntimePortsCharacterTangTangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TangTangGain>? CurrentTangTangGains(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreGameStartingDrawOriginRuntimePortsCoreOutsidePhaseDrawDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksNativeDrawInvocations { get; }

    bool ValidNativeDrawInvocation(global::CardGame.Core.NativeDrawInvocationProof proof);
}
