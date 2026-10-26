using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The chapter-two Lvbu form transcribed from the user's actual battle panel.</summary>
public sealed class StandardShanheConfidentLvbuPackage : IGameContentPackage
{
    public const string LvbuId = "shanhe:confident-lvbu";
    public const string XiaoruiId = "shanhe:confident-xiaorui";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-confident-lvbu", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-confident-lvbu");
        builder.AddGeneral(new(LvbuId, "自信的吕布", "shen_lu_bu", XiaoruiId, "qun", 6, ["classic:benxi"])
        {
            InitialHp = 6, CharacterId = "character:lu-bu", VariantId = "shanhe-confident-lvbu",
            RulesetId = "shanhe-2026", Gender = GeneralGender.Male
        });
    }
}
