using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete source2478 officer4871 body and its cumulative source14075/451 skills.</summary>
public sealed class StandardShanheSourceWuCongPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-wucong-2478-base";
    public const string HardId = "shanhe:source-wucong-2478-hard";
    public const string TiepaoSkillId = "shanhe:source-tiepao-14075";
    public const string AnjianSkillId = "classic:anjian";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-wucong-2478", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 25 => HardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-tiepao-14075");
        Add(BaseId, "base", []);
        Add(HardId, "hard", [AnjianSkillId]);
        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "吴琮", "supporter",
            TiepaoSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2478-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
