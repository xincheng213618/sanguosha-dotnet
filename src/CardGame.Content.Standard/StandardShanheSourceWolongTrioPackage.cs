using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete ordinary group2482: source37, source416 and source455 in that order.</summary>
public sealed class StandardShanheSourceWolongTrioPackage : IGameContentPackage
{
    public const string WolongBaseId = "shanhe:source-wolong-trio-2482-wolong-base";
    public const string WolongHardId = "shanhe:source-wolong-trio-2482-wolong-hard";
    public const string WolongNightmareId = "shanhe:source-wolong-trio-2482-wolong-nightmare";
    public const string WolongHellId = "shanhe:source-wolong-trio-2482-wolong-hell";
    public const string SimaHuiBaseId = "shanhe:source-wolong-trio-2482-simahui-base";
    public const string SimaHuiHardId = "shanhe:source-wolong-trio-2482-simahui-hard";
    public const string SimaHuiNightmareId = "shanhe:source-wolong-trio-2482-simahui-nightmare";
    public const string PangTongBaseId = "shanhe:source-wolong-trio-2482-pangtong-base";
    public const string PangTongNightmareId = "shanhe:source-wolong-trio-2482-pangtong-nightmare";
    public const string BazhenSkillId = "classic:bazhen";
    public const string HuojiSkillId = "shanhe:source-huoji-906";
    public const string JieyingSkillId = "ol:shen-liu-bei-jieying";
    public const string ZhaohuoSkillId = "shenwu:zhaohuo";
    public const string YinshiSkillId = "ol:yinshi-sima-hui";
    public const string ChenghaoSkillId = "ol:chenghao";
    public const string NiepanSkillId = "shanhe:source-niepan-911";
    public const string RebornHuojiSkillId = "boundary:huoji-current";
    public const string RebornKanpoSkillId = "boundary:kanpo-current";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-wolong-trio-2482", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shenwu-2020-activity", new StandardShenwu2020ActivityPackage().Manifest.Version)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetWolongGeneralId(difficulty), GetSimaHuiGeneralId(difficulty), GetPangTongGeneralId(difficulty) });

    public static string GetWolongGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => WolongBaseId,
        >= 11 and <= 15 => WolongHardId,
        >= 16 and <= 20 => WolongNightmareId,
        >= 21 and <= 25 => WolongHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetSimaHuiGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => SimaHuiBaseId,
        >= 11 and <= 15 => SimaHuiHardId,
        >= 16 and <= 25 => SimaHuiNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetPangTongGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 15 => PangTongBaseId,
        >= 16 and <= 25 => PangTongNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-wolong-trio-2482", skill =>
            skill.Id == NiepanSkillId ? skill with { Tags = SkillTag.Limited, ExecutionForms = SkillExecutionForm.Trigger } : skill);
        Add(WolongBaseId, "卧龙诸葛", "wolong_zhuge_liang", "zhuge-liang", "shu", BazhenSkillId, []);
        Add(WolongHardId, "卧龙诸葛", "wolong_zhuge_liang", "zhuge-liang", "shu", BazhenSkillId, [HuojiSkillId]);
        Add(WolongNightmareId, "卧龙诸葛", "wolong_zhuge_liang", "zhuge-liang", "shu", BazhenSkillId, [HuojiSkillId, JieyingSkillId]);
        Add(WolongHellId, "卧龙诸葛", "wolong_zhuge_liang", "zhuge-liang", "shu", BazhenSkillId, [HuojiSkillId, JieyingSkillId, ZhaohuoSkillId]);
        Add(SimaHuiBaseId, "司马徽", "ol-sima-hui", "sima-hui", "qun", YinshiSkillId, []);
        Add(SimaHuiHardId, "司马徽", "ol-sima-hui", "sima-hui", "qun", YinshiSkillId, [ChenghaoSkillId]);
        Add(SimaHuiNightmareId, "司马徽", "ol-sima-hui", "sima-hui", "qun", YinshiSkillId, [ChenghaoSkillId, JieyingSkillId]);
        Add(PangTongBaseId, "庞统", "boundary_pang_tong", "pang-tong", "shu", NiepanSkillId, []);
        Add(PangTongNightmareId, "庞统", "boundary_pang_tong", "pang-tong", "shu", NiepanSkillId, [JieyingSkillId]);

        void Add(string id, string name, string portrait, string character, string faction, string primary, IReadOnlyList<string> extra) =>
            builder.AddGeneral(new(id, name, portrait, primary, faction, 3, extra)
            {
                InitialHp = 3, CharacterId = "character:" + character, VariantId = id["shanhe:".Length..],
                RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
            });
    }
}
