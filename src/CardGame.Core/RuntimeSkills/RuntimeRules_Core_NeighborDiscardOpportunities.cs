namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_NeighborDiscardOpportunities
{
    internal static bool IsNeighborDiscardTopTrigger(SkillProgramTrigger trigger) =>
        trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.PutOwnOrPreviousFirstDiscardOnTop);
}
