using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed class StandardShanheSchoolBattlePackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-school-battle", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheSchoolEffects.MultiSlashSkillId}}}","revision":1,"modifiers":[
            {"id":"unlimited-slash","query":"slashLimit","operation":"unlimited","value":0,"priority":0}
          ]}
        ]}
        """, $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShanheSchoolEffects.MultiSlashSkillId}}}":{"name":"多刀流套装","description":"使用杀无次数限制。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        foreach (var (id, program) in Programs.Value.Programs)
        {
            var presentation = Programs.Value.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked });
        }
    }
}
