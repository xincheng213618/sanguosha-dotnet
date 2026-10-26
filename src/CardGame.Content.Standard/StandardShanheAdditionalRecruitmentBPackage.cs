using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Companion editions transcribed from the September 2026 recruitment panels.</summary>
public sealed class StandardShanheAdditionalRecruitmentBPackage : IGameContentPackage
{
    public const string DuanziSkillId = "shanhe:companion-duanzi";
    public const string ShunxueSkillId = "shanhe:companion-shunxue";
    public PackageManifest Manifest { get; } = new("shanhe-additional-recruitment-b", new(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    internal sealed record Entry(string Key, string Name, string Portrait, string Faction, int Hp, GeneralGender Gender,
        string[] Skills, string ExtraSkill, string ExtraDescription)
    {
        public string NormalId => "shanhe:companion-" + Key + "-normal";
        public string EliteId => "shanhe:companion-" + Key + "-elite";
    }

    internal static readonly Entry[] Entries =
    [
        new("wang-rong", "王荣", "ol-wang-rong", "qun", 3, GeneralGender.Female,
            ["ol:fengzi", "ol:jizhan", "ol:fusong"], "ol:zishu",
            "自书：其他角色的回合结束后，将此回合获得的手牌置入弃牌堆；于自己的回合内不因本技能获得手牌后，摸一张牌。"),
        new("liu-hong", "刘宏", "ol-liu-hong", "qun", 4, GeneralGender.Male,
            ["ol:yujue", "ol:tuxing"], DuanziSkillId, "断辎：死亡后，所有敌方角色废除其所有装备栏。"),
        new("hua-tuo", "华佗", "hua_tuo", "qun", 3, GeneralGender.Male,
            ["classic:jijiu", "classic:qingnang"], ShunxueSkillId,
            "吮血：出牌阶段开始和结束阶段开始时，将手牌摸至手牌上限。"),
        new("cao-ang", "曹昂", "cao_ang", "wei", 4, GeneralGender.Male,
            ["classic:kangkai"], "ol:weishu",
            "卫戍：摸牌阶段外非因本技能摸牌后，令一名角色摸一张牌；弃牌阶段外自己的牌因弃置置入弃牌堆后，弃置一名其他角色一张牌。"),
        new("lu-ji", "陆绩", "lu_ji", "wu", 3, GeneralGender.Male,
            ["classic:huaiju", "classic:yili", "classic:zhenglun"], "boundary:zhiheng",
            "制衡：出牌阶段限一次，弃置任意张牌后摸等量牌；弃置所有手牌时额外摸一张牌。"),
        new("liu-shan", "刘禅", "liu_shan", "shu", 3, GeneralGender.Male,
            ["classic:xiangle", "classic:fangquan"], "boundary:miji-current",
            "秘计：结束阶段，摸已损失体力值张牌，然后可以交给其他角色至多等量的牌。")
    ];

    public static IReadOnlyList<string> GeneralIds { get; } = Array.AsReadOnly(Entries.SelectMany(entry => new[] { entry.NormalId, entry.EliteId }).ToArray());

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{DuanziSkillId}}}","revision":1,"triggers":[
            {"id":"death-enemy-equipment","window":"ownerDied","subject":"owner","optional":false,
             "effects":[{"op":"abolishPublicTeamEnemyEquipmentSlots","target":"owner"}]}]},
          {"id":"{{{ShunxueSkillId}}}","revision":1,"triggers":[
            {"id":"play-refill","window":"playPhaseStarting","subject":"owner","optional":false,
             "effects":[{"op":"draw","target":"owner","numberExpression":"handLimitMinusHandCount"}]},
            {"id":"ending-refill","window":"turnEnding","subject":"owner","optional":false,
             "effects":[{"op":"draw","target":"owner","numberExpression":"handLimitMinusHandCount"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{DuanziSkillId}}}":{"name":"断辎","description":"锁定技，当你死亡后，所有敌方角色废除其所有装备栏。"},
          "{{{ShunxueSkillId}}}":{"name":"吮血","description":"出牌阶段开始和结束阶段开始时，你将手牌摸至手牌上限。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        foreach (var (id, program) in catalog.Programs)
        {
            var presentation = catalog.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = id == DuanziSkillId ? SkillTag.Locked : SkillTag.None });
        }
        foreach (var entry in Entries)
        foreach (var elite in new[] { false, true })
            builder.AddGeneral(new(elite ? entry.EliteId : entry.NormalId, entry.Name, entry.Portrait,
                entry.Skills[0], entry.Faction, entry.Hp,
                elite ? [.. entry.Skills.Skip(1), entry.ExtraSkill] : entry.Skills.Skip(1).ToArray())
            {
                CharacterId = "character:" + entry.Key, Gender = entry.Gender,
                VariantId = elite ? "shanhe-companion-elite" : "shanhe-companion-normal", RulesetId = "shanhe-2026"
            });
    }
}
