#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoHongMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterCaoHongMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterCaoHongMouCharacterCaoHongMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouCharacterCaoHongMouOperations
    {
        public bool MouCaoHongEdge(int i) => engine.CaoHongMouEntries.MouCaoHongEdge(i);
        public bool MouCaoHongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.CaoHongMouEntries.MouCaoHongFirstChild(f, child);
        public bool MouCaoHongSuffix(int n) => engine.CaoHongMouEntries.MouCaoHongSuffix(n);
        public global::CardGame.Core.NativeOwners.MouCaoHongFacts CreateMouCaoHongFacts() => engine.ComposeNativeMouCaoHongFacts();
        public bool CurrentMouCaoHongDebt(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSkillWindowContext w) => engine.ComposeNativeMouCaoHongFacts().CurrentMouCaoHongDebt(f, w);
        public global::CardGame.Core.MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt2(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => engine.CaoHongMouEntries.CurrentMouCaoHongDebt(c, w);
    }

    [ProgramCapability]
    private sealed class CharacterCaoHongMouRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterCaoHongMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterCaoHongMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterCaoHongMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoHongMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }
}
