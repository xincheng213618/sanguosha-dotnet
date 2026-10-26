using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The player-stock Dingcuo source includes self-inflicted received damage.</summary>
public sealed class StandardShanheDingcuoPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:dingcuo-3291";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-dingcuo", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-current-dingcuo");
}
