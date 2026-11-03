#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineProgramPersistentCombatPoliciesState : Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess
{

    private Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> _programPlayDamageHandLimits = [];
    internal ref readonly Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> ReferenceToProgramPlayDamageHandLimits => ref _programPlayDamageHandLimits;

    ref readonly Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits => ref _programPlayDamageHandLimits;

}
