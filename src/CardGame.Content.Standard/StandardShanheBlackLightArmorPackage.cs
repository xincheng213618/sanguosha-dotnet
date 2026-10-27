using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheBlackLightArmorPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:black-light-armor-6011";
    public const string CardId = "shanhe:black-light-armor";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-black-light-armor", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-black-light-armor-6011",
            skill => skill with { Tags = SkillTag.Locked });
        builder.AddCard(new(CardId, "黑光铠", "装备牌", EquipmentCatalog.Get(CardKind.BlackLightArmor).Description,
            CardKind.BlackLightArmor));
    }
}
