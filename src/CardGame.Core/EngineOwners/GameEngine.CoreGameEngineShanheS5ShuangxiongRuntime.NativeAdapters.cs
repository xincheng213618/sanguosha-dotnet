#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreShanheS5ShuangxiongKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ShuangxiongKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreShanheS5ShuangxiongContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ShuangxiongContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreShanheS5ShuangxiongCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ShuangxiongCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreShanheS5ShuangxiongTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ShuangxiongTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CoreShanheS5ShuangxiongResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreShanheS5ShuangxiongResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
