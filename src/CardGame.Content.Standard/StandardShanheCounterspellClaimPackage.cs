using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheCounterspellClaimPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-caochuanjiejian-2008";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-counterspell-claim", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-counterspell-claim", skill => skill with
        { Tags = SkillTag.Locked, CampaignTacticAcquisition = new(skill.Id, Repeatable: false) });
}
