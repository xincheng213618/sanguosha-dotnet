using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete source2491: guard4861, captain4871, guard4861 in source order.</summary>
public sealed class StandardShanheSourceGuardTrioPackage : IGameContentPackage
{
    public const string GuardBaseId = "shanhe:source-guard-trio-2491-guard-base";
    public const string GuardHellId = "shanhe:source-guard-trio-2491-guard-hell";
    public const string CaptainBaseId = "shanhe:source-guard-trio-2491-captain-base";
    public const string CaptainHardId = "shanhe:source-guard-trio-2491-captain-hard";
    public const string YinbingSkillId = "ol:yinbing";
    public const string JuediSkillId = "ol:juedi";
    public const string ZhenweiSkillId = "ol:zhenwei";
    public const string WeizhuSkillId = "shanhe:source-weizhu-14034";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-guard-trio-2491", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[]
    { GetGuardGeneralId(difficulty), GetCaptainGeneralId(difficulty), GetGuardGeneralId(difficulty) });
    public static string GetGuardGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 20 => GuardBaseId,
        >= 21 and <= 25 => GuardHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetCaptainGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => CaptainBaseId,
        >= 11 and <= 25 => CaptainHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-weizhu-14034");
        Add(GuardBaseId, "guard-base", "盾阵", 4861, 4, YinbingSkillId, [JuediSkillId]);
        Add(GuardHellId, "guard-hell", "盾阵", 4861, 4, YinbingSkillId, [JuediSkillId, WeizhuSkillId]);
        Add(CaptainBaseId, "captain-base", "武卫骁勇", 4871, 5, null, []);
        Add(CaptainHardId, "captain-hard", "武卫骁勇", 4871, 5, ZhenweiSkillId, []);
        void Add(string id, string tier, string name, int sourceGeneral, int hp, string? primary, string[] extra) =>
            builder.AddGeneral(new(id, name, "supporter", primary, "qun", hp, extra)
            {
                InitialHp = hp, CharacterId = sourceGeneral == 4871 ? "character:shanhe-source-general-4871" :
                    "character:shanhe-source-soldier-4861",
                VariantId = "shanhe-source-2491-" + tier + "-portrait-fallback",
                RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
            });
    }
}
