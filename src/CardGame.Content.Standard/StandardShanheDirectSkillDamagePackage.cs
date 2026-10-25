using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified S5 direct-skill damage tactics, including the table-only third tier.</summary>
public sealed class StandardShanheDirectSkillDamagePackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-direct-skill-damage-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentJiyiSkillId}}}","revision":1,"damageModifiers":[
            {"id":"direct-skill-damage","cardKinds":[],"amount":1,"condition":"directSkillDamage","sourceScope":"damageSource"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentJiyi2SkillId}}}","revision":1,"damageModifiers":[
            {"id":"direct-skill-damage","cardKinds":[],"amount":2,"condition":"directSkillDamage","sourceScope":"damageSource"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentJiyi3SkillId}}}","revision":1,"damageModifiers":[
            {"id":"direct-skill-damage","cardKinds":[],"amount":3,"condition":"directSkillDamage","sourceScope":"damageSource"}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{StandardShanheCampaignCatalog.CurrentJiyiSkillId}}}":{"name":"技艺Ⅰ","description":"当你的技能直接造成伤害时，此伤害+1。"},
          "{{{StandardShanheCampaignCatalog.CurrentJiyi2SkillId}}}":{"name":"技艺Ⅱ","description":"当你的技能直接造成伤害时，此伤害+2。"},
          "{{{StandardShanheCampaignCatalog.CurrentJiyi3SkillId}}}":{"name":"技艺Ⅲ","description":"当你的技能直接造成伤害时，此伤害+3。"}
        }}
        """));

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
