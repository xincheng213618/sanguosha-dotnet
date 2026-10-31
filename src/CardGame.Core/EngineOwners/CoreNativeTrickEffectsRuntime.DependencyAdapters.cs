#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame) => this._targetFactory().GetNullificationEffectCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeTrickUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeTrickUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId) => this._targetFactory().GetCardUsePhysicalCards(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public long? WuJingHejiProgramParent(long id) => this._targetFactory().WuJingHejiProgramParent(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool SkipDiouFinishedMovement(long id, global::CardGame.Core.Card c) => this._targetFactory().SkipDiouFinishedMovement(id, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterLuXunMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterLuXunMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> _targetFactory = targetFactory;
    public bool ResolveMouLuXunMultiIronChain(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveMouLuXunMultiIronChain(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime))]
internal sealed class CoreNativeTrickEffectsCoreExtraEffectForeignSelectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreExtraEffectForeignSelectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectForeignSelectionRuntime> _targetFactory = targetFactory;
    public bool TryBeginExtraForeignSelection(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState target) => this._targetFactory().TryBeginExtraForeignSelection(pending, target);
    public bool TryResolveExtraForeignSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot) => this._targetFactory().TryResolveExtraForeignSelection(pending, slot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeGroupAttackAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeGroupAttackOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupAttackRuntime> _targetFactory = targetFactory;
    public void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().BeginGroupAttackResponse(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool IsLeChengyanEffectUse(long id) => this._targetFactory().IsLeChengyanEffectUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get => this._targetFactory().ActiveTargetCardSelection; }
    public global::CardGame.Core.CardUseFrame? ActiveFireAttack { get => this._targetFactory().ActiveFireAttack; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreNativeTrickEffectsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default) => this._targetFactory().CaptureNativeLogicalDiscardInvocation(initiatorSeat, executorSeat, cardIds, reason, parentFrameId);
    public void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default) => this._targetFactory().CompleteNativeLogicalDiscardInvocation(draft, prepaidMaterials);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreExtraCardEffectRoundsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreExtraCardEffectRoundsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _targetFactory = targetFactory;
    public bool RestoreExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().RestoreExtraGroupDamageSource(group);
    public void CaptureExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().CaptureExtraGroupDamageSource(group);
    public bool IsExtraCardEffectRound(long id) => this._targetFactory().IsExtraCardEffectRound(id);
    public bool HasUnstartedExtraCardEffect(long id) => this._targetFactory().HasUnstartedExtraCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeTrickEffectsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreEquipmentSlotGroupProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreEquipmentSlotGroupProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _targetFactory = targetFactory;
    public bool ResolveEnteringChain(global::CardGame.Core.CharacterState player, bool requested) => this._targetFactory().ResolveEnteringChain(player, requested);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public void BeginBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool includeFactionSlash, bool keepCurrentRequest = false) => this._targetFactory().BeginBorrowedSwordSlashChoice(pending, includeFactionSlash, keepCurrentRequest);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterWuAnGuoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterWuAnGuoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> _targetFactory = targetFactory;
    public bool IsWuAnGuoLiyongDuelUse(long id) => this._targetFactory().IsWuAnGuoLiyongDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterFanJiangZhangDaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterFanJiangZhangDaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> _targetFactory = targetFactory;
    public bool IsJueshengDuelUse(long id) => this._targetFactory().IsJueshengDuelUse(id);
    public long? JueshengDuelProgramParent(long id) => this._targetFactory().JueshengDuelProgramParent(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public bool SkipMouXqJueyanFinishedMovement(long id, global::CardGame.Core.Card c) => this._targetFactory().SkipMouXqJueyanFinishedMovement(id, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeCardEffectDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeCardEffectDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _targetFactory = targetFactory;
    public bool TryBeginNativeDrawTwo(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().TryBeginNativeDrawTwo(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().FinishCardUse(frameId, card, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreNativeTrickEffectsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
    public global::CardGame.Core.Card? DrawOneToProcessing(global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawOneToProcessing(reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CoreNativeTrickEffectsCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterMengHuoMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterMengHuoMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> _targetFactory = targetFactory;
    public bool RestoreMouMengHuoDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().RestoreMouMengHuoDamageSource(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativeTrickEffectsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime))]
internal sealed class CoreNativeTrickEffectsCoreTargetedOrdinaryTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreTargetedOrdinaryTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetedOrdinaryTrickRuntime> _targetFactory = targetFactory;
    public bool HasTargetedOrdinaryTrickAdjustment { get => this._targetFactory().HasTargetedOrdinaryTrickAdjustment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeTrickEffectsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool IsQinLangXianyingUse(long id) => this._targetFactory().IsQinLangXianyingUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => this._targetFactory().IsSiruiOutsideGeneralWeaponMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeSlashTargetDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeSlashTargetDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> _targetFactory = targetFactory;
    public bool TryResolveNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().TryResolveNativeSlashTargetDuel(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreProgramCardTargetEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreProgramCardTargetEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _targetFactory = targetFactory;
    public bool IsCardEffectIneffective(long resolutionId, int targetSeat) => this._targetFactory().IsCardEffectIneffective(resolutionId, targetSeat);
    public void CompleteIneffectiveTrickTarget(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat) => this._targetFactory().CompleteIneffectiveTrickTarget(pending, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default) => this._targetFactory().RecordCharacterStateChange(targetSeat, window, parentFrameId, turnedOver);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterHaoPuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterHaoPuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> _targetFactory = targetFactory;
    public bool IsZhenyingDuelUse(long id) => this._targetFactory().IsZhenyingDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreExtraEffectMaterialClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreExtraEffectMaterialClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> _targetFactory = targetFactory;
    public void RestoreExtraEffectGroupMaterialClaims(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().RestoreExtraEffectGroupMaterialClaims(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreNativeTrickEffectsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _targetFactory = targetFactory;
    public void BeginDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, bool keepCurrentRequest = false) => this._targetFactory().BeginDuelResponse(duel, keepCurrentRequest);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreFriendEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreFriendEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> _targetFactory = targetFactory;
    public bool IsFriendRecastDuelUse(long id) => this._targetFactory().IsFriendRecastDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeTrickEffectsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterXuHuangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterXuHuangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> _targetFactory = targetFactory;
    public long? MouXuHuangDuelProgramParent(long id) => this._targetFactory().MouXuHuangDuelProgramParent(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeTrickEffectsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation Location)? FindPublicTargetCard(global::CardGame.Core.CharacterState target, int cardId) => this._targetFactory().FindPublicTargetCard(target, cardId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card) => this._targetFactory().GetJudgmentEffectiveCardKind(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterHeJinOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterHeJinOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> _targetFactory = targetFactory;
    public bool IsHeJinMouzhuDuelUse(long id) => this._targetFactory().IsHeJinMouzhuDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime))]
internal sealed class CoreNativeTrickEffectsCoreTargetCardPreselectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreTargetCardPreselectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> _targetFactory = targetFactory;
    public long TargetHandMovementSequence(int seat) => this._targetFactory().TargetHandMovementSequence(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void NotifyAiOfGroupAttack(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().NotifyAiOfGroupAttack(source, targets);
    public void NotifyAiOfFireAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => this._targetFactory().NotifyAiOfFireAttack(source, target);
    public void NotifyAiOfDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => this._targetFactory().NotifyAiOfDuel(source, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime))]
internal sealed class CoreNativeTrickEffectsCoreUnexpectedAssaultAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreUnexpectedAssaultOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> _targetFactory = targetFactory;
    public bool TryApplyUnexpectedAssaultTargetCardEffect(long resolutionId, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveCardKind, int sourceSeat, int targetSeat, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.Card targetCard, global::CardGame.Core.CardLocation targetCardFromZone) => this._targetFactory().TryApplyUnexpectedAssaultTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreTieredRoundZeroReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreTieredRoundZeroReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _targetFactory = targetFactory;
    public bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card) => this._targetFactory().SkipTieredRoundZeroFinishedMovement(id, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreWushuangTargetCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreWushuangTargetCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> _targetFactory = targetFactory;
    public void BindWushuangTargetCards(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat) => this._targetFactory().BindWushuangTargetCards(pending, targetSeat);
    public bool TryContinueWushuangTargetCards(long id, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveKind, int source, int target, global::CardGame.Core.LegalActionKind kind) => this._targetFactory().TryContinueWushuangTargetCards(id, effectCard, effectiveKind, source, target, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreSequentialTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreSequentialTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _targetFactory = targetFactory;
    public void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card) => this._targetFactory().MoveFinishedTrickCard(frameId, card);
    public bool HasRemainingSequentialTrickTargets(long frameId) => this._targetFactory().HasRemainingSequentialTrickTargets(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeTrickEffectsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreCurrentUsePhysicalClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreCurrentUsePhysicalClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentUsePhysicalClaimsRuntime> _targetFactory = targetFactory;
    public bool IsClaimedUseCardEntity(long frameId, int cardId) => this._targetFactory().IsClaimedUseCardEntity(frameId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime))]
internal sealed class CoreNativeTrickEffectsCoreProgramDrawTwoBonusAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreProgramDrawTwoBonusOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> _targetFactory = targetFactory;
    public int NativeDrawTwoCount(int sourceSeat) => this._targetFactory().NativeDrawTwoCount(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreAdvancedWeaponsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreAdvancedWeaponsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedWeaponsRuntime> _targetFactory = targetFactory;
    public bool MoveProcessingCardUnlessDestroyed(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default) => this._targetFactory().MoveProcessingCardUnlessDestroyed(card, destination, reason, beforeFact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreNativeTrickEffectsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeFireAttackAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeFireAttackOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame SetFireAttackSelection(long frameId, global::CardGame.Core.FireAttackSelectionState? selection) => this._targetFactory().SetFireAttackSelection(frameId, selection);
    public void BeginFireAttackReveal(global::CardGame.Core.CardUseFrame pending) => this._targetFactory().BeginFireAttackReveal(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool IsZeroEntityUse(long id) => this._targetFactory().IsZeroEntityUse(id);
    public bool IsMouZhijueZeroUse(long id) => this._targetFactory().IsMouZhijueZeroUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public bool IsMouLixianRemovedMaterial(long useId, int cardId) => this._targetFactory().IsMouLixianRemovedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreWushuangCombatTacticsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreWushuangCombatTacticsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangCombatTacticsRuntime> _targetFactory = targetFactory;
    public bool WushuangHasVisibleTargetHand(int source, global::CardGame.Core.CardKind kind) => this._targetFactory().WushuangHasVisibleTargetHand(source, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime))]
internal sealed class CoreNativeTrickEffectsCoreVirtualOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreVirtualOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public bool IsProgramVirtualOrdinaryTrickUse(long frameId) => this._targetFactory().IsProgramVirtualOrdinaryTrickUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreNativeTrickEffectsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterYangBiaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterYangBiaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryYangBiaoFiveGrainsUse(long id) => this._targetFactory().IsAnniversaryYangBiaoFiveGrainsUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterTianChouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterTianChouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianChouOlRuntime> _targetFactory = targetFactory;
    public bool IsTianChouShandaoUse(long id) => this._targetFactory().IsTianChouShandaoUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime))]
internal sealed class CoreNativeTrickEffectsCharacterWenChouMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCharacterWenChouMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWenChouMouOlRuntime> _targetFactory = targetFactory;
    public long? MouWenChouDuelProgramParent(long id) => this._targetFactory().MouWenChouDuelProgramParent(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime))]
internal sealed class CoreNativeTrickEffectsCoreDamageTargetRegionObtainAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreDamageTargetRegionObtainOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> _targetFactory = targetFactory;
    public int PrepareDamageTargetDuelDamage(long useId, int targetSeat) => this._targetFactory().PrepareDamageTargetDuelDamage(useId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime))]
internal sealed class CoreNativeTrickEffectsCoreNativeHarvestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreNativeHarvestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _targetFactory = targetFactory;
    public void FinishFiveGrains(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().FinishFiveGrains(group);
    public void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().BeginHarvestSelection(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreNativeTrickEffectsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeTrickEffectsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}
