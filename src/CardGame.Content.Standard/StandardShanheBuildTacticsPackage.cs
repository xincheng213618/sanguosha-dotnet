using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified S5 draw, sustain and finite ordinary-wine tactics.</summary>
public sealed class StandardShanheBuildTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-build-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentTigerWineSkillId}}}","revision":1,
           "cardPolicies":[{"id":"one-extra-ordinary-wine","kind":"additionalOrdinaryAlcoholUses","value":1}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentTigerWine2SkillId}}}","revision":1,
           "cardPolicies":[{"id":"two-extra-ordinary-wines","kind":"additionalOrdinaryAlcoholUses","value":2}]},
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
             "effects":[{"op":"loseHp","target":"owner","amount":1},{"op":"draw","target":"owner","amount":2}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{StandardShanheCampaignCatalog.CurrentTigerWineSkillId}}}":{"name":"虎骨酒","description":"每回合，你可额外使用1次酒。多份独立加算；救命酒不占普通酒次数。"},
          "{{{StandardShanheCampaignCatalog.CurrentTigerWine2SkillId}}}":{"name":"虎骨酒Ⅱ","description":"每回合，你可额外使用2次酒。多份独立加算；救命酒不占普通酒次数。"},
          "{{{StandardShanheCampaignCatalog.CurrentCuixue2SkillId}}}":{"name":"淬血Ⅱ","description":"每轮，你的杀首次造成伤害后摸2张牌。普通杀、属性杀与转化杀共用本轮资格；一次伤害多点仍只触发一次。"},
          "{{{StandardShanheCampaignCatalog.CurrentYuanzhu3SkillId}}}":{"name":"援助Ⅲ","description":"你的回合结束时，摸3张牌。沿用同源战法的原生回合结束时机；其他角色回合不触发。"},
          "{{{StandardShanheCampaignCatalog.CurrentMuniuLiumaSkillId}}}":{"name":"木牛流马","description":"摸牌阶段，你的摸牌数+2。你的手牌上限-1。技能摸牌和初始手牌不增加。"},
          "{{{StandardShanheCampaignCatalog.CurrentSougua3SkillId}}}":{"name":"搜刮Ⅲ","description":"当你击杀其他角色后，回复4点体力。实际回复受体力上限限制，可正常触发回复后的技能。"},
          "{{{StandardShanheCampaignCatalog.CurrentFenjinSkillId}}}":{"name":"奋进","description":"你的回合开始时，若体力大于2，失去1点体力，然后摸2张牌。失去体力不是受到伤害。"}
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
