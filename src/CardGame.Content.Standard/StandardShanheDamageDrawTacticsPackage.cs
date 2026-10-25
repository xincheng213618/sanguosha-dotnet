using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified S5 actual-damage draw and Duel sustain tactics.</summary>
public sealed class StandardShanheDamageDrawTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-damage-draw-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentCuixueSkillId}}}","revision":1,
           "triggers":[{"id":"first-round-slash-damage","window":"afterDamageApplied","subject":"damageSource",
             "damageOccurrence":"perDamage","damageCardKinds":["slash","fireSlash","thunderSlash"],
             "usageScope":"round","usageLimit":1,"optional":false,
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentXialuXiangfengSkillId}}}","revision":1,
           "triggers":[{"id":"received-duel-damage","window":"afterDamageApplied","subject":"owner",
             "damageOccurrence":"perDamage","damageCardKinds":["duel"],"optional":false,
             "effects":[{"op":"recover","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentZhongjijiSkillId}}}","revision":1,
           "triggers":[{"id":"large-enemy-damage","window":"afterDamageApplied","subject":"damageSource",
             "damageOccurrence":"perDamage","optional":false,
             "condition":{"kind":"all","children":[
               {"kind":"damageTargetIsPublicTeamEnemy"},
               {"kind":"compare","left":{"kind":"appliedDamageAmount"},"operator":"greaterThanOrEqual","right":{"kind":"integerConstant","value":3}}]},
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentCedingTianxiaSkillId}}}","revision":1,
           "triggers":[{"id":"own-play-trick-damage","window":"afterDamageApplied","subject":"damageSource",
             "damageOccurrence":"perDamage",
             "usageScope":"phase","usageLimit":1,"optional":false,
             "condition":{"kind":"all","children":[{"kind":"ownerIsTurnPlayer"},{"kind":"phaseIsPlay"},{"kind":"damageFromOwnerTrickUse"}]},
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentCedingTianxia2SkillId}}}","revision":1,
           "triggers":[{"id":"own-play-trick-damage","window":"afterDamageApplied","subject":"damageSource",
             "damageOccurrence":"perDamage",
             "usageScope":"phase","usageLimit":1,"optional":false,
             "condition":{"kind":"all","children":[{"kind":"ownerIsTurnPlayer"},{"kind":"phaseIsPlay"},{"kind":"damageFromOwnerTrickUse"}]},
             "effects":[{"op":"draw","target":"owner","amount":2}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{StandardShanheCampaignCatalog.CurrentCuixueSkillId}}}":{"name":"淬血","description":"每轮，你的杀首次造成伤害后摸1张牌。普通杀、属性杀与转化杀共用本轮资格；一次伤害多点仍只触发一次。"},
          "{{{StandardShanheCampaignCatalog.CurrentXialuXiangfengSkillId}}}":{"name":"狭路相逢","description":"受到决斗造成的伤害后，回复1点体力。每次实际伤害回复一次，不按伤害点数重复回复。"},
          "{{{StandardShanheCampaignCatalog.CurrentZhongjijiSkillId}}}":{"name":"重击技","description":"当你对敌方造成一次至少3点的实际伤害后，摸1张牌。友方、自身和不足3点的单次伤害不触发；多点伤害只摸一次。"},
          "{{{StandardShanheCampaignCatalog.CurrentCedingTianxiaSkillId}}}":{"name":"策定天下","description":"出牌阶段限一次，你使用锦囊造成伤害后，摸1张牌。仅本人的出牌阶段直接锦囊伤害触发，未造成伤害不消耗次数。"},
          "{{{StandardShanheCampaignCatalog.CurrentCedingTianxia2SkillId}}}":{"name":"策定天下Ⅱ","description":"出牌阶段限一次，你使用锦囊造成伤害后，摸2张牌。仅本人的出牌阶段直接锦囊伤害触发，未造成伤害不消耗次数。"}
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
