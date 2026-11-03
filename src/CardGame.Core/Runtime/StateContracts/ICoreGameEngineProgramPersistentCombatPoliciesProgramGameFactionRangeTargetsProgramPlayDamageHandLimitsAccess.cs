#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, GameEngine.ProgramPlayDamageHandLimit> _programPlayDamageHandLimits { get; }

}
