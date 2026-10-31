#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheStartingResourcesEntries
{
    internal void CaptureShanheClaimedDeclarationUse(DeclaredCardPayment paid) => _resolve().CaptureShanheClaimedDeclarationUse(paid);
    internal void CaptureShanheStartingResourceUseEntries(CardMovementBatchContext batch) => _resolve().CaptureShanheStartingResourceUseEntries(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureShanheStartingResourceUseEntries(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureShanheStartingResourceUseEntries(batch, completed);
    internal bool IsShanheStartingResource(int id) => _resolve().IsShanheStartingResource(id);
    internal bool IsShanheStartingResourceDestroyedMaterial(CardMovementRecord move) => _resolve().IsShanheStartingResourceDestroyedMaterial(move);
    internal bool IsShanheStartingResourceDestroyedUseMaterial(long useId, int cardId) => _resolve().IsShanheStartingResourceDestroyedUseMaterial(useId, cardId);
    internal bool IsShanheStartingResourceResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _resolve().IsShanheStartingResourceResponseFinish(action, cost, finish);
    internal CardLocation NormalizeShanheStartingResourceDestination(Card card, CardLocation from, CardLocation to, CardMoveReason? reason) => _resolve().NormalizeShanheStartingResourceDestination(card, from, to, reason);
    internal void PublishShanheStartingResourceOrigins() => _resolve().PublishShanheStartingResourceOrigins();
    internal void RecordShanheStartingResourceDestruction(CardMovementRecord move) => _resolve().RecordShanheStartingResourceDestruction(move);
    internal bool ShanheStartingResourceWasAcquired(int id, int before = 2147483647) => _resolve().ShanheStartingResourceWasAcquired(id, before);
    internal bool TryDescribeShanheResourceExit(int cardId, CardLocation from, CardMoveReason reason, int before, scoped out int entrySequence, scoped out long parentFrame, scoped out long actionId) => _resolve().TryDescribeShanheResourceExit(cardId, from, reason, before, out entrySequence, out parentFrame, out actionId);
}
