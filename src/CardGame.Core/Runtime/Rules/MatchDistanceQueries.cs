namespace CardGame.Core;

// These ports exist for one synchronous query and retain its exact participants.
// Their stage methods only extend the query-owned arithmetic drafts.
internal interface IMatchDistanceQuery : IProgramSelectionQuery
{
    string ModeId { get; }
    int? FixedDistance { get; }
    int SourceSeat { get; }
    int TargetSeat { get; }
    int AliveSeatDistance();
    IReadOnlyList<Card> SourceEquipment { get; }
    IReadOnlyList<Card> TargetEquipment { get; }
    EquipmentDefinition EquipmentDefinition(CardKind kind);
    void AddBaseTerms(List<RuleQueryBaseTerm> terms);
    IEnumerable<RuleQueryContribution> OutgoingContributions { get; }
    IReadOnlyList<TurnRuleModifier> OutgoingTurnModifiers { get; }
    IEnumerable<RuleQueryContribution> IncomingContributions { get; }
    void AddIncomingContributions(List<RuleQueryContribution> contributions);
    RuleQueryEvaluation Project(RuleQueryEvaluation evaluation);
}

internal interface IMatchAttackRangeQuery : IProgramSelectionQuery
{
    string ModeId { get; }
    int Seat { get; }
    int TurnNumber { get; }
    IReadOnlyList<Card> EffectiveEquipment { get; }
    EquipmentDefinition EquipmentDefinition(CardKind kind);
    int WeaponAttackRange(Card weapon);
    void AddBaseTerms(int? excludedEquipmentId, EquipmentSlot? excludedEquipmentSlot, List<RuleQueryBaseTerm> terms);
    int? AssignedRange { get; }
    IEnumerable<RuleQueryContribution> Contributions { get; }
    int LibbingRangeBonus { get; }
    int YinXianRangeBonus { get; }
    bool FuanBaseZeroed { get; }
    void AddContributions(List<RuleQueryContribution> contributions);
    IReadOnlyList<TurnRuleModifier> TurnModifiers { get; }
    RuleQueryEvaluation Project(RuleQueryEvaluation evaluation);
}

/// <summary>Composes directional distance and attack range through their original ordered stages.</summary>
[ProgramRuntimeModule]
internal sealed class MatchDistanceQueries
{
    // Struct adapters preserve supplied participant identity without exposing a
    // CharacterState or allocating/boxing a new query proxy for every read.
    internal RuleQueryEvaluation CalculateDistance<TQuery>(TQuery query, int? excludedEquipmentId = null,
        EquipmentSlot? excludedEquipmentSlot = null) where TQuery : IMatchDistanceQuery
    {
        if (query.FixedDistance is { } fixedDistance)
            return RuleQueryService.EvaluateDirectionalDistance(
                [new($"mode:{query.ModeId}:no-distance", fixedDistance)], [], []);
        var baseTerms = new List<RuleQueryBaseTerm>
        {
            new($"mode:{query.ModeId}:alive-seat-distance", query.AliveSeatDistance())
        };
        baseTerms.AddRange(query.SourceEquipment.Where(card => card.Id != excludedEquipmentId &&
                (excludedEquipmentSlot is null || query.EquipmentDefinition(card.Kind).Slot != excludedEquipmentSlot))
            .Select(card => new RuleQueryBaseTerm($"equipment:{card.Id}:outgoing-distance",
                query.EquipmentDefinition(card.Kind).OutgoingDistanceModifier)));
        baseTerms.AddRange(query.TargetEquipment.Where(card => card.Id != excludedEquipmentId)
            .Select(card => new RuleQueryBaseTerm($"equipment:{card.Id}:incoming-distance",
                query.EquipmentDefinition(card.Kind).IncomingDistanceModifier)));

        query.AddBaseTerms(baseTerms);
        var outgoing = query.OutgoingContributions.ToList();
        outgoing.AddRange(query.OutgoingTurnModifiers.Select(item => new FiniteRuleQueryContribution(
            $"turn:{item.TurnNumber}:{item.Source.SkillId}:{item.Source.BindingId}:{item.GrantSequence}",
            SkillRuleOperation.Add, item.Amount)));
        var incoming = query.IncomingContributions.ToList();
        if (query.SourceSeat != query.TargetSeat) query.AddIncomingContributions(incoming);
        return query.Project(RuleQueryService.EvaluateDirectionalDistance(baseTerms, outgoing, incoming));
    }

    internal RuleQueryEvaluation CalculateAttackRange<TQuery>(TQuery query, int? excludedEquipmentId = null,
        EquipmentSlot? excludedEquipmentSlot = null) where TQuery : IMatchAttackRangeQuery
    {
        var baseTerms = new List<RuleQueryBaseTerm>();
        var minimumRange = 1;
        var weapon = query.EffectiveEquipment.Where(card => card.Id != excludedEquipmentId &&
                (excludedEquipmentSlot is null || query.EquipmentDefinition(card.Kind).Slot != excludedEquipmentSlot))
            .Where(card => query.EquipmentDefinition(card.Kind).Slot == EquipmentSlot.Weapon)
            .MaxBy(card => query.WeaponAttackRange(card));
        if (weapon is { Kind: CardKind.RedBloodBlade } && query.WeaponAttackRange(weapon) == 0)
            minimumRange = 0;
        baseTerms.Add(weapon is null
            ? new RuleQueryBaseTerm($"mode:{query.ModeId}:attack-range", 1)
            : new RuleQueryBaseTerm($"equipment:{weapon.Id}:printed-attack-range", query.WeaponAttackRange(weapon)));

        query.AddBaseTerms(excludedEquipmentId, excludedEquipmentSlot, baseTerms);
        if (query.AssignedRange is { } assignedRange)
            baseTerms.Add(new RuleQueryBaseTerm($"turn:{query.TurnNumber}:{query.Seat}:allocated-attack-range",
                Math.Max(0, assignedRange - baseTerms[0].Value)));

        var contributions = query.Contributions.ToList();
        RuntimeSkills.RuntimeRules_Core_RuleQueries.AddFiniteContribution(contributions,
            $"issued:{query.Seat}:mobile:shi-libing:attack-range", query.LibbingRangeBonus);
        RuntimeSkills.RuntimeRules_Core_RuleQueries.AddFiniteContribution(contributions,
            $"issued:{query.Seat}:mobile:shi-yinxian:range", query.YinXianRangeBonus);
        if (query.FuanBaseZeroed)
        {
            minimumRange = 0;
            baseTerms[0] = baseTerms[0] with { Value = 0 };
        }
        query.AddContributions(contributions);
        contributions.AddRange(query.TurnModifiers.Where(item => item.Operation == SkillRuleOperation.Unlimited)
            .Select(item => new UnlimitedRuleQueryContribution(
                $"turn:{item.TurnNumber}:{item.Source.SkillId}:{item.Source.BindingId}:{item.GrantSequence}")));
        return query.Project(RuleQueryService.Evaluate(SkillRuleQuery.AttackRange,
            new RuleQueryBounds(minimumRange, int.MaxValue), baseTerms, contributions));
    }
}
