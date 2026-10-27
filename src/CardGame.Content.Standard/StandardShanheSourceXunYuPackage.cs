using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The independent source2675 Xun Yu body and its QuHuRogue program.</summary>
public sealed class StandardShanheSourceXunYuPackage : IGameContentPackage
{
    public const string GeneralId = "shanhe:source-xunyu-2675";
    public const string QuhuSkillId = "shanhe:source-quhu-14190";
    public const string JiemingSkillId = "boundary:jieming-current";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-xunyu-2675", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-quhu-14190");
        builder.AddGeneral(new(GeneralId, "荀彧", "xun_yu", QuhuSkillId, "wei", 3, [JiemingSkillId])
        {
            InitialHp = 3, CharacterId = "character:xun-yu", VariantId = "shanhe-source-2675",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
