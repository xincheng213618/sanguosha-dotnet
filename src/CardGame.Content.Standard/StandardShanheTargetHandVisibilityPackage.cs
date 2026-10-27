using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheTargetHandVisibilityPackage : IGameContentPackage
{
    public const string YanxianSkillId = "shanhe:current-yanxian-2005";
    public const string QiangquHaoduoSkillId = "shanhe:current-qiangqu-haoduo-2006";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-target-hand-visibility", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-target-hand-visibility", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
