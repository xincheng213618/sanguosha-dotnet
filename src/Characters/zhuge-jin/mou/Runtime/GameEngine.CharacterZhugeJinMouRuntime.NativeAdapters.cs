#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterZhugeJinMouCharacterZhugeJinMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouCharacterZhugeJinMouOperations
    {
        public bool MouZhugeJinEdge(int i) => engine.ZhugeJinMouEntries.MouZhugeJinEdge(i);
        public bool MouZhugeJinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.ZhugeJinMouEntries.MouZhugeJinFirstChild(f, child);
        public bool MouZhugeJinSuffix(int n) => engine.ZhugeJinMouEntries.MouZhugeJinSuffix(n);
        public global::CardGame.Core.NativeOwners.MouZhugeJinFacts CreateMouZhugeJinFacts() => engine.ComposeNativeMouZhugeJinFacts();
    }

    [ProgramCapability]
    private sealed class CharacterZhugeJinMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterZhugeJinMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterZhugeJinMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterZhugeJinMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterZhugeJinMouRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterZhugeJinMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterZhugeJinMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
