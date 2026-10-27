using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2486's complete general4871 body and exact cumulative source9015/823/3111 skills.</summary>
public sealed class StandardShanheSourceArmoryCommanderPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-armory-commander-2486-base";
    public const string HardId = "shanhe:source-armory-commander-2486-hard";
    public const string NightmareId = "shanhe:source-armory-commander-2486-nightmare";
    public const string XianshuaiSkillId = "ol:xianshuai";
    public const string JueyanSkillId = "shanhe:source-jueyan-823";
    public const string TuxingSkillId = "ol:tuxing";
    public const string JizhiSkillId = "boundary:jizhi-current";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-armory-commander-2486", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        // The classic dependency already registers all three reused abilities,
        // including the precise source870 Jizhi granted by the treasure branch.
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-jueyan-823");
        Add(BaseId, "1-10", []);
        Add(HardId, "11-15", [JueyanSkillId]);
        Add(NightmareId, "16-25", [JueyanSkillId, TuxingSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id,
            "武库统领", "supporter", XianshuaiSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2486-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
