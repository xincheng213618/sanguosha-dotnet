#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCoreDrawFundedDistinctBasicReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCoreDrawFundedDistinctBasicReturnsOperations
    {
        public bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.DrawFundedDistinctBasicReturnsEntries.DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCharacterHuanShuJunOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCharacterHuanShuJunOlOperations
    {
        public bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying) => engine.HuanShuJunOlEntries.HuanpeiDyingRescueRide(index, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCoreTieredRoundZeroReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCoreTieredRoundZeroReturnsOperations
    {
        public bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.TieredRoundZeroReturnsEntries.TieredRoundZeroDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCharacterHouChengAnniversaryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCharacterHouChengAnniversaryOperations
    {
        public bool HouChengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.HouChengAnniversaryEntries.HouChengFirstChild(f, child);
        public bool HouChengSuffix(int root) => engine.HouChengAnniversaryEntries.HouChengSuffix(root);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCoreRecoveryReplacementDamageRidesOperations
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
        public bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.DamageObserverCursorQueriesEntries.DamageFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterHouChengAnniversaryResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterHouChengAnniversaryResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }
}
