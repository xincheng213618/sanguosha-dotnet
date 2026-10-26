using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheGuideSkillsPackage : IGameContentPackage
{
    public const string BuwuSkillId = "shanhe:buwu-3365";
    public const string ShuanghunSkillId = "shanhe:shuanghun-3366";
    public PackageManifest Manifest { get; } = new("shanhe-guide-skills", new Version(1, 0, 0),
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) => EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-guide-skills",
        skill => skill with { Tags = skill.Id == ShuanghunSkillId ? SkillTag.Locked | SkillTag.Limited : SkillTag.Locked });
}
