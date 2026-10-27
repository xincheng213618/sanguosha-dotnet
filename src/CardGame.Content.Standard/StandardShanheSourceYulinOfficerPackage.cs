using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 group2499's officer4871 and the complete shared soldier4861 follower.</summary>
public sealed class StandardShanheSourceYulinOfficerPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-yulin-officer-2499-base";
    public const string HardId = "shanhe:source-yulin-officer-2499-hard";
    public const string HellId = "shanhe:source-yulin-officer-2499-hell";
    public const string KuangshouSkillId = "ol:kuangshou";
    public const string KeshouSkillId = StandardShanheSourceKeshouPackage.SkillId;
    public const string WeijingSkillId = "ol:weijing";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-yulin-officer-2499", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-source-keshou-993", StandardShanheSourceKeshouPackage.CurrentVersion),
         new("shanhe-source-yulin-guard-2498", StandardShanheSourceYulinGuardPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    /// <summary>Preserves source2499's officer-first, soldier-second member order.</summary>
    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    {
        GetGeneralId(difficulty),
        StandardShanheSourceYulinGuardPackage.GetGeneralId(difficulty)
    });

    public void Register(IContentRegistryBuilder builder)
    {
        Add(BaseId, "base", []);
        Add(HardId, "hard", [KeshouSkillId]);
        Add(HellId, "hell", [KeshouSkillId, WeijingSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "羽林卫尉",
            "supporter", KuangshouSkillId, "qun", 5, extra)
        {
            InitialHp = 5, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2499-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
