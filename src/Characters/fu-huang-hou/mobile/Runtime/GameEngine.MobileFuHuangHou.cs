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
