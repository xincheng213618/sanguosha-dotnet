using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Pinned current-source 2108/2109; grants additive paid Alcohol quota.</summary>
public sealed class StandardShanheWineTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-wine-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentHugujiuSkillId}}}","revision":1,"cardPolicies":[
            {"id":"one-more-ordinary-wine","kind":"additionalAlcoholUses","cardKinds":["alcohol"],"value":1}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentHugujiu2SkillId}}}","revision":1,"cardPolicies":[
            {"id":"two-more-ordinary-wines","kind":"additionalAlcoholUses","cardKinds":["alcohol"],"value":2}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [StandardShanheCampaignCatalog.CurrentHugujiuSkillId] = new { name = "虎骨酒", description = "每回合，你可以额外使用1次【酒】" },
                [StandardShanheCampaignCatalog.CurrentHugujiu2SkillId] = new { name = "虎骨酒Ⅱ", description = "每回合，你可以额外使用2次【酒】" }
            }
        })));

    public void Register(IContentRegistryBuilder builder)
    {
        foreach (var (id, program) in Programs.Value.Programs)
        {
            var presentation = Programs.Value.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            {
                Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
