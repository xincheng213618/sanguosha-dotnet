namespace CardGame.Core;

internal static class BoundarySunQuanMobileComposition
{
    [ProgramCatalogValidation(59209)]
    private static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "boundary:jiuyuan-mobile") return;
        if (program.Activations.Count != 0 || program.Triggers.Count != 0 || program.CardPolicies is not [
                { Id: "redirect-self-peach", Kind: SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery, FactionId: "wu", OwnerRole: Role.Lord,
                    Value: 1, ProviderDrawCount: 1, DiscardCost: 0, CardKinds.Count: 0, RequiredCardKinds.Count: 0, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException(path + ": mobile Jiuyuan requires its native self-Peach recovery replacement with one recovery and one provider draw.");
    }
}
