#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterGuoJiaMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaMouCharacterGuoJiaMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouCharacterGuoJiaMouOperations
    {
        public bool MouGjEdge(int i) => engine.GuoJiaMouEntries.MouGjEdge(i);
        public bool MouGjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.GuoJiaMouEntries.MouGjFirstChild(f, child);
        public bool MouGjSuffix(int n) => engine.GuoJiaMouEntries.MouGjSuffix(n);
        public global::CardGame.Core.NativeOwners.MouGuoJiaFacts CreateMouGuoJiaFacts() => engine.ComposeNativeMouGuoJiaFacts();
    }

    [ProgramCapability]
    private sealed class CharacterGuoJiaMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterGuoJiaMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
