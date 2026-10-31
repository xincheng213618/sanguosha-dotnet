#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreEndRefillNextTurnContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreEndRefillNextTurnContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
