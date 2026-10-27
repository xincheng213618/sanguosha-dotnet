using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2635 and its six-plus dedicated14112 form; never replaces source3389.</summary>
public sealed class StandardShanheSourceXuShengPackage : IGameContentPackage
{
    public const string BaseGeneralId = "shanhe:source-xusheng-2635-base";
    public const string MediumGeneralId = "shanhe:source-xusheng-2635-pojun";
    public const string SkillId = "shanhe:source-pojun-14112";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-xusheng-2635", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-pojun-14112");
        Add(BaseGeneralId, "徐盛", null, "shanhe-source-2635-base");
        Add(MediumGeneralId, "徐盛·破军", SkillId, "shanhe-source-2635-six-plus");
        void Add(string id, string name, string? skill, string variant) => builder.AddGeneral(new(id, name, "xu_sheng", skill, "wu", 4)
        {
            InitialHp = 4, CharacterId = "character:xu-sheng", VariantId = variant,
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
