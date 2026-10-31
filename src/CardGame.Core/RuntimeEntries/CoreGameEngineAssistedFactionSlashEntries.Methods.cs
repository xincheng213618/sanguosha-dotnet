#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedFactionSlashEntries
{
    internal void BeginAssistedProgramFactionSlashRequest(ProgramSkillFrame frame, int actorSeat, int targetSeat, string resultBind) => _resolve().BeginAssistedProgramFactionSlashRequest(frame, actorSeat, targetSeat, resultBind);
    internal bool CanRequestAssistedProgramFactionSlash(int actorSeat, int targetSeat) => _resolve().CanRequestAssistedProgramFactionSlash(actorSeat, targetSeat);
    internal bool CanSupplyAssistedFactionSlash(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) => _resolve().CanSupplyAssistedFactionSlash(pending, kind, cards, isTrueZhangba);
    internal IReadOnlyList<PromptChoice> CreateAssistedFactionSlashChoices(CharacterState provider, NativeOwners.FactionCardRequestHandle pending) => _resolve().CreateAssistedFactionSlashChoices(provider, pending);
    internal IReadOnlyList<GameEngine.AssistedFactionSlashCard> GetAssistedFactionSlashCards(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetAssistedFactionSlashCards(pending, provider);
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetFactionRequestMultiCardSelections(pending, provider);
    internal IReadOnlyList<IReadOnlyList<Card>> GetFactionRequestZhangbaPairs(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _resolve().GetFactionRequestZhangbaPairs(pending, provider);
    internal bool HasQigongFactionDistance(int actorSeat, int targetSeat) => _resolve().HasQigongFactionDistance(actorSeat, targetSeat);
    internal bool IsAssistedProvidedSlashTarget(int actorSeat, int targetSeat, CardKind kind, int? effectiveRank = default, bool allowPotentialRank = true, IReadOnlyList<int>? physicalCardIds = default) => _resolve().IsAssistedProvidedSlashTarget(actorSeat, targetSeat, kind, effectiveRank, allowPotentialRank, physicalCardIds);
    internal void ResolveAssistedFactionSlashChoice(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().ResolveAssistedFactionSlashChoice(selected, advanceToHumanBoundary);
    internal void ValidateAssistedFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame) => _resolve().ValidateAssistedFactionSlashParent(pending, frame);
}
