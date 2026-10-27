using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2462 keeps general4871's body and cumulative Tousui/base Lvli/Duorui.</summary>
public sealed class StandardShanheSourceChengZhanPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-chengzhan-2462-base";
    public const string HardId = "shanhe:source-chengzhan-2462-hard";
    public const string HellId = "shanhe:source-chengzhan-2462-hell";
    public const string TousuiSkillId = "ol:tousui";
    public const string LvliSkillId = "ol:lvli";
    public const string DuoruiSkillId = "ol:duorui";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-chengzhan-2462", CurrentVersion,
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
        // Only the source's three abilities are granted. Without either
        // awakening, Lvli stays at its original once-per-turn outgoing branch.
        Add(BaseId, "base", []);
        Add(HardId, "hard", [LvliSkillId]);
        Add(HellId, "hell", [LvliSkillId, DuoruiSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            "成湛", "supporter", TousuiSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2462-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
