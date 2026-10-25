namespace CardGame.Core;

internal static class MobileFuHuangHouComposition
{
    internal static bool IsWoundedForeignTurnPipeline(IReadOnlyList<SkillProgramEffect> effects) =>
        effects is [
            { Op: SkillProgramEffectOp.SelectTarget, Target: SkillProgramEffectTarget.Owner, TargetKind: SkillProgramTargetKind.EventSource, Condition.Kind: SkillProgramConditionKind.Always },
            { Op: SkillProgramEffectOp.StartPindian, Target: SkillProgramEffectTarget.Owner, OpponentReference.Kind: ProgramParticipantRef.SelectedTarget,
                Visibility: SkillProgramCardSetVisibility.Public, ResultBind: "zhuikong-result", Condition.Kind: SkillProgramConditionKind.Always },
            { Op: SkillProgramEffectOp.GrantTurnCardTargetRestriction, Target: SkillProgramEffectTarget.SelectedTarget, TargetRestriction: SkillProgramCardTargetRestriction.SelfOnly,
                Condition.Kind: SkillProgramConditionKind.PindianWon, Condition.SourceBind: "zhuikong-result" },
            { Op: SkillProgramEffectOp.GrantDirectedTurnCardPolicy, Target: SkillProgramEffectTarget.Owner, ActorReference.Kind: ProgramParticipantRef.SelectedTarget,
                TargetReference.Kind: ProgramParticipantRef.Owner, CardKinds.Count: 0, DirectedPolicyEffects: DirectedTurnCardPolicyEffect.IgnoreDistance,
                Condition.Kind: SkillProgramConditionKind.PindianNotWon, Condition.SourceBind: "zhuikong-result" }
        ];
}

public sealed partial class GameEngine
{
    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 73910)]
    private RuleQueryEvaluation MobileFuHuangHouZhuikongDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat) return original;
        var grants = _directedTurnCardPolicies.Where(p => p.Source.SkillId == "mobile:zhuikong" && p.TurnNumber == _turnNumber &&
            p.TurnSeat == _currentSeat && p.ActorSeat == source.Seat && p.TargetSeat == target.Seat &&
            p.Effects == DirectedTurnCardPolicyEffect.IgnoreDistance).ToArray();
        if (grants.Length == 0) return original;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue),
            [new RuleQueryBaseTerm("distance:before-mobile-zhuikong", ConvertRuleValue(original))],
            grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-zhuikong:{g.ParentFrameId}:{g.EffectIndex}", SkillRuleOperation.Set, 1)).ToArray());
    }
}
