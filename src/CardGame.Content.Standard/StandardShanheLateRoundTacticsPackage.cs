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
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuzhanSkillId}}}","revision":1,"damageModifiers":[
            {"id":"second-round-slash-damage","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":2,
             "condition":"ownerUsedFromRound","minimumRound":2,"sourceScope":"ownerUsed","damageLayer":"cardDamage"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuzhan2SkillId}}}","revision":1,"damageModifiers":[
            {"id":"second-round-slash-damage","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":1,
             "condition":"ownerUsedFromRound","minimumRound":2,"sourceScope":"ownerUsed","damageLayer":"cardDamage"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentYuzhan3SkillId}}}","revision":1,"damageModifiers":[
            {"id":"third-round-slash-damage","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"amount":1,
             "condition":"ownerUsedFromRound","minimumRound":3,"sourceScope":"ownerUsed","damageLayer":"cardDamage"}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBuzhenSkillId}}}","revision":1,"modifiers":[
            {"id":"second-round-draw","query":"drawCount","operation":"add","value":4,"minimumRound":2,"priority":0}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBuzhen2SkillId}}}","revision":1,"modifiers":[
            {"id":"second-round-draw","query":"drawCount","operation":"add","value":3,"minimumRound":2,"priority":0}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBuzhen3SkillId}}}","revision":1,"modifiers":[
            {"id":"third-round-draw","query":"drawCount","operation":"add","value":2,"minimumRound":3,"priority":0}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBattleLearningSkillId}}}","revision":1,"modifiers":[
            {"id":"second-round-extra-slashes","query":"slashLimit","operation":"add","value":4,"minimumRound":2,"priority":0}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBattleLearning2SkillId}}}","revision":1,"modifiers":[
            {"id":"second-round-extra-slashes","query":"slashLimit","operation":"add","value":2,"minimumRound":2,"priority":0}]},
          {"id":"{{{StandardShanheCampaignCatalog.CurrentBattleLearning3SkillId}}}","revision":1,"modifiers":[
            {"id":"third-round-extra-slashes","query":"slashLimit","operation":"add","value":2,"minimumRound":3,"priority":0}]}
        ]}
        """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [StandardShanheCampaignCatalog.CurrentYuzhanSkillId] = new { name = "愈战愈勇", description = "从第2轮开始，你的杀造成的伤害+2。" },
                [StandardShanheCampaignCatalog.CurrentYuzhan2SkillId] = new { name = "愈战愈勇Ⅱ", description = "从第2轮开始，你的杀造成的伤害+1。" },
                [StandardShanheCampaignCatalog.CurrentYuzhan3SkillId] = new { name = "愈战愈勇Ⅲ", description = "从第3轮开始，你的杀造成的伤害+1。" },
                [StandardShanheCampaignCatalog.CurrentBuzhenSkillId] = new { name = "布阵", description = "从第2轮开始，你的摸牌数+4。" },
                [StandardShanheCampaignCatalog.CurrentBuzhen2SkillId] = new { name = "布阵Ⅱ", description = "从第2轮开始，你的摸牌数+3。" },
                [StandardShanheCampaignCatalog.CurrentBuzhen3SkillId] = new { name = "布阵Ⅲ", description = "从第3轮开始，你的摸牌数+2。" },
                [StandardShanheCampaignCatalog.CurrentBattleLearningSkillId] = new { name = "战斗学习", description = "从第2轮开始，你的出杀次数+4。" },
                [StandardShanheCampaignCatalog.CurrentBattleLearning2SkillId] = new { name = "战斗学习Ⅱ", description = "从第2轮开始，你的出杀次数+2。" },
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
