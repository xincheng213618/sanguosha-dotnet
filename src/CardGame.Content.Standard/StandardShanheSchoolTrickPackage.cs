using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed class StandardShanheSchoolTrickPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-school-trick", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheSchoolEffects.TrickRepeatSkillId}}}","revision":1,"triggers":[
            {"id":"native-damage-trick-extra-effect","window":"cardUseCommitted","ownerRelation":"actor","optional":false,
             "effects":[{"op":"repeatCurrentDamageTrick","target":"owner"}]}
          ]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShanheSchoolEffects.TrickRepeatSkillId}}}":{"name":"锦囊流套装","description":"你的伤害类锦囊牌结算两次。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var id = ShanheSchoolEffects.TrickRepeatSkillId;
        var program = Programs.Value.Programs[id]; var presentation = Programs.Value.Presentations[id];
        builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked });
    }
}
