using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheHuxinMirrorPackage : IGameContentPackage
{
    public const string CardId = "shanhe:huxin-mirror-20746";
    public const string SkillId = "shanhe:huxin-mirror-2055";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-huxin-mirror", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-huxin-mirror-2055");
        var armor = EquipmentCatalog.Get(CardKind.HuxinMirror);
        builder.AddCard(new(CardId, armor.DisplayName, "装备牌", armor.Description, armor.Kind));
    }
}
