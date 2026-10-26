using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static class ShanheSourceRouteCatalogChecks
{
    public static void ExplicitSourceGraphAndPotentialPoolsStayImmutable()
    {
        var catalog = StandardShanheSourceRouteCatalog.Season5;
        Need(catalog.Regions.Count == 14 && catalog.Cities.Count == 112 && catalog.BattlePools.Count == 14,
            "The source graph contains all regions, locations and referenced pools, rather than local map guesses.");
        Need(catalog.ByRegionId[2].NextRegionIds.SequenceEqual([1, 6, 3, 7]) &&
            catalog.ByRegionId[4].NextRegionIds.SequenceEqual([8, 9, 5, 13]) &&
            catalog.ByRegionId[1].NextRegionIds.SequenceEqual([10]) &&
            catalog.ByRegionId[3].NextRegionIds.SequenceEqual([14]) &&
            catalog.ByRegionId[13].NextRegionIds.SequenceEqual([11, 12]),
            "Original Chapter.canGoTo links preserve both starting branches and their actual final destinations.");
        Need(catalog.TerrainMinimumSourceDifficultyId == 11 &&
            catalog.ByCityId[5011] is { SourceRegionId: 1, SourceTerrainSkillId: 13276, UniversalBattlePoolId: 14, IsBoss: false } &&
            catalog.ByCityId[5107] is { SourceRegionId: 10, SourceTerrainSkillId: null, UniversalBattlePoolId: 23, IsBoss: false } &&
            catalog.ByCityId[5148] is { SourceRegionId: 14, UniversalBattlePoolId: null, IsBoss: true } &&
            catalog.GetPotentialBattleIds(5148).SequenceEqual([2684, 2685, 2686, 2687, 22684, 22685, 22686, 22687]),
            "Source city IDs, original terrain spell IDs and direct boss battle references retain separate meanings.");
        Need(Enumerable.Range(5101, 7).All(id => catalog.GetPotentialBattleIds(id).Contains(2397)) &&
            catalog.Cities.All(city => city.Name.Length == 0) && catalog.UnresolvedBattleIds.Count > 0 &&
            catalog.UnresolvedBattleIds.All(catalog.PotentialBattleIds.Contains),
            "Mulu is a potential ordinary Luoyang pool encounter; unnamed locations and dangling source references remain explicit.");

        RejectMutation(() => ((IList<int>)catalog.ByRegionId[2].NextRegionIds)[0] = 14);
        RejectMutation(() => ((IList<int>)catalog.ByCityId[5148].DirectBattleIds).Clear());
        RejectMutation(() => ((IList<int>)catalog.GetPotentialBattleIds(5107)).Clear());
        RejectMutation(() => ((IDictionary<int, ShanheSourceRouteCityDefinition>)catalog.ByCityId).Clear());
        var exits = new[] { 2 }; var locations = new[] { 10 };
        var region = new ShanheSourceRouteRegionDefinition(1, "source", 1, 1, 10, exits, locations);
        exits[0] = 3; locations[0] = 99;
        Need(region.NextRegionIds.SequenceEqual([2]) && region.CityIds.SequenceEqual([10]),
            "The source directory freezes caller-owned collections before exposing them.");
    }

    private static void RejectMutation(Action action)
    {
        try { action(); }
        catch (NotSupportedException) { return; }
        throw new InvalidOperationException("Source graph collections must not expose mutable storage.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
