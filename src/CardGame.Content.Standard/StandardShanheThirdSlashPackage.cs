using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Every third declared Slash across the battle; damage success is not the counter.</summary>
public sealed class StandardShanheThirdSlashPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-third-slash", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentSanbanfuSkillId}}}","revision":1,"damageModifiers":[
            {"id":"every-third-native-slash","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":1,
             "condition":"ownerGameSlashUseOrdinalMultipleOfThree","sourceScope":"ownerUsed","damageLayer":"cardDamage"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentSanbanfu2SkillId}}}","revision":1,"damageModifiers":[
            {"id":"every-third-native-slash","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":2,
             "condition":"ownerGameSlashUseOrdinalMultipleOfThree","sourceScope":"ownerUsed","damageLayer":"cardDamage"}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [StandardShanheCampaignCatalog.CurrentSanbanfuSkillId] = new { name = "三板斧", description = "你使用的每第3张【杀】伤害+1" },
                [StandardShanheCampaignCatalog.CurrentSanbanfu2SkillId] = new { name = "三板斧Ⅱ", description = "你使用的每第3张【杀】伤害+2" }
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
