namespace CardGame.Core;

internal sealed class SourceQiangzhiCommittedContract : IProgramCommittedUseContract
{
    public string SkillId => "shanhe:source-qiangzhi-484";

    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        if (trigger is not { Id: "draw-on-matching-category", Window: SkillProgramTriggerWindow.CardUseCommitted,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, IncludeResponseUses: true, Optional: true,
                SingleActionInstance: false, UsageScope: null, UsageLimit: null,
                Effects: [{ Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner,
                    Amount: 1, Condition.Kind: SkillProgramConditionKind.Always }] } ||
            trigger.CardKinds.Count != 0 || trigger.CardCategories.Count != 0 ||
            trigger.Condition is not { Kind: SkillProgramTriggerConditionKind.All, Children.Count: 3 } condition ||
            condition.Children[0].Kind != SkillProgramTriggerConditionKind.CardActionActorIsCurrentTurn ||
            condition.Children[1].Kind != SkillProgramTriggerConditionKind.CardActionPhaseIsPlay ||
            condition.Children[2] is not { Kind: SkillProgramTriggerConditionKind.Any, Children.Count: 3 } categories ||
            !Matches(categories.Children[0], "shown-basic", SkillProgramCardCategory.Basic) ||
            !Matches(categories.Children[1], "shown-trick", SkillProgramCardCategory.Trick) ||
            !Matches(categories.Children[2], "shown-equipment", SkillProgramCardCategory.Equipment))
            throw new InvalidOperationException("Source Qiangzhi requires its exact optional single draw for a matching actual Use in the owner's current Play phase.");
    }

    private static bool Matches(SkillProgramTriggerCondition condition, string stateId, SkillProgramCardCategory category) =>
        condition is { Kind: SkillProgramTriggerConditionKind.All, Children.Count: 2 } &&
        condition.Children[0] is { Kind: SkillProgramTriggerConditionKind.BooleanState, ExpectedValue: true } shown &&
        shown.StateId == stateId &&
        condition.Children[1].Kind == SkillProgramTriggerConditionKind.CardActionCategoryIs &&
        condition.Children[1].CardCategories.SequenceEqual([category]);
}
