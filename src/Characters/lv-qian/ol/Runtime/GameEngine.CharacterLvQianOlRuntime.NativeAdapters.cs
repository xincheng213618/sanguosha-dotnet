#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterLvQianOlCharacterLvQianOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCharacterLvQianOlOperations
    {
        public bool LvQianFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.LvQianOlEntries.LvQianFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlCoreTieredRoundZeroReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCoreTieredRoundZeroReturnsOperations
    {
        public bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.TieredRoundZeroReturnsEntries.TieredRoundZeroDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlCoreDrawFundedDistinctBasicReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCoreDrawFundedDistinctBasicReturnsOperations
    {
        public bool DrawFundedDistinctBasicDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => engine.DrawFundedDistinctBasicReturnsEntries.DrawFundedDistinctBasicDyingRescueRide(dyingIndex, dying);
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCoreObserverSubtreesOperations2
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterLvQianOlCharacterHuanShuJunOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterLvQianOlCharacterHuanShuJunOlOperations
    {
        public bool HuanpeiDyingRescueRide(int index, global::CardGame.Core.DyingFrame dying) => engine.HuanShuJunOlEntries.HuanpeiDyingRescueRide(index, dying);
    }
}
