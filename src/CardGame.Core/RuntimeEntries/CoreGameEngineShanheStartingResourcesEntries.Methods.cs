#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheStartingResourcesEntries
{
    internal void CaptureShanheClaimedDeclarationUse(global::CardGame.Core.DeclaredCardPayment paid) => _resolve().CaptureShanheClaimedDeclarationUse(paid);
    internal void CaptureShanheStartingResourceUseEntries(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureShanheStartingResourceUseEntries(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureShanheStartingResourceUseEntries(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureShanheStartingResourceUseEntries(batch, completed);
    internal bool IsShanheStartingResource(int id) => _resolve().IsShanheStartingResource(id);
    internal bool IsShanheStartingResourceDestroyedMaterial(global::CardGame.Core.CardMovementRecord move) => _resolve().IsShanheStartingResourceDestroyedMaterial(move);
    internal bool IsShanheStartingResourceDestroyedUseMaterial(long useId, int cardId) => _resolve().IsShanheStartingResourceDestroyedUseMaterial(useId, cardId);
    internal bool IsShanheStartingResourceResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish) => _resolve().IsShanheStartingResourceResponseFinish(action, cost, finish);
    internal global::CardGame.Core.CardLocation NormalizeShanheStartingResourceDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason) => _resolve().NormalizeShanheStartingResourceDestination(card, from, to, reason);
    internal void PublishShanheStartingResourceOrigins() => _resolve().PublishShanheStartingResourceOrigins();
    internal void RecordShanheStartingResourceDestruction(global::CardGame.Core.CardMovementRecord move) => _resolve().RecordShanheStartingResourceDestruction(move);
    internal bool ShanheStartingResourceWasAcquired(int id, int before = 2147483647) => _resolve().ShanheStartingResourceWasAcquired(id, before);
    internal bool TryDescribeShanheResourceExit(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason, int before, scoped out int entrySequence, scoped out long parentFrame, scoped out long actionId) => _resolve().TryDescribeShanheResourceExit(cardId, from, reason, before, out entrySequence, out parentFrame, out actionId);
}
