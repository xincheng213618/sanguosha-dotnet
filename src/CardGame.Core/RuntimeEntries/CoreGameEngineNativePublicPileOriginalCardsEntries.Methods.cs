#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativePublicPileOriginalCardsEntries
{
    internal void AssertNativePublicPileOriginalPayments() => _resolve().AssertNativePublicPileOriginalPayments();
    internal void BindAcceptedNativePublicPileOriginal(global::CardGame.Core.IGameEvent payload) => _resolve().BindAcceptedNativePublicPileOriginal(payload);
    internal void BindNativePublicPilePayment(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action) => _resolve().BindNativePublicPilePayment(parent, action);
    internal void CaptureNativePublicPilePaymentBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureNativePublicPilePaymentBatch(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureNativePublicPilePaymentBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNativePublicPilePaymentBatch(batch, completed);
    internal global::CardGame.Core.Card? FindNativePublicPileOriginalCard(global::CardGame.Core.CharacterState provider, int id) => _resolve().FindNativePublicPileOriginalCard(provider, id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> GetNativePublicPileOriginalCards(global::CardGame.Core.NativePublicPileCardRequest request) => _resolve().GetNativePublicPileOriginalCards(request);
    internal bool IsNativePublicPileOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => _resolve().IsNativePublicPileOriginalActionCost(action, cost);
    internal bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response) => _resolve().IsNativePublicPileOriginalChoice(owner, card, kind, response);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> NativePublicPileOriginalPlayActions(global::CardGame.Core.CharacterState actor, int? selectedId) => _resolve().NativePublicPileOriginalPlayActions(actor, selectedId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileOriginalUseCards(global::CardGame.Core.CharacterState provider) => _resolve().NativePublicPileOriginalUseCards(provider);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileResponseCards(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind) => _resolve().NativePublicPileResponseCards(provider, kind);
    internal global::CardGame.Core.NativePublicPileCardRequest NativePublicPileResponseRequest(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind) => _resolve().NativePublicPileResponseRequest(provider, kind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileSlashUseCards(global::CardGame.Core.CharacterState provider) => _resolve().NativePublicPileSlashUseCards(provider);
    internal global::CardGame.Core.NativePublicPileCardRequest NativePublicPileUseRequest(int actor, int provider, global::CardGame.Core.CardKind? kind = default, long? parent = default) => _resolve().NativePublicPileUseRequest(actor, provider, kind, parent);
    internal void PrepareNativePublicPilePayment(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.NativePublicPileCardSelection selection) => _resolve().PrepareNativePublicPilePayment(parent, selection);
    internal void PrepareNativePublicPileProvidedSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? conversion) => _resolve().PrepareNativePublicPileProvidedSlash(request, provider, cards, conversion);
    internal void PrepareNativePublicPileResponsePayment(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion) => _resolve().PrepareNativePublicPileResponsePayment(provider, card, kind, conversion);
    internal void PrepareNativePublicPileUse(global::CardGame.Core.CardUseFrame use) => _resolve().PrepareNativePublicPileUse(use);
    internal global::CardGame.Core.NativePublicPileCardSelection RequireNativePublicPileOriginalCard(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection selection) => _resolve().RequireNativePublicPileOriginalCard(request, selection);
    internal bool TryNativePublicPilePaymentLocation(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, scoped out global::CardGame.Core.CardLocation from) => _resolve().TryNativePublicPilePaymentLocation(provider, card, out from);
}
