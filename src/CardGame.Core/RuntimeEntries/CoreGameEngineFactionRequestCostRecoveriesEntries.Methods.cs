#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFactionRequestCostRecoveriesEntries
{
    internal void CancelPaidFactionRequestAfterRecovery(NativeOwners.FactionCardRequestHandle request) => _resolve().CancelPaidFactionRequestAfterRecovery(request);
    internal void ContinuePaidFactionRequestCost(long producerId) => _resolve().ContinuePaidFactionRequestCost(producerId);
    internal bool HasPaidFactionRequestCostRecovery(NativeOwners.FactionCardRequestHandle? request) => _resolve().HasPaidFactionRequestCostRecovery(request);
    internal bool TryPausePaidFactionRequestCost(NativeOwners.FactionCardRequestHandle request, Card card, CardLocation from) => _resolve().TryPausePaidFactionRequestCost(request, card, from);
}
