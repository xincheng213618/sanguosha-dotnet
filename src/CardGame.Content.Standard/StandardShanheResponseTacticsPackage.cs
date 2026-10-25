using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Native counterspell finality and source-specific effective pindian rank.</summary>
public sealed class StandardShanheResponseTacticsPackage : IGameContentPackage
{
    public const string JueduiWuxieSkillId = "shanhe:current-jueduiwuxie-2167";
    public const string Dushu2SkillId = "shanhe:current-dushu-2308";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-response-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{JueduiWuxieSkillId}}}","revision":1,
           "cardPolicies":[{"id":"unrespondable-counterspell","kind":"unrespondableNullification","cardKinds":["nullification"]}]},
          {"id":"{{{Dushu2SkillId}}}","revision":1,
           "cardPolicies":[{"id":"six-more-pindian-rank","kind":"pindianRankBonus","value":6}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{JueduiWuxieSkillId}}}":{"name":"绝对无懈","description":"其他角色无法响应你的【无懈可击】。"},
          "{{{Dushu2SkillId}}}":{"name":"赌术II","description":"你的拼点牌点数+6（最大为K）。"}
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
                Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
