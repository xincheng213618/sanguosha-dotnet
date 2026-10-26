using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheXiashengPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:xiasheng-3362";
    public PackageManifest Manifest { get; } = new("shanhe-xiasheng", new Version(1, 0, 0),
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-xiasheng", skill => skill with { Tags = SkillTag.Locked });
}
