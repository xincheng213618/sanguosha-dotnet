namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Every actual finalized target owns a distinct nearby offer, even when the
    // same Slash also targets Cao Ang himself. The self branch remains draw-only.
    private static bool IsMobileCaoAngTargetTrigger(string skillId, SkillProgramTrigger trigger) =>
        skillId == "mobile:kangkai" && trigger.Id == "nearby-trigger" &&
        trigger.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized &&
        trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Observer;
}
