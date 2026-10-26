using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Panel-verified Wushuang Fengxian forms, independent of the cooperative Hulao activity.</summary>
public sealed class StandardShanheFengxianBossPackage : IGameContentPackage
{
    public const string FirstId = "shanhe:lvbu-fengxian-first";
    public const string RageId = "shanhe:lvbu-fengxian-rage";
    public const string SourceRageHardId = "shanhe:lvbu-fengxian-source-hard-264401";
    public const string SourceRageHellId = "shanhe:lvbu-fengxian-source-hell-264401";
    public const string XiuluoId = "shanhe:fengxian-xiuluo";
    public const string MashuId = "shanhe:fengxian-mashu";
    public const string ShenweiId = "shanhe:fengxian-shenwei-14130";
    public const string ShenjiId = "shanhe:fengxian-shenji-14131";
    public PackageManifest Manifest { get; } = new("shanhe-fengxian-two-stage", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-bosses-2026", StandardShanheBossPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-fengxian", skill => skill with
        { Tags = skill.Id is MashuId or ShenweiId ? SkillTag.Locked : SkillTag.None });
        builder.AddGeneral(new(FirstId, "吕布", "boundary_lu_bu", "classic:wushuang", "qun", 6,
            ["boundary:liyu-current"])
        { InitialHp = 6, CharacterId = "character:lu-bu", VariantId = "shanhe-fengxian-first",
            RulesetId = "shanhe-2026-panel", Gender = GeneralGender.Male });
        builder.AddGeneral(new(RageId, "暴怒的战神", "shen_lu_bu", "shanhe:shengui-wushuang", "qun", 6,
            [XiuluoId, MashuId])
        { InitialHp = 6, CharacterId = "character:lu-bu", VariantId = "shanhe-fengxian-rage",
            RulesetId = "shanhe-2026-panel", Gender = GeneralGender.Male });
        builder.AddGeneral(new(SourceRageHardId, "暴怒的战神", "shen_lu_bu", "shanhe:shengui-wushuang", "qun", 5,
            [XiuluoId, MashuId, ShenweiId])
        { InitialHp = 5, CharacterId = "character:lu-bu", VariantId = "shanhe-source-264401-hard",
            RulesetId = "shanhe-source-2026", Gender = GeneralGender.Male });
        builder.AddGeneral(new(SourceRageHellId, "暴怒的战神", "shen_lu_bu", "shanhe:shengui-wushuang", "qun", 5,
            [XiuluoId, MashuId, ShenweiId, ShenjiId])
        { InitialHp = 5, CharacterId = "character:lu-bu", VariantId = "shanhe-source-264401-hell",
            RulesetId = "shanhe-source-2026", Gender = GeneralGender.Male });
    }
}
