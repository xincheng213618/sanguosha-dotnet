using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    private ModeSetupPlan BuildIdentityPlayers()
    {
        var roles = new List<Role>();
        foreach (var role in Enum.GetValues<Role>())
            if (_mode.RoleCounts.TryGetValue(role.ToString(), out var count))
            {
                if (count < 0) throw new InvalidOperationException($"Mode '{_mode.Id}' has a negative {role} count.");
                for (var i = 0; i < count; i++) roles.Add(role);
            }
        var unknown = _mode.RoleCounts.Keys.Where(key => !Enum.TryParse<Role>(key, false, out _)).ToArray();
        if (unknown.Length > 0) throw new InvalidOperationException($"Mode '{_mode.Id}' contains unknown role ids: {string.Join(", ", unknown)}.");
        if (roles.Count != PlayerCount || roles.Count(role => role == Role.Lord) != 1)
            throw new InvalidOperationException($"Mode '{_mode.Id}' does not provide exactly one valid {PlayerCount}-seat identity distribution.");
        var assigned = new Role[PlayerCount];
        if (_options.HumanSeat >= 0 && _options.HumanRole is { } forced)
        {
            assigned[_options.HumanSeat] = forced;
            if (!roles.Remove(forced)) throw new ArgumentException($"Mode '{_mode.Id}' does not contain a {forced} seat.", nameof(GameOptions.HumanRole));
            _random.Shuffle(roles);
            var index = 0;
            for (var seat = 0; seat < PlayerCount; seat++) if (seat != _options.HumanSeat) assigned[seat] = roles[index++];
        }
        else
        {
            _random.Shuffle(roles);
            for (var seat = 0; seat < PlayerCount; seat++) assigned[seat] = roles[seat];
        }
        var generalsPerSeat = _mode.DualGenerals ? 2 : 1;
        if (GeneralPool.Count < PlayerCount * generalsPerSeat)
            throw new InvalidOperationException($"Mode '{_mode.Id}' needs {PlayerCount * generalsPerSeat} generals but only {GeneralPool.Count} are available.");
        var generals = GeneralPool.Where(SelectionEligible).ToList();
        if (!Interactive)
        {
            _random.Shuffle(generals);
            if (UsesFamilyDraft) generals = DraftFamilies(generals, PlayerCount * generalsPerSeat).ToList();
        }
        var players = new List<ModeSeatSetup>();
        for (var seat = 0; seat < PlayerCount; seat++)
        {
            var role = assigned[seat];
            var general = Interactive ? null : generals[seat * generalsPerSeat];
            var baseHp = general?.BaseHp ?? 4;
            var maxHp = UsesGeneralBaseHp ? baseHp + (role == Role.Lord ? 1 : 0) : role == Role.Lord ? 5 : 4;
            if (general?.SkillIds.Contains("anniversary:kunyu", StringComparer.Ordinal) == true) maxHp = 1;
            players.Add(new(seat, SeatName(seat), IsHuman(seat), role, general?.Id, maxHp,
                Math.Min(maxHp, UsesGeneralBaseHp && general?.InitialHp is { } initial ? initial + (role == Role.Lord ? 1 : 0) : maxHp))
            {
                RoleRevealed = role == Role.Lord, GeneralSelected = !Interactive, GeneralRevealed = !Interactive,
                SecondaryGeneralId = !Interactive && _mode.DualGenerals ? generals[seat * 2 + 1].Id : null,
                SecondaryGeneralSelected = !Interactive && _mode.DualGenerals,
                SecondaryGeneralRevealed = !Interactive && _mode.DualGenerals
            });
        }
        if (!Interactive) return new(players, [], []);
        _random.Shuffle(generals);
        return new(players, generals.Select(g => g.Id), players.OrderBy(p => p.Role == Role.Lord ? 0 : 1)
            .ThenBy(p => p.IsHuman ? 0 : 1).ThenBy(p => p.Seat).SelectMany(p => Enumerable.Repeat(p.Seat, generalsPerSeat)));
    }

    private ModeSetupPlan BuildPublicTeamPlayers()
    {
        var teams = TeamIds;
        if (teams.Length != 2 || _mode.TeamCounts is null)
            throw new InvalidOperationException($"Mode '{_mode.Id}' requires exactly two public teams for this engine adapter.");
        var available = _mode.TeamCounts.OrderBy(entry => entry.Key, StringComparer.Ordinal)
            .SelectMany(entry => Enumerable.Repeat(entry.Key, entry.Value)).ToList();
        var assigned = new string[PlayerCount];
        if (_options.HumanSeat >= 0 && _options.HumanTeamId is { } team)
        {
            assigned[_options.HumanSeat] = team;
            if (!available.Remove(team)) throw new ArgumentException($"Mode '{_mode.Id}' has no remaining seat for team '{team}'.", nameof(GameOptions.HumanTeamId));
            _random.Shuffle(available);
            var index = 0;
            for (var seat = 0; seat < PlayerCount; seat++) if (seat != _options.HumanSeat) assigned[seat] = available[index++];
        }
        else
        {
            _random.Shuffle(available);
            for (var seat = 0; seat < PlayerCount; seat++) assigned[seat] = available[seat];
        }
        if (GeneralPool.Count < PlayerCount)
            throw new InvalidOperationException($"Mode '{_mode.Id}' needs {PlayerCount} generals but only {GeneralPool.Count} are available.");
        var generals = GeneralPool.ToList();
        if (!Interactive) _random.Shuffle(generals);
        var players = Enumerable.Range(0, PlayerCount).Select(seat => new ModeSeatSetup(seat, SeatName(seat), IsHuman(seat),
            TeamRole(assigned[seat]), Interactive ? null : generals[seat].Id, 4, 4)
        {
            TeamId = assigned[seat], TeamRevealed = true, RoleRevealed = true,
            GeneralSelected = !Interactive, GeneralRevealed = !Interactive
        }).ToArray();
        if (!Interactive && _mode.Id == WushuangMode.ModeId)
            for (var seat = 0; seat < players.Length; seat++)
            {
                var general = generals[seat];
                players[seat] = players[seat] with { MaxHp = general.BaseHp, Hp = general.InitialHp ?? general.BaseHp };
            }
        if (!Interactive) return new(players, [], []);
        _random.Shuffle(generals);
        return new(players, generals.Select(g => g.Id), players.OrderBy(p => p.IsHuman ? 0 : 1).ThenBy(p => p.Seat).Select(p => p.Seat));
    }

    private ModeSetupPlan BuildNationalPlayers()
    {
        if (_mode.FactionCounts is not { Count: >= 2 } counts || counts.Values.Sum() != PlayerCount)
            throw new InvalidOperationException($"Mode '{_mode.Id}' requires at least two hidden factions totaling {PlayerCount} seats.");
        var factions = counts.OrderBy(entry => entry.Key, StringComparer.Ordinal)
            .SelectMany(entry => Enumerable.Repeat(entry.Key, entry.Value)).ToList();
        _random.Shuffle(factions);
        var generals = GeneralPool.ToList();
        if (!Interactive) _random.Shuffle(generals);
        var players = new List<ModeSeatSetup>();
        for (var seat = 0; seat < PlayerCount; seat++)
        {
            var faction = factions[seat];
            var selected = generals.Where(g => string.Equals(g.FactionId, faction, StringComparison.Ordinal)).Take(2).ToArray();
            if (!Interactive && selected.Length != 2)
                throw new InvalidOperationException($"Mode '{_mode.Id}' could not reserve two generals for faction '{faction}'.");
            if (!Interactive) foreach (var general in selected) generals.Remove(general);
            players.Add(new(seat, SeatName(seat), IsHuman(seat), Role.Renegade, Interactive ? null : selected[0].Id, 4, 4)
            {
                NationalFactionId = faction, GeneralSelected = !Interactive,
                SecondaryGeneralId = Interactive ? null : selected[1].Id, SecondaryGeneralSelected = !Interactive
            });
        }
        if (!Interactive) return new(players, [], []);
        generals = GeneralPool.ToList();
        _random.Shuffle(generals);
        return new(players, generals.Select(g => g.Id), players.OrderBy(p => p.IsHuman ? 0 : 1).ThenBy(p => p.Seat)
            .SelectMany(p => new[] { p.Seat, p.Seat }));
    }
}
