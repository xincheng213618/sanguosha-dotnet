using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2494's complete soldier4861 body and cumulative source761/923/3123 skills.</summary>
public sealed class StandardShanheSourceVanguardPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-vanguard-2494-base";
    public const string HardId = "shanhe:source-vanguard-2494-hard";
    public const string NightmareId = "shanhe:source-vanguard-2494-nightmare";
    public const string QingxiSkillId = "classic:qingxi";
    public const string DuoruiSkillId = "ol:duorui";
    public const string ZhuijiSkillId = "ol:zhuiji";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-vanguard-2494", CurrentVersion,
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
        // All three exact existing abilities come from the classic dependency;
        // do not import their original generals' other printed skills.
        Add(BaseId, "1-10", []);
        Add(HardId, "11-15", [DuoruiSkillId]);
        Add(NightmareId, "16-25", [DuoruiSkillId, ZhuijiSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Runtime supporter fallback; exact source art is resolved by the desktop ID binding.
            "先锋精骑", "supporter", QingxiSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2494-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
