using System.Collections.ObjectModel;

namespace CardGame.Core.Campaigns;

/// <summary>A source region and its explicit Chapter.canGoTo links, independent of the local map.</summary>
public sealed class ShanheSourceRouteRegionDefinition
{
    public ShanheSourceRouteRegionDefinition(int sourceUniqueId, string name, int chapterNumber,
        int minimumSourceDifficultyId, int bossCityId, IEnumerable<int> nextRegionIds, IEnumerable<int> cityIds)
    {
        ArgumentNullException.ThrowIfNull(nextRegionIds);
        ArgumentNullException.ThrowIfNull(cityIds);
        if (sourceUniqueId <= 0 || string.IsNullOrWhiteSpace(name) || chapterNumber <= 0 ||
            minimumSourceDifficultyId <= 0 || bossCityId <= 0)
            throw new ArgumentException("A source route region lost its identity, chapter or opening condition.");
        SourceUniqueId = sourceUniqueId; Name = name; ChapterNumber = chapterNumber;
        MinimumSourceDifficultyId = minimumSourceDifficultyId; BossCityId = bossCityId;
        NextRegionIds = Array.AsReadOnly(nextRegionIds.ToArray());
        CityIds = Array.AsReadOnly(cityIds.ToArray());
        if (NextRegionIds.Any(id => id <= 0) || NextRegionIds.Distinct().Count() != NextRegionIds.Count ||
            CityIds.Count == 0 || CityIds.Any(id => id <= 0) || CityIds.Distinct().Count() != CityIds.Count || !CityIds.Contains(bossCityId))
            throw new ArgumentException("A source route region has invalid or duplicate references.");
    }

    public int SourceUniqueId { get; }
    public string Name { get; }
    public int ChapterNumber { get; }
    public int MinimumSourceDifficultyId { get; }
    public int BossCityId { get; }
    public IReadOnlyList<int> NextRegionIds { get; }
    public IReadOnlyList<int> CityIds { get; }
}

/// <summary>
/// One original Level row. An empty source name is retained; the numeric city identity must not
/// be confused with a same-numbered Fight row. Terrain uses the original cha_spell ID.
/// </summary>
public sealed class ShanheSourceRouteCityDefinition
{
    public ShanheSourceRouteCityDefinition(int sourceCityId, int sourceRegionId, string name, bool isBoss,
        int? sourceTerrainSkillId, int? universalBattlePoolId, IEnumerable<int> directBattleIds)
    {
        ArgumentNullException.ThrowIfNull(directBattleIds);
        if (sourceCityId <= 0 || sourceRegionId <= 0 || name is null || sourceTerrainSkillId is <= 0 || universalBattlePoolId is <= 0)
            throw new ArgumentException("A source route city has invalid identities.");
        SourceCityId = sourceCityId; SourceRegionId = sourceRegionId; Name = name; IsBoss = isBoss;
        SourceTerrainSkillId = sourceTerrainSkillId; UniversalBattlePoolId = universalBattlePoolId;
        DirectBattleIds = Array.AsReadOnly(directBattleIds.ToArray());
        if (DirectBattleIds.Any(id => id <= 0) || DirectBattleIds.Distinct().Count() != DirectBattleIds.Count)
            throw new ArgumentException("A source route city has invalid direct battle references.");
    }

    public int SourceCityId { get; }
    public int SourceRegionId { get; }
    public string Name { get; }
    public bool IsBoss { get; }
    public int? SourceTerrainSkillId { get; }
    public int? UniversalBattlePoolId { get; }
    public IReadOnlyList<int> DirectBattleIds { get; }
}

/// <summary>
/// Distinct references in UniversalGroup, including references absent from the same Fight table.
/// These are potential source candidates, not eligible choices: premise groups and weights are
/// deliberately not evaluated or invented by this source directory.
/// </summary>
public sealed class ShanheSourceRouteBattlePoolDefinition
{
    public ShanheSourceRouteBattlePoolDefinition(int sourcePoolId, IEnumerable<int> potentialBattleIds)
    {
        ArgumentNullException.ThrowIfNull(potentialBattleIds);
        SourcePoolId = sourcePoolId;
        PotentialBattleIds = Array.AsReadOnly(potentialBattleIds.ToArray());
        if (sourcePoolId <= 0 || PotentialBattleIds.Count == 0 || PotentialBattleIds.Any(id => id <= 0) ||
            PotentialBattleIds.Distinct().Count() != PotentialBattleIds.Count)
            throw new ArgumentException("A source battle pool has invalid or duplicate references.");
    }

    public int SourcePoolId { get; }
    public IReadOnlyList<int> PotentialBattleIds { get; }
}

/// <summary>An immutable source graph; it does not add local encounters, choose enemies or unlock routes.</summary>
public sealed class ShanheSourceRouteCatalog
{
    public ShanheSourceRouteCatalog(int seasonId, string sourceUrl, string sourceSha256, string memberSha256,
        int terrainMinimumSourceDifficultyId, IEnumerable<ShanheSourceRouteRegionDefinition> regions,
        IEnumerable<ShanheSourceRouteCityDefinition> cities, IEnumerable<ShanheSourceRouteBattlePoolDefinition> battlePools,
        IEnumerable<int> unresolvedBattleIds)
    {
        ArgumentNullException.ThrowIfNull(regions); ArgumentNullException.ThrowIfNull(cities);
        ArgumentNullException.ThrowIfNull(battlePools); ArgumentNullException.ThrowIfNull(unresolvedBattleIds);
        if (seasonId <= 0 || string.IsNullOrWhiteSpace(sourceUrl) || !IsHash(sourceSha256) || !IsHash(memberSha256) ||
            terrainMinimumSourceDifficultyId <= 0)
            throw new ArgumentException("A source route catalog requires its provenance and terrain opening condition.");
        SeasonId = seasonId; SourceUrl = sourceUrl; SourceSha256 = sourceSha256; MemberSha256 = memberSha256;
        TerrainMinimumSourceDifficultyId = terrainMinimumSourceDifficultyId;
        Regions = Array.AsReadOnly(regions.ToArray()); Cities = Array.AsReadOnly(cities.ToArray());
        BattlePools = Array.AsReadOnly(battlePools.ToArray());
        UnresolvedBattleIds = Array.AsReadOnly(unresolvedBattleIds.ToArray());
        if (Regions.Count == 0 || Regions.Any(row => row is null) || Cities.Count == 0 || Cities.Any(row => row is null) ||
            BattlePools.Any(row => row is null) || Regions.Select(row => row.SourceUniqueId).Distinct().Count() != Regions.Count ||
            Cities.Select(row => row.SourceCityId).Distinct().Count() != Cities.Count ||
            BattlePools.Select(row => row.SourcePoolId).Distinct().Count() != BattlePools.Count)
            throw new ArgumentException("The source graph must contain distinct regions, cities and pools.");
        ByRegionId = new ReadOnlyDictionary<int, ShanheSourceRouteRegionDefinition>(Regions.ToDictionary(row => row.SourceUniqueId));
        ByCityId = new ReadOnlyDictionary<int, ShanheSourceRouteCityDefinition>(Cities.ToDictionary(row => row.SourceCityId));
        ByBattlePoolId = new ReadOnlyDictionary<int, ShanheSourceRouteBattlePoolDefinition>(BattlePools.ToDictionary(row => row.SourcePoolId));
        if (Regions.SelectMany(region => region.CityIds).Count() != Cities.Count ||
            Regions.Any(region => region.NextRegionIds.Any(id => !ByRegionId.TryGetValue(id, out var next) || next.ChapterNumber != region.ChapterNumber + 1) ||
                region.CityIds.Any(id => !ByCityId.TryGetValue(id, out var city) || city.SourceRegionId != region.SourceUniqueId ||
                    city.IsBoss != (id == region.BossCityId))) ||
            Cities.Any(city => !ByRegionId.TryGetValue(city.SourceRegionId, out var region) || !region.CityIds.Contains(city.SourceCityId) ||
                city.UniversalBattlePoolId is { } pool && !ByBattlePoolId.ContainsKey(pool)))
            throw new ArgumentException("The source graph contains unresolved or inconsistent region, city or pool links.");
        PotentialBattleIds = Array.AsReadOnly(Cities.SelectMany(city => city.DirectBattleIds)
            .Concat(BattlePools.SelectMany(pool => pool.PotentialBattleIds)).Distinct().Order().ToArray());
        if (UnresolvedBattleIds.Any(id => !PotentialBattleIds.Contains(id)) ||
            UnresolvedBattleIds.Distinct().Count() != UnresolvedBattleIds.Count)
            throw new ArgumentException("Unresolved battles must be distinct references in this graph.");
    }

    public int SeasonId { get; }
    public string SourceUrl { get; }
    public string SourceSha256 { get; }
    public string MemberSha256 { get; }
    public int TerrainMinimumSourceDifficultyId { get; }
    public IReadOnlyList<ShanheSourceRouteRegionDefinition> Regions { get; }
    public IReadOnlyList<ShanheSourceRouteCityDefinition> Cities { get; }
    public IReadOnlyList<ShanheSourceRouteBattlePoolDefinition> BattlePools { get; }
    public IReadOnlyList<int> PotentialBattleIds { get; }
    public IReadOnlyList<int> UnresolvedBattleIds { get; }
    public IReadOnlyDictionary<int, ShanheSourceRouteRegionDefinition> ByRegionId { get; }
    public IReadOnlyDictionary<int, ShanheSourceRouteCityDefinition> ByCityId { get; }
    public IReadOnlyDictionary<int, ShanheSourceRouteBattlePoolDefinition> ByBattlePoolId { get; }

    public IReadOnlyList<int> GetPotentialBattleIds(int sourceCityId)
    {
        if (!ByCityId.TryGetValue(sourceCityId, out var city)) throw new ArgumentOutOfRangeException(nameof(sourceCityId));
        return city.UniversalBattlePoolId is { } pool
            ? Array.AsReadOnly(city.DirectBattleIds.Concat(ByBattlePoolId[pool].PotentialBattleIds).Distinct().Order().ToArray())
            : city.DirectBattleIds;
    }

    private static bool IsHash(string value) => value is { Length: 64 } && value.All(Uri.IsHexDigit);
}
