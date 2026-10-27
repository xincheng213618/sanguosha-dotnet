using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The independent source268701 second form; source4871 is not a shared skill identity.</summary>
public sealed class StandardShanheSourceShenNuStagePackage : IGameContentPackage
{
    public const string GeneralId = "shanhe:source-shennu-268701";
    public const string SkillId = "shanhe:source-shennu-14214";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-shennu-stage", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-shennu-14214",
            skill => skill with { Tags = SkillTag.Locked });
        builder.AddGeneral(new(GeneralId, "篡天恶魂", "dong_zhuo", SkillId, "qun", 20,
            ["boundary:benghuai-current"])
        {
            InitialHp = 20, CharacterId = "character:dong-zhuo", VariantId = "shanhe-source-268701-skin-60501",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
