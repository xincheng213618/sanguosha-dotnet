namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int ActivitySeatOffset(int seat) => (seat - Math.Max(0, _options.HumanSeat) + _playerCount) % _playerCount;
    private string ActivityPlayerTeam => _options.HumanTeamId ?? GetModeTeamIds()[0];
    private bool IsActivityPlayerSlot(int seat) => ActivitySeatOffset(seat) is 0 or 5;
    private bool IsActivitySpiritSlot(int seat) => ActivitySeatOffset(seat) is 2 or 3;

    private int ActivityTier(int seat)
    {
        var a = _modeDefinition.TeamActivity!;
        return IsActivityPlayerSlot(seat) ? a.PlayerTier : IsActivitySpiritSlot(seat)
            ? a.SpiritTierMinimum + (int)(((uint)_options.Seed + (uint)ActivitySeatOffset(seat)) % (uint)(a.SpiritTierMaximum - a.SpiritTierMinimum + 1))
            : a.MachineTier;
    }

    private bool SamePublicTeam(CharacterState a, CharacterState b) => a.TeamId is not null && a.TeamId == b.TeamId;
    private bool HasActivitySkill(CharacterState player, string id) => player.IsAlive && GetSkillBindingShard(player).HasSkill(id);

    private IEnumerable<RuleQueryContribution> ActivityRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        foreach (var contribution in GuanduActivityRuleContributions(player, query)) yield return contribution;
        foreach (var contribution in ShanheRuleContributions(player, query)) yield return contribution;
        foreach (var contribution in WenheActivityRuleContributions(player, query)) yield return contribution;
        foreach (var contribution in DragonBoatRuleContributions(player, query)) yield return contribution;
        foreach (var contribution in ShenwuActivityRuleContributions(player, query)) yield return contribution;
        foreach (var contribution in BossActivityRuleContributions(player, query)) yield return contribution;
        if (_modeDefinition.TeamActivity is null) yield break;
        var tier = ActivityTier(player.Seat);
        var regular = IsActivityPlayerSlot(player.Seat);
        var value = query switch
        {
            SkillRuleQuery.InitialHandSize => regular ? (tier >= 3 ? 1 : 0) : (tier >= 2 ? 1 : 0) + (tier >= 4 ? 1 : 0) + (tier >= 5 ? 1 : 0),
            SkillRuleQuery.DrawCount => regular ? (tier >= 4 ? 1 : 0) : (tier >= 2 ? 1 : 0) + (tier >= 4 ? 1 : 0),
            SkillRuleQuery.SlashLimit => (tier >= 3 ? 1 : 0) + (!regular && tier >= 5 ? 1 : 0),
            _ => 0
        };
        if (value != 0) yield return new FiniteRuleQueryContribution($"activity:{_modeDefinition.Id}:tier-{query}", SkillRuleOperation.Add, value);
        if (query == SkillRuleQuery.DrawCount && IsRuleWounded(player))
            foreach (var owner in _players.Where(p => !SamePublicTeam(p, player) && HasActivitySkill(p, "jiange:jueji")))
                yield return new FiniteRuleQueryContribution($"jiange:jueji:{owner.Seat}", SkillRuleOperation.Add, -1);
    }

    private int JiangeDistanceModifier(CharacterState source, CharacterState target) =>
        _modeDefinition.TeamActivity is null || SamePublicTeam(source, target) ? 0 :
            -_players.Count(p => p.Seat != source.Seat && SamePublicTeam(p, source) && HasActivitySkill(p, "jiange:jingfan"));

    private (CardUseEffectSource Source, int Cap)? JiangeDamageCap(IDamageAttempt attack)
    {
        if (_modeDefinition.TeamActivity is null) return null;
        var target = _players[attack.TargetSeat];
        var owner = _players.FirstOrDefault(p => SamePublicTeam(p, target) && HasActivitySkill(p, "jiange:chiying"));
        if (owner is null) return null;
        var grant = GetSkillBindingShard(owner).ActiveGrants.First(g => g.SkillId == "jiange:chiying");
        return (new CardUseEffectSource(grant.SkillId, "allied-damage-cap", owner.Seat, grant.SkillInstanceId), 1);
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 10)]
    private void AddJiangeDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms)
    {
        if (_modeDefinition.TeamActivity is not null)
            terms.Add(new("jiange:jingfan:allied-distance", JiangeDistanceModifier(query.Source, query.Target)));
    }

}
