using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2474's soldier4861 body and its cumulative five source difficulty tiers.</summary>
public sealed class StandardShanheSourceVeteranPackage : IGameContentPackage
{
    public const string BaseGeneralId = "shanhe:source-veteran-2474-base";
    public const string MediumGeneralId = "shanhe:source-veteran-2474-medium";
    public const string HardGeneralId = "shanhe:source-veteran-2474-hard";
    public const string NightmareGeneralId = "shanhe:source-veteran-2474-nightmare";
    public const string HellGeneralId = "shanhe:source-veteran-2474-hell";
    public const string ZaiqiSkillId = StandardShanheCampaignCatalog.S5ZaiqiSkillId;
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-veteran-2474", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 5 => BaseGeneralId,
        >= 6 and <= 10 => MediumGeneralId,
        >= 11 and <= 15 => HardGeneralId,
        >= 16 and <= 20 => NightmareGeneralId,
        >= 21 and <= 25 => HellGeneralId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        Add(BaseGeneralId, [], "1-5");
        Add(MediumGeneralId, ["classic:lianying"], "6-10");
        Add(HardGeneralId, ["classic:lianying", "boundary:longdan"], "11-15");
        Add(NightmareGeneralId, ["classic:lianying", "boundary:longdan", "boundary:wusheng"], "16-20");
        Add(HellGeneralId, ["classic:lianying", "boundary:longdan", "boundary:wusheng", ZaiqiSkillId], "21-25");

        void Add(string id, IReadOnlyList<string> additionalSkills, string tier) => builder.AddGeneral(
            new(id, "百战老兵", "supporter", "classic:zhanjue", "qun", 4, additionalSkills)
            {
                InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
                VariantId = "shanhe-source-2474-" + tier, RulesetId = "shanhe-s5-current-config",
                Gender = GeneralGender.Male
            });
    }
}
