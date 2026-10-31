#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeCoreInvariantsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreNativeCoreInvariantsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreNativeCoreInvariantsSuppressedCardMovedEventBatchIdsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsSuppressedCardMovedEventBatchIdsOperations
    {
        public ref readonly global::System.Collections.Generic.Stack<long> _suppressedCardMovedEventBatchIds => ref engine._suppressedCardMovedEventBatchIds;
    }

    [ProgramCapability]
    private sealed class CoreNativeCoreInvariantsActiveCardMovementBatchIdsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsActiveCardMovementBatchIdsOperations
    {
        public ref readonly global::System.Collections.Generic.Stack<long> _activeCardMovementBatchIds => ref engine._activeCardMovementBatchIds;
    }

    [ProgramCapability]
    private sealed class CoreNativeCoreInvariantsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreNativeCoreInvariantsTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeCoreInvariantsTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }
}
