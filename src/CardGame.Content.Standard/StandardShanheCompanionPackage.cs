using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The screenshot's ordinary companion reuses the complete native source skills.</summary>
public sealed class StandardShanheCompanionPackage : IGameContentPackage
{
    public const string ManzhuId = "shanhe:companion-manzhu";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-companions", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => builder.AddGeneral(new(ManzhuId, "曼珠", "",
        "boundary:zhaxiang", "qun", 3, ["ol:zengou", "classic:longyin"])
    {
        CharacterId = "character:shanhe-manzhu", VariantId = "shanhe-companion-2026",
        RulesetId = "shanhe-2026", Gender = GeneralGender.Female
    });
}
