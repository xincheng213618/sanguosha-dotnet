#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeActionDispatchAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeActionDispatchOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> _targetFactory = targetFactory;
    public void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default) => this._targetFactory().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime))]
internal sealed class CoreHumanNativeCommandsCoreMultiplayerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreMultiplayerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> _targetFactory = targetFactory;
    public int HumanDecisionSeat { get => this._targetFactory().HumanDecisionSeat; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeStoneAxeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeStoneAxeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> _targetFactory = targetFactory;
    public void ResolveStoneAxeChoice(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds) => this._targetFactory().ResolveStoneAxeChoice(discardedCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetResponseCards(responder, requiredCardKind);
    public global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? FindZhangbaSlashPair(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds) => this._targetFactory().FindZhangbaSlashPair(responder, requestedCardIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeWeaponResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeWeaponResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWeaponResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveCixiongDoubleSwordsChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveCixiongDoubleSwordsChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public bool? ResolveBaguaJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender) => this._targetFactory().ResolveBaguaJudgment(attack, defender);
    public void CompleteBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded) => this._targetFactory().CompleteBaguaResponse(attack, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> _targetFactory = targetFactory;
    public void BeginFactionSlashResponseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginFactionSlashResponseRequest(attack);
    public void ResolveFactionSlashCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind = default) => this._targetFactory().ResolveFactionSlashCandidateResponse(pending, useSlash, requestedCardId, requestedEffectiveKind);
    public void ResolveFactionSlashZhuqueFanChoice(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool convertToFireSlash) => this._targetFactory().ResolveFactionSlashZhuqueFanChoice(pending, convertToFireSlash);
    public void ResolveFactionSlashZhangbaCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveFactionSlashZhangbaCandidateResponse(pending, provider, pair);
    public void ResolveFactionSlashProgramMultiCardCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveFactionSlashProgramMultiCardCandidateResponse(pending, provider, selection);
    public void BeginBorrowedSwordFactionSlashRequest(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowedSword) => this._targetFactory().BeginBorrowedSwordFactionSlashRequest(borrowedSword);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeFireAttackAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeFireAttackOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime> _targetFactory = targetFactory;
    public void ResolveFireAttackReveal(global::CardGame.Core.CardUseFrame pending, int cardId) => this._targetFactory().ResolveFireAttackReveal(pending, cardId);
    public void ResolveFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId) => this._targetFactory().ResolveFireAttackDiscard(pending, selectedCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void RequireHumanDecision(global::CardGame.Core.DecisionKind expected) => this._targetFactory().RequireHumanDecision(expected);
    public void EnsureStarted() => this._targetFactory().EnsureStarted();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
    public void RefreshNationalResponseDecision() => this._targetFactory().RefreshNationalResponseDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get => this._targetFactory().ActiveTargetCardSelection; }
    public global::CardGame.Core.CardUseFrame? ActiveFireAttack { get => this._targetFactory().ActiveFireAttack; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _targetFactory = targetFactory;
    public void ResolveDuelZhangbaResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveDuelZhangbaResponse(duel, responder, pair);
    public void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash) => this._targetFactory().ResolveDuelResponse(duel, responder, selectedSlash);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeIceSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeIceSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeIceSwordRuntime> _targetFactory = targetFactory;
    public void ResolveIceSwordChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveIceSwordChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public void ResolveBorrowedSwordZhangbaSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveBorrowedSwordZhangbaSlashChoice(pending, pair);
    public void ResolveBorrowedSwordProgramMultiCardSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveBorrowedSwordProgramMultiCardSlashChoice(pending, selection);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBorrowedSwordSlashCards(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget) => this._targetFactory().GetBorrowedSwordSlashCards(weaponOwner, slashTarget);
    public void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default) => this._targetFactory().ResolveBorrowedSwordSlashChoice(pending, slash, requestedEffectiveKind);
    public void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon) => this._targetFactory().CompleteBorrowedSwordWithoutSlash(pending, transferWeapon);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetNullificationCards(responder);
    public void ValidateNullificationConversion(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? selectedSource) => this._targetFactory().ValidateNullificationConversion(responder, card, selectedSource);
    public void ResolveNullificationChoice(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedCard, global::CardGame.Core.CardConversionSource? selectedConversionSource = default) => this._targetFactory().ResolveNullificationChoice(pending, responder, selectedCard, selectedConversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeWoodenOxAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeWoodenOxOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> _targetFactory = targetFactory;
    public void ResolveWoodenOx(global::CardGame.Core.CharacterState actor, int storedCardId, int targetSeat) => this._targetFactory().ResolveWoodenOx(actor, storedCardId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime))]
internal sealed class CoreHumanNativeCommandsCoreProgramAlcoholConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreProgramAlcoholConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> _targetFactory = targetFactory;
    public void ValidateAlcoholConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool forResponse) => this._targetFactory().ValidateAlcoholConversion(owner, card, source, forResponse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed class CoreHumanNativeCommandsCoreActualDiscardPhaseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreActualDiscardPhaseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _targetFactory = targetFactory;
    public void ResolveSkipDiscardPolicyChoice(bool useSkill) => this._targetFactory().ResolveSkipDiscardPolicyChoice(useSkill);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreHumanNativeCommandsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreHumanNativeCommandsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPlayableCards(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeQinglongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeQinglongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQinglongRuntime> _targetFactory = targetFactory;
    public void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveQinglongCrescentBladeChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveZhangbaSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards) => this._targetFactory().ResolveZhangbaSlash(source, target, physicalCards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card[] GetDyingPeaches(global::CardGame.Core.CharacterState responder) => this._targetFactory().GetDyingPeaches(responder);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetDyingPeachConversionSources(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card) => this._targetFactory().GetDyingPeachConversionSources(responder, card);
    public global::CardGame.Core.Card[] GetDyingAlcohols(global::CardGame.Core.CharacterState responder, int victimSeat) => this._targetFactory().GetDyingAlcohols(responder, victimSeat);
    public void ApplyDyingResponse(global::CardGame.Core.CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default) => this._targetFactory().ApplyDyingResponse(responder, usePeach, peachCardId, useAlcohol, alcoholCardId, peachConversionSource, alcoholConversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeTrickEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeTrickEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickEffectsRuntime> _targetFactory = targetFactory;
    public void ResolveTargetCardSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot) => this._targetFactory().ResolveTargetCardSelection(pending, slot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeFactionDefenseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeFactionDefenseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> _targetFactory = targetFactory;
    public void BeginFactionDefenseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().BeginFactionDefenseRequest(attack);
    public void ResolveFactionDefenseCandidateResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId) => this._targetFactory().ResolveFactionDefenseCandidateResponse(pending, useDodge, useBagua, requestedCardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreHumanNativeCommandsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
    public void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetResponseParentStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeHarvestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeHarvestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _targetFactory = targetFactory;
    public void ResolveHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState picker, int cardId) => this._targetFactory().ResolveHarvestSelection(group, picker, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeGroupResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeGroupResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveGroupZhangbaResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => this._targetFactory().ResolveGroupZhangbaResponse(group, responder, pair);
    public void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse) => this._targetFactory().ResolveGroupResponse(group, responder, selectedResponse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeQilinBowAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeQilinBowOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> _targetFactory = targetFactory;
    public void ResolveQilinBowChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveQilinBowChoice(selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreHumanNativeCommandsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void CompleteCurrentPlayPhase() => this._targetFactory().CompleteCurrentPlayPhase();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> GetDyingProgramCandidates(global::CardGame.Core.CharacterState responder, global::CardGame.Core.DyingFrame dying) => this._targetFactory().GetDyingProgramCandidates(responder, dying);
    public void BeginDyingProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.DyingFrame dying) => this._targetFactory().BeginDyingProgramBinding(candidate, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime))]
internal sealed class CoreHumanNativeCommandsCoreMatchParticipantSetupAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreMatchParticipantSetupOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> _targetFactory = targetFactory;
    public void ApplyGeneralSelection(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general) => this._targetFactory().ApplyGeneralSelection(player, general);
    public void ApplyGodFactionSelection(global::CardGame.Core.CharacterState player, string factionId) => this._targetFactory().ApplyGodFactionSelection(player, factionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreHumanNativeCommandsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsWenhePhysicalPoison(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasBagua(global::CardGame.Core.CharacterState player) => this._targetFactory().HasBagua(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard) => this._targetFactory().ResolveDodgeResponse(attack, defender, responseCard);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNationalGeneralRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNationalGeneralRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildNationalRevealActions(global::CardGame.Core.CharacterState actor) => this._targetFactory().BuildNationalRevealActions(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreHumanNativeCommandsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().HasProgramCardIdentity(owner, card);
    public global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source) => this._targetFactory().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
    public void ResolveGroupProgramMultiCardResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveGroupProgramMultiCardResponse(group, responder, selection);
    public void ResolveDuelProgramMultiCardResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ResolveDuelProgramMultiCardResponse(duel, responder, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreHumanNativeCommandsCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHumanNativeCommandsCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public void ResolveNextActualUseZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().ResolveNextActualUseZhangba(owner, targets, cards);
}
