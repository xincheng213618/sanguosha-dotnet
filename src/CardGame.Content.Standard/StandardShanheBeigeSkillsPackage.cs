using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The pay-before-judgment, fixed-suit outcomes from S5 source skill 3140.</summary>
public sealed class StandardShanheBeigeSkillsPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-beige-3140";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-beige-skills", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"triggers":[
            {"id":"pay-then-judge-slash-damage","window":"afterDamageApplied","subject":"any",
             "damageOccurrence":"perDamage","optional":true,
             "condition":{"kind":"all","children":[
               {"kind":"damageCardIsSlash"},
               {"kind":"compare","left":{"kind":"eventTargetHp"},"operator":"greaterThan","right":{"kind":"integerConstant","value":0}}
             ]},
             "effects":[
               {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                "zones":["hand","equipment"],"count":1,"destination":"discardPile","resultBind":"beige-cost","awaitMovementTriggers":true},
               {"op":"selectTarget","target":"owner","targetKind":"eventTarget"},
               {"op":"startJudgment","target":"selectedTarget","judgmentReason":"skill.shanhe.beige-3140",
                "resultBind":"beige-card","outcomeBind":"beige-result","visibility":"public","sourceRef":{"kind":"eventSource"}},
               {"op":"recover","target":"owner","targetRef":{"kind":"eventTarget"},"amount":1,
                "condition":{"kind":"judgmentOutcomeMatchesSuits","sourceBind":"beige-result","suits":["heart"]}},
               {"op":"draw","target":"owner","targetRef":{"kind":"eventTarget"},"amount":2,
                "condition":{"kind":"judgmentOutcomeMatchesSuits","sourceBind":"beige-result","suits":["diamond"]}},
               {"op":"selectOwnedCards","target":"owner","targetRef":{"kind":"eventSource"},"zones":["hand","equipment"],
                "amount":2,"resultBind":"beige-source-discard",
                "condition":{"kind":"judgmentOutcomeMatchesSuits","sourceBind":"beige-result","suits":["club"]}},
               {"op":"moveBoundCards","target":"owner","sourceBind":"beige-source-discard","destination":"discardPile","awaitMovementTriggers":true,
                "condition":{"kind":"judgmentOutcomeMatchesSuits","sourceBind":"beige-result","suits":["club"]}},
               {"op":"turnOver","target":"owner","targetRef":{"kind":"eventSource"},
                "condition":{"kind":"judgmentOutcomeMatchesSuits","sourceBind":"beige-result","suits":["spade"]}},
               {"op":"moveBoundCards","target":"owner","sourceBind":"beige-card","destination":"discardPile"}
             ]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"悲歌","description":"当一名角色受到【杀】的伤害后，你可以弃置一张牌令其判定，若结果为：\n•红桃，其回复1点体力；\n•方片，其摸两张牌；\n•梅花，伤害来源弃置两张牌；\n•黑桃，伤害来源翻面。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation
        });
    }
}
