using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheBadaoPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-badao-2188";
    public const string SecondSkillId = "shanhe:current-badao-2189";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-badao", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder,
        "shanhe-current-badao", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
