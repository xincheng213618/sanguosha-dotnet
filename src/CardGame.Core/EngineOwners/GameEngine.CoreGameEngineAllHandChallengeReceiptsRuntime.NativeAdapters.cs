#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAllHandChallengeReceiptsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreAllHandChallengeReceiptsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreAllHandChallengeReceiptsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAllHandChallengeReceiptsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
