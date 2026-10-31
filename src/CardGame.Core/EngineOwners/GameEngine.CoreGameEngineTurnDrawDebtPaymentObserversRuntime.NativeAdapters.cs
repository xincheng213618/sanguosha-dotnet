#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTurnDrawDebtPaymentObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDrawDebtPaymentObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreTurnDrawDebtPaymentObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDrawDebtPaymentObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreTurnDrawDebtPaymentObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTurnDrawDebtPaymentObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
