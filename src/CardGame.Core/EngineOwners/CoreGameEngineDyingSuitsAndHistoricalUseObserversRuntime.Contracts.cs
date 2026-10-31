#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDyingSuitsAndHistoricalUseObserversContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSoleTargetProtectionOriginalTargets(long id, global::System.Collections.Generic.IReadOnlyList<int> originalTargets);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreHalfHandPhaseDebtDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HalfHandPaidDamageObserverEdge(int index);
    bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramAttackState();
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreParticipantHandDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreParticipantHandProgramRescueRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreEndingHistoricalUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactHistoricalEnding(global::CardGame.Core.ProgramSkillFrame f);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.EndingHistoricalUseSlot> OwnPlayHistory(int owner);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCharacterSimaYiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MobileSimaRenjieStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreProgramChildResolutionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramChildResolutionEdge(int index);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HappyQiaoDamageHealthStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool HappyQiaoCardMaximumHealthStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertPostEventProgramInvariants();
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreRoundPricedPileAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OrderedPrintedSkillLossStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
    bool OrderedPrintedSkillLossSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCharacterZhangLiaoMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MobileZhitiProgramRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame parent);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreDelayedPlacementTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreZhenyiJudgmentWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactZhenyiBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool OverflowTargetCancellationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramSkillState();
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCoreDyingOwnedCardSelectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactDyingOwnedCardEntry(global::CardGame.Core.ProgramSkillFrame root, out global::CardGame.Core.DyingFrame dying, out global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry);
}

internal interface ICoreDyingSuitsAndHistoricalUseObserversCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SkillInvocationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}
