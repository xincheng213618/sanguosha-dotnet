namespace CardGame.Core;

internal static class BoundaryGuanPingComposition
{
    internal const string Jieyong = "boundary:jieyong";

    [ProgramCatalogValidation(55400)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != Jieyong) return;
        if (program.ViewAs is not [{ Id: "last-red-hand-slash", OutputKind: CardKind.Slash, ForPlay: true, ForResponse: true,
                InputCount: 1, AllowSameKind: true, UseEffectiveInputSuit: true } rule] ||
            !rule.SourceZones.SequenceEqual([CardZoneKind.Hand]) || !rule.InputSuits.SequenceEqual([Suit.Heart, Suit.Diamond]) ||
            rule.InputKinds.Count != 0 || rule.InputCategories.Count != 0 || rule.Condition.Kind != SkillProgramConditionKind.Always ||
            program.Activations.Count != 0 || program.Triggers.Count != 0)
            throw new InvalidOperationException(path + ": OL Jieyong requires the unique effective-red Hand entity's ordinary one-card Slash use or play.");
    }
}
