using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed class StandardShanheYongzhanPackage : IGameContentPackage
{
    public const string SkillId = ShanheYongzhanPrograms.SkillId;
    public const string SecondSkillId = ShanheYongzhanPrograms.SecondSkillId;
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-yongzhan", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder,
        "shanhe-current-yongzhan", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
