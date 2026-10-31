#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPersistentCombatPoliciesEntries
{
    internal void AccumulateProgramPlayDamageHandLimit(int sourceSeat, int amount) => _resolve().AccumulateProgramPlayDamageHandLimit(sourceSeat, amount);
    internal void GrantProgramGameFactionAttackRangeTargets(ProgramSkillFrame frame, string factionId) => _resolve().GrantProgramGameFactionAttackRangeTargets(frame, factionId);
    internal bool IsGameFactionAttackRangeTarget(int actorSeat, int targetSeat) => _resolve().IsGameFactionAttackRangeTarget(actorSeat, targetSeat);
    internal IEnumerable<RuleQueryContribution> ProgramDamageHandLimitContributions(CharacterState owner) => _resolve().ProgramDamageHandLimitContributions(owner);
    internal void SetProgramTurnHandLimitFromPlayDamage(ProgramSkillFrame frame) => _resolve().SetProgramTurnHandLimitFromPlayDamage(frame);
}
