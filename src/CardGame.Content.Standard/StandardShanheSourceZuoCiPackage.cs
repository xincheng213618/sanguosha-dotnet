using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2470's complete general447 body and cumulative source135/136/989/3232 skills.</summary>
public sealed class StandardShanheSourceZuoCiPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-zuo-ci-2470-base";
    public const string HardId = "shanhe:source-zuo-ci-2470-hard";
    public const string NightmareId = "shanhe:source-zuo-ci-2470-nightmare";
    public const string HuashenSkillId = "classic:huashen";
    public const string XinshengSkillId = "classic:xinsheng";
    public const string BolanSkillId = "ol:bolan";
    public const string ZhimingSkillId = "ol:zhiming";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-zuoci-2470", CurrentVersion,
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
        Add(BaseId, "1-10", [XinshengSkillId]);
        Add(HardId, "11-15", [XinshengSkillId, BolanSkillId]);
        Add(NightmareId, "16-25", [XinshengSkillId, BolanSkillId, ZhimingSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Source skin44702 is not separately verified; use the known local Zuo Ci portrait.
            "左慈", "zuo-ci", HuashenSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:zuo-ci",
            VariantId = "shanhe-source-2470-" + tier + "-portrait44702-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
