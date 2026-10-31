#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterXuShengMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterXuShengMouCharacterXuShengMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouCharacterXuShengMouOperations
    {
        public bool MouXuShengNativeEdge(int i) => engine.XuShengMouEntries.MouXuShengNativeEdge(i);
        public bool MouXuShengFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.XuShengMouEntries.MouXuShengFirstNativeChild(f, child);
        public bool MouXuShengNativeSuffix(int n) => engine.XuShengMouEntries.MouXuShengNativeSuffix(n);
        public global::CardGame.Core.NativeOwners.MouXuShengFacts CreateMouXuShengFacts() => engine.ComposeNativeMouXuShengFacts();
    }

    [ProgramCapability]
    private sealed class CharacterXuShengMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterXuShengMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterXuShengMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterXuShengMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXuShengMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }
}
