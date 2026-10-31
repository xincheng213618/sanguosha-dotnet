#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreTargetedOrdinaryTrickResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTargetedOrdinaryTrickResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreTargetedOrdinaryTrickTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTargetedOrdinaryTrickTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CoreTargetedOrdinaryTrickContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTargetedOrdinaryTrickContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreTargetedOrdinaryTrickKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreTargetedOrdinaryTrickKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
