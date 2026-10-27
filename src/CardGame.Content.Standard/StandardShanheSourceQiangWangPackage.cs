using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2669 uses ordinary limited Xiongluan and the native Dongdang entry rule.</summary>
public sealed class StandardShanheSourceQiangWangPackage : IGameContentPackage
{
    public const string GeneralId = "shanhe:source-qiangwang-2669";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-qiangwang-boss", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
            new("shanhe-source-zhangyan-boss", StandardShanheSourceZhangYanPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder) => builder.AddGeneral(new(GeneralId,
        "狂暴枪王", "zhang_xiu", StandardShanheSourceZhangYanPackage.DongdangSkillId, "qun", 4,
        ["classic:xiongluan"])
    {
        CharacterId = "character:zhang-xiu", VariantId = "shanhe-source-2669",
        RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
    });
}
