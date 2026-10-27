using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2451's independent general4871 body with its complete cumulative source skills.</summary>
public sealed class StandardShanheSourceBuHengPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-buheng-2451-base";
    public const string HardId = "shanhe:source-buheng-2451-hard";
    public const string NightmareId = "shanhe:source-buheng-2451-nightmare";
    public const string ZhenguSkillId = "classic:zhengu";
    public const string ShengxiSkillId = "shanhe:source-shengxi-2118";
    public const string KeshouSkillId = StandardShanheSourceKeshouPackage.SkillId;
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-buheng-2451", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-source-keshou-993", StandardShanheSourceKeshouPackage.CurrentVersion)]);
    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-shengxi-2118");
        Add(BaseId, "base", []);
        Add(HardId, "hard", [ShengxiSkillId]);
        Add(NightmareId, "nightmare", [ShengxiSkillId, KeshouSkillId]);
        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "步珩", "supporter",
            ZhenguSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2451-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
