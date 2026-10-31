#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreShanheSourceXiyingTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheSourceXiyingTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }
}
