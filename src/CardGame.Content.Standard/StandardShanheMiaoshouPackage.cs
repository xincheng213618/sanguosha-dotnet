using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The source's Snatch-only range exemption reuses the native card policy.</summary>
public sealed class StandardShanheMiaoshouPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-miaoshou-2238";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-miaoshou-2238", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"cardPolicies":[
            {"id":"snatch-ignore-distance","kind":"ignoreUseDistance","cardKinds":["snatch"]}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"妙手空空","description":"你使用【顺手牵羊】无距离限制。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value; var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(SkillId, Repeatable: false)
        });
    }
}
