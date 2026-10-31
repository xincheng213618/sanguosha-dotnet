#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterJuShouMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterJuShouMouCharacterJuShouMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouCharacterJuShouMouOperations
    {
        public bool MouJsEdge(int i) => engine.JuShouMouEntries.MouJsEdge(i);
        public bool MouJsFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.JuShouMouEntries.MouJsFirstChild(f, child);
        public bool MouJsSuffix(int n) => engine.JuShouMouEntries.MouJsSuffix(n);
        public global::CardGame.Core.NativeOwners.MouJuShouFacts CreateMouJuShouFacts() => engine.ComposeNativeMouJuShouFacts();
    }

    [ProgramCapability]
    private sealed class CharacterJuShouMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterJuShouMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterJuShouMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterJuShouMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterJuShouMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
