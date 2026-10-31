#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNationalSkillsRuntime
{
    internal static int DualGeneralMaxHp(GeneralDefinition primary, GeneralDefinition secondary) => RuntimeSkills.RuntimeRules_Core_NationalSkills.DualGeneralMaxHp(primary, secondary);
    internal void InitializeDualGeneralHealth()
    {
        if (!_coreNativeDamageHandlesOperations.IsDualGeneralMode)
            return;
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            if (!player.GeneralSelected || !player.SecondaryGeneralSelected || player.SecondaryGeneral is null)
                throw new InvalidOperationException("Both generals must be selected before initializing dual-general health.");
            player.MaxHp = DualGeneralMaxHp(player.General, player.SecondaryGeneral) + (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.DualGenerals && player.Role == Role.Lord ? 1 : 0);
            player.Hp = player.MaxHp;
        }
    }

    internal bool CanOwnPrintedSkill(CharacterState player, GeneralDefinition general, GeneralSkillDefinition skill) => !skill.Tags.HasFlag(SkillTag.Lord) || player.Role == Role.Lord || player.SkillGrants.Grants.Any(g => g.SkillId == skill.ContentId && g.SourceId == (general.Id == player.General.Id ? CharacterState.PrimarySkillSource : CharacterState.SecondarySkillSource) && _corePrintedLordSkillQualificationsOperations.IsPrintedLordGrantQualified(player, g));
    internal IEnumerable<GeneralSkillDefinition> OwnedPrintedSkills(CharacterState player, GeneralDefinition general) => general.Skills.Where(skill => CanOwnPrintedSkill(player, general, skill) && IsEnabledTemplateSkill(player, general, skill.ContentId));
    internal bool IsEnabledTemplateSkill(CharacterState player, GeneralDefinition general, string? skillId)
    {
        if (skillId is null)
            return false;
        var source = general.Id == player.General.Id ? CharacterState.PrimarySkillSource : CharacterState.SecondarySkillSource;
        return player.SkillGrants.Grants.Any(grant => grant.SourceId == source && grant.SkillId == skillId && grant.IsEnabled && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(player, grant));
    }

    internal GeneralSkillDefinition ToRuntimeSkillDefinition(string skillId)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(skillId);
        return ToRuntimeSkillDefinition(skill);
    }

    internal static GeneralSkillDefinition ToRuntimeSkillDefinition(ContentSkillDefinition skill) => RuntimeSkills.RuntimeRules_Core_NationalSkills.ToRuntimeSkillDefinition(skill);
    internal IEnumerable<GeneralSkillDefinition> OwnedRuntimeSkills(CharacterState player, GeneralDefinition general, bool includeAcquired)
    {
        var emitted = new HashSet<string>(StringComparer.Ordinal);
        foreach (var skill in OwnedPrintedSkills(player, general))
        {
            if (skill.ContentId is null || emitted.Add(skill.ContentId))
                yield return skill;
        }

        if (!includeAcquired)
            yield break;
        foreach (var skillId in EnabledNonTemplateSkillGrants(player).Select(grant => grant.SkillId))
        {
            if (!emitted.Add(skillId))
                continue;
            yield return ToRuntimeSkillDefinition(skillId);
        }
    }

    internal IEnumerable<GeneralSkillDefinition> RuntimePassiveSkillDefinitions(CharacterState player, GeneralDefinition general)
    {
        var emitted = new HashSet<string>(StringComparer.Ordinal);
        foreach (var skill in general.Skills.Where(skill => CanOwnPrintedSkill(player, general, skill) && IsEnabledTemplateSkill(player, general, skill.ContentId)))
        {
            if (skill.ContentId is null || emitted.Add(skill.ContentId))
                yield return skill;
        }

        foreach (var skillId in EnabledNonTemplateSkillGrants(player).Select(grant => grant.SkillId))
        {
            if (!emitted.Add(skillId))
                continue;
            yield return ToRuntimeSkillDefinition(skillId);
        }
    }

    internal SkillBindingShard GetSkillBindingShard(CharacterState player) => _coreGameEngineActionQueriesActionQueryAccess._actionQuery is { } query ? query.Bindings[player.Seat] ??= _skillBindingIndexOperations._skillBindingIndex.GetShard(player) : _skillBindingIndexOperations._skillBindingIndex.GetShard(player);
    internal IReadOnlyList<string> EnabledPrintedContentSkillIds(CharacterState player)
    {
        return EnabledRuntimeSkillGrants(player).Where(grant => grant.SourceId is CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource).Select(grant => grant.SkillId).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    }

    internal IReadOnlyList<string> EnabledContentSkillIds(CharacterState player)
    {
        var ids = EnabledPrintedContentSkillIds(player).ToList();
        ids.AddRange(EnabledNonTemplateSkillGrants(player).Select(grant => grant.SkillId));
        return ids.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    }

    internal IEnumerable<SkillGrant> EnabledNonTemplateSkillGrants(CharacterState player) => EnabledRuntimeSkillGrants(player).Where(grant => grant.SourceId is not CharacterState.PrimarySkillSource and not CharacterState.SecondarySkillSource);
    internal IEnumerable<SkillGrant> EnabledRuntimeSkillGrants(CharacterState player) => GetSkillBindingShard(player).ActiveGrants;
    internal bool HasRuntimeSkillInstance(CharacterState player, string skillId, string skillInstanceId) => GetSkillBindingShard(player).HasInstance(skillId, skillInstanceId) || _characterCaoPiAnniversaryGodOperations.HasKemingBorrowedSkillInstance(player, skillId, skillInstanceId);
    internal string GetRuntimeSkillInstanceId(CharacterState player, string skillId) => _corePrintedLordSkillQualificationsOperations.PreferredQualifiedPrintedLordInstance(player, skillId) ?? EnabledRuntimeSkillGrants(player).Where(grant => grant.SkillId == skillId).OrderBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).Select(grant => grant.SkillInstanceId).FirstOrDefault() ?? _characterCaoPiAnniversaryGodOperations.KemingBorrowedSkillInstance(player, skillId) ?? throw new InvalidOperationException($"Player {(player.Seat)} does not own enabled skill '{(skillId)}'.");
    internal bool HasRuntimeSkill(CharacterState player, string skillId) => GetSkillBindingShard(player).HasSkill(skillId) || _characterCaoPiAnniversaryGodOperations.HasKemingBorrowedSkill(player, skillId);
    internal string EnabledSkillNames(CharacterState player)
    {
        var names = GetSkillBindingShard(player).Definitions.Values.Select(skill => skill.Name).Distinct().ToList();
        return names.Count == 0 ? "无" : string.Join(" / ", names);
    }
}
