#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeNullificationEntries
{
    internal void BeginJizhiOrNullificationWindow(long resolutionId, Card effectCard, int sourceSeat, IReadOnlyList<int> targetSeats, LegalActionKind actionKind, int? targetCardId = default, CardKind? requiredCardKind = default, CardKind? playedCardKind = default) => _resolve().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
    internal void BeginNullificationWindow(long resolutionId, Card effectCard, int sourceSeat, IReadOnlyList<int> targetSeats, LegalActionKind actionKind, int? targetCardId = default, CardKind? requiredCardKind = default, CardKind? playedCardKind = default) => _resolve().BeginNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
    internal IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat) => _resolve().BuildNullificationCandidateSeats(startAfterSeat);
    internal void ContinueJizhiOrNullificationAfterTargetTriggers(NativeOwners.JizhiResolution pending) => _resolve().ContinueJizhiOrNullificationAfterTargetTriggers(pending);
    internal void ContinueNullificationWindow(NullificationWindowFrame pending) => _resolve().ContinueNullificationWindow(pending);
    internal IReadOnlyList<PromptChoice> CreateNullificationChoices(NullificationWindowFrame pending, CharacterState responder, IReadOnlyList<Card> cards) => _resolve().CreateNullificationChoices(pending, responder, cards);
    internal void FinishNullificationWindow(NullificationWindowFrame pending) => _resolve().FinishNullificationWindow(pending);
    internal IReadOnlyList<Card> GetNullificationCards(CharacterState responder) => _resolve().GetNullificationCards(responder);
    internal void ResolveNullifiableEffect(NullificationWindowFrame pending) => _resolve().ResolveNullifiableEffect(pending);
    internal void ResolveNullifiableEffectCore(NullificationWindowFrame pending) => _resolve().ResolveNullifiableEffectCore(pending);
    internal void ResolveNullificationChoice(NullificationWindowFrame pending, CharacterState responder, Card? selectedCard, CardConversionSource? selectedConversionSource = default) => _resolve().ResolveNullificationChoice(pending, responder, selectedCard, selectedConversionSource);
    internal void ValidateNullificationConversion(CharacterState responder, Card card, CardConversionSource? selectedSource) => _resolve().ValidateNullificationConversion(responder, card, selectedSource);
}
