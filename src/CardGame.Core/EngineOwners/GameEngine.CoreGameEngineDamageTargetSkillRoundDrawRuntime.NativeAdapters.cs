#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreDamageTargetSkillRoundDrawContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageTargetSkillRoundDrawContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreDamageTargetSkillRoundDrawResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageTargetSkillRoundDrawResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreDamageTargetSkillRoundDrawKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageTargetSkillRoundDrawKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreDamageTargetSkillRoundDrawObservedSkillGrantRevisionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreDamageTargetSkillRoundDrawObservedSkillGrantRevisionsOperations
    {
        public ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }
}
