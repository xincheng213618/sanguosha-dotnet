using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheKunlunMirrorPackage : IGameContentPackage
{
    public const string CardId = "shanhe:kunlun-mirror-20841";
    public const string SkillId = "shanhe:kunlun-mirror-14080";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-kunlun-mirror", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-kunlun-mirror-14080",
            definition => definition with { Tags = SkillTag.Locked });
        var treasure = EquipmentCatalog.Get(CardKind.KunlunMirror);
        builder.AddCard(new(CardId, treasure.DisplayName, "装备牌", treasure.Description, treasure.Kind));
    }
}
