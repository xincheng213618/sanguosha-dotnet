using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2452's three-HP general520 body and exact cumulative difficulty skills.</summary>
public sealed class StandardShanheSourceMaRiDiPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-maridi-2452-base";
    public const string HardId = "shanhe:source-maridi-2452-hard";
    public const string NightmareId = "shanhe:source-maridi-2452-nightmare";
    public const string HellId = "shanhe:source-maridi-2452-hell";
    public const string BingjieSkillId = "ol:bingjie";
    public const string ZhengdingSkillId = "ol:zhengding";
    public const string WeizhongSkillId = "shanhe:source-weizhong-239";
    public const string CaiwangSkillId = "ol:caiwang";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-maridi-2452", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 20 => NightmareId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-weizhong-239",
            tagOverrides: new Dictionary<string, SkillTag> { [WeizhongSkillId] = SkillTag.Locked });
        Add(BaseId, "base", []);
        Add(HardId, "hard", [ZhengdingSkillId]);
        Add(NightmareId, "nightmare", [ZhengdingSkillId, WeizhongSkillId]);
        Add(HellId, "hell", [ZhengdingSkillId, WeizhongSkillId, CaiwangSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "马日磾",
            "ol-ma-mi-di", BingjieSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:ma-mi-di", VariantId = "shanhe-source-2452-" + tier,
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
