namespace CardGame.Core;

// Query ports live for one synchronous calculation. Values stay lazy so each
// domain fact is read at its original stage against the supplied participant.
internal interface IMatchTurnNumericQuery : IProgramSelectionQuery
{
    string ModeId { get; }
    int Seat { get; }
    int? AssignedDefault { get; }
    IEnumerable<RuleQueryContribution> Contributions { get; }
    int MarketBonus { get; }
    int SharedRankBonus { get; }
    int GangshuBonus { get; }
    int ShunshiBonus { get; }
    int HengyeGrowth { get; }
    int AdditiveTurnBonus { get; }
    bool TracksFengjiRoundLedger { get; }
    int FengjiRoundAdjustment { get; }
    EquipmentDefinition EquipmentDefinition(CardKind kind);
    RuleQueryEvaluation Project(RuleQueryEvaluation evaluation);
}

internal interface IMatchDrawCountQuery : IMatchTurnNumericQuery
{
    int DefaultDrawCount { get; }
    IReadOnlyList<Card> EffectiveEquipment { get; }
    int DuliangNextDrawBonus { get; }
    int LuanxinNextDrawBonus { get; }
    int AlternatingNextDrawBonus { get; }
    int KuangxiangDrawPenalty { get; }
    int KongwuDrawPenalty { get; }
    int FuzhongDrawBonus { get; }
    int JuguanDrawAdjustment { get; }
    int QuxiDrawAdjustment { get; }
}

internal interface IMatchSlashLimitQuery : IMatchTurnNumericQuery
{
    int KuangguSlashBonus { get; }
    int CujunSlashBonus { get; }
    int XiezhiOverflowCount { get; }
    bool HasZhuangshiQuota { get; }
    int SequentialDeckShortfallBonus { get; }
    bool HasZhajiangPreUsePrivilege { get; }
    int QujiaSlashBonus { get; }
    int JuzhanSlashBonus { get; }
    bool HasYangweiBonus { get; }
    int DuojingSlashBonus { get; }
    int NeifaSlashBonus { get; }
    bool HasNextUnlimitedCard { get; }
    bool HasFriendQihuiAllowance { get; }
    bool HasMouWujingAllowance { get; }
    bool HasFriendYangmingQuota { get; }
    bool HasUnlimitedTurnBonus { get; }
    IReadOnlyList<Card> EquipmentCards(EquipmentSlot? excludedEquipmentSlot);
    int EquipmentSlashLimitBonus(CardKind kind, int printedBonus);
}

/// <summary>Composes draw counts and Slash quotas through their ordered numeric stages.</summary>
[ProgramRuntimeModule]
internal sealed class MatchTurnNumericQueries
{
    internal RuleQueryEvaluation EvaluateDrawCount<TQuery>(TQuery query) where TQuery : IMatchDrawCountQuery
    {
        var baseTerms = new List<RuleQueryBaseTerm>
        {
            new($"mode:{query.ModeId}:draw-count",
                query.AssignedDefault ?? query.DefaultDrawCount)
        };
        baseTerms.AddRange(query.EffectiveEquipment
            .Where(card => query.EquipmentDefinition(card.Kind).DrawCountBonus != 0)
            .Select(card => new RuleQueryBaseTerm(
                $"equipment:{card.Id}:draw-count",
                query.EquipmentDefinition(card.Kind).DrawCountBonus)));
        var contributions = query.Contributions.ToList();
        AddFiniteContribution(contributions, $"campaign:{query.Seat}:market:draw", query.MarketBonus);
        AddFiniteContribution(contributions, $"campaign:{query.Seat}:shared-rank:draw", query.SharedRankBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mobile-duliang:draw", query.DuliangNextDrawBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:shanhe-luanxin:draw", query.LuanxinNextDrawBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:alternating-next-draw", query.AlternatingNextDrawBonus);
        AddFiniteContribution(contributions, $"state:{query.Seat}:cao-xi:gangshu:draw", query.GangshuBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:shunshi:draw", query.ShunshiBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:kuangxiang:draw", -query.KuangxiangDrawPenalty);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mou-kongwu:draw", -query.KongwuDrawPenalty);
        AddFiniteContribution(contributions, $"state:{query.Seat}:hengye:growth", query.HengyeGrowth);
        AddFiniteContribution(contributions, $"state:{query.Seat}:anniversary-fuzhong:draw", query.FuzhongDrawBonus);
        AddFiniteContribution(contributions, $"turn:{query.Seat}:draw-count", query.AdditiveTurnBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:juguan:draw", query.JuguanDrawAdjustment);
        AddFiniteContribution(contributions, $"state:{query.Seat}:quxi:draw", query.QuxiDrawAdjustment);
        if (query.TracksFengjiRoundLedger)
            AddFiniteContribution(contributions, $"state:{query.Seat}:olfengji:round-draw",
                query.FengjiRoundAdjustment);
        return query.Project(RuleQueryService.Evaluate(
            SkillRuleQuery.DrawCount,
            new RuleQueryBounds(0, int.MaxValue),
            baseTerms,
            contributions));
    }

    internal RuleQueryEvaluation CalculateSlashUseLimit<TQuery>(TQuery query, int? excludedEquipmentId = null,
        EquipmentSlot? excludedEquipmentSlot = null) where TQuery : IMatchSlashLimitQuery
    {
        var baseTerms = new[]
        {
            new RuleQueryBaseTerm($"mode:{query.ModeId}:slash-use-limit",
                query.AssignedDefault ?? 1)
        };
        var contributions = query.Contributions.ToList();
        AddFiniteContribution(contributions, $"campaign:{query.Seat}:market:slash", query.MarketBonus);
        AddFiniteContribution(contributions, $"campaign:{query.Seat}:shared-rank:slash", query.SharedRankBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mobile:shi-kuanggu:slash-limit", query.KuangguSlashBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mobile:shi-cujun:slash-limit", query.CujunSlashBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mobile:shi-xiezhi:slash-limit", query.XiezhiOverflowCount);
        if (query.HasZhuangshiQuota) contributions.Add(new UnlimitedRuleQueryContribution("skill:mobile:shi-zhuangshi:quota"));
        AddFiniteContribution(contributions, $"issued:{query.Seat}:sequential-deck-shortfall:slash-limit", query.SequentialDeckShortfallBonus);
        if (query.HasZhajiangPreUsePrivilege) contributions.Add(new UnlimitedRuleQueryContribution("skill:mou-mobile:zhajiang:first-lost-hp-uses"));
        AddFiniteContribution(contributions, $"issued:{query.Seat}:yang-feng:qujia:slash-limit", query.QujiaSlashBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:boundary:juzhan-mobile:slash-limit", query.JuzhanSlashBonus);
        AddFiniteContribution(contributions, $"state:{query.Seat}:cao-xi:gangshu:slash-limit", query.GangshuBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:shunshi:slash-limit", query.ShunshiBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mou-mobile:yangwei:slash-limit", query.HasYangweiBonus ? 1 : 0);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mou-mobile:duojing:slash-limit", query.DuojingSlashBonus);
        if (query.NeifaSlashBonus is > 0 and var neifaBonus)
            AddFiniteContribution(contributions, $"turn:{query.Seat}:neifa:slash-limit", neifaBonus);
        if (query.HasNextUnlimitedCard) contributions.Add(new UnlimitedRuleQueryContribution($"skill:{query.Seat}:next-card-unlimited"));
        if (query.HasFriendQihuiAllowance) contributions.Add(new UnlimitedRuleQueryContribution($"skill:{query.Seat}:friend-qihui"));
        if (query.HasMouWujingAllowance) contributions.Add(new UnlimitedRuleQueryContribution($"skill:{query.Seat}:mou-wujing"));
        if (query.HasFriendYangmingQuota) contributions.Add(new UnlimitedRuleQueryContribution($"skill:{query.Seat}:friend-yangming"));
        foreach (var card in query.EquipmentCards(excludedEquipmentSlot).Where(card => card.Id != excludedEquipmentId))
        {
            var bonus = query.EquipmentSlashLimitBonus(card.Kind, query.EquipmentDefinition(card.Kind).SlashLimitBonus);
            if (bonus == int.MaxValue)
                contributions.Add(new UnlimitedRuleQueryContribution($"equipment:{card.Id}:slash-use-limit"));
            else
                AddFiniteContribution(contributions, $"equipment:{card.Id}:slash-use-limit", bonus);
        }
        AddFiniteContribution(contributions, $"state:{query.Seat}:hengye:growth", query.HengyeGrowth);
        AddFiniteContribution(contributions, $"turn:{query.Seat}:slash-limit",
            query.AdditiveTurnBonus);
        if (query.HasUnlimitedTurnBonus)
            contributions.Add(new UnlimitedRuleQueryContribution($"turn:{query.Seat}:slash-limit:unlimited"));
        if (query.TracksFengjiRoundLedger)
            AddFiniteContribution(contributions, $"state:{query.Seat}:olfengji:round-slash",
                query.FengjiRoundAdjustment);
        return query.Project(RuleQueryService.Evaluate(
            SkillRuleQuery.SlashLimit,
            new RuleQueryBounds(0, int.MaxValue),
            baseTerms,
            contributions));
    }

    private static void AddFiniteContribution(ICollection<RuleQueryContribution> contributions,
        string sourceId, int value) =>
        RuntimeSkills.RuntimeRules_Core_RuleQueries.AddFiniteContribution(contributions, sourceId, value);
}
