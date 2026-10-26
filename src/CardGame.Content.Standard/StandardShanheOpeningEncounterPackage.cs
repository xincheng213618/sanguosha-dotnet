using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Client-screen opening forms composed from already implemented ordinary skills.</summary>
public sealed class StandardShanheOpeningEncounterPackage : IGameContentPackage
{
    public const string ZhangheId = "shanhe:opening-zhang-he";
    public const string TadunId = "shanhe:opening-ta-dun";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-opening-encounters", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        // The screenshot uses the five-card-capped ordinary Zhouxuan, rather
        // than the separate uncapped S5 source form already in the catalogue.
        builder.AddGeneral(new(ZhangheId, "张郃", "ol-sp-zhang-he", "ol:zhouxuan", "qun", 4)
        {
            CharacterId = "character:zhang-he", VariantId = "shanhe-opening-screen",
            RulesetId = "shanhe-opening-screen", Gender = GeneralGender.Male
        });
        builder.AddGeneral(new(TadunId, "蹋顿", "ol-ta-dun", "ol:lvli", "qun", 4, ["classic:wushuang"])
        {
            CharacterId = "character:ta-dun", VariantId = "shanhe-opening-screen",
            RulesetId = "shanhe-opening-screen", Gender = GeneralGender.Male
        });
    }
}
