#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardConversionsEntries
{
    internal void AddNuzhanUnlimitedTrickSlashActions(ICollection<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playableCards) => _resolve().AddNuzhanUnlimitedTrickSlashActions(actions, actor, playableCards);
    internal void CaptureAiCardResponseChoice(PendingDecision decision, Card card, CardKind effectiveKind) => _resolve().CaptureAiCardResponseChoice(decision, card, effectiveKind);
    internal void CaptureSelectedResponseConversion(PromptChoice choice) => _resolve().CaptureSelectedResponseConversion(choice);
    internal IEnumerable<PromptChoice> CreateConversionChoiceVariants(CharacterState owner, Card card, CardKind effectiveKind, bool forResponse, string baseChoiceId, string description, IReadOnlyList<int> cards, IReadOnlyList<int> targets, IReadOnlyDictionary<string, string> baseParameters, bool dyingUse = false) => _resolve().CreateConversionChoiceVariants(owner, card, effectiveKind, forResponse, baseChoiceId, description, cards, targets, baseParameters, dyingUse);
    internal string DescribeConversion(CardConversionSource? source, string description) => _resolve().DescribeConversion(source, description);
    internal bool DistancePhysicalSlash230IgnoresProgramSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash230IgnoresProgramSlashDistance(q);
    internal bool DistancePhysicalSlash240IgnoresSpGuanYuWushengDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash240IgnoresSpGuanYuWushengDistance(q);
    internal bool DistancePhysicalSlash250IgnoresSlashUseDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash250IgnoresSlashUseDistance(q);
    internal IReadOnlyList<GameEngine.ProgramCardIdentityMatch> FindProgramCardIdentityMatches(CharacterState owner, Card card) => _resolve().FindProgramCardIdentityMatches(owner, card);
    internal ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(CharacterState owner, IReadOnlyList<int> cardIds, CardKind outputKind, bool forResponse, CardConversionSource source) => _resolve().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
    internal ProgramMultiCardViewAsSelection? FindProgramVariableMultiCardViewAsSelection(CharacterState owner, int[] orderedCardIds, CardKind outputKind, bool forResponse, CardConversionSource source) => _resolve().FindProgramVariableMultiCardViewAsSelection(owner, orderedCardIds, outputKind, forResponse, source);
    internal IReadOnlyList<CardConversionSource> FindProgramViewAsConversions(CharacterState owner, Card card, CardKind outputKind, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => _resolve().FindProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse, requestedUseTarget);
    internal IReadOnlyList<Card> FindSlashUseCards(CharacterState owner, bool ignoreSuitUseProhibition = false) => _resolve().FindSlashUseCards(owner, ignoreSuitUseProhibition);
    internal void FinishProgramMultiCardResponse(ProgramMultiCardViewAsSelection selection) => _resolve().FinishProgramMultiCardResponse(selection);
    internal CardKind GetFactionSlashEffectiveSlashKind(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, Card card) => _resolve().GetFactionSlashEffectiveSlashKind(pending, provider, card);
    internal IReadOnlyList<Card> GetFactionSlashSlashCards(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetFactionSlashSlashCards(pending, provider);
    internal GameEngine.NuzhanModifiers GetNuzhanModifiers(long frameId, CharacterState source) => _resolve().GetNuzhanModifiers(frameId, source);
    internal IReadOnlyList<CardConversionSource> GetProgramCardIdentitySources(CharacterState owner, Card card, CardKind effectiveKind, bool forResponse) => _resolve().GetProgramCardIdentitySources(owner, card, effectiveKind, forResponse);
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(CharacterState owner, CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => _resolve().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    internal CardConversionSource? GetSelectedResponseConversion(CharacterState provider, Card responseCard, CardKind effectiveKind) => _resolve().GetSelectedResponseConversion(provider, responseCard, effectiveKind);
    internal CardConversionSource? GetSelectedUseConversion(CharacterState actor, CharacterState provider, Card card, CardKind effectiveKind) => _resolve().GetSelectedUseConversion(actor, provider, card, effectiveKind);
    internal bool HasProgramCardIdentity(CharacterState owner, Card card) => _resolve().HasProgramCardIdentity(owner, card);
    internal bool HasUnlimitedStaticSlashDistance(CharacterState owner) => _resolve().HasUnlimitedStaticSlashDistance(owner);
    internal bool IgnoresProgramSlashDistance(CharacterState owner, CardConversionSource? source) => _resolve().IgnoresProgramSlashDistance(owner, source);
    internal bool IgnoresProgramSlashLimit(CharacterState owner, CardConversionSource? source) => _resolve().IgnoresProgramSlashLimit(owner, source);
    internal bool IgnoresSlashUseDistance(CharacterState actor) => _resolve().IgnoresSlashUseDistance(actor);
    internal bool IgnoresSpGuanYuWushengDistance(CharacterState player, Card card) => _resolve().IgnoresSpGuanYuWushengDistance(player, card);
    internal bool IsDirectProgramFireSlashConversion(CharacterState owner, Card card, CardConversionSource? source) => _resolve().IsDirectProgramFireSlashConversion(owner, card, source);
    internal CardActionContext MoveProgramMultiCardResponse(CharacterState responder, ProgramMultiCardViewAsSelection selection, long resolutionId, int responseTargetSeat, int? actorSeat = default) => _resolve().MoveProgramMultiCardResponse(responder, selection, resolutionId, responseTargetSeat, actorSeat);
    internal string ProgramConversionName(CardConversionSource source) => _resolve().ProgramConversionName(source);
    internal void ResolveDuelProgramMultiCardResponse(NativeOwners.DuelHandle duel, CharacterState responder, ProgramMultiCardViewAsSelection selection) => _resolve().ResolveDuelProgramMultiCardResponse(duel, responder, selection);
    internal void ResolveGroupProgramMultiCardResponse(NativeOwners.GroupCardHandle group, CharacterState responder, ProgramMultiCardViewAsSelection selection) => _resolve().ResolveGroupProgramMultiCardResponse(group, responder, selection);
    internal void ResolveProgramMultiCardSlash(CharacterState source, CharacterState target, ProgramMultiCardViewAsSelection selection, NativeOwners.BorrowedSwordHandle? borrowedSword = default, bool enforceOwnTurnSlashLimit = true) => _resolve().ResolveProgramMultiCardSlash(source, target, selection, borrowedSword, enforceOwnTurnSlashLimit);
    internal void SelectUseConversion(LegalAction action) => _resolve().SelectUseConversion(action);
}
