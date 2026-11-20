namespace CardGame.Core;

internal static class DeadGeneralPursuit
{
    internal static string[] PrintedNonLordSkills(CharacterState victim, IProgramContentQueries content) =>
        victim.General.Skills.Concat(victim.SecondaryGeneral?.Skills ?? [])
            .Select(skill => skill.ContentId)
            .OfType<string>()
            .Where(id => !content.SkillTags(id).HasFlag(SkillTag.Lord))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

    internal static bool CanPursue(CharacterState victim, IProgramContentQueries content) =>
        !victim.IsAlive && !victim.IsResting &&
        !victim.Markers.ContainsKey(PlayerMarkerKind.Pursued) && PrintedNonLordSkills(victim, content).Length > 0;
}
