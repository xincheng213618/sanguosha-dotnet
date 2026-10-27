using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2458 and source2464 retain their distinct original soldier bodies.</summary>
public sealed class StandardShanheSourceDemonRaiderPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-demon-raider-2458-base";
    public const string HardId = "shanhe:source-demon-raider-2458-hard";
    public const string HellId = "shanhe:source-demon-raider-2458-hell";
    public const string TrioBaseId = "shanhe:source-demon-trio-2464-raider-base";
    public const string TrioHardId = "shanhe:source-demon-trio-2464-raider-hard";
    public const string TrioHellId = "shanhe:source-demon-trio-2464-raider-hell";
    public const string QiangxiSkillId = "boundary:qiangxi";
    public const string JielueSkillId = "shanhe:source-jielue-1149";
    public const string LangdaoSkillId = "ol:langdao";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-demon-raider-2458", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetTrioGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => TrioBaseId,
        >= 11 and <= 20 => TrioHardId,
        >= 21 and <= 25 => TrioHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-jielue-1149",
            definition => definition with { Tags = SkillTag.Locked });
        Add(BaseId, 4861, "base", []);
        Add(HardId, 4861, "hard", [JielueSkillId]);
        Add(HellId, 4861, "hell", [JielueSkillId, LangdaoSkillId]);
        Add(TrioBaseId, 4863, "trio-base", []);
        Add(TrioHardId, 4863, "trio-hard", [JielueSkillId]);
        Add(TrioHellId, 4863, "trio-hell", [JielueSkillId, LangdaoSkillId]);

        void Add(string id, int sourceGeneral, string tier, string[] extra) => builder.AddGeneral(new(id,
            "魔军暴徒", "supporter", QiangxiSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-" + sourceGeneral,
            VariantId = "shanhe-source-" + (sourceGeneral == 4861 ? "2458-" : "2464-") + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
