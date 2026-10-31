#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineScenarioPrintedLordQualificationsRuntime
{
    internal bool HasShanhePrintedLordQualifications => _coreMatchDefinitionOptionsAccess._options.ShanheBattle is { } setup && setup.AllyRoster.Concat(setup.EnemyRoster).Any(member => member.QualifiedPrintedLordSkillIds is { Count: > 0 });

    internal bool IsShanhePrintedLordGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not { } setup || owner.Seat <= 0 || owner.Seat > setup.AllyRoster.Count + setup.EnemyRoster.Count || grant.SourceId != CharacterState.PrimarySkillSource || grant.LordProjection is not null || grant.GeneralLibraryProjection is not null || grant.GrantId != $"{(CharacterState.PrimarySkillSource)}:{(grant.SkillId)}" || grant.SkillInstanceId != grant.GrantId)
            return false;
        var member = owner.Seat <= setup.AllyRoster.Count ? setup.AllyRoster[owner.Seat - 1] : setup.EnemyRoster[owner.Seat - setup.AllyRoster.Count - 1];
        return owner.General.Id == member.GeneralId && member.QualifiedPrintedLordSkillIds?.Contains(grant.SkillId, StringComparer.Ordinal) == true && owner.General.Skills.Any(skill => skill.ContentId == grant.SkillId && skill.Tags.HasFlag(SkillTag.Lord));
    }
}
