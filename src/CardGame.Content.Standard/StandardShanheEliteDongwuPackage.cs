using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Panel-verified Huang Gai and Cheng Pu from the third-chapter elite trio.</summary>
public sealed class StandardShanheEliteDongwuPackage : IGameContentPackage
{
    public const string HuangGaiId = "shanhe:elite-huang-gai";
    public const string ChengPuId = "shanhe:elite-cheng-pu";
    public const string ChunlaoId = "shanhe:elite-chunlao";
    private readonly bool _importBaoli;
    public PackageManifest Manifest { get; }

    public StandardShanheEliteDongwuPackage(bool importBaoli = true)
    {
        _importBaoli = importBaoli;
        Manifest = new("shanhe-elite-dongwu", new Version(1, 0, 0), importBaoli
            ? [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]
            : [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
               new("shanhe-s5-zhuling-boss", StandardShanheS5ZhulingPackage.CurrentVersion)]);
    }

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-elite-chunlao");
        if (_importBaoli)
            builder.AddSkill(EmbeddedSkillProgramCatalog.Definition("shanhe-s5-zhuling", StandardShanheS5ZhulingPackage.BaoliId)
                with { Tags = SkillTag.Locked });
        builder.AddGeneral(new(HuangGaiId, "黄盖", "boundary_huang_gai", "ol:kunfen", "wu", 5,
            ["boundary:yingzi", "boundary:zhaxiang", "boundary:kurou"])
        { InitialHp = 5, CharacterId = "character:huang-gai", VariantId = "shanhe-elite-huang-gai",
            RulesetId = "shanhe-2026-panel", Gender = GeneralGender.Male });
        builder.AddGeneral(new(ChengPuId, "程普", "cheng_pu", "ol:zhihu", "wu", 5,
            [StandardShanheS5ZhulingPackage.BaoliId, "ol:huxiao", ChunlaoId, "classic:lihuo"])
        { InitialHp = 5, CharacterId = "character:cheng-pu", VariantId = "shanhe-elite-cheng-pu",
            RulesetId = "shanhe-2026-panel", Gender = GeneralGender.Male });
    }
}
