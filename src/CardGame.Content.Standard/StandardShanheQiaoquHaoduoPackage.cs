using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheQiaoquHaoduoPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-qiaoqu-haoduo-2244";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-qiaoqu-haoduo-2244", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-qiaoqu-haoduo", skill => skill with
        {
            Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(skill.Id, Repeatable: false)
        });
}
