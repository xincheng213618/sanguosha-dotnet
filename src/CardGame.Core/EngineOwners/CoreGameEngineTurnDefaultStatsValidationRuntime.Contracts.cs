#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTurnDefaultStatsValidationContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreTurnDefaultStatsValidationCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreTurnDefaultStatsValidationCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
}

internal interface ICoreTurnDefaultStatsValidationCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreTurnDefaultStatsValidationCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreTurnDefaultStatsValidationCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreTurnDefaultStatsValidationCoreTurnDefaultStatsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TurnDefaultStatChoices(global::CardGame.Core.ProgramSkillFrame f);
    bool TracksTurnDefaultStats { get; }
}

internal interface ICoreTurnDefaultStatsValidationResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreTurnDefaultStatsValidationCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}
