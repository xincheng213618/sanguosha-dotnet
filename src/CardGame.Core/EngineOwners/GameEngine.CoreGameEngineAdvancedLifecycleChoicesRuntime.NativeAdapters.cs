#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAdvancedLifecycleChoicesSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedLifecycleChoicesSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedLifecycleChoicesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedLifecycleChoicesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedLifecycleChoicesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedLifecycleChoicesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedLifecycleChoicesRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedLifecycleChoicesRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedLifecycleChoicesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedLifecycleChoicesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
