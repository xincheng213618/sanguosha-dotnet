#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPersistentCombatPoliciesState : Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess
{
    private List<GameEngine.ProgramGameFactionRangeTargets> _programGameFactionRangeTargets = [];
    internal ref readonly List<GameEngine.ProgramGameFactionRangeTargets> ReferenceToProgramGameFactionRangeTargets => ref _programGameFactionRangeTargets;

    private Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> _programPlayDamageHandLimits = [];
    internal ref readonly Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> ReferenceToProgramPlayDamageHandLimits => ref _programPlayDamageHandLimits;

    ref readonly Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits => ref _programPlayDamageHandLimits;

    ref readonly List<GameEngine.ProgramGameFactionRangeTargets> Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programGameFactionRangeTargets => ref _programGameFactionRangeTargets;
}
