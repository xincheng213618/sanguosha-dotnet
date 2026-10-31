#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDrawFundedDistinctBasicObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreDrawFundedDistinctBasicObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidDrawFundedDistinctBasicPayment(global::CardGame.Core.DrawFundedDistinctBasicPayment p);
    bool IsDrawFundedDistinctBasicHpParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child);
    bool IsDrawFundedDistinctBasicRecoveryParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryReplacementFrame child);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDrawFundedDistinctBasicObserversCharacterKongRongMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouKrLiwenPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
    bool HalfHandPaidDamageObserverEdge(int index);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDrawFundedDistinctBasicObserversCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKeBiNengPingduanPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDrawFundedDistinctBasicObserversCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuSuPaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame p);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDrawFundedDistinctBasicObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreDrawFundedDistinctBasicResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext RequireDrawFundedDistinctBasicResponseAction(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreDrawFundedDistinctBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDrawFundedDistinctBasicObserversCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreActualTurnLegacyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactLegacyActualUseCompletion(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreDrawFundedDistinctBasicObserversCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDrawFundedDistinctBasicObserversCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryMaTiePaymentFrame(global::CardGame.Core.DrawFundedDistinctBasicFrame paid);
}

internal interface ICoreDrawFundedDistinctBasicObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}
