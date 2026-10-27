namespace CardGame.Core.Campaigns;

public static class ShanheYongzhanPrograms
{
    public const string SkillId = "shanhe:current-yongzhan-2174";
    public const string SecondSkillId = "shanhe:current-yongzhan-2175";
    public const string TriggerId = "survived-dying-enemies";

    public static int DamageAmount(string skillId) => skillId switch
    { SkillId => 1, SecondSkillId => 2, _ => 0 };

    public static bool IsTriggerShape(string skillId, string triggerId, SkillProgramTriggerWindow window,
        SkillProgramTriggerSubject? subject, bool optional, SkillProgramTriggerCondition condition,
        IReadOnlyList<SkillProgramEffect> effects) =>
        DamageAmount(skillId) is > 0 and var amount && triggerId == TriggerId &&
        window == SkillProgramTriggerWindow.DyingExited && subject == SkillProgramTriggerSubject.Owner &&
        !optional && condition.Kind == SkillProgramTriggerConditionKind.Always &&
        effects is
        [
            { Op: SkillProgramEffectOp.SelectAllPublicTeamTargets, Target: SkillProgramEffectTarget.Owner,
              TargetKind: SkillProgramTargetKind.PublicTeamEnemies, MinimumTargets: 0, MaximumTargets: 8,
              Condition.Kind: SkillProgramConditionKind.Always },
            { Op: SkillProgramEffectOp.DamageParticipants, Target: SkillProgramEffectTarget.SelectedTargets,
              TargetKind: null, Condition.Kind: SkillProgramConditionKind.Always, DamageNature: DamageNature.Normal } damage
        ] && damage.Amount == amount && damage.MinimumValue == amount;
}
