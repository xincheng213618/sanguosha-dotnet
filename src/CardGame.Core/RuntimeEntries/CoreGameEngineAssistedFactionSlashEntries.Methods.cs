#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAssistedFactionSlashEntries
{
    internal void BeginAssistedProgramFactionSlashRequest(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, int targetSeat, string resultBind) => _resolve().BeginAssistedProgramFactionSlashRequest(frame, actorSeat, targetSeat, resultBind);
    internal bool CanRequestAssistedProgramFactionSlash(int actorSeat, int targetSeat) => _resolve().CanRequestAssistedProgramFactionSlash(actorSeat, targetSeat);
    internal bool CanSupplyAssistedFactionSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false) => _resolve().CanSupplyAssistedFactionSlash(pending, kind, cards, isTrueZhangba);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateAssistedFactionSlashChoices(global::CardGame.Core.CharacterState provider, global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => _resolve().CreateAssistedFactionSlashChoices(provider, pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.AssistedFactionSlashCard> GetAssistedFactionSlashCards(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => _resolve().GetAssistedFactionSlashCards(pending, provider);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => _resolve().GetFactionRequestMultiCardSelections(pending, provider);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetFactionRequestZhangbaPairs(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => _resolve().GetFactionRequestZhangbaPairs(pending, provider);
    internal bool HasQigongFactionDistance(int actorSeat, int targetSeat) => _resolve().HasQigongFactionDistance(actorSeat, targetSeat);
    internal bool IsAssistedProvidedSlashTarget(int actorSeat, int targetSeat, global::CardGame.Core.CardKind kind, int? effectiveRank = default, bool allowPotentialRank = true, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => _resolve().IsAssistedProvidedSlashTarget(actorSeat, targetSeat, kind, effectiveRank, allowPotentialRank, physicalCardIds);
    internal void ResolveAssistedFactionSlashChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => _resolve().ResolveAssistedFactionSlashChoice(selected, advanceToHumanBoundary);
    internal void ValidateAssistedFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateAssistedFactionSlashParent(pending, frame);
}
