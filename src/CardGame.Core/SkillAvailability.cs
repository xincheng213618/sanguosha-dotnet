using System.Collections.Frozen;
using System.Collections.ObjectModel;

namespace CardGame.Core;

/// <summary>Possession and effectiveness have one answer; invocation belongs to its window.</summary>
internal sealed class SkillAvailability(
    Func<string, ContentSkillDefinition> resolveDefinition, bool nationalWar, bool dualGenerals,
    Func<CharacterState, SkillGrant, bool>? effectiveQualification,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess? participants = null, CardZoneStore? cards = null,
    IActualDiscardRecoveryPhaseQueries? phases = null, IProgramRuntimeFrames? frames = null,
    RuntimeEntries.CoreGameEngineHealthQueriesEntries? health = null,
    EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess? turnState = null,
    IProgramHealthChildQueries? healthChildren = null, ICardEntityMarkQueries? cardMarks = null,
    IReadOnlySet<string>? allowedEffectiveSkillIds = null, ISelectionWoundedQueries? wounded = null,
    RuntimeEntries.CoreGameEnginePublicPileSourcesEntries? piles = null)
{
    private readonly IReadOnlySet<string>? _allowedEffectiveSkillIds =
        allowedEffectiveSkillIds?.ToFrozenSet(StringComparer.Ordinal);
    private SourceEvaluation? _evaluation;

    internal SkillAvailabilitySnapshot Query(CharacterState owner) => Evaluate(evaluation => Query(owner, evaluation));

    private SkillAvailabilitySnapshot Query(CharacterState owner, SourceEvaluation evaluation)
    {
        var owned = new List<SkillGrant>();
        var effective = new List<SkillGrant>();
        foreach (var grant in owner.SkillGrants.Grants)
        {
            if (!ReadState(owner, grant, evaluation).Owned) continue;
            owned.Add(grant);
            if (IsEffective(owner, grant, evaluation)) effective.Add(grant);
        }
        var effectiveDefinitions = effective.Select(grant => grant.SkillId).Distinct(StringComparer.Ordinal)
            .ToDictionary(id => id, id => Definition(id, evaluation), StringComparer.Ordinal);
        return new(Array.AsReadOnly(owned.ToArray()), Array.AsReadOnly(effective.ToArray()),
            new ReadOnlyDictionary<string, ContentSkillDefinition>(effectiveDefinitions));
    }

    internal bool HasConditionalSources(CharacterState owner) => owner.SkillGrants.Grants.Any(grant =>
        grant.ConditionalSource is not null || resolveDefinition(grant.SkillId).Program?.SkillGrants.Count > 0);

    internal bool IsEffectiveSource(CharacterState owner, SkillGrant grant) =>
        owner.IsAlive && Evaluate(evaluation => IsEffective(owner, grant, evaluation));

    internal bool ConditionalRuleApplies(CharacterState owner, ConditionalSkillGrantRule rule) =>
        Evaluate(_ => ProgramExpressions.EvaluateBool(rule.While,
            CreateConditionWorld(new(owner.Seat, owner.Seat), owner)) == true);

    internal int? EquipmentSuitCount(int seat) => Evaluate(evaluation =>
    {
        var owner = participants?._players.FirstOrDefault(player => player.Seat == seat);
        if (owner is null || cards is null) return (int?)null;
        if (owner.IdentityConcealed) return 0;
        if (evaluation.EquipmentSuits.TryGetValue(seat, out var count)) return count;
        // The suit read may use independently qualified roots and lower-tier
        // grants, never a grant whose own condition needs this same suit read.
        var policies = owner.SkillGrants.Grants.Where(grant =>
                Definition(grant.SkillId, evaluation).Program?.CardPolicies.Any(policy =>
                    policy.Kind == SkillProgramCardPolicyKind.RewriteSuit) == true &&
                !DependsOnEquipmentSuits(owner, grant) && IsEffective(owner, grant, evaluation))
            .GroupBy(grant => (grant.SkillId, grant.SkillInstanceId))
            .OrderBy(group => group.Key.SkillId, StringComparer.Ordinal)
            .ThenBy(group => group.Key.SkillInstanceId, StringComparer.Ordinal)
            .SelectMany(group => Definition(group.Key.SkillId, evaluation).Program!.CardPolicies
                .Where(policy => policy.Kind == SkillProgramCardPolicyKind.RewriteSuit)).ToArray();
        count = CardSuitRewrites.CountEquipmentSuits(cards.CardsAt(CardLocation.Equipment(seat)), policies);
        evaluation.EquipmentSuits.Add(seat, count);
        return count;
    });

    private bool DependsOnEquipmentSuits(CharacterState owner, SkillGrant grant)
    {
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (grant.ConditionalSource is { } source)
        {
            if (!visited.Add(grant.GrantId)) return true;
            var parent = FindParent(owner, source);
            if (parent is null) return false;
            var rule = resolveDefinition(parent.SkillId).Program?.SkillGrants.FirstOrDefault(rule => rule.Id == source.RuleId);
            if (rule?.While.Reads("equipmentSuitCount") == true) return true;
            grant = parent;
        }
        return false;
    }

    private bool IsEffective(CharacterState owner, SkillGrant grant, SourceEvaluation evaluation)
    {
        var key = (owner.Seat, grant.GrantId);
        if (evaluation.Effective.TryGetValue(key, out var effective)) return effective;
        if (!evaluation.Visiting.Add(key)) return false;
        try
        {
            effective = ReadState(owner, grant, evaluation).Effective &&
                effectiveQualification?.Invoke(owner, grant) != false && ConditionalSourceQualifies(owner, grant, evaluation);
            evaluation.Effective.Add(key, effective);
            return effective;
        }
        finally { evaluation.Visiting.Remove(key); }
    }

    private bool ConditionalSourceQualifies(CharacterState owner, SkillGrant grant, SourceEvaluation evaluation)
    {
        if (grant.ConditionalSource is not { } source) return true;
        if (!owner.IsAlive || FindParent(owner, source) is not { } parent || !IsEffective(owner, parent, evaluation)) return false;
        return Definition(parent.SkillId, evaluation).Program?.SkillGrants.Any(rule =>
            rule.Id == source.RuleId && rule.SkillId == grant.SkillId && ConditionalRuleApplies(owner, rule)) == true;
    }

    private static SkillGrant? FindParent(CharacterState owner, ConditionalSkillGrantSource source) =>
        owner.SkillGrants.Grants.FirstOrDefault(grant => grant.GrantId == source.CapabilityGrantId &&
            grant.SkillId == source.CapabilitySkillId && grant.SkillInstanceId == source.CapabilitySkillInstanceId);

    private (bool Owned, bool Effective) ReadState(CharacterState owner, SkillGrant grant, SourceEvaluation evaluation)
    {
        var raw = RawSourceAdmission(owner, grant);
        if (!raw.Owned) return (false, false);
        var definition = Definition(grant.SkillId, evaluation);
        if (!OwnsPrintedLord(owner, grant, definition)) return (false, false);
        var state = InterpretRestrictions(owner, grant, definition, ActiveRestrictions(owner));
        return (state.Owned, state.Effective && raw.Effective);
    }

    private ContentSkillDefinition Definition(string id, SourceEvaluation evaluation)
    {
        if (!evaluation.Definitions.TryGetValue(id, out var definition))
            evaluation.Definitions.Add(id, definition = resolveDefinition(id));
        return definition;
    }

    private T Evaluate<T>(Func<SourceEvaluation, T> read)
    {
        if (_evaluation is { } current) return read(current);
        _evaluation = new SourceEvaluation();
        try { return read(_evaluation); }
        finally { _evaluation = null; }
    }

    // One synchronous read shares its exact-source traversal, including calls
    // through rule-wounded facts. This is neither stored state nor a frame.
    private sealed class SourceEvaluation
    {
        internal Dictionary<string, ContentSkillDefinition> Definitions { get; } = new(StringComparer.Ordinal);
        internal Dictionary<(int Seat, string Grant), bool> Effective { get; } = [];
        internal HashSet<(int Seat, string Grant)> Visiting { get; } = [];
        internal Dictionary<int, int> EquipmentSuits { get; } = [];
    }

    /// <summary>Raw origin admission, including visibility and immutable mode content policy. Printed-lord rights and derived relationships have their own stages.</summary>
    internal bool IsRawSourceAdmitted(CharacterState owner, SkillGrant grant) => RawSourceAdmission(owner, grant).Effective;

    private (bool Owned, bool Effective) RawSourceAdmission(CharacterState owner, SkillGrant grant)
    {
        var owned = grant.IsEnabled && CanOwnSource(owner, grant.SourceId);
        return (owned, owned && IsSourceRevealed(owner, grant.SourceId) &&
            (grant.IsModeRule || !owner.IdentityConcealed) &&
            (_allowedEffectiveSkillIds is null || _allowedEffectiveSkillIds.Contains(grant.SkillId)));
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
        PrintedLordSkillQualifications.Matches(owner, grant);

    internal bool IsGrantEffectiveUnderStates(CharacterState owner, SkillGrant grant) =>
        InterpretRestrictions(owner, grant, resolveDefinition(grant.SkillId),
            ActiveRestrictions(owner)).Effective;

    internal SkillStateConditionWorld CreateConditionWorld(SkillStateConditionContext context, CharacterState owner) =>
        new(context, owner, participants, cards, phases, frames, health, turnState, healthChildren, cardMarks,
            wounded, piles, EquipmentSuitCount);

    private IEnumerable<SkillStateRestriction> ActiveRestrictions(CharacterState owner) =>
        owner.SkillGrants.Restrictions.Where(restriction => restriction.While is null ||
            ProgramExpressions.EvaluateBool(restriction.While,
                CreateConditionWorld(restriction.Context, owner)) == true);

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
