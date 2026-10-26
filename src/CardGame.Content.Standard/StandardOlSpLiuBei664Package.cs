using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Official SP Liu Bei 664, independent of the older SP Rende general 4081.</summary>
public sealed class StandardOlSpLiuBei664Package : IGameContentPackage
{
    public const string GeneralId = "ol:sp-liu-bei-664";
    public const string XudaiSkillId = "ol:xudai-3576", ZhujiuSkillId = "ol:zhujiu-3577", JingleiSkillId = "ol:jinglei-3578";
    public PackageManifest Manifest { get; } = new("ol-sp-liu-bei-664", new Version(1, 0, 0), [new("standard", StandardContentPackage.CurrentVersion)]);
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "ol-sp-liu-bei-664", skill => skill.Id == XudaiSkillId ? skill with { Tags = SkillTag.Limited } : skill);
        builder.AddGeneral(new(GeneralId, "SP刘备", "liu_bei", XudaiSkillId, "qun", 4,
            [ZhujiuSkillId, JingleiSkillId], GeneralGender.Male) { CharacterId = "character:liu-bei", VariantId = "ol-sp-664" });
    }
}
