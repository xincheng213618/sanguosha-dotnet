#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterJiaXuMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterJiaXuMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterJiaXuMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterJiaXuMouCharacterJiaXuMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouCharacterJiaXuMouOperations
    {
        public bool MouJxEdge(int i) => engine.JiaXuMouEntries.MouJxEdge(i);
        public bool MouJxFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.JiaXuMouEntries.MouJxFirstChild(f, child);
        public bool MouJxSuffix(int n) => engine.JiaXuMouEntries.MouJxSuffix(n);
        public global::CardGame.Core.NativeOwners.MouJiaXuFacts CreateMouJiaXuFacts() => engine.ComposeNativeMouJiaXuFacts();
    }

    [ProgramCapability]
    private sealed class CharacterJiaXuMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterJiaXuMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJiaXuMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
