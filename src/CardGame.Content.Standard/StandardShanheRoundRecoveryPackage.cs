using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified maximum-health cost and real round-start recovery.</summary>
public sealed class StandardShanheRoundRecoveryPackage : IGameContentPackage
{
    public const string Xuezhan2SkillId = "shanhe:current-xuezhan-2265";
    public const string Xuezhan3SkillId = "shanhe:current-xuezhan-2266";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-round-recovery", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{Xuezhan2SkillId}}}","revision":1,"triggers":[
            {"id":"opening-maximum-cost","window":"gameStarting","subject":"owner","optional":false,
             "usageScope":"game","usageLimit":1,
             "effects":[{"op":"changeMaximumHp","target":"owner","amount":-2}]},
            {"id":"round-recovery","window":"roundStarting","subject":"owner","optional":false,
             "usageScope":"round","usageLimit":1,
             "effects":[{"op":"recover","target":"owner","amount":2}]}
          ]},
          {"id":"{{{Xuezhan3SkillId}}}","revision":1,"triggers":[
            {"id":"opening-maximum-cost","window":"gameStarting","subject":"owner","optional":false,
             "usageScope":"game","usageLimit":1,
             "effects":[{"op":"changeMaximumHp","target":"owner","amount":-3}]},
            {"id":"round-recovery","window":"roundStarting","subject":"owner","optional":false,
             "usageScope":"round","usageLimit":1,
             "effects":[{"op":"recover","target":"owner","amount":3}]}
          ]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [Xuezhan2SkillId] = new { name = "血战Ⅱ", description = "体力上限-2（最低为1），每轮开始时回复2点体力" },
                [Xuezhan3SkillId] = new { name = "血战Ⅲ", description = "体力上限-3（最低为1），每轮开始时回复3点体力" }
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
