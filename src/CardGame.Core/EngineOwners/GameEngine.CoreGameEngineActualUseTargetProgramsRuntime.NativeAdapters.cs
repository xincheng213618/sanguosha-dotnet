#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreActualUseTargetProgramsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualUseTargetProgramsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreActualUseTargetProgramsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualUseTargetProgramsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreActualUseTargetProgramsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualUseTargetProgramsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreActualUseTargetProgramsKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualUseTargetProgramsKernelCoreMainOperations
    {
        public void CompleteGame() => engine.CompleteGame();
    }

    [ProgramCapability]
    private sealed class CoreActualUseTargetProgramsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreActualUseTargetProgramsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
