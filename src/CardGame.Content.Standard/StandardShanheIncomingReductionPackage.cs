using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified incoming damage reduction with the native round allowance.</summary>
public sealed class StandardShanheIncomingReductionPackage : IGameContentPackage
{
    public const string Houshi1SkillId = "shanhe:current-houshi-2196";
    public const string Houshi2SkillId = "shanhe:current-houshi-2197";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-incoming-reduction", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{Houshi1SkillId}}}","revision":1,"triggers":[
            {"id":"round-first-two-incoming","window":"beforeDamageApplied","subject":"damageTarget","optional":false,
             "usageScope":"round","usageLimit":2,"effects":[{"op":"reduceCurrentDamage","target":"owner","amount":1}]}]},
          {"id":"{{{Houshi2SkillId}}}","revision":1,"triggers":[
            {"id":"round-first-three-incoming","window":"beforeDamageApplied","subject":"damageTarget","optional":false,
             "usageScope":"round","usageLimit":3,"effects":[{"op":"reduceCurrentDamage","target":"owner","amount":2}]}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [Houshi1SkillId] = new { name = "厚实Ⅰ", description = "每轮你前2次受到的伤害-1" },
                [Houshi2SkillId] = new { name = "厚实Ⅱ", description = "每轮你前3次受到的伤害-2" }
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
