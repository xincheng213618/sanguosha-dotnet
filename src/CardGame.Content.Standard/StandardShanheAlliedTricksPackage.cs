using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheAlliedTricksPackage : IGameContentPackage
{
    public const string FengshounianSkillId = "shanhe:current-fengshounian-2241";
    public const string JiemengSkillId = "shanhe:current-jiemeng-2243";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-allied-tricks", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-allied-tricks", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
