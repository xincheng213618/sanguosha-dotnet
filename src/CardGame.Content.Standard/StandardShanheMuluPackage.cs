using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source group 2397: Mulu and four fixed elephant followers.</summary>
public sealed class StandardShanheMuluPackage : IGameContentPackage
{
    public const string MuluId = "shanhe:mulu-2026";
    public const string RageElephantId = "shanhe:mulu-rage-elephant-2026";
    public const string AngryElephantId = "shanhe:mulu-angry-elephant-2026";
    public const string SummonId = "shanhe:mulu-summon";
    public const string YangshengId = "shanhe:elephant-yangsheng";
    public static IReadOnlyList<string> GeneralIds { get; } = Array.AsReadOnly(new[] { MuluId, RageElephantId, AngryElephantId });
    public PackageManifest Manifest { get; } = new("shanhe-mulu", new(1, 0, 0));

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() => SkillProgramCatalog.Load(
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
          {"id":"{{{SummonId}}}","revision":1,"triggers":[
            {"id":"revive-followers","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"reviveShanheMuluFollowers","target":"owner"}]}]},
          {"id":"{{{YangshengId}}}","revision":1,"triggers":[
            {"id":"freeze-round-damage","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
             "effects":[{"op":"freezeShanheElephantRoundDamage","target":"owner"}]}]}
        ]}
        """,
        $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
          "{{{SummonId}}}":{"name":"召唤","description":"战斗开始时，召唤所有随从；你的回合开始时，复活所有已死亡的友方随从。"},
          "{{{YangshengId}}}":{"name":"扬升","description":"锁定技，你的回合开始时，令你所有伤害增加X点。（X为你本轮受到伤害次数，最大为3）"}
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
        Add(MuluId, "木鹿大王", 4, SummonId);
        Add(RageElephantId, "暴怒巨象", 3, YangshengId);
        Add(AngryElephantId, "愤怒巨象", 3, YangshengId);
        return;
        void Add(string id, string name, int hp, string skill) => builder.AddGeneral(new(id, name, "", skill, "qun", hp)
        { CharacterId = id == MuluId ? "character:shanhe-mulu" : "character:shanhe-elephant",
            VariantId = id == MuluId ? "shanhe-2026" : id == RageElephantId ? "shanhe-2026-rage" : "shanhe-2026-angry",
            RulesetId = "shanhe-2026" });
    }
}
