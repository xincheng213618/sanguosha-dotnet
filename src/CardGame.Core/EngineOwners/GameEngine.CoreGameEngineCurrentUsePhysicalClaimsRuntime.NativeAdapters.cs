#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCurrentUsePhysicalClaimsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreCurrentUsePhysicalClaimsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreCurrentUsePhysicalClaimsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCurrentUsePhysicalClaimsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCurrentUsePhysicalClaimsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
