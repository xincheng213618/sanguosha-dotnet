namespace CardGame.Core;

internal sealed partial class WushuangModeFeature
{
    internal int WushuangExtraSkillCapacity(int seat) => 2 + (HasWushuangTactic(seat, 2293) ? 1 : 0);

    internal static bool IsWushuangOriginalGrant(SkillGrant grant) => grant.SourceId is
        CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource or WushuangOriginalReplacementSource;

    internal IEnumerable<string> WushuangReplaceableOriginalSkills(int seat) => HasWushuangTactic(seat, 2221)
        ? _host.Player(seat).SkillGrants.Where(g => g.IsEnabled && IsWushuangOriginalGrant(g)).Select(g => g.SkillId).Distinct(StringComparer.Ordinal)
        : [];

    internal void ChangeWushuangFreeRerolls(int seat, int delta, string reason)
    {
        var progress = _players[seat];
        var count = checked(progress.FreeRerolls + delta);
        if (count < 0) throw new InvalidOperationException("Wushuang free refreshes cannot be negative.");
        progress.FreeRerolls = count;
        _host.PublishFact(new WushuangFreeRerollsChangedEvent(seat, count, reason));
    }

    internal bool HasWushuangTactic(int seat, int id) =>
        _players.TryGetValue(seat, out var progress) && progress.Tactics.Contains(id);

    internal bool IsWushuangHandLimitExempt(int seat, CardKind kind) =>
        kind == CardKind.Dodge && HasWushuangTactic(seat, 2002) ||
        kind == CardKind.Peach && HasWushuangTactic(seat, 2003);

    internal IEnumerable<RuleQueryContribution> WushuangNumericRuleContributions(int seat, SkillRuleQuery query)
    {
        if (!_players.TryGetValue(seat, out var progress)) yield break;
        var owner = _host.Player(seat);
        foreach (var id in progress.Tactics)
        {
            var amount = (id, query) switch
            {
                (2153, SkillRuleQuery.DrawCount) when _host.RoundNumber >= 3 => 1,
                (2154, SkillRuleQuery.DrawCount) when _host.RoundNumber >= 5 => 1,
                (2155, SkillRuleQuery.DrawCount) when _host.RoundNumber >= 7 => 1,
                (2156, SkillRuleQuery.SlashLimit) when _host.RoundNumber >= 3 => 1,
                (2157, SkillRuleQuery.SlashLimit) when _host.RoundNumber >= 4 => 1,
                (2158, SkillRuleQuery.SlashLimit) when _host.RoundNumber >= 7 => 1,
                (2164, SkillRuleQuery.DrawCount) when owner.Hp <= 3 => 1,
                (2165, SkillRuleQuery.DrawCount) when owner.Hp <= 5 => 1,
                (2282, SkillRuleQuery.SlashLimit) when progress.Tokens >= 3 => 1,
                (2283, SkillRuleQuery.DrawCount) when progress.Tokens >= 7 => 1,
                (2284, SkillRuleQuery.HandLimit) when progress.Tokens >= 3 => 1,
                (2226, SkillRuleQuery.SlashLimit) => 2 * Math.Max(0, WushuangExtraSkillCapacity(owner.Seat) -
                    owner.SkillGrants.Count(g => g.SourceId == WushuangGrantSource)),
                _ => 0
            };
            if (amount != 0) yield return new FiniteRuleQueryContribution($"wushuang:{owner.Seat}:{id}:{query}", SkillRuleOperation.Add, amount);
        }
    }

    internal RuleQueryEvaluation ApplyWushuangEnemyQuota(int seat, RuleQueryEvaluation ordinary, SkillRuleQuery query)
    {
        var tactic = query == SkillRuleQuery.DrawCount ? 2298 : 2299;
        if (!_host.Players.Any(p => p.IsAlive && p.Seat != seat && HasWushuangTactic(p.Seat, tactic)) ||
            ordinary.Value is not FiniteRuleQueryValue finite) return ordinary;
        var value = Math.Min(finite.Value, Math.Max(query == SkillRuleQuery.DrawCount ? 2 : 1, finite.Value - 1));
        var contribution = new FiniteRuleQueryContribution($"wushuang:{seat}:enemy:{tactic}", SkillRuleOperation.Add, value - finite.Value);
        return new(new FiniteRuleQueryValue(value, Array.AsReadOnly(ordinary.Value.Contributions.Append(contribution).ToArray())),
            ordinary.BaseTerms, Array.AsReadOnly(ordinary.Stages.Append(new RuleQueryStageSnapshot("wushuang-enemy-quota",
                finite.Value, false, value, Array.AsReadOnly<RuleQueryContribution>([contribution]))).ToArray()));
    }

    internal int WushuangSourceDamageBonus(WushuangDamageFacts attack)
    {
        if (attack.IsSourceLess || attack.IsChainPropagation ||
            !_players.TryGetValue(attack.SourceSeat, out var source)) return 0;
        var hp = _host.Player(attack.SourceSeat).Hp;
        var bonus = (source.Tactics.Contains(2162) && hp <= 2 ? 1 : 0) +
            (source.Tactics.Contains(2163) && hp <= 3 ? 1 : 0);
        if (attack.CardUserSeat == attack.SourceSeat && attack.EffectiveCardKind is { } kind && _host.IsSlash(kind))
            bonus += (source.Tactics.Contains(2150) && _host.RoundNumber >= 3 ? 1 : 0) +
                (source.Tactics.Contains(2151) && _host.RoundNumber >= 5 ? 1 : 0) +
                (source.Tactics.Contains(2152) && _host.RoundNumber >= 7 ? 1 : 0);
        return bonus;
    }
}
