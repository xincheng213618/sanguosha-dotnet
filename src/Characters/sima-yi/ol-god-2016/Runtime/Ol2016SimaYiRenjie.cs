namespace CardGame.Core;

internal static class Ol2016SimaYiRenjieComposition
{
    [ProgramCatalogValidation(40725)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "ol:renjie-2016") return;
        var discard = program.Triggers.SingleOrDefault(t => t.Id == "discard-ren");
        if (discard is not
            {
                Window: SkillProgramTriggerWindow.CardsMoved,
                Subject: SkillProgramTriggerSubject.Owner,
                MovementOccurrence: SkillProgramMovementOccurrence.PerCard,
                MovementDiscardOnly: true,
                Optional: false,
                MarkerCost: null
            } || !discard.SourceZones.SequenceEqual([CardZoneKind.Hand]) ||
            discard.MovementReasons.Count != 0 || discard.ExcludedMovementReasons.Count != 0 ||
            discard.Condition.Kind != SkillProgramTriggerConditionKind.Always ||
            discard.Effects is not
            [{ Op: SkillProgramEffectOp.ChangeAttributedMarker, Target: SkillProgramEffectTarget.Owner,
                Amount: 1, Marker: PlayerMarkerKind.Ren, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException(path + ": original Renjie requires each actual own hand discard in its frozen Discard phase.");
    }
}
