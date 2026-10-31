#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHalfHandPhaseDebtObserversCoreHalfHandTargetSupportOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandSupportPaymentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requirePaid);
}

internal interface ICoreHalfHandPhaseDebtObserversCorePaidOwnTargetObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? ActualUseTargetObserverRoot(long useId);
}

internal interface ICoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long HalfHandDebtSequence { get; }
}

internal interface ICoreHalfHandPhaseDebtObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreHalfHandPhaseDebtObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreHalfHandPhaseDebtObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHalfHandPhaseDebtObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreHalfHandPhaseDebtObserversCoreHalfHandPhaseDebtReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidHalfHandDraw(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidHalfHandGift(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidPhaseHandExchange(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidPhaseHandDebtPayment(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreHalfHandPhaseDebtObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHalfHandPhaseDebtObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}
