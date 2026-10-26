using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public sealed class StandardShanheSchoolTeamPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-school-team", CurrentVersion, [new("standard", StandardContentPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{ShanheSchoolEffects.TeamRankSkillId}}}","revision":1,"triggers":[
            {"id":"opening-team-rank","window":"gameStarting","subject":"owner","optional":false,
             "effects":[{"op":"applyShanheSchoolTeamRank","target":"owner"}]}]},
          {"id":"{{{ShanheTeamRankPrograms.SharedRebirthSkillId}}}","revision":1,"triggers":[
            {"id":"shared-campaign-rebirth","window":"selfDyingResponse","subject":"owner","optional":true,
             "usageScope":"game","usageLimit":1,"effects":[
               {"op":"recoverTo","target":"owner","numberExpression":"integerConstant","minimumValue":5,"clampToMaxHp":true},
               {"op":"draw","target":"owner","amount":5}]}]}
        ]}
        """, $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{ShanheSchoolEffects.TeamRankSkillId}}}":{"name":"队友流套装","description":"队友共享你的等阶效果，最低按3阶。"},
          "{{{ShanheTeamRankPrograms.SharedRebirthSkillId}}}":{"name":"重生","description":"濒死时可回复至5点体力并摸5张牌。每名角色各自每征程限一次。"}
        }}
        """));
    public void Register(IContentRegistryBuilder builder)
    {
        foreach (var (id, program) in Programs.Value.Programs)
        {
            var p = Programs.Value.Presentations[id];
            builder.AddSkill(new(id, p.Name, p.Description) { Program = program, ProgramPresentation = p, Tags = SkillTag.Locked });
        }
    }
}
