using System.Text.Json;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

/// <summary>Derived school effects; acquired components, rather than a shop item, grant these programs.</summary>
public sealed class StandardShanheSchoolVitalPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-school-vital-effects", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheSchoolEffects.DamageSkillId}}}","revision":1,
           "damageModifiers":[{"id":"school-all-damage","cardKinds":[],"amount":1,"condition":"always","sourceScope":"damageSource"}]},
          {"id":"{{{ShanheSchoolEffects.FullHpSkillId}}}","revision":1,
           "triggers":[{"id":"preparation-hp-adjustment","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"setShanheSchoolHpToMaximum","target":"owner"}]}]},
          {"id":"{{{ShanheSchoolEffects.DyingSkillId}}}","revision":1,
           "triggers":[{"id":"first-dying-each-turn","window":"dyingEntering","subject":"owner","optional":false,
             "usageScope":"turn","usageLimit":1,
             "effects":[{"op":"recoverTo","target":"owner","numberExpression":"integerConstant","minimumValue":1,"clampToMaxHp":true}]}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [ShanheSchoolEffects.DamageSkillId] = new { name = "伤害流套装", description = "所有伤害+1" },
                [ShanheSchoolEffects.FullHpSkillId] = new { name = "卖血流套装", description = "准备阶段，你的体力调整为体力上限" },
                [ShanheSchoolEffects.DyingSkillId] = new { name = "残血流套装", description = "每回合首次进入濒死时，回复至1点体力" }
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
                Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked
            });
        }
    }
}
