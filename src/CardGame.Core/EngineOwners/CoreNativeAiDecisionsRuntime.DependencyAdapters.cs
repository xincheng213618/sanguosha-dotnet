#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreExtendedViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreExtendedViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _targetFactory = targetFactory;
    public void ResolveExtendedViewAsResponse(global::CardGame.Core.PromptChoice choice) => this._targetFactory().ResolveExtendedViewAsResponse(choice);
    public bool TryResolveExtendedAiResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().TryResolveExtendedAiResponse(owner, card, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public bool TryResolveRoundOnceCardlessForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance) => this._targetFactory().TryResolveRoundOnceCardlessForcedSlash(selected, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeActionDispatchAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeActionDispatchOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> _targetFactory = targetFactory;
    public void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default) => this._targetFactory().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed class CoreNativeAiDecisionsCorePublicDamagePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCorePublicDamagePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PublicDamagePrediction PredictPublicActionDamage(global::CardGame.Core.GameSnapshot view, int viewerSeat, global::CardGame.Core.LegalAction action, int targetSeat) => this._targetFactory().PredictPublicActionDamage(view, viewerSeat, action, targetSeat);
    public global::CardGame.Core.PublicDamagePrediction PredictPublicIncomingDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int viewerSeat, int? sourceSeat = default, int? targetSeat = default, int? damageAmount = default) => this._targetFactory().PredictPublicIncomingDamage(attack, viewerSeat, sourceSeat, targetSeat, damageAmount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed class CoreNativeAiDecisionsCoreActualDiscardPhaseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreActualDiscardPhaseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _targetFactory = targetFactory;
    public void ResolveSkipDiscardPolicyChoice(bool useSkill) => this._targetFactory().ResolveSkipDiscardPolicyChoice(useSkill);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreNativeAiDecisionsCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public bool HoldMouSmAiAttack(int source, int target) => this._targetFactory().HoldMouSmAiAttack(source, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreNativeAiDecisionsCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeFactionDefenseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeFactionDefenseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> _targetFactory = targetFactory;
    public void ResolveFactionDefenseCandidateResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId) => this._targetFactory().ResolveFactionDefenseCandidateResponse(pending, useDodge, useBagua, requestedCardId);
    public void BeginFactionDefenseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginFactionDefenseRequest(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public void CaptureAiCardResponseChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().CaptureAiCardResponseChoice(decision, card, effectiveKind);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetFactionSlashSlashCards(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => this._targetFactory().GetFactionSlashSlashCards(pending, provider);
    public global::CardGame.Core.CardKind GetFactionSlashEffectiveSlashKind(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card) => this._targetFactory().GetFactionSlashEffectiveSlashKind(pending, provider, card);
    public void ResolveDuelProgramMultiCardResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveDuelProgramMultiCardResponse(duel, responder, selection);
    public void ResolveGroupProgramMultiCardResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveGroupProgramMultiCardResponse(group, responder, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed class CoreNativeAiDecisionsCoreColorFireAttacksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreColorFireAttacksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _targetFactory = targetFactory;
    public bool TryResolveAiColorFireAttack(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.PendingDecision decision) => this._targetFactory().TryResolveAiColorFireAttack(pending, decision);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeFireAttackAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeFireAttackOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> _targetFactory = targetFactory;
    public void ResolveFireAttackReveal(global::CardGame.Core.CardUseFrame pending, int cardId) => this._targetFactory().ResolveFireAttackReveal(pending, cardId);
    public global::CardGame.Core.Card GetFireAttackRevealedCard(global::CardGame.Core.CardUseFrame pending) => this._targetFactory().GetFireAttackRevealedCard(pending);
    public void ResolveFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId) => this._targetFactory().ResolveFireAttackDiscard(pending, selectedCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime))]
internal sealed class CoreNativeAiDecisionsCoreMultiplayerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreMultiplayerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> _targetFactory = targetFactory;
    public bool IsHumanControlledSeat(int seat) => this._targetFactory().IsHumanControlledSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance) => this._targetFactory().TryResolveTieredRoundZeroForcedSlash(selected, advance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeGroupResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeGroupResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse) => this._targetFactory().ResolveGroupResponse(group, responder, selectedResponse);
    public void ResolveGroupZhangbaResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveGroupZhangbaResponse(group, responder, pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeAiDecisionsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreFactionRequestPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreFactionRequestPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestPaymentsRuntime> _targetFactory = targetFactory;
    public void ResolveFactionRequestCostChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().ResolveFactionRequestCostChoice(selected, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _targetFactory = targetFactory;
    public void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash) => this._targetFactory().ResolveDuelResponse(duel, responder, selectedSlash);
    public void ResolveDuelZhangbaResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveDuelZhangbaResponse(duel, responder, pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
    public bool IsAiSkipDiscardPolicyPending() => this._targetFactory().IsAiSkipDiscardPolicyPending();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind PhysicalSlashUseBaseKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().PhysicalSlashUseBaseKind(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeHarvestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeHarvestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _targetFactory = targetFactory;
    public void ResolveHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState picker, int cardId) => this._targetFactory().ResolveHarvestSelection(group, picker, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeIceSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeIceSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> _targetFactory = targetFactory;
    public void ResolveIceSwordChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveIceSwordChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreNativeAiDecisionsCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeWeaponResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeWeaponResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveCixiongDoubleSwordsChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveCixiongDoubleSwordsChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame) => this._targetFactory().GetNullificationEffectCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendDuelResponseChoices(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder) => this._targetFactory().FriendDuelResponseChoices(duel, responder);
    public void ResolveFriendDuelDiscard(global::CardGame.Core.PromptChoice supplied) => this._targetFactory().ResolveFriendDuelDiscard(supplied);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.QilinBowHandle? ActiveQilinBow { get => this._targetFactory().ActiveQilinBow; set => this._targetFactory().ActiveQilinBow = value; }
    public global::CardGame.Core.NativeOwners.IceSwordHandle? ActiveIceSword { get => this._targetFactory().ActiveIceSword; set => this._targetFactory().ActiveIceSword = value; }
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
    public global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get => this._targetFactory().ActiveCixiongDoubleSwords; set => this._targetFactory().ActiveCixiongDoubleSwords = value; }
    public global::CardGame.Core.NativeOwners.StoneAxeHandle? ActiveStoneAxe { get => this._targetFactory().ActiveStoneAxe; set => this._targetFactory().ActiveStoneAxe = value; }
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get => this._targetFactory().ActiveTargetCardSelection; }
    public global::CardGame.Core.CardUseFrame? ActiveFireAttack { get => this._targetFactory().ActiveFireAttack; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeTrickEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeTrickEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> _targetFactory = targetFactory;
    public void ResolveTargetCardSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot) => this._targetFactory().ResolveTargetCardSelection(pending, slot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CoreNativeAiDecisionsCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> _targetFactory = targetFactory;
    public void ResolveFactionSlashZhuqueFanChoice(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool convertToFireSlash) => this._targetFactory().ResolveFactionSlashZhuqueFanChoice(pending, convertToFireSlash);
    public void BeginBorrowedSwordFactionSlashRequest(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowedSword) => this._targetFactory().BeginBorrowedSwordFactionSlashRequest(borrowedSword);
    public void ResolveFactionSlashZhangbaCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveFactionSlashZhangbaCandidateResponse(pending, provider, pair);
    public void ResolveFactionSlashProgramMultiCardCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveFactionSlashProgramMultiCardCandidateResponse(pending, provider, selection);
    public void ResolveFactionSlashCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind = default) => this._targetFactory().ResolveFactionSlashCandidateResponse(pending, useSlash, requestedCardId, requestedEffectiveKind);
    public void BeginFactionSlashResponseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginFactionSlashResponseRequest(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetZhangbaSlashPairs(responder);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetResponseCards(responder, requiredCardKind);
    public bool CanUseCardAsResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().CanUseCardAsResponse(responder, card, requiredCardKind);
    public global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public void BeginProgramDodgeResponse(global::CardGame.Core.PromptChoice choice, bool advanceToHumanBoundary) => this._targetFactory().BeginProgramDodgeResponse(choice, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public bool? ResolveBaguaJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender) => this._targetFactory().ResolveBaguaJudgment(attack, defender);
    public void CompleteBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded) => this._targetFactory().CompleteBaguaResponse(attack, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreNativeAiDecisionsCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public bool BeginMouXqForcedSlash(global::CardGame.Core.PromptChoice choice) => this._targetFactory().BeginMouXqForcedSlash(choice);
    public bool TryMouXqJueyanAiResponse(global::CardGame.Core.CardKind kind, bool dying = false) => this._targetFactory().TryMouXqJueyanAiResponse(kind, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime))]
internal sealed class CoreNativeAiDecisionsCoreAssistedFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreAssistedFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetFactionRequestZhangbaPairs(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => this._targetFactory().GetFactionRequestZhangbaPairs(pending, provider);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => this._targetFactory().GetFactionRequestMultiCardSelections(pending, provider);
    public void ResolveAssistedFactionSlashChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => this._targetFactory().ResolveAssistedFactionSlashChoice(selected, advanceToHumanBoundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreNativeAiDecisionsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind FuleForcedAiSlashKind(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback) => this._targetFactory().FuleForcedAiSlashKind(decision, card, fallback);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasConditionalVirtualBaguaForAi(global::CardGame.Core.CharacterState player) => this._targetFactory().HasConditionalVirtualBaguaForAi(player);
    public bool HasBagua(global::CardGame.Core.CharacterState player) => this._targetFactory().HasBagua(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBorrowedSwordSlashCards(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget) => this._targetFactory().GetBorrowedSwordSlashCards(weaponOwner, slashTarget);
    public void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default) => this._targetFactory().ResolveBorrowedSwordSlashChoice(pending, slash, requestedEffectiveKind);
    public void ResolveBorrowedSwordZhangbaSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveBorrowedSwordZhangbaSlashChoice(pending, pair);
    public void ResolveBorrowedSwordProgramMultiCardSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveBorrowedSwordProgramMultiCardSlashChoice(pending, selection);
    public void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon) => this._targetFactory().CompleteBorrowedSwordWithoutSlash(pending, transferWeapon);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => this._targetFactory().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreProvenanceCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreProvenanceCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasProvenanceCounterspellMaterials(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> validIds) => this._targetFactory().HasProvenanceCounterspellMaterials(owner, validIds);
    public bool HasProvenanceGrainCounterspellMaterials(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> validIds) => this._targetFactory().HasProvenanceGrainCounterspellMaterials(owner, validIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeStoneAxeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeStoneAxeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> _targetFactory = targetFactory;
    public void ResolveStoneAxeChoice(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds) => this._targetFactory().ResolveStoneAxeChoice(discardedCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetNullificationCards(responder);
    public void ValidateNullificationConversion(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? selectedSource) => this._targetFactory().ValidateNullificationConversion(responder, card, selectedSource);
    public void ResolveNullificationChoice(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedCard, global::CardGame.Core.CardConversionSource? selectedConversionSource = default) => this._targetFactory().ResolveNullificationChoice(pending, responder, selectedCard, selectedConversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard) => this._targetFactory().ResolveDodgeResponse(attack, defender, responseCard);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
    public void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetResponseParentStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime))]
internal sealed class CoreNativeAiDecisionsCoreRoundOnceCardlessReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreRoundOnceCardlessReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessReturnsRuntime> _targetFactory = targetFactory;
    public bool TryResolveRoundOnceCardlessAiDodge() => this._targetFactory().TryResolveRoundOnceCardlessAiDodge();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeQilinBowAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeQilinBowOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> _targetFactory = targetFactory;
    public void ResolveQilinBowChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveQilinBowChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreNativeAiDecisionsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void CompleteCurrentPlayPhase() => this._targetFactory().CompleteCurrentPlayPhase();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreNativeAiDecisionsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsXieweiSelection(selection);
    public bool TryCompleteXieweiFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryCompleteXieweiFactionDefense(pending, provider, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreNativeAiDecisionsCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => this._targetFactory().KenshangBorrowedSlashChoices(pending);
    public bool TryBeginKenshangBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryBeginKenshangBorrowedSlash(pending, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public bool TryResolveTieredRoundZeroAiCounterspell() => this._targetFactory().TryResolveTieredRoundZeroAiCounterspell();
    public bool TryResolveTieredRoundZeroAiDodge() => this._targetFactory().TryResolveTieredRoundZeroAiDodge();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeQinglongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeQinglongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _targetFactory = targetFactory;
    public void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveQinglongCrescentBladeChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreNativeAiDecisionsCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => this._targetFactory().TousuiBorrowedSlashChoices(pending);
    public bool TryBeginTousuiBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected) => this._targetFactory().TryBeginTousuiBorrowedSlash(pending, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime))]
internal sealed class CoreNativeAiDecisionsCoreConvertedSlashResponseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreConvertedSlashResponseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> _targetFactory = targetFactory;
    public bool? ConvertedSlashSameColorResponseColor(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ConvertedSlashSameColorResponseColor(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativeAiDecisionsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeAiDecisionsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsSurvivalMode { get => this._targetFactory().IsSurvivalMode; }
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
    public bool IsTeamMode { get => this._targetFactory().IsTeamMode; }
}
