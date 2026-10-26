using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheLateRoundTacticsPackage : IGameContentPackage
{
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-late-round-tactics", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load($$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuzhan2SkillId}}}","revision":1,"damageModifiers":[
            {"id":"second-round-slash-damage","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":1,
             "condition":"ownerUsedFromRound","minimumRound":2,"sourceScope":"ownerUsed","damageLayer":"cardDamage"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuzhan3SkillId}}}","revision":1,"damageModifiers":[
            {"id":"third-round-slash-damage","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":1,
             "condition":"ownerUsedFromRound","minimumRound":3,"sourceScope":"ownerUsed","damageLayer":"cardDamage"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBattleLearning3SkillId}}}","revision":1,"modifiers":[
            {"id":"third-round-extra-slashes","query":"slashLimit","operation":"add","value":2,"minimumRound":3,"priority":0}]}
        ]}
        """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [StandardShanheCampaignCatalog.CurrentYuzhan2SkillId] = new { name = "愈战愈勇Ⅱ", description = "从第2轮开始，你的杀造成的伤害+1。" },
                [StandardShanheCampaignCatalog.CurrentYuzhan3SkillId] = new { name = "愈战愈勇Ⅲ", description = "从第3轮开始，你的杀造成的伤害+1。" },
                [StandardShanheCampaignCatalog.CurrentBattleLearning3SkillId] = new { name = "战斗学习Ⅲ", description = "从第3轮开始，你的出杀次数+2。" }
            } })));

    public void Register(IContentRegistryBuilder builder)
    {
        foreach (var (id, program) in Programs.Value.Programs)
        {
            var presentation = Programs.Value.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked,
                CampaignTacticAcquisition = new(id, Repeatable: false) });
        }
    }
}
