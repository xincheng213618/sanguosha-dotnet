using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheWoxinPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:current-woxin-changdan-2127";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-woxin-changdan", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);
    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SkillId}}}","revision":1,"cardPolicies":[
            {"id":"off-turn-hit-next-damage","kind":"reserveNextDamageAfterOffTurnHit"}]}]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SkillId}}}":{"name":"卧薪尝胆","description":"回合外每受到1次伤害，你的下次造成伤害+1。按伤害事件累计，在下次实际伤害消费；被防止的伤害保留收益。"}
        }}
        """));
    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value; var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation, Tags = SkillTag.Locked,
            CampaignTacticAcquisition = new(SkillId, Repeatable: false)
        });
    }
}
