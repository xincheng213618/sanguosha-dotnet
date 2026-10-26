using System.Collections.ObjectModel;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    public static IReadOnlyList<ShanheSchoolDefinition> CurrentSchools => SchoolMetadata.Definitions;
    public static IReadOnlyDictionary<int, ShanheTacticSchoolMembership> CurrentTacticSchoolMembershipBySourceId => SchoolMetadata.Members;

    private static ShanheCampaignDefinition WithRemainingSchoolComponents(ShanheCampaignDefinition definition)
    {
        definition = WithCurrentLateRoundTactics(definition);
        definition = WithCurrentTeamHealthTactic(definition);
        definition = WithCurrentWineTactics(definition);
        definition = WithCurrentZuiquanTactic(definition);
        definition = WithCurrentLowHpDrawTactics(definition);
        definition = WithCurrentErshengsanTactic(definition);
        definition = WithCurrentHeartBladeTactic(definition);
        definition = WithCurrentDualBladeTactics(definition);
        definition = WithCurrentHandLimitStockTactics(definition);
        definition = WithCurrentDoubleStealTactic(definition);
        definition = WithCurrentDoubleResponseTactics(definition);
        definition = WithCurrentTrickResponseTactics(definition);
        return WithCurrentGuandaoTactic(definition);
    }

    private static ShanheCampaignDefinition WithCurrentSchools(ShanheCampaignDefinition definition)
    {
        if (!definition.Id.StartsWith("shanhe:zhuhou-2026-", StringComparison.Ordinal)) return definition;
        return definition with
        {
            Schools = CurrentSchools,
            Rewards = definition.Rewards.Select(reward => reward with
            {
                SchoolMembership = ShopMetadataByRewardId.TryGetValue(reward.Id, out var metadata) &&
                    CurrentTacticSchoolMembershipBySourceId.TryGetValue(metadata.SourceRewardId, out var member)
                    ? member : reward.SchoolMembership
            }).ToArray()
        };
    }

    private static class SchoolMetadata
    {
        // Full manifest school recipes; source tactics.json hash 209ce819d57fc8985308376ecc24fe12d2c3ca94dec75b775578f34543774c0e.
        internal static readonly IReadOnlyList<ShanheSchoolDefinition> Definitions = Array.AsReadOnly(new ShanheSchoolDefinition[]
        {
            new(1, "资源流", 2230, "商店可无限刷新", [2, 2, 2, 1]),
            new(2, "过牌流", 2231, "手牌少于8时候补满8", [2, 2, 2, 1]),
            new(3, "多刀流", 2232, "出杀次数无限", [0, 2, 2, 2]),
            new(4, "伤害流", 2233, "所有伤害+1", [1, 1, 3, 2]),
            new(5, "卖血流", 2364, "准备阶段，你的体力调整为体力上限", [1, 0, 3, 2]),
            new(6, "队友流", 2235, "队友共享你的等阶效果（最低3阶效果）", [2, 2, 1, 0]),
            new(7, "残血流", 2236, "每回合首次濒死时回复到1体力值", [1, 2, 2, 0]),
            new(8, "后期流", 2255, "每过一轮则首张杀伤害+1", [2, 2, 1, 0]),
            new(9, "锦囊流", 2363, "你的伤害类锦囊牌，结算两次", [0, 3, 3, 1]),
        });

        // Only concrete tactic rows have membership; derived set-effect rows are not components.
        internal static readonly IReadOnlyDictionary<int, ShanheTacticSchoolMembership> Members =
            new ReadOnlyDictionary<int, ShanheTacticSchoolMembership>(new ShanheTacticSchoolMembership[]
            {
                new(2001, 4, 3),
                new(2002, 7, 3),
                new(2003, 7, 1),
                new(2004, 9, 2),
                new(2005, 9, 2),
                new(2006, 9, 3),
                new(2007, 4, 3),
                new(2008, 9, 2),
                new(2009, 3, 4),
                new(2010, 4, 1),
                new(2011, 2, 2),
                new(2012, 1, 3),
                new(2013, 1, 2),
                new(2014, 1, 2),
                new(2015, 1, 3),
                new(2016, 1, 2),
                new(2017, 1, 2),
                new(2018, 1, 1),
                new(2024, 1, 2),
                new(2025, 2, 1),
                new(2026, 2, 2),
                new(2027, 2, 3),
                new(2028, 5, 1),
                new(2030, 2, 2),
                new(2033, 3, 2),
                new(2034, 3, 3),
                new(2036, 4, 3),
                new(2037, 4, 4),
                new(2039, 3, 2),
                new(2040, 3, 4),
                new(2042, 3, 3),
                new(2043, 3, 4),
                new(2045, 4, 4),
                new(2048, 3, 1),
                new(2049, 3, 3),
                new(2051, 3, 2),
                new(2052, 3, 4),
                new(2054, 5, 2),
                new(2055, 5, 3),
                new(2056, 5, 4),
                new(2059, 2, 2),
                new(2060, 2, 3),
                new(2064, 2, 1),
                new(2065, 2, 2),
                new(2066, 2, 3),
                new(2069, 5, 2),
                new(2070, 5, 3),
                new(2071, 5, 1),
                new(2072, 8, 2),
                new(2073, 8, 3),
                new(2074, 8, 4),
                new(2076, 2, 2),
                new(2077, 2, 3),
                new(2078, 2, 4),
                new(2079, 2, 1),
                new(2080, 2, 2),
                new(2081, 2, 3),
                new(2082, 2, 1),
                new(2083, 2, 2),
                new(2084, 2, 4),
                new(2085, 2, 2),
                new(2086, 2, 3),
                new(2088, 2, 3),
                new(2089, 1, 2),
                new(2090, 1, 2),
                new(2091, 1, 2),
                new(2092, 1, 1),
                new(2093, 3, 2),
                new(2094, 5, 4),
                new(2095, 9, 3),
                new(2096, 9, 3),
                new(2097, 9, 3),
                new(2098, 4, 3),
                new(2099, 9, 2),
                new(2100, 4, 2),
                new(2101, 4, 4),
                new(2103, 2, 2),
                new(2104, 2, 2),
                new(2107, 1, 3),
                new(2108, 4, 2),
                new(2109, 4, 4),
                new(2111, 9, 2),
                new(2112, 1, 1),
                new(2116, 5, 4),
                new(2117, 5, 4),
                new(2127, 5, 3),
                new(2129, 4, 1),
                new(2130, 4, 2),
                new(2131, 5, 2),
                new(2132, 7, 1),
                new(2133, 7, 2),
                new(2135, 7, 4),
                new(2136, 1, 2),
                new(2137, 1, 3),
                new(2139, 1, 2),
                new(2140, 1, 3),
                new(2141, 1, 4),
                new(2142, 1, 4),
                new(2143, 2, 3),
                new(2144, 7, 2),
                new(2145, 1, 3),
                new(2146, 1, 4),
                new(2150, 8, 3),
                new(2151, 8, 2),
                new(2152, 8, 1),
                new(2153, 8, 2),
                new(2154, 8, 1),
                new(2155, 8, 1),
                new(2156, 8, 2),
                new(2157, 8, 1),
                new(2158, 8, 1),
                new(2159, 2, 2),
                new(2160, 2, 1),
                new(2161, 2, 3),
                new(2162, 7, 2),
                new(2163, 7, 3),
                new(2164, 7, 1),
                new(2165, 7, 2),
                new(2166, 7, 2),
                new(2167, 9, 3),
                new(2171, 4, 3),
                new(2172, 4, 4),
                new(2174, 7, 2),
                new(2175, 7, 4),
                new(2178, 1, 1),
                new(2179, 1, 1),
                new(2180, 1, 1),
                new(2181, 1, 3),
                new(2187, 1, 1),
                new(2188, 8, 3),
                new(2189, 8, 2),
                new(2190, 9, 4),
                new(2191, 5, 3),
                new(2192, 5, 4),
                new(2196, 7, 2),
                new(2197, 7, 3),
                new(2199, 6, 3),
                new(2200, 6, 2),
                new(2201, 6, 1),
                new(2202, 6, 1),
                new(2203, 6, 3),
                new(2204, 6, 2),
                new(2211, 5, 1),
                new(2212, 9, 2),
                new(2213, 1, 1),
                new(2214, 1, 1),
                new(2215, 1, 1),
                new(2216, 1, 2),
                new(2217, 1, 1),
                new(2218, 1, 2),
                new(2219, 6, 2),
                new(2220, 6, 3),
                new(2222, 1, 2),
                new(2223, 1, 3),
                new(2224, 1, 4),
                new(2226, 3, 1),
                new(2227, 5, 3),
                new(2228, 5, 1),
                new(2229, 5, 4),
                new(2238, 9, 2),
                new(2239, 9, 3),
                new(2240, 9, 4),
                new(2241, 6, 1),
                new(2242, 9, 2),
                new(2243, 6, 1),
                new(2244, 9, 2),
                new(2248, 9, 4),
                new(2249, 9, 2),
                new(2250, 9, 3),
                new(2251, 9, 4),
                new(2252, 1, 2),
                new(2253, 1, 2),
                new(2256, 9, 3),
                new(2257, 9, 3),
                new(2314, 5, 4),
                new(2319, 5, 1),
                new(2320, 5, 2),
                new(2321, 5, 3),
                new(2322, 5, 4),
                new(2329, 6, 1),
                new(2337, 9, 2),
                new(2360, 7, 1),
                new(2361, 4, 3),
                new(2362, 9, 2),
                new(2371, 5, 4),
                new(2372, 8, 3),
                new(2373, 8, 2),
                new(2374, 8, 1),
                new(2375, 8, 3),
                new(2376, 8, 2),
                new(2377, 8, 1),
                new(2378, 8, 3),
                new(2379, 8, 2),
                new(2380, 8, 1),
            }.ToDictionary(member => member.SourceId));
    }
}
