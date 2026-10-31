#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSourceCurseObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSourceCurseObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSourceCurseObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSourceCurseObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSourceCurseObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSourceCurseObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
