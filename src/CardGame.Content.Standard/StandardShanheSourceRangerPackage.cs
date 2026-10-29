using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2481's complete soldier4861 body and cumulative source3155/3616/282/231 skills.</summary>
public sealed class StandardShanheSourceRangerPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-ranger-2481-base";
    public const string HardId = "shanhe:source-ranger-2481-hard";
    public const string NightmareId = "shanhe:source-ranger-2481-nightmare";
    public const string HellId = "shanhe:source-ranger-2481-hell";
    public const string YilieSkillId = "ol:yilie";
    public const string ChunlaoSkillId = StandardShanheS5DongwuVeteransPackage.ChunlaoId;
    public const string JuesiSkillId = "ol:juesi";
    public const string NamanSkillId = "shanhe:source-naman-231";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-ranger-2481", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-s5-dongwu-veterans-boss", StandardShanheS5DongwuVeteransPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 20 => NightmareId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-naman-231");
        Add(BaseId, "1-10", []);
        Add(HardId, "11-15", [ChunlaoSkillId]);
        Add(NightmareId, "16-20", [ChunlaoSkillId, JuesiSkillId]);
        Add(HellId, "21-25", [ChunlaoSkillId, JuesiSkillId, NamanSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id,
            // Desktop binds source skin1000251 by exact runtime ID; supporter remains the runtime fallback.
            "游侠", "supporter", YilieSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2481-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
