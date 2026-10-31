#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFactionRequestCostRecoveriesEntries
{
    internal void CancelPaidFactionRequestAfterRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request) => _resolve().CancelPaidFactionRequestAfterRecovery(request);
    internal void ContinuePaidFactionRequestCost(long producerId) => _resolve().ContinuePaidFactionRequestCost(producerId);
    internal bool HasPaidFactionRequestCostRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request) => _resolve().HasPaidFactionRequestCostRecovery(request);
    internal bool TryPausePaidFactionRequestCost(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().TryPausePaidFactionRequestCost(request, card, from);
}
