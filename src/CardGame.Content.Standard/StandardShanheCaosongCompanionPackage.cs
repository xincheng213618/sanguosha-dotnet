using System.Text.Json;
using System.Text.Json.Serialization;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>The supplied Cao Song companion keeps his native support skills; its elite adds Huanghan.</summary>
public sealed class StandardShanheCaosongCompanionPackage : IGameContentPackage
{
    public const string EliteId = "shanhe:companion-cao-song-elite";
    public const string HuanghanSkillId = "shanhe:companion-huanghan";
    public const string HuanghanDescription = "当你受到牌的伤害后，你可以摸X张牌（X为此牌牌名字数）并弃置你已损失体力值张牌；若不为本回合首次发动，你视为未发动宗族技。";
    public PackageManifest Manifest { get; } = new("shanhe-caosong-companion", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(CreatePrograms);

    private static SkillProgramCatalog CreatePrograms()
    {
        // The native damage window owns the effective card kind, including virtual cards.
        // Mutually exclusive kind groups reuse fixed Draw instructions rather than copying
        // a card-name table into a new runtime handler. Source-less skill damage has no kind.
        var triggers = CardCatalog.ImplementedCards.GroupBy(card => card.DisplayName.Length).Select(group => new
        {
            id = "card-name-length-" + group.Key,
            window = "afterDamageApplied", subject = "owner", damageOccurrence = "perDamage", optional = true,
            damageCardKinds = group.Select(card => card.Kind).ToArray(),
            effects = new object[]
            {
                new { op = "draw", target = "owner", amount = group.Key },
                new { op = "selectOwnedCards", target = "owner", zones = new[] { "hand", "equipment" },
                    numberExpression = "ownerLostHp", resultBind = "huanghan-discard" },
                new { op = "moveBoundCards", target = "owner", sourceBind = "huanghan-discard",
                    destination = "discardPile", awaitMovementTriggers = true }
            }
        }).ToArray();
        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };
        return SkillProgramCatalog.Load(JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id = HuanghanSkillId, revision = 1, triggers } }
        }, options), JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [HuanghanSkillId] = new { name = "惶汗", description = HuanghanDescription } }
        }));
    }

    public void Register(IContentRegistryBuilder builder)
    {
        var catalog = Programs.Value;
        var presentation = catalog.Presentations[HuanghanSkillId];
        builder.AddSkill(new(HuanghanSkillId, presentation.Name, presentation.Description)
        { Program = catalog.Programs[HuanghanSkillId], ProgramPresentation = presentation });
        // No implemented clan skill is carried by this body. Huanghan's clan reset
        // therefore has no target in the evidenced companion loadout; it does not
        // reset ordinary limited skills, Lilu's gift history, or Yizheng support.
        builder.AddGeneral(new(EliteId, "曹嵩", "ol-cao-song", "ol:lilu", "wei", 4,
            ["ol:yizheng-cao-song", HuanghanSkillId])
        {
            CharacterId = "character:cao-song", VariantId = "shanhe-companion-elite", RulesetId = "shanhe-2026",
            Gender = GeneralGender.Male
        });
    }
}
