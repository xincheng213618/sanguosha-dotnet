using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete ordinary group2485: Cao Song and both separate two-HP guards.</summary>
public sealed class StandardShanheSourceCaoSongTrioPackage : IGameContentPackage
{
    public const string CaoSongBaseId = "shanhe:source-caosong-trio-2485-caosong-base";
    public const string CaoSongHardId = "shanhe:source-caosong-trio-2485-caosong-hard";
    public const string CaoSongHellId = "shanhe:source-caosong-trio-2485-caosong-hell";
    public const string FirstGuardBaseId = "shanhe:source-caosong-trio-2485-guard-4861-base";
    public const string FirstGuardHardId = "shanhe:source-caosong-trio-2485-guard-4861-hard";
    public const string SecondGuardBaseId = "shanhe:source-caosong-trio-2485-guard-4862-base";
    public const string SecondGuardHardId = "shanhe:source-caosong-trio-2485-guard-4862-hard";
    public const string YizhengSkillId = "ol:yizheng-cao-song";
    public const string WeizhongSkillId = StandardShanheSourceMaRiDiPackage.WeizhongSkillId;
    public const string ChuanyunSkillId = "shanhe:source-chuanyun-665";
    public const string LongweiSkillId = "shanhe:source-longwei-1602";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-caosong-trio-2485", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-source-maridi-2452", StandardShanheSourceMaRiDiPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetCaoSongGeneralId(difficulty), GetFirstGuardGeneralId(difficulty), GetSecondGuardGeneralId(difficulty) });
    public static string GetCaoSongGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => CaoSongBaseId, >= 11 and <= 20 => CaoSongHardId,
        >= 21 and <= 25 => CaoSongHellId, _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetFirstGuardGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => FirstGuardBaseId, >= 11 and <= 25 => FirstGuardHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetSecondGuardGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => SecondGuardBaseId, >= 11 and <= 25 => SecondGuardHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-caosong-trio-2485");
        AddCao(CaoSongBaseId, []);
        AddCao(CaoSongHardId, [WeizhongSkillId]);
        AddCao(CaoSongHellId, [WeizhongSkillId, LongweiSkillId]);
        AddGuard(FirstGuardBaseId, 4861, false);
        AddGuard(FirstGuardHardId, 4861, true);
        AddGuard(SecondGuardBaseId, 4862, false);
        AddGuard(SecondGuardHardId, 4862, true);

        void AddCao(string id, IReadOnlyList<string> extra) => builder.AddGeneral(new(id, "曹嵩",
            "ol-cao-song", YizhengSkillId, "wei", 6, extra)
        {
            InitialHp = 6, CharacterId = "character:cao-song", VariantId = id["shanhe:".Length..],
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
        void AddGuard(string id, int body, bool armed) => builder.AddGeneral(new(id, "卫兵",
            "supporter", armed ? ChuanyunSkillId : null, "qun", 2)
        {
            InitialHp = 2, CharacterId = "character:shanhe-source-soldier-" + body,
            VariantId = id["shanhe:".Length..], RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
