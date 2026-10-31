#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTrustedCardZoneDiagnosticsRuntime
{
    internal IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics() => _cardZonesOperations._cardZones.CreateDiagnostics();
}
