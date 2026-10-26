using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed class StandardShanheTeamGrowthPackage : IGameContentPackage
{
    public PackageManifest Manifest { get; } = new("shanhe-team-growth", new Version(1, 0, 0));
    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-team-growth", skill => skill with
    {
        Tags = skill.Id == ShanheTeamTactics.AllyRebirthSkillId ? SkillTag.Limited : SkillTag.Locked,
        CampaignTacticAcquisition = skill.Id == ShanheTeamTactics.HealthSkillId ? new(skill.Id, Repeatable: false) : null
    });
}
