using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The six all-participant terrain rules displayed by the 2026 route cards.</summary>
public sealed class StandardShanheTerrainPackage : IGameContentPackage
{
    public const string CityId = "shanhe:terrain-city";
    public const string BattlefieldId = "shanhe:terrain-battlefield";
    public const string RiverId = "shanhe:terrain-river";
    public const string ForestId = "shanhe:terrain-forest";
    public const string PlainId = "shanhe:terrain-plain";
    public const string MountainId = "shanhe:terrain-mountain";
    public static IReadOnlyList<string> SkillIds { get; } = Array.AsReadOnly(new[]
        { CityId, BattlefieldId, RiverId, ForestId, PlainId, MountainId });
    public PackageManifest Manifest { get; } = new("shanhe-terrain", new(1, 0, 0));

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{CityId}}}","revision":1,"modifiers":[
            {"id":"outgoing-distance","query":"outgoingDistance","operation":"add","value":-1,"priority":0}]},
          {"id":"{{{BattlefieldId}}}","revision":1,"modifiers":[
            {"id":"normal-draw","query":"drawCount","operation":"add","value":2,"priority":0}]},
          {"id":"{{{RiverId}}}","revision":1,"cardPolicies":[
            {"id":"hand-limit-floor","kind":"minimumHandLimit","value":8}]},
          {"id":"{{{ForestId}}}","revision":1,"cardPolicies":[
            {"id":"own-play-use-cap","kind":"maximumOwnPlayCardUses","value":10}]},
          {"id":"{{{PlainId}}}","revision":1,"modifiers":[
            {"id":"normal-slash","query":"slashLimit","operation":"add","value":1,"priority":0}]},
          {"id":"{{{MountainId}}}","revision":1,"triggers":[
            {"id":"ending-hp-loss","window":"turnEnding","subject":"owner","optional":false,
             "effects":[{"op":"loseHp","target":"owner","amount":1}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{CityId}}}":{"name":"城池","description":"场地技能，你到其他角色的距离-1。"},
          "{{{BattlefieldId}}}":{"name":"战场","description":"场地技能，摸牌阶段摸牌+2。"},
          "{{{RiverId}}}":{"name":"河流","description":"场地技能，你的手牌上限至少为8。"},
          "{{{ForestId}}}":{"name":"密林","description":"场地技能，你的每个出牌阶段最多使用10张牌。"},
          "{{{PlainId}}}":{"name":"平原","description":"场地技能，出牌阶段使用【杀】的次数上限+1。"},
          "{{{MountainId}}}":{"name":"高山","description":"场地技能，你的回合结束时，失去1点体力。"}
        }}
        """));

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        foreach (var (id, program) in catalog.Programs)
        {
            var presentation = catalog.Presentations[id];
            builder.AddSkill(new(id, presentation.Name, presentation.Description)
            { Program = program, ProgramPresentation = presentation, Tags = SkillTag.Locked });
        }
    }
}
