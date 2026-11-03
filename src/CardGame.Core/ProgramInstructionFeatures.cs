using System.Collections.Frozen;

namespace CardGame.Core;

/// <summary>
/// Immutable definition-only facts. Operation buckets retain source order;
/// runtime predicates still evaluate against the current match, never a cache.
/// </summary>
internal sealed class ProgramInstructionFeatures
{
    private static readonly IReadOnlyList<SkillProgramEffect> Empty = Array.Empty<SkillProgramEffect>();
    private readonly FrozenDictionary<SkillProgramEffectOp, IReadOnlyList<SkillProgramEffect>> _operations;
    private readonly FrozenSet<SkillProgramTriggerConditionKind> _conditionKinds;
    private readonly FrozenSet<SkillProgramTriggerValueKind> _valueKinds;

    internal ProgramInstructionFeatures(IReadOnlyList<SkillProgramEffect> effects,
        SkillProgramTriggerCondition? triggerCondition = null)
    {
        TargetPayments = ProgramTargetPaymentPlans.Compile(effects);
        _operations = effects.GroupBy(effect => effect.Op).ToFrozenDictionary(
            group => group.Key, group => (IReadOnlyList<SkillProgramEffect>)Array.AsReadOnly(group.ToArray()));
        UsesConversionPolarity = effects.Any(effect => ProgramOperationCatalog.Default.Resolve(effect.Op).UsesConversionPolarity);
        FactRequirements = effects.Aggregate(default(ProgramFactRequirements), static (requirements, effect) =>
            requirements.Union(ProgramOperationCatalog.Default.FactRequirementsFor(effect.Op)));
        RequiresHandLimitContext = effects.Any(effect => effect.Condition.RequiresHandLimitContext ||
                effect.Options.Any(option => option.Condition.RequiresHandLimitContext)) ||
            effects.SelectMany(ExpressionRoots).Concat(triggerCondition is null ? [] : ExpressionRoots(triggerCondition))
                .Any(expression => expression.Reads("handLimit"));
        FirstInstruction = effects.FirstOrDefault();
        FirstNonTargetSelection = effects.SkipWhile(effect => effect.Op == SkillProgramEffectOp.SelectTarget).FirstOrDefault();
        InitialDiscardPayments = Array.AsReadOnly(effects
            .SkipWhile(effect => effect.Op == SkillProgramEffectOp.SelectTarget)
            .TakeWhile(effect => effect.Op == SkillProgramEffectOp.SelectAndMoveOwnedCard &&
                effect.Condition.Kind == SkillProgramConditionKind.Always &&
                effect.CardOwnerRef?.Kind == ProgramParticipantRef.Owner &&
                effect.Destination == SkillProgramCardDestination.DiscardPile).ToArray());
        InitialDiscardPaymentZones = Array.AsReadOnly(InitialDiscardPayments.SelectMany(effect => effect.Zones).Distinct().ToArray());
        InitialDiscardPaymentAmount = InitialDiscardPayments.Sum(effect => effect.Amount);
        AttributedEventOperations = Array.AsReadOnly(effects.Where(effect => effect.Op is
            SkillProgramEffectOp.ConsumeMarkerPreventDamage or SkillProgramEffectOp.AddMarkerSubjectNormalDraw).ToArray());
        AfterDamagePrerequisites = Array.AsReadOnly(effects.Where(effect =>
            effect.Op is (SkillProgramEffectOp.SelectTarget or SkillProgramEffectOp.SelectSourceCard or SkillProgramEffectOp.ClaimDamageCards) &&
            effect.Condition.CanEvaluateWithoutProgramFrame()).ToArray());
        ProhibitsEquipmentReplacement = ForOperation(SkillProgramEffectOp.SelectAndMoveOwnedCard)
            .Any(effect => effect.ProhibitReplacingEquipment);
        SelectsUnequalHandPair = ForOperation(SkillProgramEffectOp.SelectTargets)
            .Any(effect => effect.TargetKind == SkillProgramTargetKind.OtherLivingUnequalHandPair);
        UsesArrowBarrageSelection = ForOperation(SkillProgramEffectOp.UseSelectedCardsAs)
            .Any(effect => effect.OutputKind == CardKind.ArrowBarrage);
        HasEquipmentObserverInterruptOptIn = HasOperation(SkillProgramEffectOp.UseRandomDeckEquipment) ||
            ForOperation(SkillProgramEffectOp.SelectAndMoveOwnedCard).Any(effect =>
                effect.FreezeMovedCardSuit && effect.CardCategories is [SkillProgramCardCategory.Equipment]);
        Legality = effects.Select(effect => ProgramOperationCatalog.Default.Resolve(effect.Op).LegalityPolicy)
            .Aggregate(ProgramOperationLegalityPolicy.None, static (current, policy) => new(
                current.RequiresOwnerHand || policy.RequiresOwnerHand,
                current.RequiresTargetHand || policy.RequiresTargetHand,
                current.ExcludesOwnerAsTarget || policy.ExcludesOwnerAsTarget,
                current.RequiresPindianTarget || policy.RequiresPindianTarget));

        var conditionKinds = new HashSet<SkillProgramTriggerConditionKind>();
        var valueKinds = new HashSet<SkillProgramTriggerValueKind>();
        if (triggerCondition is not null) Capture(triggerCondition, conditionKinds, valueKinds);
        _conditionKinds = conditionKinds.ToFrozenSet();
        _valueKinds = valueKinds.ToFrozenSet();
    }

    internal ProgramTargetPaymentPlans TargetPayments { get; }
    internal bool UsesConversionPolarity { get; }
    internal ProgramFactRequirements FactRequirements { get; }
    internal bool RequiresHandLimitContext { get; }
    internal SkillProgramEffect? FirstInstruction { get; }
    internal SkillProgramEffect? FirstNonTargetSelection { get; }
    internal IReadOnlyList<SkillProgramEffect> InitialDiscardPayments { get; }
    internal IReadOnlyList<CardZoneKind> InitialDiscardPaymentZones { get; }
    internal int InitialDiscardPaymentAmount { get; }
    internal IReadOnlyList<SkillProgramEffect> AttributedEventOperations { get; }
    internal IReadOnlyList<SkillProgramEffect> AfterDamagePrerequisites { get; }
    internal bool ProhibitsEquipmentReplacement { get; }
    internal bool SelectsUnequalHandPair { get; }
    internal bool UsesArrowBarrageSelection { get; }
    internal bool HasEquipmentObserverInterruptOptIn { get; }
    internal ProgramOperationLegalityPolicy Legality { get; }
    internal IEnumerable<SkillProgramTriggerConditionKind> ConditionKinds => _conditionKinds;
    internal IEnumerable<SkillProgramTriggerValueKind> ValueKinds => _valueKinds;
    internal bool HasOperation(SkillProgramEffectOp op) => _operations.ContainsKey(op);
    internal bool UsesCondition(SkillProgramTriggerConditionKind kind) => _conditionKinds.Contains(kind);
    internal bool UsesValue(SkillProgramTriggerValueKind kind) => _valueKinds.Contains(kind);
    internal IReadOnlyList<SkillProgramEffect> ForOperation(SkillProgramEffectOp op) => _operations.GetValueOrDefault(op) ?? Empty;
    internal SkillProgramEffect? First(SkillProgramEffectOp op) => ForOperation(op).FirstOrDefault();
    internal SkillProgramEffect? Single(SkillProgramEffectOp op) => ForOperation(op).SingleOrDefault();

    internal static IEnumerable<ProgramExpression> ExpressionRoots(SkillProgram program) =>
        program.Triggers.SelectMany(trigger => ExpressionRoots(trigger.Condition).Concat(trigger.Effects.SelectMany(ExpressionRoots)))
            .Concat(program.Activations.SelectMany(activation => ExpressionRoots(activation.Condition).Concat(activation.Effects.SelectMany(ExpressionRoots))))
            .Concat(program.Modifiers.SelectMany(modifier => ExpressionRoots(modifier.Condition)))
            .Concat(program.ViewAs.SelectMany(rule => ExpressionRoots(rule.Condition)))
            .Concat(program.CardPolicies.SelectMany(policy => ExpressionRoots(policy.Condition)))
            .Concat(program.CardIdentities.SelectMany(identity => ExpressionRoots(identity.Condition)))
            .Concat(program.SkillGrants.Select(rule => rule.While))
            .Concat(program.SkillStates.SelectMany(declaration => new ProgramExpression?[]
                { declaration.State.Match.Filter, declaration.State.While, declaration.When }.OfType<ProgramExpression>()));

    private static IEnumerable<ProgramExpression> ExpressionRoots(SkillProgramEffect effect) =>
        ExpressionRoots(effect.Condition).Concat(effect.Options.SelectMany(option => ExpressionRoots(option.Condition)))
            .Concat(new ProgramExpression?[] { effect.AmountExpression, effect.CountExpression, effect.ValueExpression, effect.TargetSetExpression, effect.CardSetExpression,
                effect.CurrentResponseRule?.Rule.While, effect.SkillState?.Match.Filter, effect.SkillState?.While }.OfType<ProgramExpression>());

    private static IEnumerable<ProgramExpression> ExpressionRoots(SkillProgramTriggerCondition condition) =>
        (condition.Expression is { } expression ? new[] { expression } : [])
            .Concat(condition.Children.SelectMany(ExpressionRoots));

    private static IEnumerable<ProgramExpression> ExpressionRoots(SkillProgramCondition condition) =>
        (condition.Expression is { } expression ? new[] { expression } : [])
            .Concat(condition.Children.SelectMany(ExpressionRoots));

    private static void Capture(SkillProgramTriggerCondition condition,
        ISet<SkillProgramTriggerConditionKind> kinds, ISet<SkillProgramTriggerValueKind> values)
    {
        kinds.Add(condition.Kind);
        if (condition.Left is { } left) values.Add(left.Kind);
        if (condition.Right is { } right) values.Add(right.Kind);
        foreach (var child in condition.Children) Capture(child, kinds, values);
    }
}
