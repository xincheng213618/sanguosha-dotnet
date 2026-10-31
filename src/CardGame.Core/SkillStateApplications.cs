using System.Collections.ObjectModel;

namespace CardGame.Core;

internal static class SkillStateApplications
{
    internal static IReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>> Compile(ContentRegistry content) =>
        new ReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>>(content.Skills.Values
            .Where(skill => skill.Program?.SkillStates.Count > 0)
            .ToDictionary(skill => skill.Id, skill => skill.Program!.SkillStates, StringComparer.Ordinal));

    internal static void Apply(int ownerSeat, SkillGrant grant,
        IReadOnlyDictionary<string, IReadOnlyList<SkillStateDeclaration>> templates, IReadOnlyList<CharacterState> recipients)
    {
        if (!templates.TryGetValue(grant.SkillId, out var declarations)) return;
        var owner = recipients.FirstOrDefault(player => player.Seat == ownerSeat);
        if (!grant.IsEnabled || owner is null ||
            grant.SourceId == CharacterState.PrimarySkillSource && (!owner.GeneralSelected || !owner.GeneralRevealed) ||
            grant.SourceId == CharacterState.SecondarySkillSource && (!owner.SecondaryGeneralSelected || !owner.SecondaryGeneralRevealed))
            return;
        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];
            var source = new SkillStateDeclarationSource(grant.SkillId, grant.GrantId, index, declaration);
            foreach (var target in recipients)
            {
                if (declaration.Target == SkillStateTarget.Owner && target.Seat != ownerSeat) continue;
                var id = $"declared-skill-state:{ownerSeat}:{grant.GrantId.Length}:{grant.GrantId}:{index}:{target.Seat}";
                if (target.SkillGrants.Restrictions.Any(state => state.Id == id)) continue;
                target.SkillGrants.SetRestriction(new(id, id, declaration.State.Action, declaration.State.Match,
                    new(ownerSeat, target.Seat), new(null, []), declaration.State.While, source));
            }
        }
    }

    internal static IEnumerable<ContentSkillDefinition> AppliedDefinitions(CharacterSkillSet skills, ContentRegistry content) =>
        skills.Restrictions.Where(state => state.DeclarationSource is not null)
            .Select(state => state.DeclarationSource!.SkillId).Distinct(StringComparer.Ordinal).Select(content.GetSkill);
}
