namespace CardGame.Core;

internal sealed partial class MatchModeRuntime
{
    private readonly ModePorts _ports;

    private sealed class ModePorts(object[] features)
    {
        internal IMatchModeLifecycle[] Lifecycle { get; } = Ordered<IMatchModeLifecycle>(features, feature => feature.Order);
        internal IMatchModeNumericRules[] Numeric { get; } = Ordered<IMatchModeNumericRules>(features, feature => feature.Order);
        internal IMatchModeDistanceRules[] Distance { get; } = Ordered<IMatchModeDistanceRules>(features, feature => feature.Order);
        internal IMatchModeEquipmentPolicy[] Equipment { get; } = Ordered<IMatchModeEquipmentPolicy>(features, feature => feature.Order);
        internal IMatchModeSnapshotProjector[] Projection { get; } = Ordered<IMatchModeSnapshotProjector>(features, feature => feature.Order);
        internal IMatchModeSkillVisibility[] Visibility { get; } = features.OfType<IMatchModeSkillVisibility>().ToArray();
        internal IMatchModeCardActionObserver[] CardActions { get; } = Ordered<IMatchModeCardActionObserver>(features, feature => feature.Order);
        internal IMatchModeInitialDeal[] InitialDeal { get; } = Ordered<IMatchModeInitialDeal>(features, feature => feature.Order);

        private static T[] Ordered<T>(object[] source, Func<T, int> order)
        {
            var result = source.OfType<T>().OrderBy(order).ToArray();
            if (result.Any(feature => order(feature) < 0) || result.GroupBy(order).Any(group => group.Count() > 1))
                throw new InvalidOperationException($"Mode port '{typeof(T).Name}' requires unique nonnegative orders.");
            return result;
        }
    }

    internal void OnMatchStart(ModeMatchStartPhase phase)
    { foreach (var feature in _ports.Lifecycle) feature.OnMatchStart(phase); }

    internal bool TryCompleteMatch(ModeCompletionBoundary boundary)
    { foreach (var feature in _ports.Lifecycle) if (feature.TryCompleteMatch(boundary)) return true; return false; }

    internal bool TryHandleEmptyDrawPile()
    { foreach (var feature in _ports.Lifecycle) if (feature.TryHandleEmptyDrawPile()) return true; return false; }

    internal bool TryFinishAtTurnLimit(int maximumTurns)
    { foreach (var feature in _ports.Lifecycle) if (feature.TryFinishAtTurnLimit(maximumTurns)) return true; return false; }

    internal void ApplyDeathReward(int victimSeat, int? killerSeat)
    { foreach (var feature in _ports.Lifecycle) feature.ApplyDeathReward(victimSeat, killerSeat); }

    internal void RecordDeathRank(long deathFrameId, int seat) =>
        (Find<IMatchModeDeathRanking>() ?? throw new InvalidOperationException("The ranking mode requires its installed death-ranking feature."))
            .RecordDeathRank(deathFrameId, seat);

    internal IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    { foreach (var feature in _ports.Numeric) foreach (var contribution in feature.NumericContributions(seat, query)) yield return contribution; }

    internal RuleQueryEvaluation AdjustNumericEvaluation(int seat, RuleQueryEvaluation ordinary, SkillRuleQuery query)
    { foreach (var feature in _ports.Numeric) ordinary = feature.AdjustNumericEvaluation(seat, ordinary, query); return ordinary; }

    internal IEnumerable<RuleQueryBaseTerm> BaseDistanceTerms(int sourceSeat, int targetSeat)
    { foreach (var feature in _ports.Distance) foreach (var term in feature.BaseDistanceTerms(sourceSeat, targetSeat)) yield return term; }

    internal int EquipmentSlashLimitBonus(CardKind kind, int ordinary)
    { foreach (var feature in _ports.Equipment) if (feature.SlashLimitBonus(kind) is { } value) return value; return ordinary; }

    internal bool IsSkillVisible(int seat, string? skillId, int viewerSeat, bool revealAll) =>
        _ports.Visibility.All(feature => feature.IsSkillVisible(seat, skillId, viewerSeat, revealAll));

    internal PlayerSnapshot ProjectPlayer(PlayerSnapshot player, int viewerSeat, bool revealAll)
    { foreach (var feature in _ports.Projection) player = feature.ProjectPlayer(player, viewerSeat, revealAll); return player; }

    internal GameSnapshot ProjectGame(GameSnapshot game, int viewerSeat, bool revealAll)
    { foreach (var feature in _ports.Projection) game = feature.ProjectGame(game, viewerSeat, revealAll); return game; }

    internal void OnCardAction(CardActionContext action, SkillProgramTriggerWindow window)
    { foreach (var feature in _ports.CardActions) feature.OnCardAction(action, window); }

    internal void BeforeInitialDeal()
    { foreach (var feature in _ports.InitialDeal) feature.BeforeInitialDeal(); }

    internal bool TryDealInitialCard(int seat, int index)
    { foreach (var feature in _ports.InitialDeal) if (feature.TryDealInitialCard(seat, index)) return true; return false; }

    internal void AfterInitialDeal()
    { foreach (var feature in _ports.InitialDeal) feature.AfterInitialDeal(); }

    internal void EnsureRequiredFeatures(IEnumerable<Type> contracts)
    {
        foreach (var contract in contracts)
            if (_features.Values.Count(contract.IsInstanceOfType) != 1)
                throw new InvalidOperationException($"The mode requires its explicitly registered unique feature '{contract.FullName}'.");
    }
}
