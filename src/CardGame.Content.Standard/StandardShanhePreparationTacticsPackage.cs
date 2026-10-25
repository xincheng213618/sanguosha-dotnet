using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-specific opening draws from the battle's already shuffled physical deck.</summary>
public sealed class StandardShanhePreparationTacticsPackage : IGameContentPackage
{
    public const string ChoubeiSkillId = "shanhe:current-choubei-2064";
    public const string Choubei2SkillId = "shanhe:current-choubei-2065";
    public const string Choubei3SkillId = "shanhe:current-choubei-2066";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-preparation-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ChoubeiSkillId}}}","revision":1,
           "triggers":[{"id":"native-game-starting-draw","window":"gameStarting","subject":"owner","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{Choubei2SkillId}}}","revision":1,
           "triggers":[{"id":"native-game-starting-draw","window":"gameStarting","subject":"owner","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":2}]}]},
          {"id":"{{{Choubei3SkillId}}}","revision":1,
           "triggers":[{"id":"native-game-starting-draw","window":"gameStarting","subject":"owner","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":3}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ChoubeiSkillId}}}":{"name":"筹备","description":"游戏开始时，你获得1张随机手牌。"},
          "{{{Choubei2SkillId}}}":{"name":"筹备Ⅱ","description":"游戏开始时，你获得2张随机手牌。"},
          "{{{Choubei3SkillId}}}":{"name":"筹备Ⅲ","description":"游戏开始时，你获得3张随机手牌。"}
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
