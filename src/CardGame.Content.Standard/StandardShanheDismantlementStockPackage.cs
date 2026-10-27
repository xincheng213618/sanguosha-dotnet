using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheDismantlementStockPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-chenqibubei-2239";
    public const string UpgradedSkillId = "shanhe:current-chenqibubei-2240";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-dismantlement-stock", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-dismantlement-stock", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
