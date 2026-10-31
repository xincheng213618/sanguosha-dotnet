namespace CardGame.Core;

internal static class PostCostSlashRedirectContract
{
    internal static void Validate(string path, IReadOnlyList<SkillProgramEffect> effects, SkillProgramTriggerWindow? window)
    {
        if (!effects.Any(e => e.SlashRedirectPostCostRange is not null)) return;
        if (window != SkillProgramTriggerWindow.SlashTargetRedirecting || effects is not
            [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.SlashRedirectable } select,
             { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Target: SkillProgramEffectTarget.Owner,
               Amount: 1, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true } pay,
             { Op: SkillProgramEffectOp.RedirectCurrentAttack, Target: SkillProgramEffectTarget.SelectedTarget } redirect] ||
            select.Condition.Kind != SkillProgramConditionKind.Always || pay.Condition.Kind != SkillProgramConditionKind.Always ||
            redirect.Condition.Kind != SkillProgramConditionKind.Always || pay.ChooserRef?.Kind != ProgramParticipantRef.Owner ||
            pay.CardOwnerRef?.Kind != ProgramParticipantRef.Owner || pay.Zones.Count != 2 ||
            !pay.Zones.Contains(CardZoneKind.Hand) || !pay.Zones.Contains(CardZoneKind.Equipment) ||
            pay.SkipIfNoCards || pay.AllowDecline || pay.RevealBeforeMove || pay.ResultBind is not null ||
            pay.CoverageResultBind is not null || pay.CardKinds.Count != 0 || pay.CardCategories.Count != 0 || pay.Suits.Count != 0)
            throw new InvalidOperationException($"Invalid skill program at {path}: post-cost Slash redirect requires target selection, one awaited own HE discard and the original attack redirect.");
    }
}
