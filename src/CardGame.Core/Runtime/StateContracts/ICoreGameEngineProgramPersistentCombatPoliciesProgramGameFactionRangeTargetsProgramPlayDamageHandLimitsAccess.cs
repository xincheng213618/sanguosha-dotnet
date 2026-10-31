#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.ProgramPlayDamageHandLimit> _programPlayDamageHandLimits { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramGameFactionRangeTargets> _programGameFactionRangeTargets { get; }
}
