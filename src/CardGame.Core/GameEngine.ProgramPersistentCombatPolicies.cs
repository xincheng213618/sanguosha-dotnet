namespace CardGame.Core;
public sealed record ProgramDamageHandLimitChangedEvent(int OwnerSeat, string SkillId, int TurnNumber, int PhaseInstanceId, int DamageCount) : IGameEvent;
public sealed record ProgramGameFactionAttackRangeTargetsGrantedEvent(int OwnerSeat, string SkillId, string FactionId, IReadOnlyList<int> TargetSeats) : IGameEvent;
public sealed partial class GameEngine
{
    internal sealed record ProgramPlayDamageHandLimit(int OwnerSeat, string SkillId, string SkillInstanceId, int TurnNumber, int PhaseInstanceId, int DamageCount);
    internal sealed record ProgramGameFactionRangeTargets(int OwnerSeat, string SkillId, string FactionId, IReadOnlyList<int> TargetSeats);
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.ProgramPlayDamageHandLimit> _programPlayDamageHandLimits => ref _programPersistentCombatPolicies.ReferenceToProgramPlayDamageHandLimits;
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramGameFactionRangeTargets> _programGameFactionRangeTargets => ref _programPersistentCombatPolicies.ReferenceToProgramGameFactionRangeTargets;
}
