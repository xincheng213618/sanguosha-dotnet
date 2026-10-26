using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The capped hand refill from S5 source skill 3083.</summary>
public sealed class StandardShanheCappedRefillSkillsPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-jieming-3083";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-capped-refill-skills", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"triggers":[
            {"id":"capped-refill-per-damage-point","window":"afterDamageApplied","subject":"owner",
             "damageOccurrence":"perDamagePoint","optional":true,
             "effects":[
               {"op":"selectTarget","target":"owner","targetKind":"anyLiving"},
               {"op":"drawToHandCount","target":"selectedTarget","amount":5,"capAtTargetMaxHp":true}
             ]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"节命","description":"当你受到1点伤害后，你可以令一名角色将手牌摸至X张（X为其体力上限且至多为5）。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation
        });
    }
}
