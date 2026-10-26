using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified low-health outgoing damage tactics.</summary>
public sealed class StandardShanheLowHpDamagePackage : IGameContentPackage
{
    public const string KuangbaoSkillId = "shanhe:current-kuangbao-2162";
    public const string Kuangbao2SkillId = "shanhe:current-kuangbao-2163";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-low-hp-damage", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{KuangbaoSkillId}}}","revision":1,"damageModifiers":[
            {"id":"low-hp-damage","cardKinds":[],"amount":1,"condition":"sourceHpAtMost","maximumHp":2,"sourceScope":"damageSource"}]},
          {"id":"{{{Kuangbao2SkillId}}}","revision":1,"damageModifiers":[
            {"id":"low-hp-damage","cardKinds":[],"amount":1,"condition":"sourceHpAtMost","maximumHp":3,"sourceScope":"damageSource"}]}
        ]}
        """, JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [KuangbaoSkillId] = new { name = "狂暴", description = "体力值小于等于2时，你造成的伤害+1" },
                [Kuangbao2SkillId] = new { name = "狂暴Ⅱ", description = "体力值小于等于3时，你造成的伤害+1" }
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
