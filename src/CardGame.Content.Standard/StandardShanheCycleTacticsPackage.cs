using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 card-use cycle tactics, using the shared actual-turn declaration ordinal.</summary>
public sealed class StandardShanheCycleTacticsPackage : IGameContentPackage
{
    public const string ShoudaoQinlaiSkillId = "shanhe:current-shoudaoqinlai-2079";
    public const string ShoudaoQinlai2SkillId = "shanhe:current-shoudaoqinlai-2080";
    public const string ShoudaoQinlai3SkillId = "shanhe:current-shoudaoqinlai-2081";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-cycle-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShoudaoQinlaiSkillId}}}","revision":1,
           "triggers":[{"id":"seventh-card-completed","window":"cardUseCompleted","ownerRelation":"actor","optional":false,
             "condition":{"kind":"cardActionActualTurnUseOrdinalIs","value":7},
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{ShoudaoQinlai2SkillId}}}","revision":1,
           "triggers":[{"id":"fifth-card-completed","window":"cardUseCompleted","ownerRelation":"actor","optional":false,
             "condition":{"kind":"cardActionActualTurnUseOrdinalIs","value":5},
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{ShoudaoQinlai3SkillId}}}","revision":1,
           "triggers":[{"id":"sixth-card-completed","window":"cardUseCompleted","ownerRelation":"actor","optional":false,
             "condition":{"kind":"cardActionActualTurnUseOrdinalIs","value":6},
             "effects":[{"op":"draw","target":"owner","amount":2}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShoudaoQinlaiSkillId}}}":{"name":"手到擒来","description":"每回合你使用第7张牌后,你摸1张牌"},
          "{{{ShoudaoQinlai2SkillId}}}":{"name":"手到擒来Ⅱ","description":"每回合你使用第5张牌后,你摸1张牌"},
          "{{{ShoudaoQinlai3SkillId}}}":{"name":"手到擒来Ⅲ","description":"每回合你使用第6张牌后,你摸2张牌"}
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
