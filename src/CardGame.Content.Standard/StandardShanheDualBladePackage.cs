using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheDualBladePackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-dual-blade-2033";
    public const string UpgradedSkillId = "shanhe:current-dual-blade-2034";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-dual-blade", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"cardPolicies":[
            {"id":"first-round-slash-extra-target","kind":"roundFirstSlashAdditionalTargets",
             "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"value":1}]},
          {"id":"{{{UpgradedSkillId}}}","revision":1,"cardPolicies":[
            {"id":"first-round-slash-extra-targets","kind":"roundFirstSlashAdditionalTargets",
             "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"value":2}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [SkillId] = new { name = "双刃", description = "你每轮首张【杀】可以额外选择1个目标。" },
                [UpgradedSkillId] = new { name = "双刃Ⅱ", description = "你每轮首张【杀】可以额外选择2个目标。" }
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
