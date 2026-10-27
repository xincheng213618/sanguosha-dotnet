using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2480's general4871 body with its cumulative source484/3205/889 skills.</summary>
public sealed class StandardShanheSourceXunYuePackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-xunyue-2480-base";
    public const string HardId = "shanhe:source-xunyue-2480-hard";
    public const string HellId = "shanhe:source-xunyue-2480-hell";
    public const string QiangzhiSkillId = "shanhe:source-qiangzhi-484";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-xunyue-2480", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-qiangzhi-484");
        Add(BaseId, "base", []);
        Add(HardId, "hard", ["ol:mingzhe"]);
        Add(HellId, "hell", ["ol:mingzhe", "ol:yuxu"]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "荀悦",
            "supporter", QiangzhiSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2480-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
