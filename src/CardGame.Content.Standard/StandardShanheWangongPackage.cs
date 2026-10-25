using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The player-stock Wangong rule from S5 source skill 3263.</summary>
public sealed class StandardShanheWangongPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:wangong-3263";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-wangong", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,
           "triggers":[{"id":"capture-previous-actual-basic","window":"cardUseCommitted",
             "ownerRelation":"actor","cardKinds":["slash","fireSlash","thunderSlash"],"optional":false,
             "effects":[{"op":"captureWushuangWangongUse","target":"owner"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"挽弓","description":"锁定技，若你使用的上一张牌是基本牌，你使用【杀】无距离和次数限制且造成的伤害+1。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked
        });
    }
}
