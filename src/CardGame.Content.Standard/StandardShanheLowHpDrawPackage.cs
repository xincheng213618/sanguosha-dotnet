using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Current S5 low-health normal-draw tactics, independent from the damage variants.</summary>
public sealed class StandardShanheLowHpDrawPackage : IGameContentPackage
{
    public const string Kuangbao3SkillId = "shanhe:current-kuangbao-2164";
    public const string Kuangbao4SkillId = "shanhe:current-kuangbao-2165";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-low-hp-draw", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{Kuangbao3SkillId}}}","revision":1,"modifiers":[
            {"id":"normal-draw-at-most-three-hp","query":"drawCount","operation":"add","value":1,"priority":0,
             "condition":{"kind":"not","children":[{"kind":"hpAtLeast","value":4}]}
            }]},
          {"id":"{{{Kuangbao4SkillId}}}","revision":1,"modifiers":[
            {"id":"normal-draw-at-most-five-hp","query":"drawCount","operation":"add","value":1,"priority":0,
             "condition":{"kind":"not","children":[{"kind":"hpAtLeast","value":6}]}
            }]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [Kuangbao3SkillId] = new { name = "狂暴Ⅲ", description = "体力值小于等于3时，你的摸牌数+1" },
                [Kuangbao4SkillId] = new { name = "狂暴Ⅳ", description = "体力值小于等于5时，你的摸牌数+1" }
            }
        })));

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
