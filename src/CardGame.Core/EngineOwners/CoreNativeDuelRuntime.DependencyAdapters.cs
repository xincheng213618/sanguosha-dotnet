#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreNativeDuelCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendDuelResponseChoices(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder) => this._targetFactory().FriendDuelResponseChoices(duel, responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreNativeDuelCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public void PrepareNativePublicPileResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion) => this._targetFactory().PrepareNativePublicPileResponsePayment(provider, card, kind, conversion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime))]
internal sealed class CoreNativeDuelCoreNativeGroupResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeGroupResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> _targetFactory = targetFactory;
    public void MoveZhangbaResponseCards(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair, long resolutionId, int responseTargetSeat) => this._targetFactory().MoveZhangbaResponseCards(responder, pair, resolutionId, responseTargetSeat);
    public void FinishZhangbaResponseCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().FinishZhangbaResponseCards(pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreNativeDuelCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool HasWeijieRequiredBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required) => this._targetFactory().HasWeijieRequiredBasic(actor, required);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreNativeDuelCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default) => this._targetFactory().BuildLegalActions(actor, includeProgramActions, selectedPhysicalCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreNativeDuelCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default) => this._targetFactory().CreateResponseChoices(responseCards, requiredCardKind, incomingCard, effectiveCardKindSelector, includeBagua, includeFactionDefense, includeFactionSlash, responder);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreNativeDuelCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool IsIssuedCardUnrespondable(long frameId) => this._targetFactory().IsIssuedCardUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreNativeDuelCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public void PrepareLeCaoZhiWenlanResponse(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PrepareLeCaoZhiWenlanResponse(provider, kind, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreNativeDuelCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeDuelCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, int? requesterSeat, int opponentSeat, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.Suit? completedResponseUseSuit = default) => this._targetFactory().TryBeginCardResponsePrograms(attack, actor, provider, requesterSeat, opponentSeat, effectiveKind, costs, continuation, conversionSource, completedResponseUseSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreNativeDuelCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativeDuelCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime))]
internal sealed class CoreNativeDuelCoreResponseCompletionZhangbaAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreResponseCompletionZhangbaOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionZhangbaRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost>? CaptureZhangbaResponseCompletionCosts(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().CaptureZhangbaResponseCompletionCosts(responder, pair);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreNativeDuelCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
    public void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeDuelCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime))]
internal sealed class CoreNativeDuelCharacterSimaYiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterSimaYiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMobileGodRuntime> _targetFactory = targetFactory;
    public void MarkMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int responderSeat, global::CardGame.Core.CardKind requiredKind) => this._targetFactory().MarkMobileSimaRenjieResponse(attack, responderSeat, requiredKind);
    public void ClearMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ClearMobileSimaRenjieResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreNativeDuelCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetResponseCards(responder, requiredCardKind);
    public global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetZhangbaSlashPairs(responder);
    public global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreNativeDuelCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public int GetRequiredResponseCount(global::CardGame.Core.CharacterState skillOwner, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetRequiredResponseCount(skillOwner, sourceSeat, responderSeat, incomingCard, requiredCardKind);
    public bool CanRequestFactionSlashResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CanRequestFactionSlashResponse(owner, attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreNativeDuelCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? FreezeCompletedResponseUseSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => this._targetFactory().FreezeCompletedResponseUseSuit(owner, cards, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreNativeDuelCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public bool IsYingboUnrespondable(long resolutionId) => this._targetFactory().IsYingboUnrespondable(resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeDuelCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreNativeDuelCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    public global::CardGame.Core.CardConversionSource? GetSelectedResponseConversion(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().GetSelectedResponseConversion(provider, responseCard, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime))]
internal sealed class CoreNativeDuelCharacterLiuQiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterLiuQiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuQiOlRuntime> _targetFactory = targetFactory;
    public bool IsWenjiResponseForbidden(long useId, int responderSeat) => this._targetFactory().IsWenjiResponseForbidden(useId, responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreNativeDuelCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required) => this._targetFactory().HasRequestedDeckBasicSource(actor, required);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed class CoreNativeDuelCharacterZhangLiaoMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterZhangLiaoMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryBeginMobileZhitiDuelPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().TryBeginMobileZhitiDuelPrograms(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreNativeDuelCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreNativeDuelCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().TryBeginCardDeclaration(provider, card, kind, source, parent, targets);
    public global::CardGame.Core.CardConversionSource? PeekDeclarationConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool use, global::CardGame.Core.CardConversionSource? explicitSource = default) => this._targetFactory().PeekDeclarationConversion(owner, card, kind, use, explicitSource);
    public global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default) => this._targetFactory().DeclarationReturn(purpose, actor, action, target, recovery, policies, dying, fan, finalKind);
    public string PublicDeclarationDescription(global::CardGame.Core.Card card, string ordinaryDescription) => this._targetFactory().PublicDeclarationDescription(card, ordinaryDescription);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CoreNativeDuelCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreNativeDuelCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? acceptedConversion) => this._targetFactory().FindOwnedCardLocation(actor, card, acceptedConversion);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreNativeDuelCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetResponseParentStep(frameId, step);
    public void PushResponseWindow(long parentFrameId, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().PushResponseWindow(parentFrameId, sourceSeat, responderSeat, incomingCard, requiredCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreNativeDuelCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreNativeDuelCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => this._targetFactory().CapturePhysicalCardColor(seat, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreNativeDuelCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeDuelCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDuelCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsNearbyTargetResponseProhibited(int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().IsNearbyTargetResponseProhibited(sourceSeat, responderSeat, incomingKind, targetSeats);
}
