#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSimaShiMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSimaShiMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterSimaShiMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSimaShiMouRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterSimaShiMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterSimaShiMouCharacterSimaShiMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouCharacterSimaShiMouOperations
    {
        public bool MouSimaShiEdge(int i) => engine.SimaShiMouEntries.MouSimaShiEdge(i);
        public bool MouSimaShiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SimaShiMouEntries.MouSimaShiFirstChild(f, child);
        public bool MouSimaShiSuffix(int n) => engine.SimaShiMouEntries.MouSimaShiSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterSimaShiMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaShiMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
