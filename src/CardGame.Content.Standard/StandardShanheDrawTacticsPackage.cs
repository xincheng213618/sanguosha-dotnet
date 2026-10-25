using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified S5 normal-draw and own-turn-ending tactics.</summary>
public sealed class StandardShanheDrawTacticsPackage : IGameContentPackage
{
    public const string MopaiSkillId = "shanhe:current-mopai-2059";
    public const string Mopai2SkillId = "shanhe:current-mopai-2060";
    public const string JixingjunSkillId = "shanhe:current-jixingjun-2061";
    public const string HoufaXianzhiSkillId = "shanhe:current-houfaxianzhi-2103";
    public const string DuipaiSkillId = "shanhe:current-duipai-2201";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-draw-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{MopaiSkillId}}}","revision":1,
           "modifiers":[{"id":"normal-draw-one-more","query":"drawCount","operation":"add","value":1,"priority":0}]},
          {"id":"{{{Mopai2SkillId}}}","revision":1,
           "modifiers":[{"id":"normal-draw-two-more","query":"drawCount","operation":"add","value":2,"priority":0}]},
          {"id":"{{{JixingjunSkillId}}}","revision":1,
           "modifiers":[{"id":"normal-draw-one-more","query":"drawCount","operation":"add","value":1,"priority":0}]},
          {"id":"{{{HoufaXianzhiSkillId}}}","revision":1,
           "modifiers":[{"id":"normal-draw-one-less","query":"drawCount","operation":"add","value":-1,"priority":0}],
           "triggers":[{"id":"own-turn-ending-draw","window":"turnEnding","subject":"owner","turnOwnerScope":"own","optional":false,
             "effects":[{"op":"draw","target":"owner","amount":3}]}]},
          {"id":"{{{DuipaiSkillId}}}","revision":1,
           "modifiers":[{"id":"other-allied-normal-draw","query":"drawCount","operation":"add","value":2,"priority":0,
             "recipientScope":"otherPublicTeamAllies"}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{MopaiSkillId}}}":{"name":"摸牌Ⅰ","description":"摸牌阶段，你的摸牌数+1"},
          "{{{Mopai2SkillId}}}":{"name":"摸牌Ⅱ","description":"摸牌阶段，你的摸牌数+2"},
          "{{{JixingjunSkillId}}}":{"name":"急行军","description":"摸牌阶段，你的摸牌数+1"},
          "{{{HoufaXianzhiSkillId}}}":{"name":"后发先至","description":"摸牌阶段，你的摸牌数-1；回合结束时，你摸3张牌"},
          "{{{DuipaiSkillId}}}":{"name":"队·牌","description":"你的所有队友摸牌数+2"}
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
                Program = program,
                ProgramPresentation = presentation,
                Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false)
            });
        }
    }
}
