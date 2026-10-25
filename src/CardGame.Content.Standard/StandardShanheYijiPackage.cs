using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The original private top-deck distribution from S5 source skill 3081.</summary>
public sealed class StandardShanheYijiPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:yiji-3081";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-yiji", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"triggers":[
            {"id":"private-top-distribution","window":"afterDamageApplied","subject":"owner",
             "damageOccurrence":"perDamagePoint","optional":true,
             "effects":[{"op":"resolveWushuangOriginalYiji","target":"owner"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"遗计","description":"当你受到1点伤害后，你可以观看牌堆顶的两张牌，然后将这些牌交给任意角色。"}
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
