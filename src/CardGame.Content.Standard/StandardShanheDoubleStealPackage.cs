using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheDoubleStealPackage : IGameContentPackage
{
    public const string SkillId = ShanheDoubleStealPrograms.SkillId;
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-double-steal-2337", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"triggers":[
            {"id":"native-snatch-extra-effect","window":"cardUseCommitted","ownerRelation":"actor",
             "cardKinds":["snatch"],"optional":false,
             "effects":[{"op":"repeatCurrentSnatch","target":"owner"}]}]}
        ]}
        """, $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"双掠","description":"你使用的【顺手牵羊】额外结算一次。"}
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
