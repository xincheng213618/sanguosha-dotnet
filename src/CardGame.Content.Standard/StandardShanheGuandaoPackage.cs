using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Current source 2010 uses the existing effective-suit Slash distance policy.</summary>
public sealed class StandardShanheGuandaoPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-guandao-2010";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-guandao-2010", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"cardPolicies":[
            {"id":"diamond-slash-distance","kind":"ignoreSlashUseDistanceBySuit",
             "cardKinds":["slash","fireSlash","thunderSlash"],"inputSuit":"diamond"}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"关刀之脊","description":"方片【杀】无距离限制。"}
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
