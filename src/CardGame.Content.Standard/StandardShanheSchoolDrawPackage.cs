using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

/// <summary>The completed Shanhe draw school's native hand refill.</summary>
public sealed class StandardShanheSchoolDrawPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-school-draw", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheSchoolEffects.DrawSkillId}}}","revision":1,"triggers":[
            {"id":"opening-refill","window":"gameStarting","subject":"owner","optional":false,
             "condition":{"kind":"compare","left":{"kind":"currentHandCount"},"operator":"lessThan","right":{"kind":"integerConstant","value":8}},
             "effects":[{"op":"drawToHandCount","target":"owner","amount":8}]},
            {"id":"hand-loss-refill","window":"cardsMoved","subject":"owner","optional":false,
             "sourceZones":["hand"],"movementOccurrence":"perBatch",
             "condition":{"kind":"compare","left":{"kind":"currentHandCount"},"operator":"lessThan","right":{"kind":"integerConstant","value":8}},
             "effects":[{"op":"drawToHandCount","target":"owner","amount":8}]}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShanheSchoolEffects.DrawSkillId}}}":{"name":"过牌流套装","description":"手牌少于8时补满8"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[ShanheSchoolEffects.DrawSkillId];
        builder.AddSkill(new(ShanheSchoolEffects.DrawSkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[ShanheSchoolEffects.DrawSkillId], ProgramPresentation = presentation,
            Tags = SkillTag.Locked
        });
    }
}
