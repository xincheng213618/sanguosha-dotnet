using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Verified S5 legendary tactics using the existing native policy and damage operations.</summary>
public sealed class StandardShanheLegendaryTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-legendary-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-legendary-tactics", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
