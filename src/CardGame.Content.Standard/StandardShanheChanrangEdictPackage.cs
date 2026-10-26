using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheChanrangEdictPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:chanrang-edict-1510";
    public PackageManifest Manifest { get; } = new("shanhe-chanrang-edict", new(1, 0, 0));
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-chanrang-edict");
        builder.AddCard(new("shanhe:chanrang-edict", "禅让诏书", "装备牌",
            EquipmentCatalog.Get(CardKind.ShanRangZhaoShu).Description, CardKind.ShanRangZhaoShu));
    }
}
