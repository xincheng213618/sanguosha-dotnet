using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-specific per-damage draws and one-time opening recovery.</summary>
public sealed class StandardShanheSustainOpeningTacticsPackage : IGameContentPackage
{
    public const string RuoFankuiSkillId = "shanhe:current-ruofankui-2227";
    public const string LiangcaoChubeiSkillId = "shanhe:current-liangcaochubei-2182";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-sustain-opening-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{RuoFankuiSkillId}}}","revision":1,
           "triggers":[{"id":"received-damage-point-draw","window":"afterDamageApplied","subject":"owner",
             "damageOccurrence":"perDamagePoint","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":1}]}]},
          {"id":"{{{LiangcaoChubeiSkillId}}}","revision":1,
           "triggers":[{"id":"native-game-starting-recovery","window":"gameStarting","subject":"owner","optional":false,
             "effects":[{"op":"recover","target":"owner","amount":1}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{RuoFankuiSkillId}}}":{"name":"弱反馈","description":"你受到1点伤害后，摸一张牌。"},
          "{{{LiangcaoChubeiSkillId}}}":{"name":"粮草储备","description":"游戏开始时，你回复1点体力。"}
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
