using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

/// <summary>Source 2199's allied health privilege; its native initializer preserves wounds.</summary>
public sealed class StandardShanheTeamHealthPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-team-health-2199", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheTeamRankPrograms.TeamHealthSkillId}}}","revision":1,"triggers":[
            {"id":"allied-health-privilege","window":"gameStarting","subject":"owner","optional":false,
             "usageScope":"game","usageLimit":1,
             "effects":[{"op":"applyShanheAlliedHealthBonus","target":"owner"}]}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShanheTeamRankPrograms.TeamHealthSkillId}}}":{"name":"队·体","description":"你的所有队友体力+3。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value; var id = ShanheTeamRankPrograms.TeamHealthSkillId;
        var presentation = catalog.Presentations[id];
        builder.AddSkill(new(id, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[id], ProgramPresentation = presentation, Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(id, Repeatable: false)
        });
    }
}
