using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheQianlongPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-qianlong-2226";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-qianlong", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder,
        "shanhe-current-qianlong", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
