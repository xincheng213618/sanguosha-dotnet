#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreEquipmentContentContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentContentContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreEquipmentContentModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentContentModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreEquipmentContentEquipmentRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEquipmentContentEquipmentRulesOperations
    {
        public ref global::CardGame.Core.EquipmentContentRegistry? _equipmentRules => ref engine._equipmentRules;
    }
}
