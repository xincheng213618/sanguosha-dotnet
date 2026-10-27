using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 group 2655: Sun Ce's distinct wounded-preparation awakening.</summary>
public sealed class StandardShanheSourceSunCePackage : IGameContentPackage
{
    public const string SunCeId = "shanhe:source-sunce-2655";
    public const string HunziSkillId = "shanhe:source-hunzi-14153";
    public const string YinghunSkillId = "shanhe:source-yinghun-108";
    public const string JiangSkillId = "boundary:jiang-current";
    public const string YingziSkillId = "boundary:yingzi";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-sunce-boss", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-hunzi-14153",
            tagOverrides: new Dictionary<string, SkillTag> { [HunziSkillId] = SkillTag.Awakening });
        builder.AddGeneral(new(SunCeId, "孙策", "boundary-sun-ce", JiangSkillId, "wu", 3, [HunziSkillId])
        {
            InitialHp = 3, CharacterId = "character:sun-ce", VariantId = "shanhe-source-2655",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
