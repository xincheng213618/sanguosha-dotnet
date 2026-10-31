#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreModeSetupEquipmentCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupEquipmentCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreModeSetupEquipmentKernelCoreGameEngineModeSetupEquipmentAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupEquipmentKernelCoreGameEngineModeSetupEquipmentOperations
    {
        public global::CardGame.Core.NativeOwners.ModeSetupEquipmentHost CreateModeSetupEquipmentHost() => engine.ComposeNativeModeSetupEquipmentHost();
    }

    [ProgramCapability]
    private sealed class CoreModeSetupEquipmentKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreModeSetupEquipmentKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
