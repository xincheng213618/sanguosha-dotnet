using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2469 keeps body505's judge-first Beige and cumulative Moshi/Tiandu.</summary>
public sealed class StandardShanheSourceCaiWenjiPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-cai-wen-ji-2469-base";
    public const string HardId = "shanhe:source-cai-wen-ji-2469-hard";
    public const string NightmareId = "shanhe:source-cai-wen-ji-2469-nightmare";
    public const string BeigeSkillId = "boundary:beige";
    public const string MoshiSkillId = "ol:moshi";
    public const string TianduSkillId = "classic:tiandu";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-cai-wen-ji-2469", CurrentVersion,
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
        // The classic dependency supplies all three exact abilities. The source
        // row grants neither the body's Duanchang nor SP Cai Wenji's Chenqing.
        Add(BaseId, "base", []);
        Add(HardId, "hard", [MoshiSkillId]);
        Add(NightmareId, "nightmare", [MoshiSkillId, TianduSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            "蔡文姬", "boundary-cai-wen-ji", BeigeSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:cai-wen-ji",
            VariantId = "shanhe-source-2469-" + tier, RulesetId = "shanhe-s5-current-config",
            Gender = GeneralGender.Female
        });
    }
}
