using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The complete source2467 soldier body and its cumulative S5 skills.</summary>
public sealed class StandardShanheSourceWuhuanRiderPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-wuhuan-rider-2467-base";
    public const string HardId = "shanhe:source-wuhuan-rider-2467-hard";
    public const string HellId = "shanhe:source-wuhuan-rider-2467-hell";
    public const string TuxiSkillId = "shanhe:source-tuxi-722";
    public const string JixiSkillId = "shanhe:source-jixi-1600";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-wuhuan-rider", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), "Source S5 difficulty must be 1 through 25.")
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-tuxi-722");
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-jixi-1600");
        Add(BaseId, "base", []);
        Add(HardId, "hard", [TuxiSkillId]);
        Add(HellId, "hell", [TuxiSkillId, JixiSkillId]);

        void Add(string id, string tier, string[] addedSkills) => builder.AddGeneral(new(id, "乌桓骑手",
            // General4861 is the source soldier, not Gongsun Yuan's body. The
            // source d=1000107 portrait is unverified; supporter is a local fallback.
            "supporter", "classic:huaiyi", "qun", 4, addedSkills)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2467-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
