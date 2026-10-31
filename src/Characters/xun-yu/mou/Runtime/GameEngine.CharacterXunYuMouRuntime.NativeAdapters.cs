#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterXunYuMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterXunYuMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterXunYuMouCharacterXunYuMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouCharacterXunYuMouOperations
    {
        public bool MouXyEdge(int i) => engine.XunYuMouEntries.MouXyEdge(i);
        public bool MouXyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.XunYuMouEntries.MouXyFirstChild(f, child);
        public bool MouXySuffix(int n) => engine.XunYuMouEntries.MouXySuffix(n);
        public global::CardGame.Core.NativeOwners.MouXunYuAnniversaryFacts CreateMouXunYuAnniversaryFacts() => engine.ComposeNativeMouXunYuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class CharacterXunYuMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterXunYuMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterXunYuMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
