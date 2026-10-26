using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    private const string DrawSetFive = "fixture:shop-draw-set-five", DrawSameSetFive = "fixture:shop-draw-same-set-five",
        DrawAddThree = "fixture:shop-draw-add-three", DrawAddTwo = "fixture:shop-draw-add-two",
        DrawHigherSet = "fixture:shop-draw-higher-set", DrawNegative = "fixture:shop-draw-negative", DrawNeutral = "fixture:shop-draw-neutral";

    public static void DrawCountSetAndAddUseNativeMarginalValueAndReplacementCost()
    {
        var definitions = ContentRegistry.Build(new DrawCountShopPackage());
        var set = BuildSkillOffer("set-five", DrawSetFive, 1);
        var add = BuildSkillOffer("add-three", DrawAddThree, 1);
        var native = Tactic(2208, 1);
        const double future = 2.5, unit = future * 7;
        static bool Equal(double left, double right) => Math.Abs(left - right) < 1e-9;
        WushuangShopDecision Choose(GameSnapshot view, WushuangSnapshot shop) => Brain().ChooseWushuangShop(view, shop, definitions);
        static double Score(WushuangShopDecision decision, string offer, string? replaced = null) =>
            decision.Candidates.Single(candidate => candidate.OfferId == offer && candidate.ReplaceSkillId == replaced).Score;

        var empty = Choose(View([]), Shop(2, set, add, native));
        Need(Equal(Score(empty, set.Id), 3 * unit - 3) && Equal(Score(empty, add.Id), Score(empty, set.Id)) &&
            Equal(Score(empty, native.Id), Score(empty, set.Id)),
            "The real native Set5 tactic and an equivalent typed Set5 skill increase base draw two by three, exactly matching Add3 after the same price; they are not five-card additions.");
        var ownedAdd = Choose(OwnSkills(View([]), DrawAddTwo), Shop(2, set, add));
        Need(Equal(Score(ownedAdd, set.Id), 3 * unit / 1.6 - 3) && Equal(Score(ownedAdd, set.Id), Score(ownedAdd, add.Id)),
            "Set replaces the base before existing Add2 is included: native counts four then seven, matching another Add3 under the same existing-income discount.");
        var sameSet = BuildSkillOffer("same-set-five", DrawSameSetFive, 1);
        var ownedSet = Choose(OwnSkills(View([]), DrawSetFive), Shop(2, sameSet, add));
        Need(Equal(Score(ownedSet, sameSet.Id), -3) && Equal(Score(ownedSet, add.Id), 3 * unit / 1.9 - 3),
            "A second identical Set supplies no incremental draw, while Add3 still stacks on the existing five-card base and uses that background for diminishing returns.");
        var higher = Choose(OwnSkills(View([]), DrawHigherSet), Shop(2, set));
        var clamped = Choose(OwnSkills(View([]), DrawNegative), Shop(2, set, add));
        Need(Equal(Score(higher, set.Id), -3) && Equal(Score(clamped, set.Id), unit - 3) && Equal(Score(clamped, add.Id), unit - 3),
            "A higher-priority Set dominates a lower Set; the final native zero lower bound is applied after Set and Add, so an existing negative draw modifier limits either new offer to one actual extra card.");

        var fullView = OwnSkills(View([]), DrawAddThree, DrawNeutral);
        var full = Choose(fullView, Shop(2, set) with { ExtraSkillIds = [DrawAddThree, DrawNeutral] });
        Need(full.Action == WushuangShopAction.Buy && full.ReplaceSkillId == DrawNeutral &&
            Equal(Score(full, set.Id, DrawAddThree), -3) && Equal(Score(full, set.Id, DrawNeutral), 3 * unit / 1.9 - 3),
            "A full skill build prices both offers against the same remaining skills: exchanging equivalent Add3 for Set5 loses the price, while replacing a genuinely empty capability retains the existing draw engine and gains three cards.");
        var reverse = Choose(OwnSkills(View([]), DrawSetFive, DrawAddThree), Shop(1, add with { SkillId = DrawAddTwo }) with
            { ExtraSkillIds = [DrawSetFive, DrawAddThree] });
        Need(reverse.Action == WushuangShopAction.Finish && reverse.Candidates.Where(candidate => candidate.Action == WushuangShopAction.Buy).All(candidate => candidate.Score < 0),
            "The reverse full-slot replacement also charges the lost Set or Add rather than rewarding raw modifier values; an actual reduction from the current draw count is declined.");
        // Load mutually incompatible test definitions separately: the global
        // registry must continue rejecting a single invalid content catalogue.
        static SkillProgram SetThree(string id, object? condition = null)
        {
            var modifier = new Dictionary<string, object> { ["id"] = "draw", ["query"] = "drawCount", ["operation"] = "set", ["value"] = 3, ["priority"] = 0 };
            if (condition is not null) modifier["condition"] = condition;
            var catalogue = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id, revision = 1, modifiers = new[] { modifier } } } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [id] = new { name = "独立Set背景", description = "用于纯评分假想组合。" } } }));
            return catalogue.Programs[id];
        }
        var skillValue = typeof(SimpleAiBrain).GetMethod("ShopSkillValue", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!;
        double Value(SkillProgram program, SkillProgram[] background, PlayerSnapshot? self = null) => (double)skillValue.Invoke(null,
            [program, self ?? View([]).Players[0], View([]).Players[1], background, future, true])!;
        var setProgram = definitions.Skills[DrawSetFive].Program!;
        var incompatible = SetThree("fixture:shop-conflicting-set-three");
        var conditionalSet = SetThree("fixture:shop-conditional-set-three", new { kind = "hpAtLeast", value = 5 });
        Need(!double.IsFinite(Value(setProgram, [incompatible])) &&
            Value(setProgram, []) - Value(incompatible, []) - 3 > 0,
            "An incompatible highest-priority Set combination is an unavailable estimate rather than a thrown reducer exception; excluding the replaced old Set still permits the genuine positive upgrade.");
        Need(Equal(Value(setProgram, [conditionalSet]), Value(setProgram, [])) &&
            !double.IsFinite(Value(setProgram, [conditionalSet], View([]).Players[0] with { Hp = 5, MaxHp = 6 })) &&
            empty.Candidates.Concat(full.Candidates).All(candidate => double.IsFinite(candidate.Score)),
            "A publicly false conditional Set does not manufacture a conflict, a currently true incompatible Set is refused, and only finite estimates enter published shop candidates.");
        foreach (var policy in new[] { 1, 10 })
        {
            var legacy = new SimpleAiBrain(0, 71, policy).ChooseWushuangShop(fullView, Shop(2, set, add), definitions);
            Need(legacy.Action == WushuangShopAction.Finish && legacy.Candidates is [{ Action: WushuangShopAction.Finish, Score: 0 }],
                "Historical policies before the Wushuang shop gate keep the exact previous Finish decision and candidate list.");
        }
    }

    private sealed class DrawCountShopPackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wushuang-draw-value", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var definitions = new[]
            {
                (DrawSetFive, "set", 5, 0), (DrawSameSetFive, "set", 5, 0),
                (DrawAddThree, "add", 3, 0), (DrawAddTwo, "add", 2, 0),
                (DrawHigherSet, "set", 6, 1), (DrawNegative, "add", -4, 0)
            };
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = definitions.Select(item => new { id = item.Item1, revision = 1,
                    modifiers = new[] { new { id = "draw", query = "drawCount", operation = item.Item2, value = item.Item3, priority = item.Item4 } } }) });
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = definitions.ToDictionary(item => item.Item1, _ => new { name = "摸牌规则", description = "原生数值归约的边际购买价值。" }) });
            var catalogue = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalogue.Programs)
                builder.AddSkill(new(id, "摸牌规则", "原生数值归约的边际购买价值。")
                    { Program = program, ProgramPresentation = catalogue.Presentations[id] });
            builder.AddSkill(new(DrawNeutral, "空能力", "此夹具能力没有可兑现收益。"));
            builder.AddSkill(Registry.Value.Skills["wushuang:tactic-2208"]);
        }
    }
}
