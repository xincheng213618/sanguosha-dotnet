#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTrustedCardZoneDiagnosticsRuntime
{
    internal IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => _cardZonesOperations._cardZones.CreateDiagnostics();
}
