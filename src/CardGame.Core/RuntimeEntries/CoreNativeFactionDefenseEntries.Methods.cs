#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFactionDefenseEntries
{
    internal void AdvanceFactionDefenseCandidate(bool keepCurrentRequest = false) => _resolve().AdvanceFactionDefenseCandidate(keepCurrentRequest);
    internal void BeginFactionDefenseRequest(NativeOwners.CardAttackHandle attack) => _resolve().BeginFactionDefenseRequest(attack);
    internal void CompleteFactionDefenseResponse(NativeOwners.FactionDefenseHandle pending, CharacterState provider, Card? selectedDodge, bool usedBagua, CardConversionSource? virtualResponseSource = default) => _resolve().CompleteFactionDefenseResponse(pending, provider, selectedDodge, usedBagua, virtualResponseSource);
    internal IReadOnlyList<PromptChoice> CreateFactionDefenseResponseChoices(CharacterState provider, NativeOwners.FactionDefenseHandle pending, IReadOnlyList<Card> responseCards, bool includeBagua) => _resolve().CreateFactionDefenseResponseChoices(provider, pending, responseCards, includeBagua);
    internal void ResolveFactionDefenseCandidateResponse(NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId) => _resolve().ResolveFactionDefenseCandidateResponse(pending, useDodge, useBagua, requestedCardId);
}
