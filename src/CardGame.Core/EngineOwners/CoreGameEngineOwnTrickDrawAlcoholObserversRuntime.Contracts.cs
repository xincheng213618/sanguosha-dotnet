#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOwnTrickDrawAlcoholObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreOwnTrickDrawAlcoholObserversCoreLegacyDyingAlcoholReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactLegacyDyingAlcoholReturnRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreOwnTrickDrawAlcoholObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOwnTrickDrawAlcoholObserversCoreOwnTrickDrawTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OwnTrickDrawMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent);
    bool OwnTrickDrawFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}
