using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Earlier source tiers of native end-turn draws and killer recovery.</summary>
public sealed class StandardShanheSustainTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-sustain-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuanzhuSkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-ending-draw","window":"turnEnding","subject":"owner","turnOwnerScope":"own","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuanzhu2SkillId}}}","revision":1,
           "triggers":[{"id":"own-turn-ending-draw","window":"turnEnding","subject":"owner","turnOwnerScope":"own","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":2}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentSouguaSkillId}}}","revision":1,
           "triggers":[{"id":"real-killer-recovers","window":"characterDied","subject":"owner","optional":false,
             "condition":{"kind":"deathKillerIsOwner"},"effects":[{"op":"recover","target":"owner","amount":1}]}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentSougua2SkillId}}}","revision":1,
           "triggers":[{"id":"real-killer-recovers","window":"characterDied","subject":"owner","optional":false,
             "condition":{"kind":"deathKillerIsOwner"},"effects":[{"op":"recover","target":"owner","amount":2}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{StandardShanheCampaignCatalog.CurrentYuanzhuSkillId}}}":{"name":"援助","description":"你的回合结束时，摸1张牌。"},
          "{{{StandardShanheCampaignCatalog.CurrentYuanzhu2SkillId}}}":{"name":"援助Ⅱ","description":"你的回合结束时，摸2张牌。"},
          "{{{StandardShanheCampaignCatalog.CurrentSouguaSkillId}}}":{"name":"搜刮Ⅰ","description":"当你击杀其他角色后，回复1点体力。"},
          "{{{StandardShanheCampaignCatalog.CurrentSougua2SkillId}}}":{"name":"搜刮Ⅱ","description":"当你击杀其他角色后，回复2点体力。"}
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
