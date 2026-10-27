using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2493's complete soldier4861 body, including the dedicated sole-target Yidu.</summary>
public sealed class StandardShanheSourceWuqueAssassinPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-wuque-assassin-2493-base";
    public const string HardId = "shanhe:source-wuque-assassin-2493-hard";
    public const string HellId = "shanhe:source-wuque-assassin-2493-hell";
    public const string YiduSkillId = "shanhe:source-yidu-3113";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-wuque-assassin-2493", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-yidu-3113");
        Add(BaseId, [], "1-10");
        Add(HardId, [YiduSkillId], "11-20");
        Add(HellId, [YiduSkillId, "classic:jueqing"], "21-25");

        void Add(string id, IReadOnlyList<string> extra, string tier) => builder.AddGeneral(new(id,
            "乌鹊暗杀者", "supporter", "ol:zhendu", "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2493-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
