#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheNullifiedClaimObserversContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheNullifiedClaimObserversContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreShanheNullifiedClaimObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheNullifiedClaimObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreShanheNullifiedClaimObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheNullifiedClaimObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
