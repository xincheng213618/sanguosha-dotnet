using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Recruitment logistics exchange initial entities for native draw-phase additions.</summary>
public sealed class StandardShanheLogisticsTacticsPackage : IGameContentPackage
{
    public const string ZhaomuHouqin2SkillId = "shanhe:current-zhaomu-houqin-2159";
    public const string ZhaomuHouqinSkillId = "shanhe:current-zhaomu-houqin-2160";
    public const string ZhaomuHouqin3SkillId = "shanhe:current-zhaomu-houqin-2161";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-logistics-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ZhaomuHouqin2SkillId}}}","revision":1,"modifiers":[
            {"id":"one-less-initial-card","query":"initialHandSize","operation":"add","value":-1,"priority":0},
            {"id":"one-more-normal-draw","query":"drawCount","operation":"add","value":1,"priority":0}]},
          {"id":"{{{ZhaomuHouqinSkillId}}}","revision":1,"modifiers":[
            {"id":"two-less-initial-cards","query":"initialHandSize","operation":"add","value":-2,"priority":0},
            {"id":"one-more-normal-draw","query":"drawCount","operation":"add","value":1,"priority":0}]},
          {"id":"{{{ZhaomuHouqin3SkillId}}}","revision":1,"modifiers":[
            {"id":"two-less-initial-cards","query":"initialHandSize","operation":"add","value":-2,"priority":0},
            {"id":"three-more-normal-draw","query":"drawCount","operation":"add","value":3,"priority":0}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ZhaomuHouqin2SkillId}}}":{"name":"招募后勤Ⅱ","description":"初始手牌-1，摸牌数+1。"},
          "{{{ZhaomuHouqinSkillId}}}":{"name":"招募后勤","description":"初始手牌-2，摸牌数+1。"},
          "{{{ZhaomuHouqin3SkillId}}}":{"name":"招募后勤Ⅲ","description":"初始手牌-2，摸牌数+3。"}
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
