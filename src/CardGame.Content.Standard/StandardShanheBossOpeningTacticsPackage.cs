using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source tiers of boss-only opening cards, dealt by the ordinary initial-hand query.</summary>
public sealed class StandardShanheBossOpeningTacticsPackage : IGameContentPackage
{
    public const string WeiyuChoumouSkillId = "shanhe:current-weiyuchoumou-2025";
    public const string WeiyuChoumou2SkillId = "shanhe:current-weiyuchoumou-2026";
    public const string WeiyuChoumou3SkillId = "shanhe:current-weiyuchoumou-2027";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-boss-opening-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{WeiyuChoumouSkillId}}}","revision":1,
           "modifiers":[{"id":"boss-opening-cards","query":"initialHandSize","operation":"add","value":2,"priority":0}]},
          {"id":"{{{WeiyuChoumou2SkillId}}}","revision":1,
           "modifiers":[{"id":"boss-opening-cards","query":"initialHandSize","operation":"add","value":3,"priority":0}]},
          {"id":"{{{WeiyuChoumou3SkillId}}}","revision":1,
           "modifiers":[{"id":"boss-opening-cards","query":"initialHandSize","operation":"add","value":4,"priority":0}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{WeiyuChoumouSkillId}}}":{"name":"未雨绸缪","description":"仅首领战中，你随机获得2张初始手牌。"},
          "{{{WeiyuChoumou2SkillId}}}":{"name":"未雨绸缪Ⅱ","description":"仅首领战中，你随机获得3张初始手牌。"},
          "{{{WeiyuChoumou3SkillId}}}":{"name":"未雨绸缪Ⅲ","description":"仅首领战中，你随机获得4张初始手牌。"}
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
                Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
