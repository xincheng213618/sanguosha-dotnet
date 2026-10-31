#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreEquipmentPairPaymentsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentPairPaymentsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreEquipmentPairPaymentsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentPairPaymentsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreEquipmentPairPaymentsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentPairPaymentsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreEquipmentPairPaymentsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentPairPaymentsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
