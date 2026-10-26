using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    private int BossSeat => RelativeSeat(3);
    private int HellAuxiliarySeat => RelativeSeat(4);
    private bool ActivityPlayerSlot(int seat) => SeatOffset(seat) is 0 or 5;
    private bool ActivitySpiritSlot(int seat) => SeatOffset(seat) is 2 or 3;
    private bool ShenwuPlayerSlot(int seat) => _mode.ShenwuActivity!.PlayerSeatOffsets.Contains(SeatOffset(seat));
    private IReadOnlyList<string> ActivitySeatPool(int seat, string team) =>
        (ActivityPlayerSlot(seat) ? _mode.TeamActivity!.PlayerPools : ActivitySpiritSlot(seat)
            ? _mode.TeamActivity!.SpiritPools : _mode.TeamActivity!.MachinePools)[team];
    private IReadOnlyList<string> BossSeatPool(int seat) => _mode.HellActivity is { } hell && seat == HellAuxiliarySeat
        ? [hell.AuxiliaryGeneralId] : seat == BossSeat ? _mode.NianActivity is { } nian ? [nian.FormGeneralIds[0]] :
            _mode.BossActivity!.Stages[0].GeneralPoolIds : _mode.BossActivity!.PlayerPoolIds;
    private int ActivityTier(int seat)
    {
        var activity = _mode.TeamActivity!;
        return ActivityPlayerSlot(seat) ? activity.PlayerTier : ActivitySpiritSlot(seat)
            ? activity.SpiritTierMinimum + (int)(((uint)_options.Seed + (uint)SeatOffset(seat)) %
                (uint)(activity.SpiritTierMaximum - activity.SpiritTierMinimum + 1)) : activity.MachineTier;
    }
    private ModeSeatSetup SelectInitial(ModeSeatSetup seat, ContentGeneralDefinition general)
    {
        var plan = PrepareSelectedGeneral(Facts(seat), general.Id);
        return seat with { GeneralId = general.Id, GeneralSelected = true, GeneralRevealed = true,
            ExtraGeneralSkillIds = plan.ExtraSkillIds, MaxHp = plan.MaximumHp ?? seat.MaxHp, Hp = plan.Hp ?? seat.Hp };
    }
    private ModeSetupPlan BuildSurvivalPlayers()
    {
        if (_mode.WenheActivity is not { } activity) throw new InvalidOperationException("Survival requires an explicit activity definition.");
        var generals = GeneralPool.Where(SelectionEligible).ToList();
        _random.Shuffle(generals);
        if (generals.Count < PlayerCount) throw new InvalidOperationException("Wenhe needs eight eligible generals.");
        var players = Enumerable.Range(0, PlayerCount).Select(seat =>
        {
            var grants = new[] { "wenhe:rules", "wenhe:chaos-resolution" }
                .Concat(activity.PlayerTier >= 5 ? ["wenhe:niepan"] : Array.Empty<string>())
                .Concat(activity.EnableAspirations ? ["wenhe:champion-mark"] : Array.Empty<string>())
                .Select(id => new ModeSetupSkillGrant($"mode:wenhe:{seat}:{id}", id, $"mode:wenhe:{seat}:{id}", "mode:wenhe", true)).ToArray();
            var player = new ModeSeatSetup(seat, seat == _options.HumanSeat ? "你" : $"人机对手 {seat + 1}",
                seat == _options.HumanSeat, Role.Contestant, null, 4, 4) { RoleRevealed = true, SkillGrants = grants };
            return Interactive ? player : SelectInitial(player, generals[seat]);
        }).ToArray();
        return new(players, Interactive ? generals.Select(g => g.Id) : [], Interactive
            ? players.OrderByDescending(p => p.IsHuman).ThenBy(p => p.Seat).Select(p => p.Seat) : []);
    }

    private ModeSetupPlan BuildTeamActivityPlayers()
    {
        var activity = _mode.TeamActivity!;
        var playerTeam = _options.HumanTeamId ?? TeamIds[0];
        var enemyTeam = TeamIds.Single(team => team != playerTeam);
        var players = Enumerable.Range(0, PlayerCount).Select(seat =>
        {
            var team = SeatOffset(seat) is 0 or 4 or 5 or 6 ? playerTeam : enemyTeam;
            return new ModeSeatSetup(seat, seat == _options.HumanSeat ? "你" :
                $"{(ActivityPlayerSlot(seat) ? "人机搭档" : ActivitySpiritSlot(seat) ? "英灵" : "器械")} {seat + 1} · {ActivityTier(seat)}阶",
                seat == _options.HumanSeat, TeamRole(team), null, 4, 4)
            { TeamId = team, TeamRevealed = true, RoleRevealed = true };
        }).ToArray();
        var generals = GeneralPool.ToList();
        _random.Shuffle(generals);
        if (Interactive) return new(players, generals.Select(g => g.Id),
            players.OrderBy(p => p.IsHuman ? 0 : 1).ThenBy(p => p.Seat).Select(p => p.Seat));
        for (var seat = 0; seat < players.Length; seat++)
        {
            var general = generals.First(g => ActivitySeatPool(seat, players[seat].TeamId!).Contains(g.Id, StringComparer.Ordinal));
            generals.Remove(general);
            players[seat] = SelectInitial(players[seat], general);
        }
        return new(players, generals.Select(g => g.Id), []);
    }

    private ModeSetupPlan BuildBossPlayers()
    {
        var activity = _mode.BossActivity!;
        var hell = _mode.HellActivity;
        var players = Enumerable.Range(0, PlayerCount).Select(seat =>
        {
            var auxiliary = hell is not null && seat == HellAuxiliarySeat;
            var npc = seat == BossSeat || auxiliary;
            var team = npc ? activity.EnemyTeamId : activity.PlayerTeamId;
            var name = seat == _options.HumanSeat ? "你" : hell is not null
                ? auxiliary ? "孟婆" : npc ? "十殿阎罗" : $"人机搭档 {seat + 1} · 5阶"
                : seat == BossSeat ? _mode.HulaoActivity is not null ? "神吕布" : _mode.NianActivity is not null ? "年兽" : "鬼怪首领"
                : $"人机搭档 {seat + 1} · {activity.PlayerTier}阶";
            var player = new ModeSeatSetup(seat, name, seat == _options.HumanSeat, TeamRole(team), null, 4, auxiliary ? 0 : 4)
            { TeamId = team, TeamRevealed = true, RoleRevealed = true, IsAlive = !auxiliary };
            if (!auxiliary) return player;
            var selected = SelectInitial(player, _registry.Generals[hell!.AuxiliaryGeneralId]);
            return selected with { GeneralRevealed = false, Hp = 0 };
        }).ToArray();
        var generals = GeneralPool.Where(g => hell is null || g.Id != hell.AuxiliaryGeneralId).ToList();
        _random.Shuffle(generals);
        if (Interactive) return new(players, generals.Select(g => g.Id), players.Where(p => hell is null || p.Seat != HellAuxiliarySeat)
            .OrderBy(p => p.IsHuman ? 0 : 1).ThenBy(p => p.Seat).Select(p => p.Seat));
        for (var seat = 0; seat < players.Length; seat++)
        {
            if (hell is not null && seat == HellAuxiliarySeat) continue;
            var general = generals.First(g => BossSeatPool(seat).Contains(g.Id, StringComparer.Ordinal));
            generals.Remove(general);
            players[seat] = SelectInitial(players[seat], general);
        }
        return new(players, generals.Select(g => g.Id), []);
    }

    private ModeSetupPlan BuildShenwuPlayers()
    {
        var activity = _mode.ShenwuActivity!;
        var gods = activity.GodPoolIds.ToList();
        _random.Shuffle(gods);
        var leader = activity.LeaderPoolIds[_random.Next(activity.LeaderPoolIds.Count)];
        var shared = gods.Take(3).ToArray();
        var players = Enumerable.Range(0, PlayerCount).Select(seat =>
        {
            var allied = ShenwuPlayerSlot(seat);
            var team = allied ? activity.PlayerTeamId : activity.EnemyTeamId;
            return new ModeSeatSetup(seat, seat == _options.HumanSeat ? "你" : allied ? $"人机搭档 {seat + 1} · {activity.PlayerTier}阶"
                : seat == RelativeSeat(activity.LeaderSeatOffset) ? "凶兽首领" : $"烛阴 {seat + 1}",
                seat == _options.HumanSeat, TeamRole(team), null, 4, 4)
            { TeamId = team, TeamRevealed = true, RoleRevealed = true };
        }).ToArray();
        var available = GeneralPool.ToList();
        _random.Shuffle(available);
        var order = activity.PlayerSeatOffsets.Select(RelativeSeat).Concat(Enumerable.Range(0, PlayerCount).Where(seat => !ShenwuPlayerSlot(seat))).ToArray();
        if (Interactive) return new(players, available.Select(g => g.Id), order, shared, leader);
        foreach (var seat in order)
        {
            var offers = ShenwuOffers(new(Facts(players[seat]), available.Select(g => g.Id), players.Select(Facts),
                shenwuSharedGodIds: shared, shenwuLeaderGeneralId: leader));
            var general = available.Single(g => g.Id == offers.Offers.First().GeneralId);
            available.Remove(general);
            players[seat] = SelectInitial(players[seat], general);
        }
        return new(players, available.Select(g => g.Id), [], shared, leader);
    }

    private ModeSetupPlan BuildCampaignPlayers()
    {
        var setup = _options.ShanheBattle!;
        var members = new[] { new ShanheCombatantDefinition(setup.PlayerGeneralId, setup.MaxHpBonus, setup.Hp,
            setup.InitialHandBonus, setup.DrawBonus, setup.SlashBonus, setup.ExtraSkillIds) }
            .Concat(setup.AllyRoster).Concat(setup.EnemyRoster).ToArray();
        var players = new List<ModeSeatSetup>();
        for (var seat = 0; seat < members.Length; seat++)
        {
            var member = members[seat];
            var general = GeneralPool.Single(g => g.Id == member.GeneralId);
            var sharedHp = seat > 0 && seat <= setup.AllyRoster.Count ? ShanheTeamRankPrograms.EntryBonuses(setup).MaxHp : 0;
            var team = seat <= setup.AllyRoster.Count ? ShanheBattleSetup.PlayerTeamId : ShanheBattleSetup.EnemyTeamId;
            var grants = new List<ModeSetupSkillGrant>();
            if (setup.TerrainSkillId is { } terrain)
            {
                var grant = $"mode:shanhe-terrain:{seat}:{terrain}";
                grants.Add(new(grant, terrain, grant, "mode:shanhe-terrain", IsModeRule: true));
            }
            foreach (var id in member.ExtraSkillIds ?? [])
                if (!general.SkillIds.Contains(id, StringComparer.Ordinal))
                {
                    var grant = $"campaign:shanhe:{seat}:{id}";
                    grants.Add(new(grant, id, grant, "campaign:shanhe"));
                }
            if (seat == 0)
            {
                var seen = new HashSet<string>(StringComparer.Ordinal);
                for (var index = 0; index < setup.TacticSkillIds.Count; index++)
                {
                    var id = setup.TacticSkillIds[index];
                    var grant = seen.Add(id) ? $"campaign:shanhe-tactic:{id}" :
                        $"campaign:shanhe-tactic-copy:{index.ToString(System.Globalization.CultureInfo.InvariantCulture)}:{id}";
                    grants.Add(new(grant, id, grant, "campaign:shanhe-tactic"));
                }
            }
            players.Add(new(seat, seat == 0 ? "你" : $"{(team == ShanheBattleSetup.PlayerTeamId ? "同行" : "敌军")} · {general.Name}",
                seat == _options.HumanSeat, TeamRole(team), general.Id, general.BaseHp + member.MaxHpBonus + sharedHp,
                (member.Hp ?? general.InitialHp ?? general.BaseHp + member.MaxHpBonus) + sharedHp)
            { TeamId = team, TeamRevealed = true, RoleRevealed = true, GeneralSelected = true, GeneralRevealed = true,
                SkillGrants = grants, InitialArmor = member.InitialArmor });
        }
        return new(players, [], []);
    }

    private ModeGeneralOfferPlan ShenwuOffers(ModeGeneralOfferRequest request)
    {
        var activity = _mode.ShenwuActivity!;
        var available = request.AvailableGeneralIds.Select(id => _registry.Generals[id]).ToArray();
        if (!ShenwuPlayerSlot(request.Seat.Seat))
        {
            var id = request.Seat.Seat == RelativeSeat(activity.LeaderSeatOffset) ? request.ShenwuLeaderGeneralId :
                activity.MinionGeneralIds[activity.MinionSeatOffsets.ToList().IndexOf(SeatOffset(request.Seat.Seat))];
            return new(available.Where(g => g.Id == id).Select(g => new ModeGeneralOffer(g.Id, g.Id)));
        }
        var ordinary = available.Where(g => activity.PlayerPoolIds.Contains(g.Id, StringComparer.Ordinal)).ToArray();
        var representatives = ordinary.DistinctBy(CharacterFamily, StringComparer.Ordinal).Take(request.FullRoster ? int.MaxValue : 5).ToArray();
        var offers = representatives.SelectMany(representative => ordinary.Where(g => CharacterFamily(g) == CharacterFamily(representative)),
            (representative, general) => new ModeGeneralOffer(general.Id, representative.Id)).ToList();
        offers.AddRange(available.Where(g => request.ShenwuSharedGodIds!.Contains(g.Id, StringComparer.Ordinal)).Select(g => new ModeGeneralOffer(g.Id, g.Id)));
        return new(offers);
    }
}
