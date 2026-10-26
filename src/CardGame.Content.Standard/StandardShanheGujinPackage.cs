using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The named first-chapter enemy shown in the supplied battle panel.</summary>
public sealed class StandardShanheGujinPackage : IGameContentPackage
{
    public const string GujinId = "shanhe:gu-jin";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-gu-jin", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        // The panel's Lvli is exactly the unawakened OL skill. Merely borrowing
        // it does not grant Wen Yang's Choujue, Beishui, Qingjiao or upgrades.
        builder.AddGeneral(new(GujinId, "骨进", "supporter", "classic:wushuang", "qun", 6, ["ol:lvli"])
        {
            InitialHp = 6, CharacterId = "character:shanhe-gu-jin", VariantId = "shanhe-gu-jin-panel",
            RulesetId = "shanhe-2026", Gender = GeneralGender.Male
        });
    }
}
