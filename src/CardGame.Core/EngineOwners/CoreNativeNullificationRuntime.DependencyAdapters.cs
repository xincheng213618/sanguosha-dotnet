#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreNativeNullificationCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public bool IsYingboUnrespondable(long resolutionId) => this._targetFactory().IsYingboUnrespondable(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreNativeNullificationCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> CreateConversionChoiceVariants(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false) => this._targetFactory().CreateConversionChoiceVariants(owner, card, effectiveKind, forResponse, baseChoiceId, description, cards, targets, baseParameters, dyingUse);
    public bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasProgramCardIdentity(owner, card);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime))]
internal sealed class CoreNativeNullificationCoreNativeTrickEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeTrickEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> _targetFactory = targetFactory;
    public void ResolveDrawTwoEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveDrawTwoEffect(pending);
    public void ResolveGroupCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveGroupCardEffect(pending);
    public void ResolvePeachGardenEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolvePeachGardenEffect(pending);
    public void ResolveFiveGrainsEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveFiveGrainsEffect(pending);
    public void ResolveTargetCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveTargetCardEffect(pending);
    public void ResolveFireAttackEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveFireAttackEffect(pending);
    public void ResolveBorrowedSwordEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveBorrowedSwordEffect(pending);
    public void ResolveDuelEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveDuelEffect(pending);
    public void ResolveIronChainEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveIronChainEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreNativeNullificationCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed class CoreNativeNullificationCoreSequentialTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreSequentialTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> FreezeSequentialTrickTarget(long frameId, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.LegalActionKind kind, int? firstTargetCardId, global::CardGame.Core.CardKind? requiredCardKind) => this._targetFactory().FreezeSequentialTrickTarget(frameId, targets, kind, firstTargetCardId, requiredCardKind);
    public void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card) => this._targetFactory().MoveFinishedTrickCard(frameId, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeNullificationCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime))]
internal sealed class CoreNativeNullificationCharacterChengYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterChengYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> _targetFactory = targetFactory;
    public bool HasMouCyMatchingName(int actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HasMouCyMatchingName(actor, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeNullificationCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed class CoreNativeNullificationCharacterHuangZhongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterHuangZhongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHzPendingInvalidation(int actor) => this._targetFactory().HasAnniversaryHzPendingInvalidation(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime))]
internal sealed class CoreNativeNullificationCoreCurrentCardEnhancementsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCurrentCardEnhancementsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentCardEnhancementsRuntime> _targetFactory = targetFactory;
    public bool HasCurrentCardEnhancement(long frameId, global::CardGame.Core.CurrentCardEnhancement flag) => this._targetFactory().HasCurrentCardEnhancement(frameId, flag);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed class CoreNativeNullificationCharacterSimaYiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterSimaYiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginMobileSimaRenjieNullification(global::CardGame.Core.NullificationWindowFrame supplied, bool explicitPass) => this._targetFactory().TryBeginMobileSimaRenjieNullification(supplied, explicitPass);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreNativeNullificationCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
    public bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCardRestricted(player, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime))]
internal sealed class CoreNativeNullificationCharacterRuanYuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterRuanYuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterRuanYuOlRuntime> _targetFactory = targetFactory;
    public bool HasRuanYuCapability { get => this._targetFactory().HasRuanYuCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed class CoreNativeNullificationCoreProgramCardUseNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreProgramCardUseNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _targetFactory = targetFactory;
    public bool IsProgramUseIneffective(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsProgramUseIneffective(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime))]
internal sealed class CoreNativeNullificationCoreCommittedResponseUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCommittedResponseUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> _targetFactory = targetFactory;
    public bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().TryBeginCommittedResponseUsePrograms(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed class CoreNativeNullificationCoreShanheS5QuyiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreShanheS5QuyiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _targetFactory = targetFactory;
    public bool ShanheS5FuqiNullificationBanned(int responderSeat) => this._targetFactory().ShanheS5FuqiNullificationBanned(responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreNativeNullificationCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public bool HasHappyWeiYanPotentialCancellation(int seat) => this._targetFactory().HasHappyWeiYanPotentialCancellation(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime))]
internal sealed class CoreNativeNullificationCoreWushuangTargetCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreWushuangTargetCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTargetCardsRuntime> _targetFactory = targetFactory;
    public void WushuangClaimNullifiedTrick(global::CardGame.Core.NullificationWindowFrame window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> finishedPhysicalCards, int beforeFinishMovement) => this._targetFactory().WushuangClaimNullifiedTrick(window, finishedPhysicalCards, beforeFinishMovement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreNativeNullificationCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public void ResolveWenheCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveWenheCardEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime))]
internal sealed class CoreNativeNullificationCharacterLiuQiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterLiuQiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> _targetFactory = targetFactory;
    public bool IsWenjiNullificationResponseForbidden(global::CardGame.Core.NullificationWindowFrame window, int responderSeat) => this._targetFactory().IsWenjiNullificationResponseForbidden(window, responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreNativeNullificationCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreNativeNullificationCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false) => this._targetFactory().IsProgramMaterialUseForbidden(actorSeat, kind, cardIds, response);
    public bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false) => this._targetFactory().IsProgramNativeResponseAllowed(provider, kind, cardIds, source, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeNullificationCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPlayableCards(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime))]
internal sealed class CoreNativeNullificationCharacterLuXunWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterLuXunWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunWumiaoRuntime> _targetFactory = targetFactory;
    public int? CaptureWuLuXunResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().CaptureWuLuXunResponseRank(provider, effectiveKind, costs, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreNativeNullificationCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreNativeNullificationCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime))]
internal sealed class CoreNativeNullificationCoreGroupTargetNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreGroupTargetNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> _targetFactory = targetFactory;
    public string NullificationEffectDescription(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().NullificationEffectDescription(window);
    public void CompleteGroupTargetNullification(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().CompleteGroupTargetNullification(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreNativeNullificationCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreNativeNullificationCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> OriginalUseLightningPlacementTargets(global::CardGame.Core.NativeOwners.JizhiResolution pending) => this._targetFactory().OriginalUseLightningPlacementTargets(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreNativeNullificationCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileResponseCards(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind) => this._targetFactory().NativePublicPileResponseCards(provider, kind);
    public void PrepareNativePublicPileResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion) => this._targetFactory().PrepareNativePublicPileResponsePayment(provider, card, kind, conversion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreNativeNullificationCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public void ResolveHellReturnSoulEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveHellReturnSoulEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreNativeNullificationCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public void ResolveDragonBoatTrickEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveDragonBoatTrickEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreNativeNullificationCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().TryBeginCardDeclaration(provider, card, kind, source, parent, targets);
    public global::CardGame.Core.CardConversionSource? PeekDeclarationConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool use, global::CardGame.Core.CardConversionSource? explicitSource = default) => this._targetFactory().PeekDeclarationConversion(owner, card, kind, use, explicitSource);
    public global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default) => this._targetFactory().DeclarationReturn(purpose, actor, action, target, recovery, policies, dying, fan, finalKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreNativeNullificationCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public void ResolveQizhengXiangshengEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveQizhengXiangshengEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreNativeNullificationCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public void CompleteDelayedNullification(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.DelayedEffectContinuation delayed) => this._targetFactory().CompleteDelayedNullification(window, delayed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreNativeNullificationCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public bool TryBeginCommittedCardUse(long frameId, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.ProgramTrickContinuation? trick = default, global::CardGame.Core.ProgramSimpleCardContinuation? simple = default) => this._targetFactory().TryBeginCommittedCardUse(frameId, continuation, trick, simple);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed class CoreNativeNullificationCoreProgramFactionActionRewardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreProgramFactionActionRewardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureFactionAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreNativeNullificationCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsIssuedCardUnrespondable(long frameId) => this._targetFactory().IsIssuedCardUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreNativeNullificationCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
    public void ConsumeProgramViewAsUsage(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> sources) => this._targetFactory().ConsumeProgramViewAsUsage(sources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreNativeNullificationCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? FreezeCompletedResponseUseSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => this._targetFactory().FreezeCompletedResponseUseSuit(owner, cards, kind);
    public void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().ContinueNullificationAfterResponseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreNativeNullificationCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed class CoreNativeNullificationCharacterMaMiDiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterMaMiDiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> _targetFactory = targetFactory;
    public bool HasMaMiDiCapability { get => this._targetFactory().HasMaMiDiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreNativeNullificationCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().TryPauseRecoveryPaidCardUse(useId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreNativeNullificationCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeNullificationCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime))]
internal sealed class CoreNativeNullificationCoreShanheTrickResponseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreShanheTrickResponseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> _targetFactory = targetFactory;
    public bool ShanheTrickNullificationBanned(int responderSeat) => this._targetFactory().ShanheTrickNullificationBanned(responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed class CoreNativeNullificationCharacterLiFengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterLiFengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _targetFactory = targetFactory;
    public bool HasLiFengCapability { get => this._targetFactory().HasLiFengCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed class CoreNativeNullificationCharacterCuiLieAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterCuiLieAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiLieAnniversaryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> AnniversaryCuiLieDelayedPlacementTargets(global::CardGame.Core.NativeOwners.JizhiResolution pending, global::System.Collections.Generic.IReadOnlyList<int> original) => this._targetFactory().AnniversaryCuiLieDelayedPlacementTargets(pending, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime))]
internal sealed class CoreNativeNullificationCoreProgramCardTargetEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreProgramCardTargetEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _targetFactory = targetFactory;
    public void CompleteIneffectiveTrickTarget(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat) => this._targetFactory().CompleteIneffectiveTrickTarget(pending, targetSeat);
    public bool IsCardEffectIneffective(long resolutionId, int targetSeat) => this._targetFactory().IsCardEffectIneffective(resolutionId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeNullificationCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginFinalizedTrickPrograms(global::CardGame.Core.NativeOwners.JizhiResolution pending) => this._targetFactory().TryBeginFinalizedTrickPrograms(pending);
    public bool TryBeginProgramCardUseBeforeTargetEffects(global::CardGame.Core.NativeOwners.JizhiResolution pending) => this._targetFactory().TryBeginProgramCardUseBeforeTargetEffects(pending);
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreNativeNullificationCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next) => this._targetFactory().ReplaceNullificationWindowFrame(next);
    public global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame) => this._targetFactory().GetNullificationEffectCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime))]
internal sealed class CoreNativeNullificationCoreUnexpectedAssaultAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreUnexpectedAssaultOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> _targetFactory = targetFactory;
    public void ResolveUnexpectedAssaultEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveUnexpectedAssaultEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreNativeNullificationCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().FinishCardUse(frameId, card, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreNativeNullificationCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public void PrepareLeCaoZhiWenlanResponse(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PrepareLeCaoZhiWenlanResponse(provider, kind, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreNativeNullificationCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public void PrepareSiruiResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().PrepareSiruiResponsePayment(provider, card, kind, source);
    public bool IsSiruiOutsideResponseMaterial(long parentId, int cardId) => this._targetFactory().IsSiruiOutsideResponseMaterial(parentId, cardId);
    public bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsSiruiSource(source);
    public void BindSiruiResponseAction(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action) => this._targetFactory().BindSiruiResponseAction(parent, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime))]
internal sealed class CoreNativeNullificationCharacterZhugeGuoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterZhugeGuoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoLeRuntime> _targetFactory = targetFactory;
    public bool TryBypassLeChengyanNullification(long id, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.LegalActionKind action, int? targetCard, global::CardGame.Core.CardKind? required) => this._targetFactory().TryBypassLeChengyanNullification(id, card, targets, action, targetCard, required);
    public bool TryResolveLeChengyanNativeEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().TryResolveLeChengyanNativeEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime))]
internal sealed class CoreNativeNullificationCoreDelayedPlacementTargetProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreDelayedPlacementTargetProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginFinalizedDelayedPlacementPrograms(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.LegalActionKind actionKind) => this._targetFactory().TryBeginFinalizedDelayedPlacementPrograms(frameId, card, actionKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed class CoreNativeNullificationCharacterCuiYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterCuiYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _targetFactory = targetFactory;
    public bool IsCuiYanNativeResponseForbidden(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind required, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().IsCuiYanNativeResponseForbidden(owner, required, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeNullificationCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed class CoreNativeNullificationCoreNativeTrickUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeTrickUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId) => this._targetFactory().GetCardUsePhysicalCards(resolutionId);
    public void ResolveDelayedCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveDelayedCardEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreNativeNullificationCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated) => this._targetFactory().CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed class CoreNativeNullificationCoreWushuangShopCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreWushuangShopCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _targetFactory = targetFactory;
    public bool IsWushuangShopDestroyedNullificationPayment(long windowId, int provider, global::CardGame.Core.CardActionCost material, int afterSequence = -1) => this._targetFactory().IsWushuangShopDestroyedNullificationPayment(windowId, provider, material, afterSequence);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed class CoreNativeNullificationCoreFriendEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreFriendEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> _targetFactory = targetFactory;
    public void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action) => this._targetFactory().CaptureFriendMinimumHandUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreNativeNullificationCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public void PrepareWuZhangFeiResponseRanks(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? source, int? actorSeat = default) => this._targetFactory().PrepareWuZhangFeiResponseRanks(provider, kind, cards, source, actorSeat);
    public int? CaptureWuZhangFeiResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().CaptureWuZhangFeiResponseRank(provider, kind, costs, source);
    public void BindWuZhangFeiResponseRanks(global::CardGame.Core.CardActionContext action) => this._targetFactory().BindWuZhangFeiResponseRanks(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime))]
internal sealed class CoreNativeNullificationCoreOffTurnUsedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreOffTurnUsedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> _targetFactory = targetFactory;
    public void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureOffTurnResponseUseOrigin(owningUseId, action);
    public void CaptureOffTurnResponseUsedCards(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureOffTurnResponseUsedCards(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime))]
internal sealed class CoreNativeNullificationCharacterGuoJiaMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterGuoJiaMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource? MouGjLunshiSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cards, global::CardGame.Core.CardConversionSource? selected) => this._targetFactory().MouGjLunshiSource(owner, cards, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed class CoreNativeNullificationCoreShenwuCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreShenwuCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _targetFactory = targetFactory;
    public void ResolveShenwuSowBeansEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveShenwuSowBeansEffect(pending);
    public void ResolveShenwuTransplantWoodEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveShenwuTransplantWoodEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime))]
internal sealed class CoreNativeNullificationCoreOrderedSelectedActorDuelsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreOrderedSelectedActorDuelsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> _targetFactory = targetFactory;
    public bool IsOrderedSelectedActorDuelCannotNullify(long useId) => this._targetFactory().IsOrderedSelectedActorDuelCannotNullify(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreNativeNullificationCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqJueyanResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dying = false) => this._targetFactory().MouXqJueyanResponseChoices(actor, kind, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed class CoreNativeNullificationCoreExtendedViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreExtendedViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ExtendedViewAsResponseChoices(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => this._targetFactory().ExtendedViewAsResponseChoices(owner, kind);
    public void ResolveExtendedViewAsResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveExtendedViewAsResponse(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreNativeNullificationCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dyingUse = false) => this._targetFactory().TieredRoundZeroResponseChoices(actor, kind, dyingUse);
    public void TryIssueTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => this._targetFactory().TryIssueTieredRoundResponse(owner, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeNullificationCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().IsNearbyTargetResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreNativeNullificationCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public bool TryFinishUnrespondableCounterspell(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().TryFinishUnrespondableCounterspell(window);
    public global::CardGame.Core.CardConversionSource? FreezeUnrespondableCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds) => this._targetFactory().FreezeUnrespondableCounterspellSource(owner, physicalCardIds);
    public bool TryBeginPolicyCounterspellPayment(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource? frozenSource, global::System.Collections.Generic.IReadOnlyList<int> frozenMaterialIds) => this._targetFactory().TryBeginPolicyCounterspellPayment(window, action, frozenSource, frozenMaterialIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreNativeNullificationCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => this._targetFactory().CapturePhysicalCardColor(seat, card);
    public bool TracksActionDiscardColor { get => this._targetFactory().TracksActionDiscardColor; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreNativeNullificationCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public void ResolveGuanduTrickEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveGuanduTrickEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime))]
internal sealed class CoreNativeNullificationCoreCardEffectProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreCardEffectProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginOrdinaryCardEffectPrograms(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().TryBeginOrdinaryCardEffectPrograms(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime))]
internal sealed class CoreNativeNullificationCharacterHanDangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterHanDangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouHanDangNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, bool dyingUse = false) => this._targetFactory().IsMouHanDangNativeResponseAllowed(provider, card, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime))]
internal sealed class CoreNativeNullificationCoreHulaoCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreHulaoCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> _targetFactory = targetFactory;
    public void ResolveAllianceFeastEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveAllianceFeastEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime))]
internal sealed class CoreNativeNullificationCoreNamedAmbushInvalidationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNamedAmbushInvalidationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame CompleteNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().CompleteNamedAmbushCounterspell(pending);
    public bool HasEnabledMatchingNamedAmbush(int actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HasEnabledMatchingNamedAmbush(actor, kind);
    public global::CardGame.Core.NullificationWindowFrame PrepareNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending, int responder, int cardId) => this._targetFactory().PrepareNamedAmbushCounterspell(pending, responder, cardId);
    public void BindNamedAmbushCounterspell(long id, global::CardGame.Core.CardActionContext action) => this._targetFactory().BindNamedAmbushCounterspell(id, action);
    public bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsNamedAmbushUseIneffective(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime))]
internal sealed class CoreNativeNullificationCoreWushuangSkillsBAuditAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreWushuangSkillsBAuditOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBAuditRuntime> _targetFactory = targetFactory;
    public bool WushuangFuqiNullificationBanned(int responder) => this._targetFactory().WushuangFuqiNullificationBanned(responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed class CoreNativeNullificationCharacterFeiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterFeiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _targetFactory = targetFactory;
    public void BindFeiYiRankEffects(long id, global::CardGame.Core.ProgramTrickContinuation trick) => this._targetFactory().BindFeiYiRankEffects(id, trick);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreNativeNullificationCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public void PrepareMantianGuohaiEffectRound(long id) => this._targetFactory().PrepareMantianGuohaiEffectRound(id);
    public void ResolveMantianGuohaiEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveMantianGuohaiEffect(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime))]
internal sealed class CoreNativeNullificationCoreNativeSlashTargetDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreNativeSlashTargetDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> _targetFactory = targetFactory;
    public bool TryCompleteCanceledNativeSlashTargetDuel(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().TryCompleteCanceledNativeSlashTargetDuel(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime))]
internal sealed class CoreNativeNullificationCoreUnnullifiableOrdinaryTricksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreUnnullifiableOrdinaryTricksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnnullifiableOrdinaryTricksRuntime> _targetFactory = targetFactory;
    public bool IsOriginalOrdinaryTrickCannotNullify(long useId) => this._targetFactory().IsOriginalOrdinaryTrickCannotNullify(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreNativeNullificationCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordActualPlayPhaseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreNativeNullificationCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeNullificationCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuyouIdentityKind(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().IsWuyouIdentityKind(holder, card, kind);
    public bool IsWuyouIdentityMaterial(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsWuyouIdentityMaterial(holder, card, kind, source);
    public int? CaptureJuewuResponseRank(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => this._targetFactory().CaptureJuewuResponseRank(provider, costs);
    public void ResolveWumiaoFloodEffect(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ResolveWumiaoFloodEffect(pending);
}
