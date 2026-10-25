using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 armor bypass and armor damage supplied to the native damage payment.</summary>
public sealed class StandardShanheArmorBreakTacticsPackage : IGameContentPackage
{
    public const string GeshanDaniuSkillId = "shanhe:current-geshan-daniu-2177";
    public const string TijiaShuSkillId = "shanhe:current-tijia-shu-2193";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-armor-break-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{GeshanDaniuSkillId}}}","revision":1,
           "cardPolicies":[{"id":"bypass-other-armor","kind":"bypassOtherParticipantArmor"}]},
          {"id":"{{{TijiaShuSkillId}}}","revision":1,
           "cardPolicies":[{"id":"double-armor-damage","kind":"doubleArmorDamage"}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{GeshanDaniuSkillId}}}":{"name":"隔山打牛","description":"你对其他角色造成伤害时，无视其数值护甲。对自己的伤害仍正常支付护甲。"},
          "{{{TijiaShuSkillId}}}":{"name":"剔甲术","description":"你造成的伤害对数值护甲造成双倍扣损；不增加伤害点数。护甲不足时沿原生双倍护甲支付规则结算。"}
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
