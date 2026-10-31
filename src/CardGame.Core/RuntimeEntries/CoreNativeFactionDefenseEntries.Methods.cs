#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFactionDefenseEntries
{
    internal void AdvanceFactionDefenseCandidate(bool keepCurrentRequest = false) => _resolve().AdvanceFactionDefenseCandidate(keepCurrentRequest);
    internal void BeginFactionDefenseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().BeginFactionDefenseRequest(attack);
    internal void CompleteFactionDefenseResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card? selectedDodge, bool usedBagua, global::CardGame.Core.CardConversionSource? virtualResponseSource = default) => _resolve().CompleteFactionDefenseResponse(pending, provider, selectedDodge, usedBagua, virtualResponseSource);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateFactionDefenseResponseChoices(global::CardGame.Core.CharacterState provider, global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, bool includeBagua) => _resolve().CreateFactionDefenseResponseChoices(provider, pending, responseCards, includeBagua);
    internal void ResolveFactionDefenseCandidateResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId) => _resolve().ResolveFactionDefenseCandidateResponse(pending, useDodge, useBagua, requestedCardId);
}
