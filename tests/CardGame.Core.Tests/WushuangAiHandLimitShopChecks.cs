using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    private const string HandAddThree = "fixture:shop-hand-add-three", HandAddTwo = "fixture:shop-hand-add-two",
        HandHighSet = "fixture:shop-hand-high-set", HandNegative = "fixture:shop-hand-negative", HandNeutral = "fixture:shop-hand-neutral";

    public static void HandLimitSetUsesActualMarginalCapacityAndReplacementCost()
    {
        var definitions = ContentRegistry.Build(new HandLimitShopPackage());
        var stable = Tactic(2166, 1); var freeHand = Tactic(2211, 1);
        WushuangShopDecision Choose(GameSnapshot view, WushuangSnapshot shop) => Brain().ChooseWushuangShop(view, shop, definitions);
        static double Score(WushuangShopDecision result, string offer, string? replacement = null) =>
            result.Candidates.Single(candidate => candidate.OfferId == offer && candidate.ReplaceSkillId == replacement).Score;
        static bool Equal(double left, double right) => Math.Abs(left - right) < 1e-9;
        var hand = Enumerable.Range(1, 4).Select(id => Card(id, CardKind.Dodge)).ToArray();
        var healthy = Choose(View(hand), Shop(1, stable, freeHand));
        Need(healthy.Action == WushuangShopAction.Finish && Equal(Score(healthy, stable.Id), -3) && Equal(Score(healthy, freeHand.Id), -3),
            "Both real native SetMaxHp tactics add no present capacity to a full-health four-HP owner and must not consume a token as if they added four cards.");
        var wounded = View(hand, hp: 1);
        var useful = Choose(wounded, Shop(1, stable));
        Need(useful.Action == WushuangShopAction.Buy && Equal(Score(useful, stable.Id), 3 * 2.5 - 3),
            "The same real tactic has three actual additional hand slots for a wounded owner with overflow demand, preserving useful defensive purchases.");
        var duplicate = Choose(OwnSkills(wounded, "wushuang:tactic-2166"), Shop(1, freeHand) with { TacticIds = [2166] });
        Need(duplicate.Action == WushuangShopAction.Finish && Equal(Score(duplicate, freeHand.Id), -3),
            "The other catalogue identity with the same native Set cannot create another MaxHp of capacity or spend a second token.");
        var stacked = Choose(OwnSkills(wounded, HandAddTwo), Shop(1, stable));
        var dominated = Choose(OwnSkills(wounded, HandHighSet), Shop(1, stable));
        var clamped = Choose(OwnSkills(wounded, HandNegative), Shop(1, stable));
        Need(Equal(Score(stacked, stable.Id), Score(useful, stable.Id)) &&
            Equal(Score(dominated, stable.Id), -3) && Equal(Score(clamped, stable.Id), -3),
            "Existing Add contributions survive Set, higher-priority Set dominates the MaxHp base, and the final native zero bound removes fictitious gains under a larger negative modifier.");
        var add = BuildSkillOffer("hand-add-three", HandAddThree, 1);
        var full = Choose(OwnSkills(wounded, "wushuang:tactic-2166", HandNeutral), Shop(1, add) with
            { ExtraSkillIds = ["wushuang:tactic-2166", HandNeutral] });
        Need(full.Action == WushuangShopAction.Buy && full.ReplaceSkillId == HandNeutral &&
            Equal(Score(full, add.Id, "wushuang:tactic-2166"), -3) && Equal(Score(full, add.Id, HandNeutral), 3 * 2.5 - 3),
            "Full-slot replacement charges the lost hand-limit capability on the same remaining build: equivalent Add3-for-SetMaxHp wastes the price, while replacing an empty skill retains the useful base.");
    }

    private sealed class HandLimitShopPackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wushuang-hand-limit-value", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rows = new[] { (HandAddThree, "add", 3, 0), (HandAddTwo, "add", 2, 0),
                (HandHighSet, "set", 8, 1), (HandNegative, "add", -6, 0) };
            var catalogue = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = rows.Select(row => new { id = row.Item1, revision = 1,
                    modifiers = new[] { new { id = "hand", query = "handLimit", operation = row.Item2, value = row.Item3, priority = row.Item4 } } }) }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = rows.ToDictionary(row => row.Item1, _ => new { name = "手牌容量", description = "原生数值归约的边际容量。" }) }));
            foreach (var (id, program) in catalogue.Programs)
                builder.AddSkill(new(id, "手牌容量", "原生数值归约的边际容量。") { Program = program, ProgramPresentation = catalogue.Presentations[id] });
            builder.AddSkill(new(HandNeutral, "空能力", "没有手牌容量收益。"));
            foreach (var id in new[] { 2166, 2211 }) builder.AddSkill(Registry.Value.Skills[$"wushuang:tactic-{id}"]);
        }
    }
}
