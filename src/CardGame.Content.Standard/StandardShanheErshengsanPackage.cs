using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Current source 2030 changes the native resolved DrawTwo quantity.</summary>
public sealed class StandardShanheErshengsanPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-ershengsan-2030";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-ershengsan-2030", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"cardPolicies":[
            {"id":"one-more-resolved-ex-nihilo-card","kind":"additionalDrawTwoCards",
             "cardKinds":["drawTwo"],"value":1}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"二生三","description":"【无中生有】额外摸1张牌。"}
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
