#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterWenChouMouOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterWenChouMouOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlCoreObserverSubtreesOperations
    {
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterWenChouMouOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterWenChouMouOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterWenChouMouOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterWenChouMouOlCharacterWenChouMouOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlCharacterWenChouMouOlOperations
    {
        public bool MouWenChouObserverEdge(int n) => engine.WenChouMouOlEntries.MouWenChouObserverEdge(n);
        public bool MouWenChouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.WenChouMouOlEntries.MouWenChouFirstChild(f, child);
        public bool MouWenChouObserverSuffix(int n) => engine.WenChouMouOlEntries.MouWenChouObserverSuffix(n);
    }

    [ProgramCapability]
    private sealed class CharacterWenChouMouOlPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenChouMouOlPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }
}
