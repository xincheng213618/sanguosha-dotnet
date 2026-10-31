using System.Collections.Frozen;
using System.Collections.ObjectModel;

namespace CardGame.Core;

/// <summary>Possession and effectiveness have one answer; invocation belongs to its window.</summary>
internal sealed class SkillAvailability(
    Func<string, ContentSkillDefinition> resolveDefinition, bool nationalWar, bool dualGenerals,
    Func<CharacterState, SkillGrant, bool>? effectiveQualification,
    Func<CharacterState, SkillGrant, bool>? additionalPrintedLordQualification,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess? participants = null, CardZoneStore? cards = null,
    IActualDiscardRecoveryPhaseQueries? phases = null, IProgramRuntimeFrames? frames = null,
    RuntimeEntries.CoreGameEngineHealthQueriesEntries? health = null)
{
    internal SkillAvailabilitySnapshot Query(CharacterState owner)
    {
        var owned = new List<SkillGrant>();
        var effective = new List<SkillGrant>();
        var definitions = new Dictionary<string, ContentSkillDefinition>(StringComparer.Ordinal);
        var restrictions = ActiveRestrictions(owner, participants, cards, phases, frames, health).ToArray();
        foreach (var grant in owner.SkillGrants.Grants)
        {
            if (!grant.IsEnabled || !CanOwnSource(owner, grant.SourceId)) continue;
            if (!definitions.TryGetValue(grant.SkillId, out var definition))
                definitions[grant.SkillId] = definition = resolveDefinition(grant.SkillId);
            if (!OwnsPrintedLord(owner, grant, definition)) continue;
            var state = InterpretRestrictions(owner, grant, definition, restrictions);
            if (!state.Owned) continue;
            owned.Add(grant);
            if (state.Effective && IsSourceRevealed(owner, grant.SourceId) &&
                effectiveQualification?.Invoke(owner, grant) != false) effective.Add(grant);
        }
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
        !SkillStateMatcher.IsPrinted(grant) || !skill.Tags.HasFlag(SkillTag.Lord) || owner.Role == Role.Lord ||
        PrintedLordSkillQualifications.Matches(owner, grant) || additionalPrintedLordQualification?.Invoke(owner, grant) == true;

    internal bool IsGrantEffectiveUnderStates(CharacterState owner, SkillGrant grant) =>
        InterpretRestrictions(owner, grant, resolveDefinition(grant.SkillId),
            ActiveRestrictions(owner, participants, cards, phases, frames, health)).Effective;

    private static IEnumerable<SkillStateRestriction> ActiveRestrictions(CharacterState owner,
        EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess? participants, CardZoneStore? cards,
        IActualDiscardRecoveryPhaseQueries? phases, IProgramRuntimeFrames? frames,
        RuntimeEntries.CoreGameEngineHealthQueriesEntries? health) =>
        owner.SkillGrants.Restrictions.Where(restriction => restriction.While is null ||
            ProgramExpressions.EvaluateBool(restriction.While,
                new SkillStateConditionWorld(restriction.Context, owner, participants, cards, phases, frames, health)) == true);

    private static (bool Owned, bool Effective) InterpretRestrictions(CharacterState owner, SkillGrant grant,
        ContentSkillDefinition skill, IEnumerable<SkillStateRestriction> restrictions)
    {
        if (!grant.IsEnabled) return (false, false);
        var matching = restrictions.Where(restriction => SkillStateMatcher.Matches(restriction.Match,
            grant, owner, skill, restriction.Lifetime.Origin?.Source.SkillId, sourceSkillIds: EquivalentStateSources(restriction))).ToArray();
        return (!matching.Any(restriction => restriction.Action == SkillStateAction.Lose), matching.Length == 0);

        IReadOnlySet<string>? EquivalentStateSources(SkillStateRestriction restriction) =>
            restriction.DeclarationSource is not { } source ? null : owner.SkillGrants.Restrictions
                .Where(state => state.DeclarationSource is { } other && source.Declaration.StructurallyEquals(other.Declaration))
                .Select(state => state.DeclarationSource!.SkillId).ToHashSet(StringComparer.Ordinal);
    }
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
