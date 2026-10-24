namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryLiuShanFangquan="boundary:fangquan-mobile",BoundaryLiuShanRuoyu="boundary:ruoyu-mobile";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection,54600)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,54600)]
    private bool QualifyBoundaryLiuShanRuoyu(ProgramTriggerLegalityContext gate)=>gate.SkillId!=BoundaryLiuShanRuoyu||
        HasSkillRoleQualification(gate.Owner,gate.SkillId,gate.Candidate.SkillInstanceId,Role.Lord);

    private bool HasBoundaryLiuShanMaximumHandLimit(CharacterState player)=>player.Seat==_turnProgression.OwnerSeat&&
        _programBooleanStates.Any(entry=>entry.Key.OwnerSeat==player.Seat&&entry.Key.SkillId==BoundaryLiuShanFangquan&&
            entry.Key.StateId=="fangquan-pending"&&entry.Value);

    private RuleQueryEvaluation ApplyBoundaryLiuShanHandLimit(CharacterState player,RuleQueryEvaluation ordinary)
    {
        if(!HasBoundaryLiuShanMaximumHandLimit(player))return ordinary;
        var maximum=Math.Max(0,player.MaxHp);
        var contribution=new FiniteRuleQueryContribution($"turn:{_turnNumber}:{player.Seat}:{BoundaryLiuShanFangquan}:maximum-hp",SkillRuleOperation.Set,maximum,54600);
        var terms=Array.AsReadOnly(ordinary.Value.Contributions.Append(contribution).OrderBy(c=>c.SourceId,StringComparer.Ordinal).ToArray());
        var stage=new RuleQueryStageSnapshot("fangquan-current-maximum-hp",ordinary.Value is FiniteRuleQueryValue finite?finite.Value:0,false,maximum,Array.AsReadOnly<RuleQueryContribution>([contribution]));
        return new(new FiniteRuleQueryValue(maximum,terms),ordinary.BaseTerms,Array.AsReadOnly(ordinary.Stages.Append(stage).ToArray()));
    }
}
