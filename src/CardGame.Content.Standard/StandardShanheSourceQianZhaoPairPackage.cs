using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete ordinary group2471: source580 followed by source4861.</summary>
public sealed class StandardShanheSourceQianZhaoPairPackage : IGameContentPackage
{
    public const string QianZhaoBaseId = "shanhe:source-qianzhao-pair-2471-qianzhao-base";
    public const string QianZhaoHardId = "shanhe:source-qianzhao-pair-2471-qianzhao-hard";
    public const string WuhuanBaseId = "shanhe:source-qianzhao-pair-2471-wuhuan-base";
    public const string WuhuanHardId = "shanhe:source-qianzhao-pair-2471-wuhuan-hard";
    public const string WuhuanNightmareId = "shanhe:source-qianzhao-pair-2471-wuhuan-nightmare";
    public const string WeifuSkillId = "ol:weifu";
    public const string KuansaiSkillId = "ol:kuansai";
    public const string XiaosiSkillId = "ol:xiaosi";
    public const string YuanchouSkillId = "ol:yuanchou";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-qianzhao-pair-2471", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetQianZhaoGeneralId(difficulty), GetWuhuanGeneralId(difficulty) });

    public static string GetQianZhaoGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => QianZhaoBaseId,
        >= 11 and <= 25 => QianZhaoHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public static string GetWuhuanGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => WuhuanBaseId,
        >= 11 and <= 15 => WuhuanHardId,
        >= 16 and <= 25 => WuhuanNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        AddQianZhao(QianZhaoBaseId, []);
        AddQianZhao(QianZhaoHardId, [KuansaiSkillId]);
        AddWuhuan(WuhuanBaseId, null, []);
        AddWuhuan(WuhuanHardId, XiaosiSkillId, []);
        AddWuhuan(WuhuanNightmareId, XiaosiSkillId, [YuanchouSkillId]);

        void AddQianZhao(string id, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            "牵招", "ol-qian-zhao", WeifuSkillId, "wei", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:qian-zhao", VariantId = id["shanhe:".Length..],
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });

        void AddWuhuan(string id, string? primary, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Keep runtime carrier metadata; desktop artwork uses the verified PVE skin1000297.
            "乌桓奇袭兵", "supporter", primary, "qun", 2, extra)
        {
            InitialHp = 2, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = id["shanhe:".Length..] + "-portrait1000297-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
