using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2475's complete ordered soldier4861/4862/4863 roster.</summary>
public sealed class StandardShanheSourceMilitiaTrioPackage : IGameContentPackage
{
    public const string FirstBaseId = "shanhe:source-militia-trio-2475-first-base";
    public const string FirstHardId = "shanhe:source-militia-trio-2475-first-hard";
    public const string FirstHellId = "shanhe:source-militia-trio-2475-first-hell";
    public const string SecondBaseId = "shanhe:source-militia-trio-2475-second-base";
    public const string SecondNightmareId = "shanhe:source-militia-trio-2475-second-nightmare";
    public const string ThirdBaseId = "shanhe:source-militia-trio-2475-third-base";
    public const string ThirdHardId = "shanhe:source-militia-trio-2475-third-hard";
    public const string ThirdNightmareId = "shanhe:source-militia-trio-2475-third-nightmare";
    public const string SujianSkillId = "ol:sujian";
    public const string YingyuanSkillId = "ol:yingyuan";
    public const string QingjianSkillId = "shanhe:source-qingjian-721";
    public const string JushouSkillId = "shanhe:source-jushou-370";
    public const string YuejianSkillId = "ol:yuejian";
    public const string ZhuhaiSkillId = "boundary:zhuhai-current";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-militia-trio-2475", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-source-yulin-guard-2498", StandardShanheSourceYulinGuardPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetFirstGeneralId(difficulty), GetSecondGeneralId(difficulty), GetThirdGeneralId(difficulty) });

    public static string GetFirstGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => FirstBaseId,
        >= 11 and <= 20 => FirstHardId,
        >= 21 and <= 25 => FirstHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetSecondGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 15 => SecondBaseId,
        >= 16 and <= 25 => SecondNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetThirdGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => ThirdBaseId,
        >= 11 and <= 15 => ThirdHardId,
        >= 16 and <= 25 => ThirdNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-qingjian-721");
        Add(FirstBaseId, "first-base", 4861, null, []);
        Add(FirstHardId, "first-hard", 4861, SujianSkillId, []);
        Add(FirstHellId, "first-hell", 4861, SujianSkillId, [YingyuanSkillId]);
        Add(SecondBaseId, "second-base", 4862, QingjianSkillId, []);
        Add(SecondNightmareId, "second-nightmare", 4862, QingjianSkillId, [JushouSkillId]);
        Add(ThirdBaseId, "third-base", 4863, null, []);
        Add(ThirdHardId, "third-hard", 4863, YuejianSkillId, []);
        Add(ThirdNightmareId, "third-nightmare", 4863, YuejianSkillId, [ZhuhaiSkillId]);

        void Add(string id, string tier, int sourceGeneral, string? primary, string[] extra) =>
            builder.AddGeneral(new(id, "民兵", "supporter", primary, "qun", 4, extra)
            {
                InitialHp = 4, CharacterId = "character:shanhe-source-soldier-" + sourceGeneral,
                VariantId = "shanhe-source-2475-" + tier + "-portrait-fallback",
                RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
            });
    }
}
