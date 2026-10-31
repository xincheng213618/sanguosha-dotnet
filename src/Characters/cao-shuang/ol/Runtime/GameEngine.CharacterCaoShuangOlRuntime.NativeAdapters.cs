#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CharacterCaoShuangOlKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangOlKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangOlContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangOlContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangOlCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangOlCoreObserverSubtreesOperations
    {
        public bool HasMovementObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasMovementObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangOlResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangOlResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangOlCharacterCaoShuangOlAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangOlCharacterCaoShuangOlOperations
    {
        public bool CaoShuangFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => engine.CaoShuangOlEntries.CaoShuangFirstChild(root, child);
    }

    [ProgramCapability]
    private sealed class CharacterCaoShuangOlObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICharacterCaoShuangOlObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }
}
