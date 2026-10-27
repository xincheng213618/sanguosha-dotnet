using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheSteadyHandPackage : IGameContentPackage
{
    public const string SkillId = ShanheSteadyHandPrograms.SkillId;
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-steady-hand-2166", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-steady-hand", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
