using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheDuanhunPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:duanhun-14079";
    public const string CardId = "shanhe:duanhun";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-duanhun", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-duanhun-14079",
            skill => skill with { Tags = SkillTag.Locked });
        builder.AddCard(new(CardId, "断魂", "装备牌", EquipmentCatalog.Get(CardKind.Duanhun).Description, CardKind.Duanhun));
    }
}
