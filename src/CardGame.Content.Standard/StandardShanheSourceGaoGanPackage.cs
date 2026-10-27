using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2484 preserves body482 and cumulative Juguan/Fengpo/Hulie.</summary>
public sealed class StandardShanheSourceGaoGanPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-gaogan-2484-base";
    public const string HardId = "shanhe:source-gaogan-2484-hard";
    public const string HellId = "shanhe:source-gaogan-2484-hell";
    public const string JuguanSkillId = "ol:juguan";
    public const string FengpoSkillId = "ol:fengpo";
    public const string HulieSkillId = "mou-ol:hulie";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-gaogan-2484", CurrentVersion,
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
        // The dependency provides all three exact source abilities; grants do
        // not import Ma Yunlu's Mashu or Mou Sun Jian's Yipo.
        Add(BaseId, "base", []);
        Add(HardId, "hard", [FengpoSkillId]);
        Add(HellId, "hell", [FengpoSkillId, HulieSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            "高干", "ol-gao-gan", JuguanSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:gao-gan",
            VariantId = "shanhe-source-2484-" + tier, RulesetId = "shanhe-s5-current-config",
            Gender = GeneralGender.Male
        });
    }
}
