using System.Collections.ObjectModel;

namespace CardGame.Core.Campaigns;

public enum ShanheGuideTaskKind
{
    WinExpeditionWithGeneral,
    ReachExpeditionScoreWithGeneral,
    WinSpecifiedBattle,
    WinBattleInSpecifiedCity
}

/// <summary>Unmodified source conditions. Local difficulty and score evidence are separate concerns.</summary>
public sealed record ShanheGuideTaskDefinition
{
    internal ShanheGuideTaskDefinition(int sourceId, int stage, string name, string description, ShanheGuideTaskKind kind,
        int sourceRequirementType, int minimumSourceDifficultyId, int requiredCount, int? minimumScore,
        IEnumerable<int> generalIds, IEnumerable<int> targetIds, IEnumerable<int> battleIds, IEnumerable<int> cityIds)
    {
        SourceId = sourceId; Stage = stage; Name = name; Description = description; Kind = kind;
        SourceRequirementType = sourceRequirementType; MinimumSourceDifficultyId = minimumSourceDifficultyId;
        RequiredCount = requiredCount; MinimumScore = minimumScore;
        SourceGeneralIds = Array.AsReadOnly(generalIds.ToArray());
        SourceTargetIds = Array.AsReadOnly(targetIds.ToArray());
        SourceBattleIds = Array.AsReadOnly(battleIds.ToArray());
        SourceCityIds = Array.AsReadOnly(cityIds.ToArray());
    }

    public int SourceId { get; }
    public int Stage { get; }
    public string Name { get; }
    public string Description { get; }
    public ShanheGuideTaskKind Kind { get; }
    public int SourceRequirementType { get; }
    public int MinimumSourceDifficultyId { get; }
    public int RequiredCount { get; }
    public int? MinimumScore { get; }
    public IReadOnlyList<int> SourceGeneralIds { get; }
    public IReadOnlyList<int> SourceTargetIds { get; }
    public IReadOnlyList<int> SourceBattleIds { get; }
    public IReadOnlyList<int> SourceCityIds { get; }
    public int BookReward => 10;
    public int SourceRewardItemId => 14000105;
    public bool RequiresScore => Kind == ShanheGuideTaskKind.ReachExpeditionScoreWithGeneral;
    public bool SupportedScore => false;
    public string VerificationNote => RequiresScore
        ? "已核任务阈值，尚无可验证的原版战绩计分公式；不可据本地自拟分数结算图册。"
        : "在2026配置复现的25档中按实际来源难度与战果累计，达成后自动领取一次10册；旧本地三档不计入。";
}

/// <summary>The six chapter tasks shown by the 2026 returning-season guide.</summary>
public static class ShanheGuideTaskCatalog
{
    public const string SourceUrl = "https://s.xiaochao.org/data/tasks.json";
    public const string GuideUrl = "https://club.sanguosha.com/thread-2364517-1-1.html";
    public const bool HasVerifiedScoreFormula = false;

    public static IReadOnlyList<ShanheGuideTaskDefinition> Tasks { get; } = Array.AsReadOnly(new[]
    {
        new ShanheGuideTaskDefinition(200347, 1, "联军举旌", "用袁术通关普通1以上难度",
            ShanheGuideTaskKind.WinExpeditionWithGeneral, 92, 1, 1, null,
            [100, 215, 646], [], [], []),
        new(200357, 1, "诸侯会盟", "用曹操通关普通2以上难度",
            ShanheGuideTaskKind.WinExpeditionWithGeneral, 92, 2, 1, null,
            [15, 311, 661, 713], [], [], []),
        new(200364, 1, "盟主初彰", "用袁绍在中等1以上难度中战绩分达到40000",
            ShanheGuideTaskKind.ReachExpeditionScoreWithGeneral, 97, 6, 1, 40000,
            [41, 450, 623], [], [], []),
        new(200365, 2, "武圣立威", "用关羽在中等3以上难度中战绩分达到60000",
            ShanheGuideTaskKind.ReachExpeditionScoreWithGeneral, 97, 8, 1, 60000,
            [300, 2, 593, 201], [], [], []),
        new(200352, 2, "虎牢战吕", "在中等1以上难度通过关卡无双奉先3次",
            ShanheGuideTaskKind.WinSpecifiedBattle, 93, 6, 3, null,
            [], [2644, 2645, 22644, 22645], [2644, 2645, 22644, 22645], [5048]),
        new(200358, 2, "一炬皆倾", "在困难1以上难度通过洛阳-碳墟任意战斗关卡",
            ShanheGuideTaskKind.WinBattleInSpecifiedCity, 94, 11, 1, null,
            // Requirement 94 names a city. The site's derived battle lookup collides
            // with battle 5107 (an unrelated Wu prince); retain only the typed city.
            [], [5107], [], [5107])
    });

    public static IReadOnlyDictionary<int, ShanheGuideTaskDefinition> BySourceId { get; } =
        new ReadOnlyDictionary<int, ShanheGuideTaskDefinition>(Tasks.ToDictionary(task => task.SourceId));

    public static string SourceDifficultyName(int sourceDifficultyId)
    {
        if (sourceDifficultyId is < 1 or > 25)
            throw new ArgumentOutOfRangeException(nameof(sourceDifficultyId), "The source uses five groups of five difficulty tiers.");
        var group = (sourceDifficultyId - 1) / 5;
        return new[] { "普通", "中等", "困难", "噩梦", "炼狱" }[group] + ((sourceDifficultyId - 1) % 5 + 1)
            .ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
