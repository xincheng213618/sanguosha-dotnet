using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2465's three ordered independent bodies, including the complete supply officer.</summary>
public sealed class StandardShanheSourceCoalitionTrioPackage : IGameContentPackage
{
    public const string SpyBaseId = "shanhe:source-coalition-trio-2465-spy-base";
    public const string SpyHardId = "shanhe:source-coalition-trio-2465-spy-hard";
    public const string SpyNightmareId = "shanhe:source-coalition-trio-2465-spy-nightmare";
    public const string ScoutBaseId = "shanhe:source-coalition-trio-2465-scout-base";
    public const string ScoutHardId = "shanhe:source-coalition-trio-2465-scout-hard";
    public const string ScoutHellId = "shanhe:source-coalition-trio-2465-scout-hell";
    public const string QuartermasterBaseId = "shanhe:source-coalition-trio-2465-quartermaster-base";
    public const string QuartermasterHardId = "shanhe:source-coalition-trio-2465-quartermaster-hard";
    public const string DuliangSkillId = "classic:duliang";
    public const string CangchuSkillId = "shanhe:source-cangchu-1182";
    public const string SushouSkillId = "shanhe:source-sushou-1183";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-coalition-trio-2465", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetSpyGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => SpyBaseId,
        >= 11 and <= 15 => SpyHardId,
        >= 16 and <= 25 => SpyNightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetScoutGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => ScoutBaseId,
        >= 11 and <= 20 => ScoutHardId,
        >= 21 and <= 25 => ScoutHellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public static string GetQuartermasterGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => QuartermasterBaseId,
        >= 11 and <= 25 => QuartermasterHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-coalition-trio-2465",
            definition => definition.Id == CangchuSkillId ? definition with { Tags = SkillTag.Locked } : definition);
        Add(SpyBaseId, "spy-base", "联军细作", 4861, 3, StandardShanheSourceCoalitionSpyPackage.QiceSkillId, []);
        Add(SpyHardId, "spy-hard", "联军细作", 4861, 3, StandardShanheSourceCoalitionSpyPackage.QiceSkillId,
            [StandardShanheSourceCoalitionSpyPackage.TishenSkillId]);
        Add(SpyNightmareId, "spy-nightmare", "联军细作", 4861, 3, StandardShanheSourceCoalitionSpyPackage.QiceSkillId,
            [StandardShanheSourceCoalitionSpyPackage.TishenSkillId, StandardShanheSourceCoalitionSpyPackage.DuanfaSkillId]);
        Add(ScoutBaseId, "scout-base", "联军斥候", 4862, 4, "classic:dangxian", []);
        Add(ScoutHardId, "scout-hard", "联军斥候", 4862, 4, "classic:dangxian", ["classic:kanpo"]);
        Add(ScoutHellId, "scout-hell", "联军斥候", 4862, 4, "classic:dangxian", ["classic:kanpo", "ol:dianhu"]);
        Add(QuartermasterBaseId, "quartermaster-base", "联军督运使", 4871, 3, DuliangSkillId, []);
        Add(QuartermasterHardId, "quartermaster-hard", "联军督运使", 4871, 3, DuliangSkillId, [CangchuSkillId, SushouSkillId]);

        void Add(string id, string tier, string name, int sourceGeneral, int hp, string primary, string[] extra) =>
            builder.AddGeneral(new(id, name, "supporter", primary, "qun", hp, extra)
            {
                InitialHp = hp, CharacterId = sourceGeneral == 4871 ? "character:shanhe-source-general-4871" :
                    "character:shanhe-source-soldier-" + sourceGeneral,
                VariantId = "shanhe-source-2465-" + tier + "-portrait-fallback",
                RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
            });
    }
}
