using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    private static readonly string[] DragonTeams = ["team:wei", "team:shu", "team:wu", "team:qun"];
    private ModeSetupPlan BuildDragonBoatPlayers()
    {
        var activity = _mode.DragonBoatActivity!;
        var offset = _options.HumanSeat >= 0 && _options.HumanTeamId is { } requested
            ? (Array.IndexOf(DragonTeams, requested) - _options.HumanSeat % 4 + 4) % 4 : 0;
        var players = Enumerable.Range(0, 8).Select(seat => new ModeSeatSetup(seat,
            seat == _options.HumanSeat ? "你" : $"人机 {seat + 1}", seat == _options.HumanSeat, Role.Contestant, null, 4, 4)
        {
            TeamId = DragonTeams[(seat + offset) % 4], TeamRevealed = true, RoleRevealed = true,
            SkillGrants = new[] { "dragonboat:aiye" }.Concat(activity.PlayerTier >= 5 ? ["dragonboat:niepan"] : Array.Empty<string>())
                .Select(id => new ModeSetupSkillGrant($"mode:dragonboat:{seat}:{id}", id, $"mode:dragonboat:{seat}:{id}", "mode:dragonboat", true)).ToArray()
        }).ToArray();
        var matches = _random.Next(2) == 0 ? new[] { "tang-zi", "huang-quan", "su-fei", "liu-qi" } :
            new[] { "huang-quan", "liu-qi", "tang-zi", "su-fei" };
        var reserved = new Dictionary<int, ContentGeneralDefinition>();
        for (var index = 0; index < 4; index++)
        {
            var seats = players.Where(p => p.TeamId == DragonTeams[index]).ToArray();
            reserved[seats[_random.Next(2)].Seat] = GeneralPool.Single(g => g.Id == $"dragonboat:{matches[index]}");
        }
        var used = reserved.Values.Select(CharacterFamily).ToHashSet(StringComparer.Ordinal);
        for (var seat = 0; seat < players.Length; seat++)
        {
            var offer = new List<ContentGeneralDefinition>();
            if (reserved.TryGetValue(seat, out var special)) offer.Add(special);
            var pool = activity.PlayerPoolIds[players[seat].TeamId!];
            var ordinary = GeneralPool.Where(g => pool.Contains(g.Id) && !g.Id.StartsWith("dragonboat:", StringComparison.Ordinal))
                .GroupBy(CharacterFamily, StringComparer.Ordinal).Where(g => !used.Contains(g.Key)).ToList();
            while (offer.Count < 6 && ordinary.Count > 0)
            {
                var at = _random.Next(ordinary.Count);
                var family = ordinary[at].ToArray();
                ordinary.RemoveAt(at);
                var general = family[_random.Next(family.Length)];
                offer.Add(general);
                used.Add(CharacterFamily(general));
            }
            if (offer.Count != 6) throw new InvalidOperationException("Dragon boat needs six distinct unclaimed candidate families per seat.");
            _random.Shuffle(offer);
            players[seat] = players[seat] with { GeneralDraftCandidateIds = offer.Select(g => g.Id).ToArray() };
        }
        if (Interactive) return new(players, GeneralPool.Select(g => g.Id),
            players.OrderByDescending(p => p.IsHuman).ThenBy(p => p.Seat).Select(p => p.Seat));
        for (var seat = 0; seat < players.Length; seat++)
        {
            var general = _registry.Generals[players[seat].GeneralDraftCandidateIds![_random.Next(6)]];
            players[seat] = SelectInitial(players[seat], general) with { GeneralDraftCandidateIds = null };
        }
        return new(players, [], []);
    }

    private ModeSetupPlan BuildGuanduPlayers()
    {
        var activity = _mode.GuanduActivity!;
        var layoutIndex = _random.Next(activity.SeatLayouts.Count);
        var layout = activity.SeatLayouts[layoutIndex];
        var playerTeam = _options.HumanTeamId ?? (_random.Next(2) == 0 ? "team:cao" : "team:yuan");
        var humanRole = _options.HumanGuanduRole ?? GuanduSeatRole.General;
        var yuanCommander = Enumerable.Range(1, 7).First(i => layout[i] == "team:yuan");
        var candidates = Enumerable.Range(0, 8).Where(i => layout[i] == playerTeam &&
            ((i == 0 || i == yuanCommander) == (humanRole == GuanduSeatRole.Commander))).ToArray();
        var canonicalHuman = candidates[_random.Next(candidates.Length)];
        var rotation = _options.HumanSeat >= 0 ? (_options.HumanSeat - canonicalHuman + 8) % 8 : 0;
        var configured = new GuanduBattleConfiguredEvent(activity.Events[_random.Next(activity.Events.Count)], layoutIndex,
            rotation, (yuanCommander + rotation) % 8, playerTeam, humanRole);
        bool Commander(int seat) => seat == configured.CaoCommanderSeat || seat == configured.YuanCommanderSeat;
        var used = activity.Commanders.Values.Select(id => CharacterFamily(GeneralPool.Single(g => g.Id == id))).ToHashSet(StringComparer.Ordinal);
        var players = new ModeSeatSetup[8];
        for (var seat = 0; seat < 8; seat++)
        {
            var team = layout[(seat - rotation + 8) % 8];
            var offer = new List<ContentGeneralDefinition>();
            if (Commander(seat)) offer.Add(GeneralPool.Single(g => g.Id == activity.Commanders[team]));
            else
            {
                var pool = GeneralPool.Where(g => activity.GeneralPools[team].Contains(g.Id) && !activity.Commanders.Values.Contains(g.Id) &&
                    !used.Contains(CharacterFamily(g))).ToList();
                _random.Shuffle(pool);
                offer.AddRange(pool.Take(activity.CandidateCount));
                if (offer.Count != activity.CandidateCount) throw new InvalidOperationException("Guandu has too few eligible candidates.");
            }
            players[seat] = new(seat, seat == _options.HumanSeat ? "你" : $"人机 {seat + 1}", seat == _options.HumanSeat,
                TeamRole(team), null, 4, 4)
            {
                TeamId = team, TeamRevealed = true, RoleRevealed = true, GeneralDraftCandidateIds = offer.Select(g => g.Id).ToArray(),
                SkillGrants = new[] { "guandu:battle-event" }.Concat(activity.PlayerTier >= 5 ? ["guandu:niepan"] : Array.Empty<string>())
                    .Select(id => new ModeSetupSkillGrant($"mode:guandu:{seat}:{id}", id, $"mode:guandu:{seat}:{id}", "mode:guandu", true)).ToArray()
            };
        }
        if (Interactive) return new(players, GeneralPool.Select(g => g.Id),
            players.OrderByDescending(p => p.IsHuman).ThenBy(p => p.Seat).Select(p => p.Seat), guanduConfiguration: configured);
        foreach (var seat in players.OrderByDescending(p => Commander(p.Seat)).ThenBy(p => p.Seat).Select(p => p.Seat).ToArray())
        {
            var player = players[seat];
            var offer = player.GeneralDraftCandidateIds!.Select(id => _registry.Generals[id])
                .Where(g => Commander(seat) || !used.Contains(CharacterFamily(g))).ToArray();
            if (offer.Length == 0) offer = GeneralPool.Where(g => activity.GeneralPools[player.TeamId!].Contains(g.Id) && !used.Contains(CharacterFamily(g))).ToArray();
            var general = offer[_random.Next(offer.Length)];
            used.Add(CharacterFamily(general));
            var selected = SelectInitial(player, general);
            var bonus = (activity.PlayerTier >= 2 ? 1 : 0) + (activity.PlayerTier >= 5 ? 1 : 0) + (Commander(seat) ? 1 : 0);
            players[seat] = selected with { GeneralDraftCandidateIds = null,
                MaxHp = general.BaseHp + bonus, Hp = (general.InitialHp ?? general.BaseHp) + bonus };
        }
        return new(players, [], [], guanduConfiguration: configured);
    }

    private ModeGeneralOfferPlan GuanduOffers(ModeGeneralOfferRequest request)
    {
        var activity = _mode.GuanduActivity!;
        var configured = request.GuanduConfiguration ?? throw new InvalidOperationException("Guandu draft lost its opening configuration.");
        var commander = request.Seat.Seat == configured.CaoCommanderSeat || request.Seat.Seat == configured.YuanCommanderSeat;
        var taken = request.Players.Where(p => p.GeneralSelected).Select(p => CharacterFamily(_registry.Generals[p.GeneralId!]))
            .ToHashSet(StringComparer.Ordinal);
        var offer = (request.HeldDraftCandidateIds ?? []).Select(id => _registry.Generals[id])
            .Where(g => commander || !taken.Contains(CharacterFamily(g))).ToArray();
        if (!commander && offer.Length < activity.CandidateCount)
            offer = offer.Concat(GeneralPool.Where(g => activity.GeneralPools[request.Seat.TeamId!].Contains(g.Id) &&
                !activity.Commanders.Values.Contains(g.Id) && !taken.Contains(CharacterFamily(g)) &&
                !offer.Any(other => CharacterFamily(other) == CharacterFamily(g))).OrderBy(g => g.Id, StringComparer.Ordinal))
                .Take(activity.CandidateCount).ToArray();
        return new(offer.Select(g => new ModeGeneralOffer(g.Id, g.Id)));
    }
}
