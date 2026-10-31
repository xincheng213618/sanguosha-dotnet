#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCompletedCardGiftsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedCardGiftsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreCompletedCardGiftsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedCardGiftsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreCompletedCardGiftsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedCardGiftsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCompletedCardGiftsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCompletedCardGiftsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
