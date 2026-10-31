#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiCharacterHuanJieMobileShiAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiCharacterHuanJieMobileShiOperations
    {
        public bool ShiHjChildEdge(int i) => engine.HuanJieMobileShiEntries.ShiHjChildEdge(i);
        public bool ShiHjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.HuanJieMobileShiEntries.ShiHjFirstChild(f, child);
        public bool ShiHjChildSuffix(int n) => engine.HuanJieMobileShiEntries.ShiHjChildSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterHuanJieMobileShiContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHuanJieMobileShiContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
