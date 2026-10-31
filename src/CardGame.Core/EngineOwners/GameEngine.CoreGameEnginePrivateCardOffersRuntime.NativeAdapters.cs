#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CorePrivateCardOffersKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateCardOffersKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CorePrivateCardOffersCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateCardOffersCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CorePrivateCardOffersContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateCardOffersContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CorePrivateCardOffersResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICorePrivateCardOffersResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
