#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheEmptySlotEquipmentContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheEmptySlotEquipmentContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreShanheEmptySlotEquipmentCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheEmptySlotEquipmentCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreShanheEmptySlotEquipmentRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheEmptySlotEquipmentRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreShanheEmptySlotEquipmentKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheEmptySlotEquipmentKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
