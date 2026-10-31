#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryPendingCardsMovedBatchesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryPendingCardsMovedBatchesOperations
    {
        public ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
        public bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.DamageObserverCursorQueriesEntries.DamageFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCharacterQiaoRuiAnniversaryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCharacterQiaoRuiAnniversaryOperations
    {
        public bool QiaoRuiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.QiaoRuiAnniversaryEntries.QiaoRuiFirstChild(f, child);
        public bool QiaoRuiObserverSuffix(int i) => engine.QiaoRuiAnniversaryEntries.QiaoRuiObserverSuffix(i);
        public bool QiaoRuiNativeEdge(int child) => engine.QiaoRuiAnniversaryEntries.QiaoRuiNativeEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCharacterHuanShuJunOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCharacterHuanShuJunOlOperations
    {
        public bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying) => engine.HuanShuJunOlEntries.HuanpeiDyingRescueRide(index, dying);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCoreRecoveryReplacementDamageRidesOperations2
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCoreDrawFundedDistinctBasicReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCoreDrawFundedDistinctBasicReturnsOperations
    {
        public bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.DrawFundedDistinctBasicReturnsEntries.DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCoreTieredRoundZeroReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCoreTieredRoundZeroReturnsOperations
    {
        public bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.TieredRoundZeroReturnsEntries.TieredRoundZeroDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterQiaoRuiAnniversaryCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterQiaoRuiAnniversaryCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
