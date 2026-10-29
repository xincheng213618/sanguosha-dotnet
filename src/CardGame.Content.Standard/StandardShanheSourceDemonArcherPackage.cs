using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2461 and source2464's second member keep their distinct soldier bodies and exact cumulative skills.</summary>
public sealed class StandardShanheSourceDemonArcherPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-demon-archer-2461-base";
    public const string HardId = "shanhe:source-demon-archer-2461-hard";
    public const string NightmareId = "shanhe:source-demon-archer-2461-nightmare";
    public const string TrioBaseId = "shanhe:source-demon-trio-2464-archer-base";
    public const string TrioHardId = "shanhe:source-demon-trio-2464-archer-hard";
    public const string TrioNightmareId = "shanhe:source-demon-trio-2464-archer-nightmare";
    public const string GongqiSkillId = "classic:gongqi";
    public const string YouyanSkillId = "ol:youyan";
    public const string SaoguSkillId = "ol:saogu";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-demon-archer-2461", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public static string GetTrioGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => TrioBaseId,
        >= 11 and <= 15 => TrioHardId,
        >= 16 and <= 25 => TrioNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        // The classic dependency already supplies all three exact abilities.
        // Do not attach Han Dang, Yang Wan or Duan Jiong's other printed skills.
        Add(BaseId, "2461-base", 4861, []);
        Add(HardId, "2461-hard", 4861, [YouyanSkillId]);
        Add(NightmareId, "2461-nightmare", 4861, [YouyanSkillId, SaoguSkillId]);
        Add(TrioBaseId, "2464-archer-base", 4862, []);
        Add(TrioHardId, "2464-archer-hard", 4862, [YouyanSkillId]);
        Add(TrioNightmareId, "2464-archer-nightmare", 4862, [YouyanSkillId, SaoguSkillId]);

        void Add(string id, string tier, int sourceGeneral, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Keep the generic runtime carrier; desktop artwork may use the verified PVE skin1000238.
            "魔军弓骑", "supporter", GongqiSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-" + sourceGeneral,
            VariantId = "shanhe-source-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
