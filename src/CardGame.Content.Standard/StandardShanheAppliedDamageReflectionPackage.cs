using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source-verified outside-turn applied-damage reflection tactic.</summary>
public sealed class StandardShanheAppliedDamageReflectionPackage : IGameContentPackage
{
    public const string PofuChenzhouSkillId = "shanhe:current-pofu-chenzhou-2131";

    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-applied-damage-reflection", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{PofuChenzhouSkillId}}}","revision":1,"triggers":[
            {"id":"reflect-large-outside-turn-segment","window":"afterDamageApplied","subject":"owner",
             "damageOccurrence":"perDamage","optional":false,
             "condition":{"kind":"all","children":[
               {"kind":"not","children":[{"kind":"ownerIsTurnPlayer"}]},
               {"kind":"compare","left":{"kind":"appliedDamageAmount"},"operator":"greaterThanOrEqual",
                "right":{"kind":"integerConstant","value":3}}
             ]},
             "effects":[{"op":"reflectAppliedDamage","target":"owner"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{PofuChenzhouSkillId}}}":{"name":"破釜沉舟","description":"回合外受到伤害一次大于等于3点时，对伤害来源造成等量同属性伤害。"}
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
