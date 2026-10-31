#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreObserverSubtreesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public bool MatchesSoleTargetProtectionOriginalTargets(long id, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => this._targetFactory().MatchesSoleTargetProtectionOriginalTargets(id, originalTargets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreHalfHandPhaseDebtDamageObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreHalfHandPhaseDebtDamageObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _targetFactory = targetFactory;
    public bool HalfHandPaidDamageObserverEdge(int index) => this._targetFactory().HalfHandPaidDamageObserverEdge(index);
    public bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreProgramAttackAttemptAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreProgramAttackAttemptOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackAttemptRuntime> _targetFactory = targetFactory;
    public void AssertProgramAttackState() => this._targetFactory().AssertProgramAttackState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreProgramDiscardOriginsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreProgramDiscardOriginsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().GetProgramDiscardSource(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreParticipantHandDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreParticipantHandDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreParticipantHandProgramRescueRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreParticipantHandProgramRescueRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _targetFactory = targetFactory;
    public bool IsPaidHandRepaymentProgramAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsPaidHandRepaymentProgramAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame) => this._targetFactory().IsValidProgramJudgmentContinuation(pending, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreEndingHistoricalUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreEndingHistoricalUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _targetFactory = targetFactory;
    public bool ExactHistoricalEnding(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ExactHistoricalEnding(f);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.EndingHistoricalUseSlot> OwnPlayHistory(int owner) => this._targetFactory().OwnPlayHistory(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreRecoveryReplacementDamageRidesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreRecoveryReplacementDamageRidesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryReplacementDamageRidesRuntime> _targetFactory = targetFactory;
    public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RecoveryReplacementFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RandomEquipmentFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCharacterSimaYiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCharacterSimaYiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _targetFactory = targetFactory;
    public bool MobileSimaRenjieStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().MobileSimaRenjieStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsWenhePoisonDyingChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.DyingFrame child) => this._targetFactory().IsWenhePoisonDyingChild(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreProgramChildResolutionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreProgramChildResolutionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _targetFactory = targetFactory;
    public bool ProgramChildResolutionEdge(int index) => this._targetFactory().ProgramChildResolutionEdge(index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().KemingSkillEchoFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool HappyQiaoDamageHealthStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().HappyQiaoDamageHealthStructuralEdge(parent, child);
    public bool HappyQiaoCardMaximumHealthStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().HappyQiaoCardMaximumHealthStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public void AssertPostEventProgramInvariants() => this._targetFactory().AssertPostEventProgramInvariants();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreRoundPricedPileAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreRoundPricedPileAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundPricedPileAlcoholRuntime> _targetFactory = targetFactory;
    public bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsRoundPricedPileAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreOrderedPrintedSkillLossObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreOrderedPrintedSkillLossObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> _targetFactory = targetFactory;
    public bool OrderedPrintedSkillLossStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OrderedPrintedSkillLossStructuralEdge(parent, child);
    public bool OrderedPrintedSkillLossSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OrderedPrintedSkillLossSkillsChangedEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => this._targetFactory().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool MobileZhitiProgramRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().MobileZhitiProgramRidesOn(f, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreDelayedPlacementTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreDelayedPlacementTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> _targetFactory = targetFactory;
    public bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsExactFinalizedDelayedPlacementWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreZhenyiJudgmentWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreZhenyiJudgmentWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> _targetFactory = targetFactory;
    public bool IsExactZhenyiBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child) => this._targetFactory().IsExactZhenyiBeforeResultProgramChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreOverflowTargetCancellationObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreOverflowTargetCancellationObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> _targetFactory = targetFactory;
    public bool OverflowTargetCancellationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().OverflowTargetCancellationStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void AssertProgramSkillState() => this._targetFactory().AssertProgramSkillState();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().CurrentJudgmentCandidate(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCoreDyingOwnedCardSelectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCoreDyingOwnedCardSelectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnedCardSelectionsRuntime> _targetFactory = targetFactory;
    public bool ExactDyingOwnedCardEntry(global::CardGame.Core.ProgramSkillFrame root, out global::CardGame.Core.DyingFrame dying, out global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry) => this._targetFactory().ExactDyingOwnedCardEntry(root, out dying, out entry);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreDyingSuitsAndHistoricalUseObserversCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDyingSuitsAndHistoricalUseObserversCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool SkillInvocationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().SkillInvocationStructuralEdge(parent, child);
}
