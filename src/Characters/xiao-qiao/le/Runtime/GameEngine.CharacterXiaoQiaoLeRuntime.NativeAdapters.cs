#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCoreSameNameHandDiscardOrDamageObserversAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCoreSameNameHandDiscardOrDamageObserversOperations
    {
        public bool IsSameNameHandObserverEdge(int child) => engine.SameNameHandDiscardOrDamageObserversEntries.IsSameNameHandObserverEdge(child);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCoreRecoveryReplacementDamageRidesOperations2
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCoreObserverSubtreesOperations
    {
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCoreDamageObserverCursorQueriesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCoreDamageObserverCursorQueriesOperations
    {
        public bool TypedDamagePostEventEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => engine.DamageObserverCursorQueriesEntries.TypedDamagePostEventEdge(parent, child);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCharacterYanPuOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCharacterYanPuOlOperations
    {
        public bool IsYanPuDyingExitEdge(int index) => engine.YanPuOlEntries.IsYanPuDyingExitEdge(index);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCharacterXiaoQiaoLeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCharacterXiaoQiaoLeOperations
    {
        public bool QiqinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.XiaoQiaoLeEntries.QiqinFirstChild(f, child);
        public bool QiqinObserverSuffix(int index) => engine.XiaoQiaoLeEntries.QiqinObserverSuffix(index);
        public bool WeiwanLifecycleEdge(int index) => engine.XiaoQiaoLeEntries.WeiwanLifecycleEdge(index);
        public bool WeiwanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.XiaoQiaoLeEntries.WeiwanFirstChild(f, child);
        public bool WeiwanObserverSuffix(int index) => engine.XiaoQiaoLeEntries.WeiwanObserverSuffix(index);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeCharacterSunHongOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeCharacterSunHongOlOperations
    {
        public bool ZenrunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.SunHongOlEntries.ZenrunFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterXiaoQiaoLeKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterXiaoQiaoLeKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }
}
