using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheMultiTargetTricksPackage : IGameContentPackage
{
    public const string HengjiangSkillId = "shanhe:current-hengjiang-2007";
    public const string DantiaowangSkillId = "shanhe:current-dantiaowang-2212";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-multi-target-tricks", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-multi-target-tricks", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
