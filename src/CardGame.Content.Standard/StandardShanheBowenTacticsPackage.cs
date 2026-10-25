using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 turn-start trick gains from the battle's shuffled physical deck.</summary>
public sealed class StandardShanheBowenTacticsPackage : IGameContentPackage
{
    public const string BowenSkillId = "shanhe:current-bowen-2249";
    public const string Bowen2SkillId = "shanhe:current-bowen-2250";
    public const string Bowen3SkillId = "shanhe:current-bowen-2251";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-bowen-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{BowenSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-trick-gain","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":1,"category":"trick"}]}]},
          {"id":"{{{Bowen2SkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-trick-gain","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":2,"category":"trick"}]}]},
          {"id":"{{{Bowen3SkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-start-trick-gain","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"obtainDeckCards","target":"owner","amount":3,"category":"trick"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{BowenSkillId}}}":{"name":"博闻Ⅰ","description":"你的回合开始时，从牌堆中获得1张随机锦囊牌"},
          "{{{Bowen2SkillId}}}":{"name":"博闻Ⅱ","description":"你的回合开始时，从牌堆中获得2张随机锦囊牌"},
          "{{{Bowen3SkillId}}}":{"name":"博闻Ⅲ","description":"你的回合开始时，从牌堆中获得3张随机锦囊牌"}
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
