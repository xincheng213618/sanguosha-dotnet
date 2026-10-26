using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified Slash recast and outside-turn empty-hand tactics.</summary>
public sealed class StandardShanheRecastTacticsPackage : IGameContentPackage
{
    public const string ZhudaoSkillId = "shanhe:current-zhudao-2039";
    public const string Zhudao2SkillId = "shanhe:current-zhudao-2040";
    public const string JishiyuSkillId = "shanhe:current-jishiyu-2128";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-recast-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ZhudaoSkillId}}}","revision":1,"triggers":[
            {"id":"slash-completed-recast-one","window":"cardUseCompleted","ownerRelation":"actor","optional":true,
             "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],
             "effects":[
               {"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,
                "zones":["hand","equipment"],"resultBind":"recast-cost"},
               {"op":"recastBoundCards","target":"owner","sourceBind":"recast-cost"}
             ]}]},
          {"id":"{{{Zhudao2SkillId}}}","revision":1,"triggers":[
            {"id":"slash-completed-recast-up-to-two","window":"cardUseCompleted","ownerRelation":"actor","optional":true,
             "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],
             "effects":[
               {"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":2,
                "zones":["hand","equipment"],"resultBind":"recast-cost"},
               {"op":"recastBoundCards","target":"owner","sourceBind":"recast-cost"}
             ]}]},
          {"id":"{{{JishiyuSkillId}}}","revision":1,"triggers":[
            {"id":"outside-turn-last-hand-loss","window":"cardsMoved","subject":"owner","optional":false,
             "sourceZones":["hand"],"movementOccurrence":"perBatch",
             "condition":{"kind":"all","children":[
               {"kind":"not","children":[{"kind":"ownerIsTurnPlayer"}]},
               {"kind":"compare","left":{"kind":"sourceZoneCountBefore"},"operator":"greaterThan","right":{"kind":"integerConstant","value":0}},
               {"kind":"compare","left":{"kind":"sourceZoneCountAfter"},"operator":"equal","right":{"kind":"integerConstant","value":0}}
             ]},
             "effects":[{"op":"draw","target":"owner","amount":2}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ZhudaoSkillId}}}":{"name":"铸刀","description":"你使用【杀】后可以重铸1张牌"},
          "{{{Zhudao2SkillId}}}":{"name":"铸刀Ⅱ","description":"你使用【杀】后可以重铸至多2张牌"},
          "{{{JishiyuSkillId}}}":{"name":"及时雨","description":"回合外失去最后一张手牌后，摸两张牌"}
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
