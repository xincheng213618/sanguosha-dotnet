using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.Campaigns;

public enum ShanheSourceEnemyAttribute { Draw = 1, ExtraSlash = 2, ArmorGain = 3, Hp = 4, StartingCards = 5, Armor = 6, ExtraDamage = 7 }
public enum ShanheSourceEncounterCategory { Boss = 1, Elite = 2, Ordinary = 3 }

/// <summary>The seven attributes read by the public client's getEnemyGrowCnt; Hp is current HP, not MaxHp.</summary>
public sealed record ShanheSourceEnemyAttributes(int Hp = 0, int Draw = 0, int ExtraSlash = 0,
    int ArmorGain = 0, int StartingCards = 0, int Armor = 0, int ExtraDamage = 0)
{
    public int Get(ShanheSourceEnemyAttribute attribute) => attribute switch
    {
        ShanheSourceEnemyAttribute.Hp => Hp, ShanheSourceEnemyAttribute.Draw => Draw,
        ShanheSourceEnemyAttribute.ExtraSlash => ExtraSlash, ShanheSourceEnemyAttribute.ArmorGain => ArmorGain,
        ShanheSourceEnemyAttribute.StartingCards => StartingCards, ShanheSourceEnemyAttribute.Armor => Armor,
        ShanheSourceEnemyAttribute.ExtraDamage => ExtraDamage,
        _ => throw new ArgumentOutOfRangeException(nameof(attribute))
    };
}

public sealed record ShanheSourceEnemyDayGrowth(int SourceDifficultyId, int Day, ShanheSourceEnemyAttributes Attributes,
    int? HpHundredths = null, int? DrawHundredths = null);
public sealed record ShanheSourceEnemyChapterGrowth(int DifficultyGroupId, int Chapter, ShanheSourceEnemyAttributes Attributes);
public sealed record ShanheSourceEnemyCountGrowth(ShanheSourceEncounterCategory Category, int VisibleEnemyCount,
    ShanheSourceEnemyAttributes Percentages);
public sealed record ShanheSourceEnemyAppearanceGrowth(int Day, ShanheSourceEnemyAttributes Attributes);
public sealed record ShanheSourceEnemyTierAttributes(int DifficultyGroupId, ShanheSourceEnemyAttributes Attributes);

/// <summary>Verbatim seed Property inputs. Client goals: all=1, self=2, enemy=3, friend=10; Property type=1.</summary>
public sealed record ShanheSourceEnemySeedProperty(int Goal, int Type, int AttributeType, bool Subtract, int Value);

/// <param name="Day">Null means no chapterData. A present day is an exact lookup, not a clamp to the last source row.</param>
/// <param name="Chapter">The active run's passed-chapter count plus one; zero means the client is not currently in a run.</param>
/// <param name="VisibleEnemyCount">The source roster size excluding hide=1, not the number of enemies still alive.</param>
/// <param name="AppearanceDay">Null skips the optional EnemyShow lookup; current official configuration supplies no such rows.</param>
/// <param name="GeneralTierGroupId">The client's global CurrentBigDif for General attributes. Null uses the selected tier's group, valid when inspecting the current run. Pass it explicitly for a different historical run.</param>
public sealed record ShanheSourceEnemyGrowthContext(int SourceDifficultyId, int? Day, int Chapter,
    int VisibleEnemyCount, ShanheSourceEncounterCategory Category, bool IsFriend = false, int? AppearanceDay = null,
    int? GeneralTierGroupId = null);

public sealed record ShanheSourceEnemyGrowthStep(ShanheSourceEnemyAttribute Attribute, int Base,
    int DayGrowth, int AppearanceGrowth, int DifficultyGrowth, int ChapterGrowth, double DayFraction,
    int EnemyCountPercentage, double ScaledGrowth, int RoundedGrowth, int GeneralTierGrowth,
    int BeforeSeed, int SeedGrowth, int Result);

public sealed class ShanheSourceEnemyGrowthResult
{
    internal ShanheSourceEnemyGrowthResult(ShanheSourceEnemyAttributes attributes, IEnumerable<ShanheSourceEnemyGrowthStep> steps)
    { Attributes = attributes; Steps = Array.AsReadOnly(steps.ToArray()); }
    public ShanheSourceEnemyAttributes Attributes { get; }
    public IReadOnlyList<ShanheSourceEnemyGrowthStep> Steps { get; }
}

/// <summary>
/// Reproduces the normal-tier public-client attribute calculation, including its asymmetric rounding,
/// cumulative General att_PT..att_LY additions and final Property seed additions. It does not calculate
/// MaxHp, server battle packets, hidden server modifiers, skill/card replacement or armor recovery timing.
/// DifficultySelection.hprate/drawrate are not read by that client function and are not used here.
/// </summary>
public sealed class ShanheSourceEnemyGrowthCalculator
{
    private readonly IReadOnlyDictionary<(int Difficulty, int Day), ShanheSourceEnemyDayGrowth> _days;
    private readonly IReadOnlyDictionary<(int Group, int Chapter), ShanheSourceEnemyChapterGrowth> _chapters;
    private readonly IReadOnlyDictionary<(ShanheSourceEncounterCategory Category, int Count), ShanheSourceEnemyCountGrowth> _counts;
    private readonly IReadOnlyDictionary<int, ShanheSourceEnemyAppearanceGrowth> _appearances;

    public ShanheSourceEnemyGrowthCalculator(ShanheSourceDifficultyCatalog difficulties,
        IEnumerable<ShanheSourceEnemyDayGrowth> days, IEnumerable<ShanheSourceEnemyChapterGrowth> chapters,
        IEnumerable<ShanheSourceEnemyCountGrowth> counts, IEnumerable<ShanheSourceEnemyAppearanceGrowth> appearances)
    {
        Difficulties = difficulties ?? throw new ArgumentNullException(nameof(difficulties));
        DayRows = Array.AsReadOnly(days.ToArray()); ChapterRows = Array.AsReadOnly(chapters.ToArray());
        CountRows = Array.AsReadOnly(counts.ToArray()); AppearanceRows = Array.AsReadOnly(appearances.ToArray());
        if (DayRows.Any(row => row is null || row.Attributes is null || row.Day < 0 || row.SourceDifficultyId < 0 ||
                row.SourceDifficultyId > difficulties.MaximumDifficulty) ||
            ChapterRows.Any(row => row is null || row.Attributes is null || row.DifficultyGroupId <= 0 || row.Chapter <= 0) ||
            CountRows.Any(row => row is null || row.Percentages is null || !Enum.IsDefined(row.Category) || row.VisibleEnemyCount <= 0) ||
            AppearanceRows.Any(row => row is null || row.Attributes is null || row.Day < 0))
            throw new ArgumentException("A source growth row has invalid identity or attributes.");
        _days = new ReadOnlyDictionary<(int, int), ShanheSourceEnemyDayGrowth>(DayRows.ToDictionary(row => (row.SourceDifficultyId, row.Day)));
        _chapters = new ReadOnlyDictionary<(int, int), ShanheSourceEnemyChapterGrowth>(ChapterRows.ToDictionary(row => (row.DifficultyGroupId, row.Chapter)));
        _counts = new ReadOnlyDictionary<(ShanheSourceEncounterCategory, int), ShanheSourceEnemyCountGrowth>(CountRows.ToDictionary(row => (row.Category, row.VisibleEnemyCount)));
        _appearances = new ReadOnlyDictionary<int, ShanheSourceEnemyAppearanceGrowth>(AppearanceRows.ToDictionary(row => row.Day));
        ContentHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(ShanheCampaignJson.SerializeValue(new
        { Difficulties, DayRows, ChapterRows, CountRows, AppearanceRows })))).ToLowerInvariant();
    }

    public ShanheSourceDifficultyCatalog Difficulties { get; }
    public IReadOnlyList<ShanheSourceEnemyDayGrowth> DayRows { get; }
    public IReadOnlyList<ShanheSourceEnemyChapterGrowth> ChapterRows { get; }
    public IReadOnlyList<ShanheSourceEnemyCountGrowth> CountRows { get; }
    public IReadOnlyList<ShanheSourceEnemyAppearanceGrowth> AppearanceRows { get; }
    public string ContentHash { get; }

    public ShanheSourceEnemyGrowthResult Calculate(ShanheSourceEnemyAttributes basis, ShanheSourceEnemyGrowthContext context,
        IEnumerable<ShanheSourceEnemyTierAttributes>? generalTierAttributes = null,
        IEnumerable<ShanheSourceEnemySeedProperty>? seedProperties = null)
    {
        ArgumentNullException.ThrowIfNull(basis); ArgumentNullException.ThrowIfNull(context);
        if (context.Day is < 0 || context.AppearanceDay is < 0 || context.Chapter < 0 || context.VisibleEnemyCount <= 0 ||
            context.GeneralTierGroupId is < 0 || !Enum.IsDefined(context.Category))
            throw new ArgumentException("Source enemy context has invalid day, chapter, category or roster size.", nameof(context));
        var difficulty = Difficulties.Get(context.SourceDifficultyId);
        var tiers = generalTierAttributes?.ToArray() ?? [];
        var seeds = seedProperties?.ToArray() ?? [];
        if (tiers.Any(row => row is null || row.Attributes is null || row.DifficultyGroupId <= 0) || seeds.Any(row => row is null))
            throw new ArgumentException("Source enemy tier attributes or seed data is incomplete.");
        ShanheSourceEnemyDayGrowth? day = null;
        if (context.Day is { } dayIndex && !_days.TryGetValue((context.SourceDifficultyId, dayIndex), out day))
            _days.TryGetValue((0, dayIndex), out day);
        _chapters.TryGetValue((difficulty.GroupId, context.Chapter), out var chapter);
        _counts.TryGetValue((context.Category, context.VisibleEnemyCount), out var count);
        ShanheSourceEnemyAppearanceGrowth? appearance = null;
        if (context.Day is not null && context.AppearanceDay is { } appearanceIndex)
            _appearances.TryGetValue(appearanceIndex, out appearance);
        var steps = new List<ShanheSourceEnemyGrowthStep>(7);
        foreach (var attribute in Enum.GetValues<ShanheSourceEnemyAttribute>())
        {
            var dayValue = 0; var appearanceValue = 0; var difficultyValue = 0; var chapterValue = 0;
            var fraction = 0d; var scaled = 0d; var rounded = 0;
            var percentage = count?.Percentages.Get(attribute) ?? 100;
            if (!context.IsFriend)
            {
                dayValue = day?.Attributes.Get(attribute) ?? 0;
                appearanceValue = attribute == ShanheSourceEnemyAttribute.ExtraDamage ? 0 : appearance?.Attributes.Get(attribute) ?? 0;
                difficultyValue = DifficultyValue(difficulty.Modifiers, attribute);
                chapterValue = chapter?.Attributes.Get(attribute) ?? 0;
                fraction = attribute switch
                {
                    ShanheSourceEnemyAttribute.Hp => (day?.HpHundredths ?? 0) / 100d,
                    ShanheSourceEnemyAttribute.Draw => (day?.DrawHundredths ?? 0) / 100d,
                    _ => 0
                };
                scaled = (checked(dayValue + appearanceValue + difficultyValue + chapterValue) + fraction) * (percentage / 100d);
                rounded = checked((int)(scaled >= 1 || scaled < 0 ? Math.Floor(scaled) : Math.Ceiling(scaled)));
            }
            var tierValue = tiers.Where(row => row.DifficultyGroupId <= (context.GeneralTierGroupId ?? difficulty.GroupId))
                .Sum(row => row.Attributes.Get(attribute));
            var beforeSeed = Math.Max(attribute == ShanheSourceEnemyAttribute.Hp ? 1 : 0, checked(basis.Get(attribute) + rounded + tierValue));
            var seedValue = seeds.Where(seed => seed.Type == 1 && (seed.Goal == 1 || seed.Goal == (context.IsFriend ? 10 : 3)) &&
                MatchesSeedAttribute(attribute, seed.AttributeType)).Sum(seed => seed.Subtract ? checked(-seed.Value) : seed.Value);
            var result = checked(beforeSeed + seedValue);
            steps.Add(new(attribute, basis.Get(attribute), dayValue, appearanceValue, difficultyValue, chapterValue,
                fraction, percentage, scaled, rounded, tierValue, beforeSeed, seedValue, result));
        }
        int Value(ShanheSourceEnemyAttribute attribute) => steps.Single(step => step.Attribute == attribute).Result;
        return new(new(Value(ShanheSourceEnemyAttribute.Hp), Value(ShanheSourceEnemyAttribute.Draw),
            Value(ShanheSourceEnemyAttribute.ExtraSlash), Value(ShanheSourceEnemyAttribute.ArmorGain),
            Value(ShanheSourceEnemyAttribute.StartingCards), Value(ShanheSourceEnemyAttribute.Armor), Value(ShanheSourceEnemyAttribute.ExtraDamage)), steps);
    }

    private static int DifficultyValue(ShanheSourceDifficultyModifiers modifiers, ShanheSourceEnemyAttribute attribute) => attribute switch
    {
        ShanheSourceEnemyAttribute.Draw => modifiers.Draw, ShanheSourceEnemyAttribute.ExtraSlash => modifiers.Skill,
        ShanheSourceEnemyAttribute.ArmorGain => modifiers.GetArmor, ShanheSourceEnemyAttribute.Hp => modifiers.Hp,
        ShanheSourceEnemyAttribute.StartingCards => modifiers.CardNum, ShanheSourceEnemyAttribute.Armor => modifiers.Armor,
        ShanheSourceEnemyAttribute.ExtraDamage => modifiers.OtherAd,
        _ => throw new ArgumentOutOfRangeException(nameof(attribute))
    };

    private static bool MatchesSeedAttribute(ShanheSourceEnemyAttribute attribute, int sourceAttributeType) => attribute switch
    {
        ShanheSourceEnemyAttribute.Draw => sourceAttributeType == 5,
        ShanheSourceEnemyAttribute.ExtraSlash => sourceAttributeType == 7,
        ShanheSourceEnemyAttribute.ArmorGain => sourceAttributeType == 3,
        ShanheSourceEnemyAttribute.Hp => sourceAttributeType == 1,
        ShanheSourceEnemyAttribute.StartingCards => sourceAttributeType == 4,
        ShanheSourceEnemyAttribute.Armor => sourceAttributeType == 2,
        _ => false
    };
}
