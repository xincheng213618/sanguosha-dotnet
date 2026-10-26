using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheHandLimitStockPackage : IGameContentPackage
{
    public const string HaoshenfaSkillId = "shanhe:current-haoshenfa-2002";
    public const string CangtaohuSkillId = "shanhe:current-cangtaohu-2003";
    public const string StableCapacitySkillId = "shanhe:current-stable-capacity-2211";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-hand-limit-stock", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{HaoshenfaSkillId}}}","revision":1,"cardPolicies":[
            {"id":"dodge-not-counted","kind":"handLimitCardKindExemption","cardKinds":["dodge"]}]},
          {"id":"{{{CangtaohuSkillId}}}","revision":1,"cardPolicies":[
            {"id":"peach-not-counted","kind":"handLimitCardKindExemption","cardKinds":["peach"]}]},
          {"id":"{{{StableCapacitySkillId}}}","revision":1,"modifiers":[
            {"id":"capacity-is-current-max-hp","query":"handLimit","operation":"set","valueExpression":"ownerMaxHp","priority":-1}]}
        ]}
        """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [HaoshenfaSkillId] = new { name = "好身法", description = "【闪】不计入手牌上限。" },
                [CangtaohuSkillId] = new { name = "藏桃户", description = "【桃】不计入手牌上限。" },
                [StableCapacitySkillId] = new { name = "稳定承载", description = "手牌上限等于你的体力上限。" }
            } })));

    public void Register(IContentRegistryBuilder builder)
    {
        foreach (var (id, program) in Programs.Value.Programs)
        {
            var presentation = Programs.Value.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false) });
        }
    }
}
