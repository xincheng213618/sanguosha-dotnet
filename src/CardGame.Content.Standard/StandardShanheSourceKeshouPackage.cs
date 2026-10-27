using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>One shared registration of the exact source993 rule used by several source rosters.</summary>
public sealed class StandardShanheSourceKeshouPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:source-keshou-993";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-keshou-993", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder) =>
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-keshou-993");
}
