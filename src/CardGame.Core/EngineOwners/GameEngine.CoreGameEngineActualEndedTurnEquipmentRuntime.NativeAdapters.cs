#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActualEndedTurnEquipmentResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualEndedTurnEquipmentResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreActualEndedTurnEquipmentCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualEndedTurnEquipmentCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreActualEndedTurnEquipmentContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualEndedTurnEquipmentContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreActualEndedTurnEquipmentKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualEndedTurnEquipmentKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
