#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativePublicPileOriginalCardsEntries
{
    internal void AssertNativePublicPileOriginalPayments() => _resolve().AssertNativePublicPileOriginalPayments();
    internal void BindAcceptedNativePublicPileOriginal(IGameEvent payload) => _resolve().BindAcceptedNativePublicPileOriginal(payload);
    internal void BindNativePublicPilePayment(ResolutionFrame parent, CardActionContext action) => _resolve().BindNativePublicPilePayment(parent, action);
    internal void CaptureNativePublicPilePaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureNativePublicPilePaymentBatch(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureNativePublicPilePaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNativePublicPilePaymentBatch(batch, completed);
    internal Card? FindNativePublicPileOriginalCard(CharacterState provider, int id) => _resolve().FindNativePublicPileOriginalCard(provider, id);
    internal IReadOnlyList<NativePublicPileCardSelection> GetNativePublicPileOriginalCards(NativePublicPileCardRequest request) => _resolve().GetNativePublicPileOriginalCards(request);
    internal bool IsNativePublicPileOriginalActionCost(CardActionContext action, CardActionCost cost) => _resolve().IsNativePublicPileOriginalActionCost(action, cost);
    internal bool IsNativePublicPileOriginalChoice(CharacterState owner, Card card, CardKind kind, bool response) => _resolve().IsNativePublicPileOriginalChoice(owner, card, kind, response);
    internal IReadOnlyList<LegalAction> NativePublicPileOriginalPlayActions(CharacterState actor, int? selectedId) => _resolve().NativePublicPileOriginalPlayActions(actor, selectedId);
    internal IReadOnlyList<Card> NativePublicPileOriginalUseCards(CharacterState provider) => _resolve().NativePublicPileOriginalUseCards(provider);
    internal IReadOnlyList<Card> NativePublicPileResponseCards(CharacterState provider, CardKind kind) => _resolve().NativePublicPileResponseCards(provider, kind);
    internal NativePublicPileCardRequest NativePublicPileResponseRequest(CharacterState provider, CardKind kind) => _resolve().NativePublicPileResponseRequest(provider, kind);
    internal IReadOnlyList<Card> NativePublicPileSlashUseCards(CharacterState provider) => _resolve().NativePublicPileSlashUseCards(provider);
    internal NativePublicPileCardRequest NativePublicPileUseRequest(int actor, int provider, CardKind? kind = default, long? parent = default) => _resolve().NativePublicPileUseRequest(actor, provider, kind, parent);
    internal void PrepareNativePublicPilePayment(ResolutionFrame parent, NativePublicPileCardSelection selection) => _resolve().PrepareNativePublicPilePayment(parent, selection);
    internal void PrepareNativePublicPileProvidedSlash(NativeOwners.FactionCardRequestHandle request, CharacterState provider, IReadOnlyList<Card> cards, CardConversionSource? conversion) => _resolve().PrepareNativePublicPileProvidedSlash(request, provider, cards, conversion);
    internal void PrepareNativePublicPileResponsePayment(CharacterState provider, Card card, CardKind kind, CardConversionSource? conversion) => _resolve().PrepareNativePublicPileResponsePayment(provider, card, kind, conversion);
    internal void PrepareNativePublicPileUse(CardUseFrame use) => _resolve().PrepareNativePublicPileUse(use);
    internal NativePublicPileCardSelection RequireNativePublicPileOriginalCard(NativePublicPileCardRequest request, NativePublicPileCardSelection selection) => _resolve().RequireNativePublicPileOriginalCard(request, selection);
    internal bool TryNativePublicPilePaymentLocation(CharacterState provider, Card card, scoped out CardLocation from) => _resolve().TryNativePublicPilePaymentLocation(provider, card, out from);
}
