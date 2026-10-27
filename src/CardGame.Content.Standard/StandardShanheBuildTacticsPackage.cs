using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified S5 draw and sustain tactics.</summary>
public sealed class StandardShanheBuildTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-build-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentCuixue2SkillId}}}","revision":1,
           "triggers":[{"id":"first-round-slash-damage","window":"afterDamageApplied","subject":"damageSource",
             "damageOccurrence":"perDamage","damageCardKinds":["slash","fireSlash","thunderSlash"],
             "usageScope":"round","usageLimit":1,"optional":false,
             "effects":[{"op":"draw","target":"owner","amount":2}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuanzhu3SkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-ending-draw","window":"turnEnding","subject":"owner","turnOwnerScope":"own","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":3}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentMuniuLiumaSkillId}}}","revision":1,
           "modifiers":[{"id":"normal-draw-two-more","query":"drawCount","operation":"add","value":2,"priority":0},
                        {"id":"one-less-hand-capacity","query":"handLimit","operation":"add","value":-1,"priority":0}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentSougua3SkillId}}}","revision":1,
           "triggers":[{"id":"real-killer-recovers-four","window":"characterDied","subject":"owner","optional":false,
             "condition":{"kind":"deathKillerIsOwner"},"effects":[{"op":"recover","target":"owner","amount":4}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentFenjinSkillId}}}","revision":1,
           "triggers":[{"id":"own-start-exchange-hp-for-cards","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "condition":{"kind":"compare","left":{"kind":"currentHp"},"operator":"greaterThan","right":{"kind":"integerConstant","value":2}},
             "effects":[{"op":"loseHp","target":"owner","amount":1},{"op":"draw","target":"owner","amount":2}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentFenjin2SkillId}}}","revision":1,
           "triggers":[{"id":"own-start-higher-hp-exchange","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "condition":{"kind":"compare","left":{"kind":"currentHp"},"operator":"greaterThan","right":{"kind":"integerConstant","value":4}},
             "effects":[{"op":"loseHp","target":"owner","amount":1},{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentQinmianSkillId}}}","revision":1,
           "triggers":[{"id":"skip-judgment","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"skipTurnPhases","target":"owner","phases":["judgment"]}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{StandardShanheCampaignCatalog.CurrentCuixue2SkillId}}}":{"name":"淬血Ⅱ","description":"每轮，你的【杀】首次造成伤害后摸2张牌。"},
          "{{{StandardShanheCampaignCatalog.CurrentYuanzhu3SkillId}}}":{"name":"援助Ⅲ","description":"你的回合结束时，摸3张牌。"},
          "{{{StandardShanheCampaignCatalog.CurrentMuniuLiumaSkillId}}}":{"name":"木牛流马","description":"摸牌阶段，摸牌数+2；手牌上限-1。"},
          "{{{StandardShanheCampaignCatalog.CurrentSougua3SkillId}}}":{"name":"搜刮Ⅲ","description":"当你击杀其他角色后，回复4点体力。"},
          "{{{StandardShanheCampaignCatalog.CurrentFenjinSkillId}}}":{"name":"奋进","description":"你的回合开始时，若体力大于2，失去1点体力，然后摸2张牌。"},
          "{{{StandardShanheCampaignCatalog.CurrentFenjin2SkillId}}}":{"name":"奋进Ⅱ","description":"你的回合开始时，若体力大于4，失去1点体力，然后摸1张牌。"},
          "{{{StandardShanheCampaignCatalog.CurrentQinmianSkillId}}}":{"name":"勤勉","description":"你始终跳过判定阶段。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        foreach (var (id, program) in catalog.Programs)
        {
            var presentation = catalog.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            {
                Program = program,
                ProgramPresentation = presentation,
                Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
