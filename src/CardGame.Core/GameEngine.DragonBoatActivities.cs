namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly string[] DragonBoatTeams = ["team:wei", "team:shu", "team:wu", "team:qun"];
    private bool IsDragonBoatActivity => _modeDefinition.DragonBoatActivity is not null;
    private CharacterState DragonBoatTeammate(int seat) => _players.Single(p => p.Seat != seat && p.TeamId == _players[seat].TeamId);
    private int DragonBoatTeamScore(string teamId) => ProgramEventHistory<DragonBoatTreasureChangedEvent>()
        .LastOrDefault(e => e.TeamId == teamId)?.Current ?? 0;
    private string DragonBoatFamily(GeneralDefinition general) => _contentRegistry.Generals.GetValueOrDefault(general.Id)?.CharacterId ?? general.Id;

    private IReadOnlyList<DragonBoatTeamSnapshot>? GetDragonBoatStandings(bool finalizing = false)
    {
        if (!IsDragonBoatActivity) return null;
        var completed = ProgramEventHistory<DragonBoatTeamRankedEvent>().ToArray();
        if (completed.Length != 0) return Array.AsReadOnly(completed.OrderBy(e => e.Rank)
            .Select(e => new DragonBoatTeamSnapshot(e.TeamId, e.Treasures, e.Rank, e.ReachedSequence, e.TangZiVictory)).ToArray());
        var rows = DragonBoatTeams.Select(team =>
        {
            var fact = ProgramEventHistory<DragonBoatTreasureChangedEvent>().LastOrDefault(e => e.TeamId == team);
            var score = fact?.Current ?? 0;
            var priority = score == 12 && _players.Any(p => p.TeamId == team && p.IsAlive && HasActivitySkill(p, "dragonboat:xingzhao"));
            return new DragonBoatTeamSnapshot(team, score, 0, fact?.ReachedSequence ?? 0, priority);
        }).OrderByDescending(e => finalizing && e.TangZiVictory).ThenByDescending(e => e.Treasures)
            .ThenBy(e => e.ReachedSequence).ThenBy(e => Array.IndexOf(DragonBoatTeams, e.TeamId)).ToArray();
        return Array.AsReadOnly(rows.Select((row, index) => row with { Rank = index + 1 }).ToArray());
    }

    private DragonBoatDraftSnapshot? GetDragonBoatDraft(int viewer)
    {
        if (!IsDragonBoatActivity || _setupComplete || !IsValidPlayerSeat(viewer)) return null;
        var teammate = DragonBoatTeammate(viewer);
        return new(teammate.Seat) { GeneralIds = teammate.GeneralSelected ? [teammate.General.Id] :
            teammate.GeneralDraftCandidates?.Select(g => g.Id).ToArray() ?? [] };
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 40000)]
    private void AwardDragonBoatTreasure(IGameEvent payload)
    {
        if (!IsDragonBoatActivity || payload is not DamageAppliedEvent { SourceLess: false, Amount: > 0 } damage ||
            !IsValidPlayerSeat(damage.SourceSeat) || !IsValidPlayerSeat(damage.TargetSeat) ||
            _players[damage.SourceSeat].TeamId is not { } team || team == _players[damage.TargetSeat].TeamId) return;
        var before = DragonBoatTeamScore(team);
        var after = Math.Min(12, before + damage.Amount);
        if (after == before) return;
        var order = ProgramEventHistory<DragonBoatTreasureChangedEvent>().LongCount() + 1;
        AdvanceEventRulesAndQueueFact(new DragonBoatTreasureChangedEvent(team, before, after, damage.SourceSeat, damage.TargetSeat, order));
        AddLog("DragonBoatTreasure", $"{GetFactionName(team[5..])}队获得{after - before}枚龙船至宝，现有{after}/12。", damage.SourceSeat, damage.TargetSeat);
    }

    private bool TryCompleteDragonBoatRace()
    {
        if (!IsDragonBoatActivity || _winner != Winner.None || _resolutionStack.Count != 0 ||
            !ProgramEventHistory<RoundEndedEvent>().Any(e => e.RoundNumber == _modeDefinition.DragonBoatActivity!.Rounds)) return false;
        var ranks = GetDragonBoatStandings(finalizing: true)!;
        foreach (var rank in ranks) AdvanceEventRulesAndQueueFact(new DragonBoatTeamRankedEvent(
            rank.TeamId, rank.Rank, rank.Treasures, rank.ReachedSequence, rank.TangZiVictory));
        _winnerTeamId = ranks[0].TeamId;
        _winner = Winner.RankedTeams;
        AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(_winner, _winnerTeamId));
        CompleteGame();
        return true;
    }

    private IEnumerable<RuleQueryContribution> DragonBoatRuleContributions(CharacterState p, SkillRuleQuery query)
    {
        if (_modeDefinition.DragonBoatActivity is not { } a) yield break;
        var amount = query switch { SkillRuleQuery.InitialHandSize => a.PlayerTier >= 3 ? 1 : 0,
            SkillRuleQuery.DrawCount => a.PlayerTier >= 4 ? 1 : 0, SkillRuleQuery.SlashLimit => a.PlayerTier >= 3 ? 1 : 0, _ => 0 };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"dragonboat:tier:{query}", SkillRuleOperation.Add, amount);
    }

}
