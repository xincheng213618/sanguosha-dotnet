namespace CardGame.Core;

[ProgramPassiveFactRequirements("shanhe:black-light-armor-6011", ProgramTrackedFact.NativeActionAppearance)]
internal static class BlackLightArmorContract
{
    internal const string Skill = "shanhe:black-light-armor-6011";

    [ProgramCatalogValidation(62051)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != Skill) return;
        if (program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.Modifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 || program.Contributions.Count != 0 ||
            program.Triggers is not [{ Id: "multi-target-immunity", Window: SkillProgramTriggerWindow.CardUseTargetsFinalized,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Target, Optional: false,
                Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.NullifyCurrentCardEffect, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always }] } trigger] ||
            trigger.CardKinds.Count != 0 || trigger.Suits.Count != 0 || trigger.UsageScope is not null ||
            trigger.UsageLimit is not null || trigger.DynamicUsageLimit is not null || trigger.ChoiceGroup is not null ||
            trigger.SourceSkillId is not null || trigger.SourceViewAsId is not null ||
            trigger.CardCategories.Count != 2 || !trigger.CardCategories.Contains(SkillProgramCardCategory.Basic) ||
            !trigger.CardCategories.Contains(SkillProgramCardCategory.Trick))
            throw new InvalidOperationException(path + ": Black Light Armor requires its single locked finalized-target native effect nullification.");
    }
}
