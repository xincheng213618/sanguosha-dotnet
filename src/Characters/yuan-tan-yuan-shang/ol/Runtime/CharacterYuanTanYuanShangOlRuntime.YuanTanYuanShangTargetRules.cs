#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanTanYuanShangOlRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.TargetCountContributions, 10)]
    internal RuleQueryContribution? NeifaTargetCountContribution(ProgramTargetCountQuery query) => NeifaSlashTargetBonus(query.Actor.Seat, query.Kind) != 0 ? new FiniteRuleQueryContribution($"turn:{(query.Actor.Seat)}:neifa:slash-target", SkillRuleOperation.Add, 1) : null;
}
