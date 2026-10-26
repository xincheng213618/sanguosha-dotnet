using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed class StandardShanheSchoolLatePackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-school-late", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheSchoolEffects.LateSlashSkillId}}}","revision":1,"damageModifiers":[
            {"id":"elapsed-round-first-slash","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],
             "amount":1,"amountExpression":"completedRoundCount","condition":"ownerRoundSlashUseOrdinalWithinLimit",
             "sourceScope":"ownerUsed","maximumUseOrdinal":1,"damageLayer":"cardDamage"}
          ]}
        ]}
        """, $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShanheSchoolEffects.LateSlashSkillId}}}":{"name":"后期流套装","description":"每轮首张杀的伤害+已过轮数。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var id = ShanheSchoolEffects.LateSlashSkillId;
        var presentation = Programs.Value.Presentations[id];
        builder.AddSkill(new(id, presentation.Name, presentation.Description)
        { Program = Programs.Value.Programs[id], ProgramPresentation = presentation, Tags = SkillTag.Locked });
    }
}
