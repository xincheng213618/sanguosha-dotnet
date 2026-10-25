using System.Text.Json.Nodes;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 player-stock Longhun inherits OL conversion, with compulsory two-black discard.</summary>
public sealed class StandardShanheLonghunPackage : IGameContentPackage
{
    public const string SkillId = "shanhe:longhun-3023";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-longhun", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(CreatePrograms);

    private static SkillProgramCatalog CreatePrograms()
    {
        using var stream = typeof(StandardShanheLonghunPackage).Assembly.GetManifestResourceStream(
            "CardGame.Content.Standard.SkillPrograms.ol-shen-zhao-yun.rules.json") ??
            throw new InvalidOperationException("Missing OL Longhun conversion source.");
        using var reader = new StreamReader(stream);
        var original = JsonNode.Parse(reader.ReadToEnd())!["skills"]!.AsArray()
            .Single(skill => skill!["id"]!.GetValue<string>() == "ol:longhun")!;
        var skill = original.DeepClone();
        skill["id"] = SkillId;
        foreach (var trigger in skill["triggers"]!.AsArray().Where(trigger =>
                     trigger!["id"]!.GetValue<string>() is "two-club-discard" or "two-spade-discard"))
        {
            trigger!["sourceSkillId"] = SkillId;
            trigger["optional"] = false;
        }
        var rules = new JsonObject
        {
            ["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion,
            ["skills"] = new JsonArray(skill)
        };
        return SkillProgramCatalog.Load(rules.ToJsonString(), $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.PresentationSchemaVersion}}},"skills":{
              "{{{SkillId}}}":{"name":"龙魂","description":"你可以将至多两张花色相同的牌按以下规则使用或打出：红桃当【桃】；方片当【火杀】；梅花当【闪】；黑桃当【无懈可击】。若你以此法转化使用了两张红色牌，此牌回复值或伤害值+1；若为两张黑色牌，你弃置当前回合角色一张牌。"}
            }}
            """);
    }

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[SkillId];
        builder.AddSkill(new(SkillId, presentation.Name, presentation.Description)
        {
            Program = catalog.Programs[SkillId], ProgramPresentation = presentation
        });
    }
}
