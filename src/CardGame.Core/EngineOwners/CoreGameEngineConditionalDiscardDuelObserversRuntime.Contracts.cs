#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreConditionalDiscardDuelObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreConditionalDiscardDuelObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreConditionalDiscardDuelObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreConditionalDiscardDuelObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreConditionalDiscardDuelObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreConditionalDiscardDuelObserversCorePaidColorDamageClaimObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidColorDamageClaimObserverEdge(int index);
}

internal interface ICoreConditionalDiscardDuelObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreConditionalDiscardDuelObserversCoreConditionalDiscardDuelReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidConditionalDuelPayments(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreConditionalDiscardDuelObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}
