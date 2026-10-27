using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The source2487 soldier4861 body with its cumulative source290/488/723 skills.</summary>
public sealed class StandardShanheSourceWarriorPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-warrior-2487-base";
    public const string HardId = "shanhe:source-warrior-2487-hard";
    public const string HellId = "shanhe:source-warrior-2487-hell";
    public const string YoudiSkillId = "shanhe:source-youdi-488";
    public const string LuoyiSkillId = "shanhe:source-luoyi-723";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-warrior-2487", CurrentVersion,
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
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-youdi-488");
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-luoyi-723");
        Add(BaseId, "base", []);
        Add(HardId, "hard", [YoudiSkillId]);
        Add(HellId, "hell", [YoudiSkillId, LuoyiSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "武卫兵",
            "supporter", "ol:tuifeng", "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2487-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
