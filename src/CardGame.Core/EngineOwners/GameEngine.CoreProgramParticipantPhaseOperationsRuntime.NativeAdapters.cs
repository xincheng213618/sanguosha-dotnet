#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramParticipantPhaseOperationsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramParticipantPhaseOperationsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreProgramParticipantPhaseOperationsSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramParticipantPhaseOperationsSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreProgramParticipantPhaseOperationsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramParticipantPhaseOperationsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreProgramParticipantPhaseOperationsTurnCardUseEffectsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramParticipantPhaseOperationsTurnCardUseEffectsOperations
    {
        public ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class CoreProgramParticipantPhaseOperationsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramParticipantPhaseOperationsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }
}
