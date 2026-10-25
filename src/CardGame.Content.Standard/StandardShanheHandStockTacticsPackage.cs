using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 hand capacity and low-hand Slash tactics using native numeric and damage rules.</summary>
public sealed class StandardShanheHandStockTacticsPackage : IGameContentPackage
{
    public const string PinangSkillId = "shanhe:current-pinang-2132";
    public const string Pinang2SkillId = "shanhe:current-pinang-2133";
    public const string Pinang3SkillId = "shanhe:current-pinang-2135";
    public const string JingyuMoulueSkillId = "shanhe:current-jingyu-moulue-2194";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-hand-stock-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{PinangSkillId}}}","revision":1,
           "modifiers":[{"id":"hand-capacity-one-more","query":"handLimit","operation":"add","value":1,"priority":0}]},
          {"id":"{{{Pinang2SkillId}}}","revision":1,
           "modifiers":[{"id":"hand-capacity-two-more","query":"handLimit","operation":"add","value":2,"priority":0}]},
          {"id":"{{{Pinang3SkillId}}}","revision":1,
           "modifiers":[{"id":"hand-capacity-five-more","query":"handLimit","operation":"add","value":5,"priority":0}]},
          {"id":"{{{JingyuMoulueSkillId}}}","revision":1,
           "damageModifiers":[{"id":"low-hand-slash","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],
             "amount":1,"condition":"sourceHandBelowLimit","sourceHandCountLimit":4,"damageLayer":"additionalDamage"}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{PinangSkillId}}}":{"name":"皮囊","description":"手牌上限+1"},
          "{{{Pinang2SkillId}}}":{"name":"皮囊Ⅱ","description":"手牌上限+2"},
          "{{{Pinang3SkillId}}}":{"name":"皮囊Ⅲ","description":"手牌上限+5"},
          "{{{JingyuMoulueSkillId}}}":{"name":"精于谋略","description":"手牌数小于4时，你使用【杀】造成的伤害+1"}
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
