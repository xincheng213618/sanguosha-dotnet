using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2457's complete soldier4861 body and cumulative source428/953/3113 skills.</summary>
public sealed class StandardShanheSourceDemonAssassinPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-demon-assassin-2457-base";
    public const string HardId = "shanhe:source-demon-assassin-2457-hard";
    public const string HellId = "shanhe:source-demon-assassin-2457-hell";
    public const string QianxiSkillId = "wushuang:skill-3180";
    public const string YoudiSkillId = "mobile:youdi";
    public const string YiduSkillId = StandardShanheSourceWuqueAssassinPackage.YiduSkillId;
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-demon-assassin-2457", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-source-wuque-assassin-2493", StandardShanheSourceWuqueAssassinPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        // Import only the exact existing judgment version of Qianxi. The Wuque
        // dependency supplies source3113 once, including when both troops meet.
        builder.AddSkill(EmbeddedSkillProgramCatalog.Definition("wushuang-skills-a", QianxiSkillId));
        Add(BaseId, [], "1-10");
        Add(HardId, [YoudiSkillId], "11-20");
        Add(HellId, [YoudiSkillId, YiduSkillId], "21-25");

        void Add(string id, IReadOnlyList<string> extra, string tier) => builder.AddGeneral(new(id,
            "魔军刺客", "supporter", QianxiSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2457-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
