using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 group2498's soldier4861 body and cumulative source370/993/3121 skills.</summary>
public sealed class StandardShanheSourceYulinGuardPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-yulin-guard-2498-base";
    public const string HardId = "shanhe:source-yulin-guard-2498-hard";
    public const string HellId = "shanhe:source-yulin-guard-2498-hell";
    public const string JushouSkillId = "shanhe:source-jushou-370";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-yulin-guard-2498", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-source-keshou-993", StandardShanheSourceKeshouPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-jushou-370");
        Add(BaseId, "base", []);
        Add(HardId, "hard", [StandardShanheSourceKeshouPackage.SkillId]);
        Add(HellId, "hell", [StandardShanheSourceKeshouPackage.SkillId, "ol:qiongshou"]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "羽林军",
            "supporter", JushouSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2498-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
