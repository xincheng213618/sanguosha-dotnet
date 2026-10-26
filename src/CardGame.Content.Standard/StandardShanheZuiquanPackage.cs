using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 source 2001 uses the native Slash response prohibition without a Wine prerequisite.</summary>
public sealed class StandardShanheZuiquanPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-zuiquan-2001";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-zuiquan-2001", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"triggers":[
            {"id":"slash-cannot-be-canceled","window":"slashBeforeResponse","ownerRelation":"actor",
             "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"optional":false,
             "effects":[{"op":"prohibitCurrentResponse","target":"owner"}]}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"醉拳","description":"【酒】【杀】不能被抵消。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(SkillId, Repeatable: false)
        });
    }
}
