namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_DamageTargetSkillRoundDraw
{
    internal static ProgramSkillGrantIdentity RoundSkillIdentity(SkillGrant grant) => new(grant.GrantId, grant.SkillId, grant.SkillInstanceId, grant.SourceId);

    internal static bool SameRoundSkillIdentity(SkillGrant grant, ProgramSkillGrantIdentity identity) => RuntimeRules_Core_DamageTargetSkillRoundDraw.RoundSkillIdentity(grant) == identity;
}
