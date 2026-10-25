using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>
/// Battle effects granted only while an owned wealth tactic meets its campaign balance threshold.
/// Damage uses the existing all-source additional-damage layer, including its per-recipient chain application.
/// </summary>
public sealed class StandardShanheWealthPackage : IGameContentPackage
{
    public const string WealthHandLimitSkillId = "shanhe:current-shoucainu-2178";
    public const string WealthInitialHandSkillId = "shanhe:current-shoucainu-2179";
    public const string WealthDrawSkillId = "shanhe:current-shoucainu-2180";
    public const string WealthDamageSkillId = "shanhe:current-shoucainu-2181";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-wealth-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{WealthHandLimitSkillId}}}","revision":1,
           "modifiers":[{"id":"wealth-hand-capacity","query":"handLimit","operation":"add","value":2,"priority":0}]},
          {"id":"{{{WealthInitialHandSkillId}}}","revision":1,
           "modifiers":[{"id":"wealth-opening-cards","query":"initialHandSize","operation":"add","value":2,"priority":0}]},
          {"id":"{{{WealthDrawSkillId}}}","revision":1,
           "modifiers":[{"id":"wealth-normal-draw","query":"drawCount","operation":"add","value":2,"priority":0}]},
          {"id":"{{{WealthDamageSkillId}}}","revision":1,
           "damageModifiers":[{"id":"wealth-damage","cardKinds":[],"amount":1,"condition":"always","sourceScope":"damageSource"}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{WealthHandLimitSkillId}}}":{"name":"守财奴·手牌上限","description":"持有此战法且进入战斗时铜币大于300，你的手牌上限+2。"},
          "{{{WealthInitialHandSkillId}}}":{"name":"守财奴·初始手牌","description":"持有此战法且进入战斗时铜币大于500，你的初始手牌数+2。"},
          "{{{WealthDrawSkillId}}}":{"name":"守财奴·摸牌数","description":"持有此战法且进入战斗时铜币大于700，你的摸牌阶段摸牌数+2。"},
          "{{{WealthDamageSkillId}}}":{"name":"守财奴·伤害","description":"持有此战法且进入战斗时铜币大于1000，你造成的伤害+1。"}
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
                Tags = SkillTag.Locked
            });
        }
    }
}
