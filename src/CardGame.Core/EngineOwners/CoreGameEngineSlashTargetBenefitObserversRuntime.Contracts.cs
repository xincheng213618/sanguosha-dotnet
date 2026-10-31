#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSlashTargetBenefitObserversCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreSlashTargetBenefitObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreSlashTargetBenefitObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSlashTargetBenefitObserversCoreSlashTargetBenefitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SlashBenefitProgramParentMatches(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSlashTargetBenefitObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSlashTargetBenefitObserversCorePaidColorDamageClaimObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidColorDamageClaimObserverEdge(int index);
}

internal interface ICoreSlashTargetBenefitObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreSlashTargetBenefitObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreSlashTargetBenefitObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSlashTargetBenefitObserversCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreSlashTargetBenefitObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSlashTargetBenefitObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSlashTargetBenefitObserversCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreSlashTargetBenefitObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreSlashTargetBenefitObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreSlashTargetBenefitObserversCoreSlashTargetBenefitWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSlashTargetBenefitUse(global::CardGame.Core.ActualUseTargetIdentity identity);
    global::CardGame.Core.SlashTargetBenefitReceipt? SlashTargetBenefitCurrent(global::CardGame.Core.SlashTargetBenefitReceipt identity);
}

internal interface ICoreSlashTargetBenefitObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}
