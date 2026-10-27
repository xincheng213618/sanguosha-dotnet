using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The physical source20883 Gouxiang armor and its source14088 rule.</summary>
public sealed class StandardShanheGouXiangArmorPackage : IGameContentPackage
{
    public const string CardId = "shanhe:gouxiang-armor-20883";
    public const string SkillId = "shanhe:gouxiang-armor-14088";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-gouxiang-armor", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-gouxiang-armor-14088",
            definition => definition with { Tags = SkillTag.Locked });
        var armor = EquipmentCatalog.Get(CardKind.GouXiang);
        builder.AddCard(new(CardId, armor.DisplayName, "装备牌", armor.Description, armor.Kind));
    }
}
