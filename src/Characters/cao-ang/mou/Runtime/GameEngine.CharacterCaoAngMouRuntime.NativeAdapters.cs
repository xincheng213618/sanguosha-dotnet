#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoAngMouCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoAngMouCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterCaoAngMouCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoAngMouCoreObserverSubtreesOperations2
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterCaoAngMouCharacterCaoAngMouAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoAngMouCharacterCaoAngMouOperations
    {
        public bool MouCaEdge(int i) => engine.CaoAngMouEntries.MouCaEdge(i);
        public bool MouCaFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.CaoAngMouEntries.MouCaFirstChild(f, child);
        public bool MouCaSuffix(int n) => engine.CaoAngMouEntries.MouCaSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterCaoAngMouKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoAngMouKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterCaoAngMouContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoAngMouContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterCaoAngMouResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoAngMouResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
