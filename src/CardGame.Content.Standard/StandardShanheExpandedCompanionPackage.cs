using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>2026 opening companions from the author's roster and current public companion groups.</summary>
public sealed class StandardShanheExpandedCompanionPackage : IGameContentPackage
{
    public const string AlingId = "shanhe:companion-aling";
    public const string YaohuaId = "shanhe:companion-yaohua-elite";
    public const string DuanqingId = "shanhe:companion-duanqing-elite";
    public const string QingliId = "shanhe:companion-qingli-elite";
    public const string ZiyuanId = "shanhe:companion-ziyuan-elite";
    public const string GuangaoSkillId = "shanhe:companion-guangao";
    public const string YingxiangSkillId = "shanhe:companion-yingxiang";
    public const string NiepanSkillId = "shanhe:companion-niepan";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-expanded-companions", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static bool Contains(string id) => id is AlingId or YaohuaId or DuanqingId or QingliId or ZiyuanId;

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-companion-guangao");
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-companion-yingxiang");
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-companion-niepan", skill => skill with { Tags = SkillTag.Limited });
        Add(AlingId, "阿绫", 3, "ol:jingzhou", [NiepanSkillId, "classic:lianhuo"]);
        Add(YaohuaId, "夭华", 6, GuangaoSkillId, ["boundary:yiji"]);
        Add(DuanqingId, "段青", 6, "classic:xiantu", [YingxiangSkillId, "ol:jujia"]);
        Add(QingliId, "青璃", 5, "ol:mo-duoqi", ["classic:fangquan"]);
        Add(ZiyuanId, "紫鸢", 6, "ol:heji", ["boundary:kuanggu", "ol:wushen"]);

        void Add(string id, string name, int hp, string primary, IReadOnlyList<string> additional) =>
            builder.AddGeneral(new(id, name, "", primary, "qun", hp, additional)
            {
                CharacterId = "character:" + id["shanhe:companion-".Length..].Replace("-elite", ""), VariantId = id == AlingId ? "shanhe-companion-2026" : "shanhe-companion-elite-2026",
                RulesetId = "shanhe-2026", Gender = GeneralGender.Female
            });
    }
}
