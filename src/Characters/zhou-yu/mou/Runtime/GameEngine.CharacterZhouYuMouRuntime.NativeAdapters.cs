#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhouYuMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhouYuMouCharacterZhouYuMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouCharacterZhouYuMouOperations
    {
        public global::CardGame.Core.NativeOwners.MouZhouYuFacts CreateMouZhouYuFacts() => engine.ComposeNativeMouZhouYuFacts();
        public bool MouZyEdge(int i) => engine.ZhouYuMouEntries.MouZyEdge(i);
        public bool MouZyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ZhouYuMouEntries.MouZyFirstChild(f, child);
        public bool MouZySuffix(int i) => engine.ZhouYuMouEntries.MouZySuffix(i);
    }

    [ProgramCapability]
    private sealed class CharacterZhouYuMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterZhouYuMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterZhouYuMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterZhouYuMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhouYuMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
