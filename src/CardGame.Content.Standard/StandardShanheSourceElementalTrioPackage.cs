using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete source2472: Xuan Ming4067, Zhu Rong4028 and Qiongqi4038.</summary>
public sealed class StandardShanheSourceElementalTrioPackage : IGameContentPackage
{
    public const string WaterBaseId = "shanhe:source-elemental-trio-2472-water-base";
    public const string FireBaseId = "shanhe:source-elemental-trio-2472-fire-base";
    public const string FireHardId = "shanhe:source-elemental-trio-2472-fire-hard";
    public const string FireHellId = "shanhe:source-elemental-trio-2472-fire-hell";
    public const string QiongqiBaseId = "shanhe:source-elemental-trio-2472-qiongqi-base";
    public const string QiongqiNightmareId = "shanhe:source-elemental-trio-2472-qiongqi-nightmare";
    public const string QiongqiHellId = "shanhe:source-elemental-trio-2472-qiongqi-hell";
    public const string ZhouyuSkillId = "ol:zhouyu";
    public const string LianyuSkillId = "shanhe:source-lianyu-623";
    public const string MoyanSkillId = StandardShanheGuanpingCompanionPackage.MoyanSkillId;
    public const string HuihuoSkillId = StandardShanheRecruitmentPackage.HuihuoSkillId;
    public const string LeijieSkillId = "ol:leijie";
    public const string LuoleiSkillId = "shanhe:source-luolei-619";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-elemental-trio-2472", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-guanping-companion", new StandardShanheGuanpingCompanionPackage().Manifest.Version),
         new("shanhe-recruitment", new StandardShanheRecruitmentPackage().Manifest.Version)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetWaterGeneralId(difficulty), GetFireGeneralId(difficulty), GetQiongqiGeneralId(difficulty) });

    public static string GetWaterGeneralId(int difficulty) => difficulty is >= 1 and <= 25
        ? WaterBaseId : throw new ArgumentOutOfRangeException(nameof(difficulty));

    public static string GetFireGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => FireBaseId,
        >= 11 and <= 20 => FireHardId,
        >= 21 and <= 25 => FireHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public static string GetQiongqiGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 15 => QiongqiBaseId,
        >= 16 and <= 20 => QiongqiNightmareId,
        >= 21 and <= 25 => QiongqiHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-elemental-trio-2472", skill =>
            skill with { Tags = SkillTag.Locked, ExecutionForms = SkillExecutionForm.Trigger });
        Add(WaterBaseId, "水神玄冥", 4067, "4453", "god", GeneralGender.Female, 3, ZhouyuSkillId, []);
        Add(FireBaseId, "火神祝融", 4028, "4446", "god", GeneralGender.Male, 4, LianyuSkillId, []);
        Add(FireHardId, "火神祝融", 4028, "4446", "god", GeneralGender.Male, 4, LianyuSkillId, [MoyanSkillId]);
        Add(FireHellId, "火神祝融", 4028, "4446", "god", GeneralGender.Male, 4, LianyuSkillId, [MoyanSkillId, HuihuoSkillId]);
        Add(QiongqiBaseId, "穷奇", 4038, "4481", "qun", GeneralGender.Male, 4, null, []);
        Add(QiongqiNightmareId, "穷奇", 4038, "4481", "qun", GeneralGender.Male, 4, LeijieSkillId, []);
        Add(QiongqiHellId, "穷奇", 4038, "4481", "qun", GeneralGender.Male, 4, LeijieSkillId, [LuoleiSkillId]);

        void Add(string id, string name, int body, string portrait, string faction, GeneralGender gender,
            int hp, string? primary, IReadOnlyList<string> extra) =>
            // Source skins4453/4446/4481 have no confirmed local image mappings.
            builder.AddGeneral(new(id, name, "supporter", primary, faction, hp, extra)
            {
                InitialHp = hp, CharacterId = "character:shanhe-source-general-" + body,
                VariantId = id["shanhe:".Length..] + "-portrait" + portrait + "-fallback",
                RulesetId = "shanhe-s5-current-config", Gender = gender
            });
    }
}
