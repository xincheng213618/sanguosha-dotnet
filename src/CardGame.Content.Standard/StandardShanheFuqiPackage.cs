using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 player-stock Fuqi keeps the source's distance-one response boundary.</summary>
public sealed class StandardShanheFuqiPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:fuqi-3100";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-fuqi-current", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,
           "cardPolicies":[{"id":"distance-one-cannot-respond","kind":"prohibitOthersResponse","value":1}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"伏骑","description":"锁定技，与你距离为1的其他角色不能使用或打出牌响应你使用的牌。"}
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
