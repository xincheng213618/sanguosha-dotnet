#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSequentialDiscardObserversResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardObserversResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreSequentialDiscardObserversCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardObserversCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
