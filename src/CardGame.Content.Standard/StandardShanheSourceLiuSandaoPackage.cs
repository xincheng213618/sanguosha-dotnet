using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2479's complete general4871 body and cumulative source208/1612/386/249/314/507/917 skills.</summary>
public sealed class StandardShanheSourceLiuSandaoPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-liusandao-2479-base";
    public const string MediumId = "shanhe:source-liusandao-2479-medium";
    public const string HardId = "shanhe:source-liusandao-2479-hard";
    public const string NightmareId = "shanhe:source-liusandao-2479-nightmare";
    public const string HellId = "shanhe:source-liusandao-2479-hell";
    public const string ChongzhenSkillId = "sp:chongzhen";
    public const string HuchenSkillId = "shanhe:source-huchen-1612";
    public const string AozhanSkillId = "hulao:aozhan";
    public const string NuzhanSkillId = "sp:nuzhan";
    public const string WuqianSkillId = "classic:wuqian";
    public const string ZhanshenSkillId = "shanhe:source-zhanshen-507";
    public const string DengjiSkillId = "ol:dengji";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    private static readonly PackageManifest Hulao = new StandardHulaoActivityPackage().Manifest;
    public PackageManifest Manifest { get; } = new("shanhe-source-liusandao-2479", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion), new(Hulao.Id, Hulao.Version)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 5 => BaseId,
        >= 6 and <= 10 => MediumId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 20 => NightmareId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-liusandao-2479",
            tagOverrides: new Dictionary<string, SkillTag> { [HuchenSkillId] = SkillTag.Locked, [ZhanshenSkillId] = SkillTag.Awakening });
        Add(BaseId, "1-5", [HuchenSkillId]);
        Add(MediumId, "6-10", [HuchenSkillId, AozhanSkillId, NuzhanSkillId]);
        Add(HardId, "11-15", [HuchenSkillId, AozhanSkillId, NuzhanSkillId, WuqianSkillId]);
        Add(NightmareId, "16-20", [HuchenSkillId, AozhanSkillId, NuzhanSkillId, WuqianSkillId, ZhanshenSkillId]);
        Add(HellId, "21-25", [HuchenSkillId, AozhanSkillId, NuzhanSkillId, WuqianSkillId, ZhanshenSkillId, DengjiSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id,
            "刘三刀", "supporter", ChongzhenSkillId, "qun", 5, extra)
        {
            InitialHp = 5, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2479-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
