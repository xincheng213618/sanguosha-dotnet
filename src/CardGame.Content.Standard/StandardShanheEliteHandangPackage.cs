using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The five-skill elite Han Dang shown in the supplied third-chapter panel.</summary>
public sealed class StandardShanheEliteHandangPackage : IGameContentPackage
{
    public const string HandangId = "shanhe:elite-han-dang";
    public const string ZizhongId = "shanhe:elite-zizhong";
    public const string MengwuId = "shanhe:elite-mengwu";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-elite-handang", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-elite-handang", skill =>
            skill.Id == MengwuId ? skill with { Tags = SkillTag.Locked } : skill);
        builder.AddGeneral(new(HandangId, "韩当", "han_dang", "boundary:xuanfeng-current", "wu", 5,
            [ZizhongId, "classic:jiefan", MengwuId, "classic:gongqi"])
        {
            InitialHp = 5, CharacterId = "character:han-dang", VariantId = "shanhe-elite-handang-panel",
            RulesetId = "shanhe-2026", Gender = GeneralGender.Male
        });
    }
}
