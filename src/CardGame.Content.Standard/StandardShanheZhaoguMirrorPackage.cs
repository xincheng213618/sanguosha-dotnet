using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The physical source20771 treasure and its source6018 equipment skill.</summary>
public sealed class StandardShanheZhaoguMirrorPackage : IGameContentPackage
{
    public const string CardId = "shanhe:zhaogu-mirror-20771";
    public const string SkillId = "shanhe:zhaogu-mirror-6018";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-zhaogu-mirror", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-zhaogu-mirror-6018");
        var treasure = EquipmentCatalog.Get(CardKind.ZhaoguMirror);
        builder.AddCard(new(CardId, treasure.DisplayName, "装备牌", treasure.Description, treasure.Kind));
    }
}
