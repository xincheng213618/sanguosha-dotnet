#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreSequentialDiscardProgramsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardProgramsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreSequentialDiscardProgramsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardProgramsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreSequentialDiscardProgramsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardProgramsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreSequentialDiscardProgramsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreSequentialDiscardProgramsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
