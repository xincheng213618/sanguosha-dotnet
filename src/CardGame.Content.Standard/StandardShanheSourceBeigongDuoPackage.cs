using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Full ordinary2450 source order: stone statue4861, Beigong Boyu4871.</summary>
public sealed class StandardShanheSourceBeigongDuoPackage : IGameContentPackage
{
    public const string StatueId = "shanhe:source-beigong-duo-2450-statue";
    public const string BeigongBaseId = "shanhe:source-beigong-duo-2450-beigong-base";
    public const string BeigongHardId = "shanhe:source-beigong-duo-2450-beigong-hard";
    public const string DingxiSkillId = "shanhe:source-dingxi-3601";
    public const string HuojieSkillId = "shanhe:source-huojie-3603";
    public const string XingxiaSkillId = "shanhe:source-xingxia-1117";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-beigong-duo-2450", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);
    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty) => Array.AsReadOnly(new[] { StatueId, GetBeigongGeneralId(difficulty) });
    public static string GetBeigongGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BeigongBaseId, >= 11 and <= 25 => BeigongHardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-beigong-2450", tagOverrides: new Dictionary<string, SkillTag>
            { [HuojieSkillId] = SkillTag.Locked, [XingxiaSkillId] = SkillTag.Locked });
        builder.AddGeneral(new(StatueId, "镇关石像", "supporter", null, "qun", 4)
        { InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861", VariantId = "shanhe-source2450-statue", RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male });
        Add(BeigongBaseId, [HuojieSkillId]); Add(BeigongHardId, [HuojieSkillId, XingxiaSkillId]);
        void Add(string id, IReadOnlyList<string> skills) => builder.AddGeneral(new(id, "北宫伯玉", "supporter", DingxiSkillId, "qun", 4, skills)
        { InitialHp = 4, CharacterId = "character:shanhe-source-general-4871", VariantId = id["shanhe:".Length..], RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male });
    }
}
