#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedFactionSlashEntries
{
    internal ProgramSlashRequestAcceptance AcceptAssistedFactionSlashRequest(NativeOwners.FactionCardRequestHandle request, ProgramSkillFrame frame) => _resolve().AcceptAssistedFactionSlashRequest(request, frame);
    internal void BeginAssistedProgramFactionSlashRequest(ProgramSkillFrame frame, int actorSeat, int targetSeat, string resultBind, bool ignoreDistance = false, int? rangeOwnerSeat = default) => _resolve().BeginAssistedProgramFactionSlashRequest(frame, actorSeat, targetSeat, resultBind, ignoreDistance, rangeOwnerSeat);
    internal bool CanRequestAssistedProgramFactionSlash(int actorSeat, int targetSeat, bool ignoreDistance = false, int? rangeOwnerSeat = default) => _resolve().CanRequestAssistedProgramFactionSlash(actorSeat, targetSeat, ignoreDistance, rangeOwnerSeat);
    internal bool CanSupplyAssistedFactionSlash(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) => _resolve().CanSupplyAssistedFactionSlash(pending, kind, cards, isTrueZhangba);
    internal IReadOnlyList<PromptChoice> CreateAssistedFactionSlashChoices(CharacterState provider, NativeOwners.FactionCardRequestHandle pending) => _resolve().CreateAssistedFactionSlashChoices(provider, pending);
    internal IReadOnlyList<GameEngine.AssistedFactionSlashCard> GetAssistedFactionSlashCards(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetAssistedFactionSlashCards(pending, provider);
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetFactionRequestMultiCardSelections(pending, provider);
    internal IReadOnlyList<IReadOnlyList<Card>> GetFactionRequestZhangbaPairs(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetFactionRequestZhangbaPairs(pending, provider);
    internal bool IsAssistedProvidedSlashTarget(NativeOwners.FactionCardRequestHandle request, CardKind kind, int? effectiveRank, IReadOnlyList<int> physicalCardIds, Suit? physicalSuit, bool? effectiveColor) => _resolve().IsAssistedProvidedSlashTarget(request, kind, effectiveRank, physicalCardIds, physicalSuit, effectiveColor);
    internal bool IsAssistedProvidedSlashTarget(int actorSeat, int targetSeat, CardKind kind, int? effectiveRank = default, bool allowPotentialRank = true, IReadOnlyList<int>? physicalCardIds = default, Suit? physicalSuit = default, bool? effectiveColor = default, bool ignoreDistance = false, int? rangeOwnerSeat = default) => _resolve().IsAssistedProvidedSlashTarget(actorSeat, targetSeat, kind, effectiveRank, allowPotentialRank, physicalCardIds, physicalSuit, effectiveColor, ignoreDistance, rangeOwnerSeat);
    internal void ResolveAssistedFactionSlashChoice(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().ResolveAssistedFactionSlashChoice(selected, advanceToHumanBoundary);
    internal void ValidateAssistedFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame) => _resolve().ValidateAssistedFactionSlashParent(pending, frame);
}
