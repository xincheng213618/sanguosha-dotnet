using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Current source 2045; the native use freezes the effective Heart suit.</summary>
public sealed class StandardShanheHeartBladePackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-guangongren-2045";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-guangongren-2045", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"damageModifiers":[
            {"id":"native-heart-slash-damage","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],
             "amount":2,"condition":"ownerUsedHeartSlash","sourceScope":"ownerUsed","damageLayer":"cardDamage"}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [SkillId] = new { name = "关公刃", description = "红桃【杀】伤害+2。" }
            }
        })));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var program = catalog.Programs[SkillId];
        var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(SkillId, Repeatable: false)
        });
    }
}
