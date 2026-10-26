using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Both source support skills and Fengzi already have complete native implementations.</summary>
public sealed class StandardShanheWuguotaiCompanionPackage : IGameContentPackage
{
    public const string NormalId = "boundary:wu-guo-tai";
    public const string EliteId = "shanhe:companion-wu-guo-tai-elite";
    public PackageManifest Manifest { get; } = new("shanhe-wuguotai-companion", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => builder.AddGeneral(new(EliteId, "吴国太", "boundary_wu_guo_tai",
        "boundary:ganlu-current", "wu", 3, ["boundary:buyi-current", "ol:fengzi"])
    {
        CharacterId = "character:wu-guo-tai", VariantId = "shanhe-companion-elite", RulesetId = "shanhe-2026",
        Gender = GeneralGender.Female
    });
}
