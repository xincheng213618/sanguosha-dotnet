#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPersistentCombatPoliciesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramGameFactionRangeTargets> _programGameFactionRangeTargets = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramGameFactionRangeTargets> ReferenceToProgramGameFactionRangeTargets => ref _programGameFactionRangeTargets;

    private global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.ProgramPlayDamageHandLimit> _programPlayDamageHandLimits = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.ProgramPlayDamageHandLimit> ReferenceToProgramPlayDamageHandLimits => ref _programPlayDamageHandLimits;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.ProgramPlayDamageHandLimit> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits => ref _programPlayDamageHandLimits;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramGameFactionRangeTargets> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programGameFactionRangeTargets => ref _programGameFactionRangeTargets;
}
