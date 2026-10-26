using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified actual drawn-card threshold tactic.</summary>
public sealed class StandardShanheDrawThresholdPackage : IGameContentPackage
{
    public const string ShenlongBaiweiSkillId = "shanhe:current-shenlong-baiwei-2104";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-draw-threshold", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShenlongBaiweiSkillId}}}","revision":1,"triggers":[
            {"id":"ninth-actual-drawn-card","window":"cardsGained","subject":"owner",
             "destinationZones":["hand"],"movementOccurrence":"perBatch","optional":false,
             "effects":[{"op":"damageRandomEnemyAtDrawThreshold","target":"owner","threshold":9,"amount":1}]}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [ShenlongBaiweiSkillId] = new { name = "神龙摆尾", description = "你每摸9张牌，对随机敌方造成1点伤害" }
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
                Program = program,
                ProgramPresentation = presentation,
                Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
