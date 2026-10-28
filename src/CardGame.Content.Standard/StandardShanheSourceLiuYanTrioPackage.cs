using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Ordinary group2463 in original order: Dongzhou soldier4861, Liu Yan634, soldier4862.</summary>
public sealed class StandardShanheSourceLiuYanTrioPackage : IGameContentPackage
{
    public const string FirstSoldierBaseId = "shanhe:source-liuyan-trio-2463-soldier-4861-base";
    public const string FirstSoldierHardId = "shanhe:source-liuyan-trio-2463-soldier-4861-hard";
    public const string LiuYanBaseId = "shanhe:source-liuyan-trio-2463-liuyan-base";
    public const string LiuYanHardId = "shanhe:source-liuyan-trio-2463-liuyan-hard";
    public const string LiuYanHellId = "shanhe:source-liuyan-trio-2463-liuyan-hell";
    public const string SecondSoldierBaseId = "shanhe:source-liuyan-trio-2463-soldier-4862-base";
    public const string SecondSoldierHardId = "shanhe:source-liuyan-trio-2463-soldier-4862-hard";
    public const string JingqiSkillId = "shanhe:source-jingqi-1161";
    public const string XiechanSkillId = "shanhe:source-xiechan-4055";
    public const string TusheSkillId = "ol:tushe";
    public const string LimuSkillId = "ol:limu";
    public const string GuanbianSkillId = "mou-ol:guanbian";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-liuyan-trio-2463", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetFirstSoldierGeneralId(difficulty), GetLiuYanGeneralId(difficulty), GetSecondSoldierGeneralId(difficulty) });
    public static string GetFirstSoldierGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => FirstSoldierBaseId, >= 11 and <= 25 => FirstSoldierHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetLiuYanGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => LiuYanBaseId, >= 11 and <= 20 => LiuYanHardId,
        >= 21 and <= 25 => LiuYanHellId, _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetSecondSoldierGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => SecondSoldierBaseId, >= 11 and <= 25 => SecondSoldierHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        // This passive has no pending program. Its public-team distance terms
        // are supplied by the existing native distance-rule extension point.
        builder.AddSkill(new(JingqiSkillId, "精骑", "锁定技，友方角色计算与敌方角色距离-1。") { Tags = SkillTag.Locked });
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-xiechan-4055",
            tagOverrides: new Dictionary<string, SkillTag> { [XiechanSkillId] = SkillTag.Limited });
        AddSoldier(FirstSoldierBaseId, 4861, false);
        AddSoldier(FirstSoldierHardId, 4861, true);
        AddLiuYan(LiuYanBaseId, []);
        AddLiuYan(LiuYanHardId, [LimuSkillId]);
        AddLiuYan(LiuYanHellId, [LimuSkillId, GuanbianSkillId]);
        AddSoldier(SecondSoldierBaseId, 4862, false);
        AddSoldier(SecondSoldierHardId, 4862, true);

        void AddSoldier(string id, int body, bool hard) => builder.AddGeneral(new(id, "东州兵", "supporter",
            JingqiSkillId, "qun", 2, hard ? [XiechanSkillId] : [])
        {
            InitialHp = 2, CharacterId = "character:shanhe-source-soldier-" + body,
            VariantId = id["shanhe:".Length..], RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
        void AddLiuYan(string id, IReadOnlyList<string> extra) => builder.AddGeneral(new(id, "刘焉", "ol-liu-yan",
            TusheSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:liu-yan", VariantId = id["shanhe:".Length..],
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
