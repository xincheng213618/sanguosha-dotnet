using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheXuyanPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-xuyan-2004";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-xuyan", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder,
        "shanhe-current-xuyan", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
