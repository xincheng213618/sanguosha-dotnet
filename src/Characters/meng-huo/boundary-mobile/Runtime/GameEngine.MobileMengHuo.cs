namespace CardGame.Core;

internal static class MobileMengHuoComposition
{
    [ProgramCatalogValidation(66109)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "boundary:zaiqi-mobile") return;
        if (program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.CardPolicies.Count != 0 || program.Modifiers.Count != 0 ||
            program.Triggers is not [{ Id: "red-discard-phase-end", Window: SkillProgramTriggerWindow.DiscardPhaseEnded,
                Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, AllowOwnDiscardPhaseEnded: true,
                Optional: false, Effects: [{ Op: SkillProgramEffectOp.OfferRedDiscardRecoveryChoice, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path + ": mobile Zaiqi requires its actual own discard-phase end and one shared red-card-budget choice.");
    }
}
