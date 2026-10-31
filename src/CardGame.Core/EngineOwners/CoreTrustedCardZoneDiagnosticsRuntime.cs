#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreTrustedCardZoneDiagnosticsRuntime
{
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    internal CoreTrustedCardZoneDiagnosticsRuntime(IKernelCardZoneStoreAccess cardZonesOperations)
    {
        this._cardZonesOperations = cardZonesOperations;
    }
}
