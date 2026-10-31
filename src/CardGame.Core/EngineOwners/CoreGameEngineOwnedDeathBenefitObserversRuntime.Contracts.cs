#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOwnedDeathBenefitObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreOwnedDeathBenefitObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreOwnedDeathBenefitObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreOwnedDeathBenefitObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOwnedDeathBenefitObserversCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramSkillState();
}

internal interface ICoreOwnedDeathBenefitObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreOwnedDeathBenefitObserversCoreOwnedDeathBenefitReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f);
    bool IsOriginalDyingSuspendedByOwnedDeathBenefit(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreOwnedDeathBenefitObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreOwnedDeathBenefitObserversCorePairBenefitObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PairBenefitFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreOwnedDeathBenefitObserversCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramAttackState();
}

internal interface ICoreOwnedDeathBenefitObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreOwnedDeathBenefitObserversCoreFixedRecipientBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidFixedRecipientReceipt(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreOwnedDeathBenefitObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool HalfHandPaidDamageObserverEdge(int index);
}
