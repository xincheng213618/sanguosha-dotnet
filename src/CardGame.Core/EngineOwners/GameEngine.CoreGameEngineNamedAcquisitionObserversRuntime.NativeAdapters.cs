#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNamedAcquisitionObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNamedAcquisitionObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNamedAcquisitionObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNamedAcquisitionObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreNamedAcquisitionObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNamedAcquisitionObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
