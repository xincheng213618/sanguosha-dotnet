using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete source2490 five-body scholar/Kong Rong roster in source order.</summary>
public sealed class StandardShanheSourceScholarsPackage : IGameContentPackage
{
    private const string Prefix = "shanhe:source-scholars-2490-";
    public const string CaiYongBaseId = Prefix + "caiyong-base", CaiYongHardId = Prefix + "caiyong-hard", CaiYongHellId = Prefix + "caiyong-hell";
    public const string YuFanBaseId = Prefix + "yufan-base", YuFanHardId = Prefix + "yufan-hard";
    public const string KongRongBaseId = Prefix + "kongrong-base", KongRongNightmareId = Prefix + "kongrong-nightmare";
    public const string MaRiDiBaseId = Prefix + "maridi-base", MaRiDiHardId = Prefix + "maridi-hard";
    public const string WangCanBaseId = Prefix + "wangcan-base", WangCanHardId = Prefix + "wangcan-hard", WangCanHellId = Prefix + "wangcan-hell";
    public const string BizhuanSkillId = "classic:bizhuan", ZhiyanSkillId = "boundary:zhiyan-current";
    public const string LiwenSkillId = "mou-ol:liwen", ZhengyiSkillId = "mou-ol:zhengyi";
    public const string BingjieSkillId = "ol:bingjie", SanwenSkillId = "ol:sanwen", ZirunSkillId = "shanhe:source-zirun-1172";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-scholars-2490", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static IReadOnlyList<string> GetRosterGeneralIds(int difficulty)
    {
        if (difficulty is < 1 or > 25) throw new ArgumentOutOfRangeException(nameof(difficulty));
        return Array.AsReadOnly(new[]
        {
            difficulty >= 21 ? CaiYongHellId : difficulty >= 11 ? CaiYongHardId : CaiYongBaseId,
            difficulty >= 11 ? YuFanHardId : YuFanBaseId,
            difficulty >= 16 ? KongRongNightmareId : KongRongBaseId,
            difficulty >= 11 ? MaRiDiHardId : MaRiDiBaseId,
            difficulty >= 21 ? WangCanHellId : difficulty >= 11 ? WangCanHardId : WangCanBaseId
        });
    }

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-zirun-1172",
            tagOverrides: new Dictionary<string, SkillTag> { [ZirunSkillId] = SkillTag.Locked });
        Add(CaiYongBaseId, "名士", "cai_yong", "cai-yong", 3, BizhuanSkillId, []);
        Add(CaiYongHardId, "名士", "cai_yong", "cai-yong", 3, BizhuanSkillId, [LiwenSkillId]);
        Add(CaiYongHellId, "名士", "cai_yong", "cai-yong", 3, BizhuanSkillId, [LiwenSkillId, ZirunSkillId]);
        Add(YuFanBaseId, "名士", "boundary_yu_fan", "yu-fan", 3, ZhiyanSkillId, []);
        Add(YuFanHardId, "名士", "boundary_yu_fan", "yu-fan", 3, ZhiyanSkillId, [LiwenSkillId]);
        Add(KongRongBaseId, "孔融", "mou-kong-rong-ol", "kong-rong", 4, LiwenSkillId, []);
        Add(KongRongNightmareId, "孔融", "mou-kong-rong-ol", "kong-rong", 4, LiwenSkillId, [ZhengyiSkillId]);
        Add(MaRiDiBaseId, "名士", "ol-ma-mi-di", "ma-mi-di", 3, BingjieSkillId, []);
        Add(MaRiDiHardId, "名士", "ol-ma-mi-di", "ma-mi-di", 3, BingjieSkillId, [LiwenSkillId]);
        Add(WangCanBaseId, "名士", "ol-wang-can", "wang-can", 3, SanwenSkillId, []);
        Add(WangCanHardId, "名士", "ol-wang-can", "wang-can", 3, SanwenSkillId, [LiwenSkillId]);
        Add(WangCanHellId, "名士", "ol-wang-can", "wang-can", 3, SanwenSkillId, [LiwenSkillId, ZirunSkillId]);
        void Add(string id, string name, string portrait, string character, int hp, string primary, string[] extra) =>
            builder.AddGeneral(new(id, name, portrait, primary, character == "yu-fan" ? "wu" : "qun", hp, extra)
            {
                InitialHp = hp, CharacterId = "character:" + character, VariantId = id["shanhe:".Length..],
                RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
            });
    }
}
