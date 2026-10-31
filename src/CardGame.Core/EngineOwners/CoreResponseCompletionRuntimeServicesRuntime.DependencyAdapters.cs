#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCorePairedColorResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCorePairedColorResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> _targetFactory = targetFactory;
    public void CapturePairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().CapturePairedColorResponseLink(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().HasNullificationEffectOwner(window);
    public long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().NullificationParentActionId(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyZuoCiDestroyedResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish) => this._targetFactory().IsHappyZuoCiDestroyedResponseFinish(action, cost, finish);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterFanJiangZhangDaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterFanJiangZhangDaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFanJiangZhangDaOlRuntime> _targetFactory = targetFactory;
    public bool IsJueshengDuelUse(long id) => this._targetFactory().IsJueshengDuelUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieResponseObserver(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasChenglieResponseObserver(a, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreWushuangJinzhiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreWushuangJinzhiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _targetFactory = targetFactory;
    public bool TryGetWushuangJinzhiResponseMovements(global::CardGame.Core.CardActionContext action, int card, out global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => this._targetFactory().TryGetWushuangJinzhiResponseMovements(action, card, out movements);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool TryGetXieweiResponseCostMovement(global::CardGame.Core.CardActionContext action, int cardId, out global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().TryGetXieweiResponseCostMovement(action, cardId, out movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public bool TryBeginCompletedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().TryBeginCompletedResponseUsePrograms(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreGuanduChariotResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreGuanduChariotResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> _targetFactory = targetFactory;
    public bool HasGuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasGuanduChariotResponsePayment(action, continuation);
    public global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default) => this._targetFactory().GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default) => this._targetFactory().CollectSharedCardActionCandidates(action, window, eventTargets, cardUseCausedDamage, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame ClearTieredRoundCounterspellResponse(global::CardGame.Core.NullificationWindowFrame input) => this._targetFactory().ClearTieredRoundCounterspellResponse(input);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ContinueFinishedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueFinishedCardResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public void CaptureLeCaoZhiWenlanQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().CaptureLeCaoZhiWenlanQualification(action, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreDeferredTurnEndPreludesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreDeferredTurnEndPreludesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(global::CardGame.Core.CardMovementBatchContext batch, long? resumeProgramFrameId = default) => this._targetFactory().CreateCardsMovedProgramWindow(batch, resumeProgramFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool TryGetSiruiDestroyedResponseCost(global::CardGame.Core.CardActionContext action, int cardId, out global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().TryGetSiruiDestroyedResponseCost(action, cardId, out movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public bool IsShanheStartingResourceResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish) => this._targetFactory().IsShanheStartingResourceResponseFinish(action, cost, finish);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuZglReturnedCopyCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord? finish = default) => this._targetFactory().IsWuZglReturnedCopyCost(action, cost, finish);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public bool IsNativePublicPileOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsNativePublicPileOriginalActionCost(action, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterZhouCangBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterZhouCangBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool HasBoundaryZhouCangResponseObserver(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasBoundaryZhouCangResponseObserver(a, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ContinueNullificationWindow(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterMaMiDiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterMaMiDiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> _targetFactory = targetFactory;
    public void CaptureMaMiDiResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => this._targetFactory().CaptureMaMiDiResponseLink(w);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsExchangedCardClaim(long actionId, int cardId) => this._targetFactory().IsExchangedCardClaim(actionId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreWushuangShopCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreWushuangShopCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementRecord? DestroyedEquipmentResponsePayment(global::CardGame.Core.CardActionContext action, long windowId, global::CardGame.Core.CardActionCost material, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().DestroyedEquipmentResponsePayment(action, windowId, material, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCharacterLiFengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCharacterLiFengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _targetFactory = targetFactory;
    public bool HasJiyunResponseCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasJiyunResponseCompletion(action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementRecord? DestroyedAlternativeResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().DestroyedAlternativeResponsePayment(action, parentFrameId, cost, continuation);
    public bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId) => this._targetFactory().IsProgramAlternativeCost(action, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreResponseCompletionRuntimeServicesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionRuntimeServicesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}
