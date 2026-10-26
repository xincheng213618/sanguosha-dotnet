using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The source3076 own-turn use rule includes genuine response uses.</summary>
public sealed class StandardShanheGuanchaoPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:guanchao-3076";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-guanchao", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-guanchao");
}
