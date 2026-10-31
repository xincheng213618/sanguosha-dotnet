#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhangXiuMouOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangXiuMouOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterZhangXiuMouOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangXiuMouOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhangXiuMouOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangXiuMouOlCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterZhangXiuMouOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangXiuMouOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhangXiuMouOlCharacterZhangXiuMouOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangXiuMouOlCharacterZhangXiuMouOlOperations
    {
        public bool MouZhangXiuOlObserverEdge(int n) => engine.ZhangXiuMouOlEntries.MouZhangXiuOlObserverEdge(n);
        public bool MouZhangXiuOlFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ZhangXiuMouOlEntries.MouZhangXiuOlFirstChild(f, child);
        public bool MouZhangXiuOlObserverSuffix(int n) => engine.ZhangXiuMouOlEntries.MouZhangXiuOlObserverSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterZhangXiuMouOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhangXiuMouOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
