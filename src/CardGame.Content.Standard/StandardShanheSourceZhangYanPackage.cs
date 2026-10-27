using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The source2653/265301 bodies used by the supported medium difficulty group.</summary>
public sealed class StandardShanheSourceZhangYanPackage : IGameContentPackage
{
    public const string ZhangniujiaoMediumId = "shanhe:source-zhangniujiao-2653-medium";
    public const string ZhangYanBaseId = "shanhe:source-zhangyan-265301-base";
    public const string ZhangYanMediumId = "shanhe:source-zhangyan-265301-medium";
    public const string YanyuSkillId = "shanhe:source-yanyu-3292";
    public const string BiriSkillId = "shanhe:source-biri-14039";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-zhangyan-boss", CurrentVersion,
        [new("shanhe-s5-xuncan-caoshi-boss", StandardShanheS5XuncanCaoshiPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-yanyu-3292",
            skill => skill with { Tags = SkillTag.Locked });
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-biri-14039",
            skill => skill with { Tags = SkillTag.Locked });

        // General4871/skin1000367 has no base skill. ZD adds only source3231.
        builder.AddGeneral(new(ZhangniujiaoMediumId, "张牛角", "supporter",
            StandardShanheS5XuncanCaoshiPackage.LeijieId, "qun", 3)
        {
            InitialHp = 3, CharacterId = "character:zhang-niu-jiao",
            VariantId = "shanhe-source-2653-medium-skin-1000367",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });

        // Source4872/skin1000364 is a separate boss body, not OL Suji/Langdao.
        builder.AddGeneral(new(ZhangYanBaseId, "张燕", "ol-zhang-yan", YanyuSkillId, "qun", 4)
        {
            InitialHp = 4, CharacterId = "character:zhang-yan",
            VariantId = "shanhe-source-265301-base-skin-1000364",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
        builder.AddGeneral(new(ZhangYanMediumId, "张燕", "ol-zhang-yan", YanyuSkillId, "qun", 4, [BiriSkillId])
        {
            InitialHp = 4, CharacterId = "character:zhang-yan",
            VariantId = "shanhe-source-265301-medium-skin-1000364",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
