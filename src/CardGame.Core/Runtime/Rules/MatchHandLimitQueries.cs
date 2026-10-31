namespace CardGame.Core;

// Each port is scoped to one synchronous calculation. Facts retain the supplied
// participant and are read only when their original arithmetic stage is reached.
internal interface IMatchHandLimitQuery : IProgramSelectionQuery
{
    int Seat { get; }
    bool HasClassicBuqu { get; }
    bool HasBoundaryBuqu { get; }
    int BuquWoundCount { get; }
    int? AssignedDefault { get; }
    int? HappyQiaoHandLimitBase { get; }
    int Hp { get; }
    IEnumerable<RuleQueryContribution> Contributions { get; }
    int YingziBonus { get; }
    int RunweiBonus { get; }
    int YongzuBonus { get; }
    int ShunshiBonus { get; }
    int OriginalHandBonus { get; }
    int XiugengBonus { get; }
    int XiezhiOverflowCount { get; }
    int KejiArmorBonus { get; }
    IEnumerable<RuleQueryContribution> ProgramDamageContributions { get; }
    IEnumerable<RuleQueryContribution> PersistentContributions { get; }
    IEnumerable<RuleQueryContribution> PingxiangContributions { get; }
    IEnumerable<RuleQueryContribution> ShenZfContributions { get; }
    IEnumerable<RuleQueryContribution> LiuLiContributions { get; }
    IEnumerable<RuleQueryContribution> YanXiangContributions { get; }
    IEnumerable<RuleQueryContribution> HuCheErContributions { get; }
    bool HasLiushiEffect { get; }
    int LiushiMarkerCount { get; }
    IEnumerable<RuleQueryContribution> ActorPenaltyContributions { get; }
    int XionghuoPenalty { get; }
    int AdditiveTurnBonus { get; }
    IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> FactionPolicies { get; }
    int LivingOtherFactionCount(string? factionId);
    int HengyeGrowth { get; }
    RuleQueryEvaluation ApplyEquipmentSuit(RuleQueryEvaluation evaluation);
    RuleQueryEvaluation ApplyZhangYiKangrui(RuleQueryEvaluation evaluation);
    RuleQueryEvaluation ApplyBoundaryZhuZhi(RuleQueryEvaluation evaluation);
    RuleQueryEvaluation ApplyBoundaryLiuShan(RuleQueryEvaluation evaluation);
    RuleQueryEvaluation ApplyHanSongYinbi(RuleQueryEvaluation evaluation);
    RuleQueryEvaluation ApplyMinimumPolicy(RuleQueryEvaluation evaluation);
}

/// <summary>Composes hand limits through their original ordered numeric and projection stages.</summary>
[ProgramRuntimeModule]
internal sealed class MatchHandLimitQueries
{
    internal RuleQueryEvaluation EvaluateHandLimit<TQuery>(TQuery query, bool suppressHanSongYinbi = false)
        where TQuery : IMatchHandLimitQuery
    {
        var woundCount = query.HasClassicBuqu || query.HasBoundaryBuqu ? query.BuquWoundCount : 0;
        var baseTerms = new List<RuleQueryBaseTerm>
        {
            woundCount > 0
                ? new RuleQueryBaseTerm($"state:{query.Seat}:classic:buqu:wounds", woundCount)
                : new RuleQueryBaseTerm($"state:{query.Seat}:current-hp",
                    query.AssignedDefault ?? query.HappyQiaoHandLimitBase ?? Math.Max(0, query.Hp))
        };
        var contributions = query.Contributions.ToList();
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mou-mobile:yingzi:hand-limit", query.YingziBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:lu-yu-sheng:runwei:hand-limit", query.RunweiBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:cao-teng:yongzu:hand-limit", query.YongzuBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:shunshi:hand-limit", query.ShunshiBonus);
        AddFiniteContribution(contributions, $"state:{query.Seat}:original-hand:hand-limit", query.OriginalHandBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mobile:shi-xiugeng:hand-limit", query.XiugengBonus);
        AddFiniteContribution(contributions, $"issued:{query.Seat}:mobile:shi-xiezhi:hand-limit", query.XiezhiOverflowCount);
        AddFiniteContribution(contributions, $"state:{query.Seat}:mou-mobile:keji:armor-hand-limit", query.KejiArmorBonus);
        contributions.AddRange(query.ProgramDamageContributions);
        contributions.AddRange(query.PersistentContributions);
        contributions.AddRange(query.PingxiangContributions);
        contributions.AddRange(query.ShenZfContributions);
        contributions.AddRange(query.LiuLiContributions);
        contributions.AddRange(query.YanXiangContributions);
        contributions.AddRange(query.HuCheErContributions);
        if (query.HasLiushiEffect)
            AddFiniteContribution(contributions, $"state:{query.Seat}:liushi:hand-limit", -query.LiushiMarkerCount);
        contributions.AddRange(query.ActorPenaltyContributions);
        AddFiniteContribution(contributions, $"turn:{query.Seat}:xionghuo:hand-limit", -query.XionghuoPenalty);
        AddFiniteContribution(contributions, $"turn:{query.Seat}:hand-limit", query.AdditiveTurnBonus);
        foreach (var (source, policy) in query.FactionPolicies)
        {
            var allies = query.LivingOtherFactionCount(policy.FactionId);
            AddFiniteContribution(contributions,
                $"skill:{source.SkillId}:{source.SkillInstanceId}:policy:{policy.Id}",
                checked(allies * policy.Value));
        }
        AddFiniteContribution(contributions, $"state:{query.Seat}:hengye:growth", query.HengyeGrowth);
        var ordinary = query.ApplyZhangYiKangrui(query.ApplyEquipmentSuit(RuleQueryService.Evaluate(
            SkillRuleQuery.HandLimit,
            new RuleQueryBounds(0, int.MaxValue),
            baseTerms,
            contributions)));
        ordinary = query.ApplyBoundaryZhuZhi(ordinary);
        ordinary = query.ApplyBoundaryLiuShan(ordinary);
        ordinary = suppressHanSongYinbi ? ordinary : query.ApplyHanSongYinbi(ordinary);
        return query.ApplyMinimumPolicy(ordinary);
    }

    private static void AddFiniteContribution(ICollection<RuleQueryContribution> contributions,
        string sourceId, int value) =>
        RuntimeSkills.RuntimeRules_Core_RuleQueries.AddFiniteContribution(contributions, sourceId, value);
}
