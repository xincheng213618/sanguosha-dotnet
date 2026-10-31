#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTrustedCardZoneDiagnosticsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTrustedCardZoneDiagnosticsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
