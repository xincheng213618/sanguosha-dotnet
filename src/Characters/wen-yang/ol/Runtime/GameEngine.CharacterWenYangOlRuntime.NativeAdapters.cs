#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterWenYangOlRandomAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlRandomOperations
    {
        public ref readonly global::CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlCharacterWenYangOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlCharacterWenYangOlOperations
    {
        public bool WenYangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => engine.WenYangOlEntries.WenYangFirstChild(f, child);
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlCoreObserverSubtreesOperations2
    {
        public bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix) => engine.ObserverSubtreesEntries.HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
        public bool IsMovementObserverEdge(int child) => engine.ObserverSubtreesEntries.IsMovementObserverEdge(child);
        public bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying) => engine.ObserverSubtreesEntries.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlResolutionStackAdapter2(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlResolutionStackOperations2
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlKernelCoreGameEngineAdvancedLifecycleHostAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlKernelCoreGameEngineAdvancedLifecycleHostOperations
    {
        public global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlCoreRecoveryReplacementDamageRidesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlCoreRecoveryReplacementDamageRidesOperations
    {
        public bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => engine.RecoveryReplacementDamageRidesEntries.RecoveryReplacementFrameRidesOn(ride, beneath);
    }

    [ProgramCapability]
    private sealed class CharacterWenYangOlObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterWenYangOlObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }
}
