using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2468 keeps the ordinary capped Zhouxuan body357 and cumulative Weijing.</summary>
public sealed class StandardShanheSourceZhangHePackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-zhang-he-2468-base";
    public const string WeijingId = "shanhe:source-zhang-he-2468-weijing";
    public const string ZhouxuanSkillId = "ol:zhouxuan";
    public const string WeijingSkillId = "ol:weijing";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-zhang-he-2468", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 15 => BaseId,
        >= 16 and <= 25 => WeijingId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        // The classic dependency already supplies both exact ordinary abilities.
        // Starting-hand and Slash deltas belong to each source encounter row.
        Add(BaseId, "base", []);
        Add(WeijingId, "weijing", [WeijingSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            "张郃", "ol-sp-zhang-he", ZhouxuanSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:zhang-he",
            VariantId = "shanhe-source-2468-" + tier, RulesetId = "shanhe-s5-current-config",
            Gender = GeneralGender.Male
        });
    }
}
