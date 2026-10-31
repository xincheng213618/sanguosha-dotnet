#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreCapturedEquipmentDrawsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCapturedEquipmentDrawsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreCapturedEquipmentDrawsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCapturedEquipmentDrawsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreCapturedEquipmentDrawsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCapturedEquipmentDrawsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreCapturedEquipmentDrawsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreCapturedEquipmentDrawsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
