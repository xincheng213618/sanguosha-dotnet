#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramOrdinaryTrickUsesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreProgramOrdinaryTrickUsesKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreProgramOrdinaryTrickUsesContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreProgramOrdinaryTrickUsesResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreProgramOrdinaryTrickUsesModeRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramOrdinaryTrickUsesModeRulesOperations
    {
        public ref readonly global::CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }
}
