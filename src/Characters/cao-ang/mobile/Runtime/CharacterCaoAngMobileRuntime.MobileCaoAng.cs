#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoAngMobileRuntime
{
    internal static bool IsMobileCaoAngTargetTrigger(string skillId, SkillProgramTrigger trigger) => skillId == "mobile:kangkai" && trigger.Id == "nearby-trigger" && trigger.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Observer;
}
