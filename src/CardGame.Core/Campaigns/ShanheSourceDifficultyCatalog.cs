using System.Collections.ObjectModel;

namespace CardGame.Core.Campaigns;

/// <summary>
/// Verbatim integers from the source modifiers object. In particular, HpRate and DrawRate
/// are not an executable percentage formula, and GetArmor does not specify its trigger timing.
/// </summary>
public sealed record ShanheSourceDifficultyModifiers(
    int Hp, int Draw, int CardNum, int Skill, int Armor, int GetArmor, int OtherAd,
    int KillGetCard, int HpRate, int DrawRate, int? HpSave = null);

/// <summary>
/// Raw official client availability fields. OpenTimeSpec overrides OpenFlag in the client;
/// the timestamp text has no timezone suffix and must not silently become UTC.
/// </summary>
public sealed record ShanheSourceDifficultyAvailability(
    int? PreviousSourceId, int RequiredPlayerLevel, int OpenFlag, string? OpenTimeSpec,
    string? UnlockDescription, string? UnlockDescription1);

public sealed record ShanheSourceDifficultyOfficialEvidence(
    string ConfigUrl, string ConfigSha256, string MemberName, string ClientBundleUrl,
    string ClientBundleSha256, string VerifiedOn);

/// <summary>A source tier, independent of the locally authored campaign difficulty.</summary>
public sealed record ShanheSourceDifficultyDefinition(
    int SourceId, int GroupId, string GroupName, string Name, string Description,
    string RewardSpec, ShanheSourceDifficultyModifiers Modifiers)
{
    public int? SourceTierInGroup { get; init; }
    public ShanheSourceDifficultyAvailability? Availability { get; init; }
    /// <summary>Verbatim recSlotCtn field; the catalog does not infer a combat skill-slot grant.</summary>
    public int? RecSlotCtn { get; init; }
}

/// <summary>
/// An immutable source snapshot. Descriptions and reward item IDs remain source metadata;
/// this catalog neither calculates combat statistics nor grants rewards or unlocks.
/// </summary>
public sealed class ShanheSourceDifficultyCatalog
{
    public ShanheSourceDifficultyCatalog(int seasonId, int maximumDifficulty, string sourceUrl,
        string generatedAt, string groupUnlockRuleText, IEnumerable<ShanheSourceDifficultyDefinition> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);
        if (seasonId <= 0) throw new ArgumentOutOfRangeException(nameof(seasonId));
        if (maximumDifficulty <= 0) throw new ArgumentOutOfRangeException(nameof(maximumDifficulty));
        if (string.IsNullOrWhiteSpace(sourceUrl) || string.IsNullOrWhiteSpace(generatedAt) || string.IsNullOrWhiteSpace(groupUnlockRuleText))
            throw new ArgumentException("A source difficulty catalog requires its provenance and unlock evidence.");
        var rows = entries.ToArray();
        if (rows.Length != maximumDifficulty || rows.Any(row => row is null) ||
            !rows.Select(row => row.SourceId).Order().SequenceEqual(Enumerable.Range(1, maximumDifficulty)))
            throw new ArgumentException("Source difficulties must cover their declared IDs exactly once.", nameof(entries));
        foreach (var row in rows)
        {
            if (row.GroupId <= 0 || string.IsNullOrWhiteSpace(row.GroupName) || string.IsNullOrWhiteSpace(row.Name) ||
                row.Description is null || row.RewardSpec is null || row.Modifiers is null)
                throw new ArgumentException("A source difficulty row lost a required source field.", nameof(entries));
        }
        if (rows.GroupBy(row => row.GroupId).Any(group => group.Select(row => row.GroupName).Distinct(StringComparer.Ordinal).Count() != 1))
            throw new ArgumentException("A source difficulty group has conflicting names.", nameof(entries));
        SeasonId = seasonId; MaximumDifficulty = maximumDifficulty; SourceUrl = sourceUrl;
        GeneratedAt = generatedAt; GroupUnlockRuleText = groupUnlockRuleText;
        Entries = Array.AsReadOnly(rows.OrderBy(row => row.SourceId).ToArray());
        BySourceId = new ReadOnlyDictionary<int, ShanheSourceDifficultyDefinition>(Entries.ToDictionary(row => row.SourceId));
    }

    public int SeasonId { get; }
    public int MaximumDifficulty { get; }
    public string SourceUrl { get; }
    public string GeneratedAt { get; }
    /// <summary>The season-wide explanation; official row availability can provide more specific prerequisites.</summary>
    public string GroupUnlockRuleText { get; }
    public ShanheSourceDifficultyOfficialEvidence? OfficialEvidence { get; init; }
    public IReadOnlyList<ShanheSourceDifficultyDefinition> Entries { get; }
    public IReadOnlyDictionary<int, ShanheSourceDifficultyDefinition> BySourceId { get; }

    public ShanheSourceDifficultyDefinition Get(int sourceDifficultyId) => BySourceId.TryGetValue(sourceDifficultyId, out var row)
        ? row : throw new ArgumentOutOfRangeException(nameof(sourceDifficultyId), "The source difficulty is absent from this season snapshot.");
}
