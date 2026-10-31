#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterSimaYiMouTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouCharacterSimaYiMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouCharacterSimaYiMouOperations
    {
        public bool MouSmEdge(int i) => engine.SimaYiMouEntries.MouSmEdge(i);
        public bool MouSmFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SimaYiMouEntries.MouSmFirstChild(f, child);
        public bool MouSmSuffix(int n) => engine.SimaYiMouEntries.MouSmSuffix(n);
        public global::CardGame.Core.NativeOwners.MouSmFacts CreateMouSmFacts() => engine.ComposeNativeMouSmFacts();
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterSimaYiMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterSimaYiMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
