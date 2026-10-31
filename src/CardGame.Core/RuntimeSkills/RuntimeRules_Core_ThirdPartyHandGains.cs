namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ThirdPartyHandGains
{
    internal static bool IsThirdPartyHandGainTrigger(SkillProgramTrigger trigger) =>
        trigger.Window == SkillProgramTriggerWindow.CardsGained &&
        trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerThirdPartyHandGain;

    internal static bool IsOwnerSourceHandGainTrigger(SkillProgramTrigger trigger) =>
        trigger.MovementOccurrence == SkillProgramMovementOccurrence.PerOwnerSourceHandGain;
}
