#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreRecoveryReplacementCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool HappyQiaoAnyWounded(int seat) => this._targetFactory().HappyQiaoAnyWounded(seat);
    public int HappyQiaoRecoveryHpBefore(global::CardGame.Core.RecoveryReplacementFrame frame) => this._targetFactory().HappyQiaoRecoveryHpBefore(frame);
    public bool TryBeginHappyQiaoRecoveryRequestChoice(global::CardGame.Core.RecoveryReplacementFrame frame) => this._targetFactory().TryBeginHappyQiaoRecoveryRequestChoice(frame);
    public bool TryBeginHappyQiaoRecoveryChoice(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.PreparedRecoveryApplication application) => this._targetFactory().TryBeginHappyQiaoRecoveryChoice(frame, application);
    public bool HasHappyQiaoHealth(int seat) => this._targetFactory().HasHappyQiaoHealth(seat);
    public void CompleteHappyQiaoHealthPayment(long parentFrameId, int targetSeat, global::CardGame.Core.HpChangeKind kind, int before, int after) => this._targetFactory().CompleteHappyQiaoHealthPayment(parentFrameId, targetSeat, kind, before, after);
    public void ReleaseHappyQiaoHealthSelection(long parentFrameId) => this._targetFactory().ReleaseHappyQiaoHealthSelection(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreRecoveryReplacementCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime))]
internal sealed class CoreRecoveryReplacementCoreCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool IsCardUsePaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => this._targetFactory().IsCardUsePaymentBatch(use, batch);
    public bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => this._targetFactory().IsPaidCardUseMovementReturn(use, movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime))]
internal sealed class CoreRecoveryReplacementCoreActualTurnLegacyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreActualTurnLegacyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnLegacyCompletionRuntime> _targetFactory = targetFactory;
    public bool FinishActualTurnLegacyDyingAlcohol(long useId, long programParentId) => this._targetFactory().FinishActualTurnLegacyDyingAlcohol(useId, programParentId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime))]
internal sealed class CoreRecoveryReplacementCharacterCaoSongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterCaoSongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> _targetFactory = targetFactory;
    public bool HasRecoveryAmountSupport(int targetSeat) => this._targetFactory().HasRecoveryAmountSupport(targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed class CoreRecoveryReplacementCoreProgramCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreProgramCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool ObservesPaidCardUseChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().ObservesPaidCardUseChildren(use, continuation);
    public global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreRecoveryReplacementCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public void ContinueDragonBoatCardEffect(long id) => this._targetFactory().ContinueDragonBoatCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreRecoveryReplacementCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void ContinueTurnAfterHengye(global::CardGame.Core.CharacterState current) => this._targetFactory().ContinueTurnAfterHengye(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed class CoreRecoveryReplacementCoreDrawFundedDistinctBasicPaymentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreDrawFundedDistinctBasicPaymentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _targetFactory = targetFactory;
    public void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => this._targetFactory().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    public void ResumeDrawFundedDistinctBasicRecovery(global::CardGame.Core.RecoveryReplacementFrame completed) => this._targetFactory().ResumeDrawFundedDistinctBasicRecovery(completed);
    public bool IsDrawFundedDistinctBasicRecoveryParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryReplacementFrame child) => this._targetFactory().IsDrawFundedDistinctBasicRecoveryParent(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreRecoveryReplacementCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public void ContinuePolicyCounterspellPayment(long windowId) => this._targetFactory().ContinuePolicyCounterspellPayment(windowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreRecoveryReplacementCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime))]
internal sealed class CoreRecoveryReplacementCoreBoundDyingAlcoholReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreBoundDyingAlcoholReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundDyingAlcoholReturnsRuntime> _targetFactory = targetFactory;
    public void RecordBoundDyingAlcoholPayment(long useId, int recovered, long? recoveryAttemptId = default) => this._targetFactory().RecordBoundDyingAlcoholPayment(useId, recovered, recoveryAttemptId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreRecoveryReplacementCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreRecoveryReplacementCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public void ContinueWumiaoFlood(long id) => this._targetFactory().ContinueWumiaoFlood(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreRecoveryReplacementCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreRecoveryReplacementCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsSkillInvocationHealthChild(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => this._targetFactory().IsSkillInvocationHealthChild(f, recovery);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreRecoveryReplacementCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().HasRuntimeSkill(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime))]
internal sealed class CoreRecoveryReplacementCoreRecoveryAmountBonusesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreRecoveryAmountBonusesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> _targetFactory = targetFactory;
    public bool HasOwnRecoveryAmountSupport(int targetSeat) => this._targetFactory().HasOwnRecoveryAmountSupport(targetSeat);
    public global::CardGame.Core.RecoveryAmountBonusReceipt? PrepareOwnRecoveryAmountBonus(global::CardGame.Core.PreparedRecoveryApplication application) => this._targetFactory().PrepareOwnRecoveryAmountBonus(application);
    public void AssertRecoveryAmountBonusReceipts() => this._targetFactory().AssertRecoveryAmountBonusReceipts();
    public int SupportedRecoveryAmount(global::CardGame.Core.RecoveryReplacementFrame recovery, int baseAmount) => this._targetFactory().SupportedRecoveryAmount(recovery, baseAmount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeCardEffectDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeCardEffectDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _targetFactory = targetFactory;
    public void ContinueNativeDrawTwo(long id) => this._targetFactory().ContinueNativeDrawTwo(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativePublicPileEquipmentChildrenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativePublicPileEquipmentChildrenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileEquipmentChildrenRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.RecoveryPaidCardUseContinuation FreezeNativePublicPileEquipmentChildren(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().FreezeNativePublicPileEquipmentChildren(use, continuation);
    public bool HasNativePublicPileEquipmentTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().HasNativePublicPileEquipmentTail(use, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed class CoreRecoveryReplacementCoreColorFireAttacksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreColorFireAttacksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _targetFactory = targetFactory;
    public void ContinueColorFireAttackPayment(long useId) => this._targetFactory().ContinueColorFireAttackPayment(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreRecoveryReplacementCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().BeginSimpleCardUse(frameId, continuation);
    public bool TryMarkProgramUseCommitted(long frameId) => this._targetFactory().TryMarkProgramUseCommitted(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreRecoveryReplacementCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void AdvanceRuntimeFrame(long frameId) => this._targetFactory().AdvanceRuntimeFrame(frameId);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReturnRuntimeProgramMovement(long frameId) => this._targetFactory().ReturnRuntimeProgramMovement(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreRecoveryReplacementCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeRecoveryFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeRecoveryFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryFramesRuntime> _targetFactory = targetFactory;
    public long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount) => this._targetFactory().BeginRecovery(parentFrameId, sourceSeat, targetSeat, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().FinishCardUse(frameId, card, playedCardKind);
    public void PopFinishedCardUse(long frameId) => this._targetFactory().PopFinishedCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreRecoveryReplacementCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool ReturnCardSupplyCompletionRecovery(global::CardGame.Core.RecoveryReplacementFrame child) => this._targetFactory().ReturnCardSupplyCompletionRecovery(child);
    public bool IsCardSupplyCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().IsCardSupplyCompletionHealthChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreRecoveryReplacementCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime))]
internal sealed class CoreRecoveryReplacementCoreResponseCompletionHealthAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreResponseCompletionHealthOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> _targetFactory = targetFactory;
    public bool ReturnCardResponseCompletionRecovery(global::CardGame.Core.RecoveryReplacementFrame child) => this._targetFactory().ReturnCardResponseCompletionRecovery(child);
    public bool IsResponseCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().IsResponseCompletionHealthChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeGroupRecoveryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeGroupRecoveryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> _targetFactory = targetFactory;
    public void CompleteGroupRecoveryTarget() => this._targetFactory().CompleteGroupRecoveryTarget();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed class CoreRecoveryReplacementCoreHuxinMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreHuxinMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> _targetFactory = targetFactory;
    public void ContinueHuxinMirrorPlacement(long id) => this._targetFactory().ContinueHuxinMirrorPlacement(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreRecoveryReplacementCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreRecoveryReplacementCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreRecoveryReplacementCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreRecoveryReplacementCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreRecoveryReplacementCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public void ContinueWenheCardEffect(long id) => this._targetFactory().ContinueWenheCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed class CoreRecoveryReplacementCoreVirtualBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreVirtualBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _targetFactory = targetFactory;
    public void FinishVirtualBasicUse(long id) => this._targetFactory().FinishVirtualBasicUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreRecoveryReplacementCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime))]
internal sealed class CoreRecoveryReplacementCoreProgramRecoveryReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreProgramRecoveryReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> _targetFactory = targetFactory;
    public void RecordProgramRecoveryRequest(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryAttempt attempt) => this._targetFactory().RecordProgramRecoveryRequest(parent, attempt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed class CoreRecoveryReplacementCoreSoleIncomingRangeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreSoleIncomingRangeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _targetFactory = targetFactory;
    public void ContinueSoleIncomingRangeDraw(long id) => this._targetFactory().ContinueSoleIncomingRangeDraw(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreRecoveryReplacementCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreRecoveryReplacementCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public void ContinueFriendDuelResponse(long id) => this._targetFactory().ContinueFriendDuelResponse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreRecoveryReplacementCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public void ContinueGuanduCardEffect(long id) => this._targetFactory().ContinueGuanduCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreRecoveryReplacementCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanBoundaryMobileRuntime))]
internal sealed class CoreRecoveryReplacementCharacterSunQuanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterSunQuanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> BoundarySunQuanMobileRecoveryCandidates(long parentFrameId, int sourceSeat, int targetSeat, global::CardGame.Core.RecoveryAttemptCompletion completion, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> ordinary) => this._targetFactory().BoundarySunQuanMobileRecoveryCandidates(parentFrameId, sourceSeat, targetSeat, completion, ordinary);
    public bool IsBoundarySunQuanMobileRecoveryCandidateCurrent(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.RecoveryReplacementCandidate candidate) => this._targetFactory().IsBoundarySunQuanMobileRecoveryCandidateCurrent(frame, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreRecoveryReplacementCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime))]
internal sealed class CoreRecoveryReplacementCoreMouLuMengAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreMouLuMengOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> _targetFactory = targetFactory;
    public bool UsesFormalMouLuMeng { get => this._targetFactory().UsesFormalMouLuMeng; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteSuccessfulDodgeResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreRecoveryReplacementCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public int GetHengyeGrowth(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHengyeGrowth(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed class CoreRecoveryReplacementCoreFactionRequestCostRecoveriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreFactionRequestCostRecoveriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _targetFactory = targetFactory;
    public void ContinuePaidFactionRequestCost(long producerId) => this._targetFactory().ContinuePaidFactionRequestCost(producerId);
    public void AssertPaidFactionRequestCostRecoveries() => this._targetFactory().AssertPaidFactionRequestCostRecoveries();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreRecoveryReplacementCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().KemingSkillEchoFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime))]
internal sealed class CoreRecoveryReplacementCoreEquipmentRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreEquipmentRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> _targetFactory = targetFactory;
    public void ContinueEquipmentRecast(long frameId) => this._targetFactory().ContinueEquipmentRecast(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreRecoveryReplacementCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreRecoveryReplacementCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreRecoveryReplacementCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRecoveryReplacementCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginSlashTargetResolution(attack);
}
