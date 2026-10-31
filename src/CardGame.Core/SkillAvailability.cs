using System.Collections.Frozen;
using System.Collections.ObjectModel;

namespace CardGame.Core;

/// <summary>Answers possession and effectiveness; invocation legality remains with its own window.</summary>
internal sealed class SkillAvailability(
    Func<string, ContentSkillDefinition> resolveDefinition, bool nationalWar, bool dualGenerals,
    Func<CharacterState, SkillGrant, bool>? effectiveQualification,
    Func<CharacterState, IReadOnlySet<string>>? suppressionInputs,
    Func<CharacterState, SkillGrant, bool>? additionalPrintedLordQualification)
{
    internal SkillAvailabilitySnapshot Query(CharacterState owner)
    {
        var owned = new List<SkillGrant>();
        var effective = new List<SkillGrant>();
        var definitions = new Dictionary<string, ContentSkillDefinition>(StringComparer.Ordinal);
        var restrictions = Restrictions(owner).ToArray();
        foreach (var grant in owner.SkillGrants.Grants)
        {
            if (!grant.IsEnabled || grant.SkillState is { Acquisition: false } || !CanOwnSource(owner, grant.SourceId)) continue;
            if (!definitions.TryGetValue(grant.SkillId, out var definition))
                definitions[grant.SkillId] = definition = resolveDefinition(grant.SkillId);
            if (!OwnsPrintedLord(owner, grant, definition)) continue;
            var state = InterpretRestrictions(grant, definition, restrictions);
            if (!state.Owned) continue;
            owned.Add(grant);
            if (state.Effective && IsSourceRevealed(owner, grant.SourceId) &&
                effectiveQualification?.Invoke(owner, grant) != false) effective.Add(grant);
        }
        var suppressors = suppressionInputs?.Invoke(owner) ?? effective.Where(grant =>
                definitions[grant.SkillId].SuppressionRule is { } rule && owner.Hp == rule.OwnerHpEquals)
            .Select(grant => grant.SkillId).ToHashSet(StringComparer.Ordinal);
        if (suppressors.Count > 0)
            effective.RemoveAll(grant => !grant.IsModeRule && !suppressors.Contains(grant.SkillId) &&
                !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal));
        var effectiveDefinitions = effective.Select(grant => grant.SkillId).Distinct(StringComparer.Ordinal)
            .ToDictionary(id => id, id => definitions[id], StringComparer.Ordinal);
        return new(Array.AsReadOnly(owned.ToArray()), Array.AsReadOnly(effective.ToArray()),
            new ReadOnlyDictionary<string, ContentSkillDefinition>(effectiveDefinitions));
    }

    private bool CanOwnSource(CharacterState owner, string source) => source switch
    {
        CharacterState.PrimarySkillSource => !nationalWar || owner.GeneralSelected,
        CharacterState.SecondarySkillSource => (nationalWar || dualGenerals) && owner.SecondaryGeneral is not null && owner.SecondaryGeneralSelected,
        _ => true
    };

    private bool IsSourceRevealed(CharacterState owner, string source) => source switch
    {
        CharacterState.PrimarySkillSource => !nationalWar || owner.GeneralRevealed,
        CharacterState.SecondarySkillSource => owner.SecondaryGeneralRevealed,
        _ => true
    };

    private bool OwnsPrintedLord(CharacterState owner, SkillGrant grant, ContentSkillDefinition skill) =>
        !SkillStateQuery.IsPrinted(grant) || !skill.Tags.HasFlag(SkillTag.Lord) || owner.Role == Role.Lord ||
        PrintedLordSkillQualifications.Matches(owner, grant) || additionalPrintedLordQualification?.Invoke(owner, grant) == true;

    internal static bool IsEffectiveUnderRestrictions(CharacterState owner, SkillGrant grant, ContentSkillDefinition skill) =>
        InterpretRestrictions(grant, skill, Restrictions(owner)).Effective;

    private static IEnumerable<SkillGrant> Restrictions(CharacterState owner) =>
        owner.SkillGrants.Grants.Where(grant => grant.SkillState is { Acquisition: false });

    private static (bool Owned, bool Effective) InterpretRestrictions(SkillGrant grant, ContentSkillDefinition skill,
        IEnumerable<SkillGrant> restrictions)
    {
        if (!grant.IsEnabled || grant.SkillState is { Acquisition: false }) return (false, false);
        var matching = restrictions.Where(restriction => Matches(restriction.SkillState!, grant, skill)).ToArray();
        return (!matching.Any(restriction => restriction.SkillState!.Action == SkillStateAction.Lose), matching.Length == 0);
    }

    internal static bool Matches(SkillStateLease state, SkillGrant grant, ContentSkillDefinition skill) =>
        !grant.IsModeRule && (!state.CharacterOnly || SkillStateQuery.IsCharacter(grant)) &&
        (state.TargetGrantId is null || state.TargetGrantId == grant.GrantId) &&
        (!state.PrintedOnly || SkillStateQuery.IsPrinted(grant)) &&
        (!state.ExcludeLocked && state.Scope != SkillStateScope.NonLocked || !skill.Tags.HasFlag(SkillTag.Locked)) &&
        (state.NamedSkill is null || state.NamedSkill == grant.SkillId);
}

internal sealed class SkillAvailabilitySnapshot(
    IReadOnlyList<SkillGrant> ownedGrants, IReadOnlyList<SkillGrant> effectiveGrants,
    IReadOnlyDictionary<string, ContentSkillDefinition> effectiveDefinitions)
{
    private readonly FrozenSet<(string SkillId, string SkillInstanceId)> _ownedInstances =
        ownedGrants.Select(grant => (grant.SkillId, grant.SkillInstanceId)).ToFrozenSet();
    private readonly FrozenSet<(string SkillId, string SkillInstanceId)> _effectiveInstances =
        effectiveGrants.Select(grant => (grant.SkillId, grant.SkillInstanceId)).ToFrozenSet();
    internal IReadOnlyList<SkillGrant> OwnedGrants { get; } = ownedGrants;
    internal IReadOnlyList<SkillGrant> EffectiveGrants { get; } = effectiveGrants;
    internal IReadOnlyDictionary<string, ContentSkillDefinition> EffectiveDefinitions { get; } = effectiveDefinitions;
    internal bool HasOwnedSkill(string skillId) => OwnedGrants.Any(grant => grant.SkillId == skillId);
    internal bool HasOwnedInstance(string skillId, string instanceId) => _ownedInstances.Contains((skillId, instanceId));
    internal bool HasEffectiveSkill(string skillId) => EffectiveDefinitions.ContainsKey(skillId);
    internal bool HasEffectiveInstance(string skillId, string instanceId) => _effectiveInstances.Contains((skillId, instanceId));
}
