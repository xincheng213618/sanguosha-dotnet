using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2497's soldier4861 body and its cumulative source409/396/397 skills.</summary>
public sealed class StandardShanheSourceJiangdongVeteranPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-jiangdong-veteran-2497-base";
    public const string HardId = "shanhe:source-jiangdong-veteran-2497-hard";
    public const string NightmareId = "shanhe:source-jiangdong-veteran-2497-nightmare";
    public const string FenliSkillId = "wushuang:skill-3125";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-jiangdong-veteran-2497", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        // Keep the exact existing native Fenli identity and program. Importing
        // its whole activity package would also register unrelated modes,
        // tactics and shared Shanhe skill definitions.
        builder.AddSkill(EmbeddedSkillProgramCatalog.Definition("wushuang-skills-a", FenliSkillId));
        Add(BaseId, [], "1-10");
        Add(HardId, [FenliSkillId], "11-15");
        Add(NightmareId, [FenliSkillId, "mobile:pingkou"], "16-25");

        void Add(string id, IReadOnlyList<string> additionalSkills, string tier) => builder.AddGeneral(
            new(id, "江东老兵", "supporter", "classic:xuanfeng", "qun", 4, additionalSkills)
            {
                InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
                VariantId = "shanhe-source-2497-" + tier, RulesetId = "shanhe-s5-current-config",
                Gender = GeneralGender.Male
            });
    }
}
