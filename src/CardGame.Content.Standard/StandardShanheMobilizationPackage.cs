using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheMobilizationPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-mobilization-2203";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-mobilization", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder,
        "shanhe-current-mobilization", skill => skill with
        { Tags = SkillTag.Locked, CampaignTacticAcquisition = new(skill.Id, Repeatable: false) });
}
