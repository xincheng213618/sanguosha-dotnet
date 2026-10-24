using CardGame.Core;

namespace CardGame.Modes.Activities;

internal sealed class DragonBoatBattleRulesFeature : IDragonBoatBattleRulesFeature, IMatchModeRuleObserverProvider,
    IMatchModeNumericRules, IMatchModeLifecycle, IMatchModeSnapshotProjector, IMatchModeEquipmentPolicy
{
    private static readonly string[] Teams = ["team:wei", "team:shu", "team:wu", "team:qun"];
    private readonly ContentModeDefinition _mode;
    private readonly IModeBattleRulesHost _host;
    private bool Active => _mode.DragonBoatActivity is not null;
    internal DragonBoatBattleRulesFeature(ContentModeDefinition mode, IModeBattleRulesHost host)
    {
        (_mode, _host) = (mode, host);
        RuleObservers = Active ? Array.AsReadOnly<ModeRuleObserverRegistration>(
            [ModeRuleObserverRegistration.ForEvent<DamageAppliedEvent>(ProgramRulePhase.AfterEvent, 40000, AwardTreasure)]) : [];
    }
    public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    int IMatchModeNumericRules.Order => 40;
    int IMatchModeLifecycle.Order => 30;
    int IMatchModeSnapshotProjector.Order => 30;
    int IMatchModeEquipmentPolicy.Order => 40;
    int? IMatchModeEquipmentPolicy.SlashLimitBonus(CardKind kind) => EquipmentSlashLimitBonus(kind);
    public int? EquipmentSlashLimitBonus(CardKind kind) => Active && kind == CardKind.Crossbow ? 3 : null;
    public int TeamScore(string teamId) => _host.History<DragonBoatTreasureChangedEvent>().LastOrDefault(fact => fact.TeamId == teamId)?.Current ?? 0;

    public IReadOnlyList<DragonBoatTeamSnapshot>? Standings(bool finalizing = false)
    {
        if (!Active) return null;
        var completed = _host.History<DragonBoatTeamRankedEvent>();
        if (completed.Count != 0) return Array.AsReadOnly(completed.OrderBy(fact => fact.Rank)
            .Select(fact => new DragonBoatTeamSnapshot(fact.TeamId, fact.Treasures, fact.Rank, fact.ReachedSequence, fact.TangZiVictory)).ToArray());
        var facts = _host.CaptureFacts();
        var scores = _host.History<DragonBoatTreasureChangedEvent>();
        var rows = Teams.Select(team =>
        {
            var fact = scores.LastOrDefault(item => item.TeamId == team);
            var score = fact?.Current ?? 0;
            var priority = score == 12 && facts.Players.Any(player => player.TeamId == team && player.IsAlive &&
                _host.HasActivitySkill(player.Seat, "dragonboat:xingzhao"));
            return new DragonBoatTeamSnapshot(team, score, 0, fact?.ReachedSequence ?? 0, priority);
        }).OrderByDescending(row => finalizing && row.TangZiVictory).ThenByDescending(row => row.Treasures)
            .ThenBy(row => row.ReachedSequence).ThenBy(row => Array.IndexOf(Teams, row.TeamId)).ToArray();
        return Array.AsReadOnly(rows.Select((row, index) => row with { Rank = index + 1 }).ToArray());
    }

    public DragonBoatDraftSnapshot? Draft(int viewerSeat)
    {
        if (!Active) return null;
        var facts = _host.CaptureFacts();
        if (facts.SetupComplete || viewerSeat < 0 || viewerSeat >= facts.Players.Count) return null;
        var teammate = facts.Players.Single(player => player.Seat != viewerSeat && player.TeamId == facts.Players[viewerSeat].TeamId);
        var draft = _host.Draft(teammate.Seat);
        return new(teammate.Seat) { GeneralIds = draft.GeneralSelected ? [draft.GeneralId] : draft.Candidates };
    }

    public GameSnapshot ProjectGame(GameSnapshot snapshot, int viewerSeat, bool revealAll) => snapshot with
        { DragonBoatStandings = Standings(), DragonBoatDraft = Draft(viewerSeat) };

    private void AwardTreasure(DamageAppliedEvent damage)
    {
        if (damage.SourceLess || damage.Amount <= 0) return;
        var facts = _host.CaptureFacts();
        if (damage.SourceSeat < 0 || damage.SourceSeat >= facts.Players.Count || damage.TargetSeat < 0 ||
            damage.TargetSeat >= facts.Players.Count || facts.Players[damage.SourceSeat].TeamId is not { } team ||
            team == facts.Players[damage.TargetSeat].TeamId) return;
        var before = TeamScore(team);
        var after = Math.Min(12, before + damage.Amount);
        if (after == before) return;
        var order = _host.History<DragonBoatTreasureChangedEvent>().LongCount() + 1;
        _host.PublishFact(new DragonBoatTreasureChangedEvent(team, before, after, damage.SourceSeat, damage.TargetSeat, order));
        var faction = _host.FactionName(team[5..]);
        _host.Log("DragonBoatTreasure", $"{faction}队获得{after - before}枚龙船至宝，现有{after}/12。", damage.SourceSeat, damage.TargetSeat);
    }

    public bool TryCompleteMatch(ModeCompletionBoundary boundary)
    {
        if (!Active || boundary != ModeCompletionBoundary.AfterTurnEnded) return false;
        var facts = _host.CaptureFacts();
        if (facts.HasWinner || facts.ResolutionCount != 0 || !_host.History<RoundEndedEvent>()
            .Any(fact => fact.RoundNumber == _mode.DragonBoatActivity!.Rounds)) return false;
        var ranks = Standings(finalizing: true)!;
        foreach (var rank in ranks) _host.PublishFact(new DragonBoatTeamRankedEvent(
            rank.TeamId, rank.Rank, rank.Treasures, rank.ReachedSequence, rank.TangZiVictory));
        _host.SetWinner(Winner.RankedTeams, ranks[0].TeamId);
        _host.PublishFact(new WinnerDeterminedEvent(Winner.RankedTeams, ranks[0].TeamId));
        _host.CompleteGame();
        return true;
    }

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        if (_mode.DragonBoatActivity is not { } activity) yield break;
        var amount = query switch { SkillRuleQuery.InitialHandSize => activity.PlayerTier >= 3 ? 1 : 0,
            SkillRuleQuery.DrawCount => activity.PlayerTier >= 4 ? 1 : 0, SkillRuleQuery.SlashLimit => activity.PlayerTier >= 3 ? 1 : 0, _ => 0 };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"dragonboat:tier:{query}", SkillRuleOperation.Add, amount);
    }
}
