using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Both official physical faces grant the same source3278 Sanshou armor rule.</summary>
public sealed class StandardShanheSanshouArmorPackage : IGameContentPackage
{
    public const string CardId = "shanhe:sanshou-armor-20776";
    public const string SecondSourceCardId = "shanhe:sanshou-armor-20777";
    public const string SkillId = "shanhe:sanshou-armor-3278";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-sanshou-armor", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-sanshou-armor-3278");
        var armor = EquipmentCatalog.Get(CardKind.ShanheSanshou);
        builder.AddCard(new(CardId, armor.DisplayName, "装备牌", armor.Description, armor.Kind));
        builder.AddCard(new(SecondSourceCardId, armor.DisplayName, "装备牌", armor.Description, armor.Kind));
    }
}
