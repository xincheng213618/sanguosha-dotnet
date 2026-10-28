using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete ordinary group2492: Cheng Pu, Han Dang and Huang Gai in source order.</summary>
public sealed class StandardShanheSourceJiangdongTrioPackage : IGameContentPackage
{
    public const string ChengPuBaseId = "shanhe:source-jiangdong-trio-2492-chengpu-base";
    public const string ChengPuHardId = "shanhe:source-jiangdong-trio-2492-chengpu-hard";
    public const string ChengPuHellId = "shanhe:source-jiangdong-trio-2492-chengpu-hell";
    public const string HanDangBaseId = "shanhe:source-jiangdong-trio-2492-handang-base";
    public const string HanDangHardId = "shanhe:source-jiangdong-trio-2492-handang-hard";
    public const string HanDangNightmareId = "shanhe:source-jiangdong-trio-2492-handang-nightmare";
    public const string HuangGaiBaseId = "shanhe:source-jiangdong-trio-2492-huanggai-base";
    public const string HuangGaiHardId = "shanhe:source-jiangdong-trio-2492-huanggai-hard";
    public const string LihuoSkillId = "shanhe:source-lihuo-3160";
    public const string ChunlaoSkillId = "shanhe:source-chunlao-3161";
    public const string QixiSkillId = "classic:qixi";
    public const string GongqiSkillId = "classic:gongqi";
    public const string JiefanSkillId = "classic:jiefan";
    public const string AnjianSkillId = "classic:anjian";
    public const string KurouSkillId = "boundary:kurou";
    public const string ZhaxiangSkillId = "boundary:zhaxiang";
    public const string QiangxiSkillId = "classic:qiangxi";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-jiangdong-trio-2492", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetChengPuGeneralId(difficulty), GetHanDangGeneralId(difficulty), GetHuangGaiGeneralId(difficulty) });
    public static string GetChengPuGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => ChengPuBaseId, >= 11 and <= 20 => ChengPuHardId,
        >= 21 and <= 25 => ChengPuHellId, _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetHanDangGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => HanDangBaseId, >= 11 and <= 15 => HanDangHardId,
        >= 16 and <= 25 => HanDangNightmareId, _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetHuangGaiGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => HuangGaiBaseId, >= 11 and <= 25 => HuangGaiHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-jiangdong-trio-2492");
        Add(ChengPuBaseId, "程普", "cheng-pu", "cheng-pu", LihuoSkillId, []);
        Add(ChengPuHardId, "程普", "cheng-pu", "cheng-pu", LihuoSkillId, [ChunlaoSkillId]);
        Add(ChengPuHellId, "程普", "cheng-pu", "cheng-pu", LihuoSkillId, [ChunlaoSkillId, QixiSkillId]);
        Add(HanDangBaseId, "韩当", "han-dang", "han-dang", GongqiSkillId, []);
        Add(HanDangHardId, "韩当", "han-dang", "han-dang", GongqiSkillId, [JiefanSkillId]);
        Add(HanDangNightmareId, "韩当", "han-dang", "han-dang", GongqiSkillId, [JiefanSkillId, AnjianSkillId]);
        Add(HuangGaiBaseId, "黄盖", "boundary-huang-gai", "huang-gai", KurouSkillId, [ZhaxiangSkillId]);
        Add(HuangGaiHardId, "黄盖", "boundary-huang-gai", "huang-gai", KurouSkillId, [ZhaxiangSkillId, QiangxiSkillId]);
        void Add(string id, string name, string portrait, string character, string primary, IReadOnlyList<string> extra) =>
            builder.AddGeneral(new(id, name, portrait, primary, "wu", 4, extra)
            {
                InitialHp = 4, CharacterId = "character:" + character, VariantId = id["shanhe:".Length..],
                RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
            });
    }
}
