#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterWangLingMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangLingMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterWangLingMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangLingMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterWangLingMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWangLingMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
