using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

public sealed class StandardShanheTrickResponsePackage : IGameContentPackage
{
    public const string YinyangSkillId = "shanhe:current-yinyang-2190";
    public const string SuanwuyiceSkillId = "shanhe:current-suanwuyice-2248";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-current-trick-response", CurrentVersion,
        [new("standard", StandardContentPackage.CurrentVersion)]);

    private static readonly Lazy<SkillProgramCatalog> Programs = new(() =>
    {
        CardKind[] damageKinds = [CardKind.Duel, CardKind.FireAttack, CardKind.BarbarianAssault,
            CardKind.ArrowBarrage, CardKind.WumiaoFlood, CardKind.QizhengXiangsheng, CardKind.UnexpectedAssault];
        var ordinaryKinds = Enum.GetValues<CardKind>().Where(kind => CardCatalog.Get(kind).CategoryName == "锦囊牌" &&
            kind is not (CardKind.Nullification or CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning)).ToArray();
        string[] Names(IEnumerable<CardKind> kinds) => kinds.Select(kind => JsonNamingPolicy.CamelCase.ConvertName(kind.ToString())).ToArray();
        return SkillProgramCatalog.Load(JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[]
            {
                new { id = YinyangSkillId, revision = 1, cardPolicies = new[]
                    { new { id = "enemies-cannot-respond-to-damage-tricks", kind = "prohibitEnemyDamageTrickResponse", cardKinds = Names(damageKinds) } } },
                new { id = SuanwuyiceSkillId, revision = 1, cardPolicies = new[]
                    { new { id = "targets-cannot-respond-to-ordinary-tricks", kind = "prohibitTargetOrdinaryTrickResponse", cardKinds = Names(ordinaryKinds) } } }
            }
        }), JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object>
            {
                [YinyangSkillId] = new { name = "阴阳术法", description = "敌方无法响应你的伤害锦囊牌。" },
                [SuanwuyiceSkillId] = new { name = "算无遗策", description = "目标无法响应你的普通锦囊牌。" }
            }
        }));
    });

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
