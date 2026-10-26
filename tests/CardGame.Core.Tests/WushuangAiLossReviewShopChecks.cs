using System.Reflection;
using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    public static void LossReviewJudgmentTracksBeneficiarySingleOutcomeAndPayment()
    {
        const int current = AiDecisionPolicy.WushuangLossReviewVersion, historical = AiDecisionPolicy.WushuangStrategyVersion;
        var hand = Enumerable.Range(1, 4).Select(id => Card(id, CardKind.Slash)).ToArray();
        var view = View(hand, hp: 2);
        var beige = Registry.Value.Skills["wushuang:skill-3140"].Program!;
        Need(LossReviewEqual(LossReviewValue(beige, view, historical), 70) &&
            LossReviewEqual(LossReviewValue(beige, view, current), 5.25),
            "Beige keeps its historical score; review prices one paid judgment, not four full-frequency rewards: incoming expected ten minus discard three, outgoing is declined, then the damage opportunity is applied once.");
        var offer = BuildSkillOffer("paid-judgment", beige.Id, 2);
        WushuangShopDecision Choose(int version, GameSnapshot ownerView, WushuangSnapshot shop) =>
            new SimpleAiBrain(0, 71, version).ChooseWushuangShop(ownerView, shop, Registry.Value);
        static double Score(WushuangShopDecision decision, string id) => decision.Candidates.Single(c => c.OfferId == id).Score;
        var old = Choose(historical, view, Shop(2, offer));
        var reviewed = Choose(current, view, Shop(2, offer));
        Need(LossReviewEqual(Score(old, offer.Id), 64) && LossReviewEqual(Score(reviewed, offer.Id), -.75) &&
            reviewed.Action != WushuangShopAction.Buy,
            "The actual two-token page retains the old 64-point purchase only for policy 11; the complete 5.25-point benefit cannot pay six points of currency value under review.");

        var replacement = BuildSkillOffer("replace-with-draw-two", "wushuang:tactic-2060", 1);
        var fullView = OwnSkills(view, beige.Id, "classic:yingzi");
        var fullShop = Shop(2, replacement) with { ExtraSkillIds = [beige.Id, "classic:yingzi"] };
        Need(Choose(historical, fullView, fullShop).ReplaceSkillId == "classic:yingzi" &&
            Choose(current, fullView, fullShop).ReplaceSkillId == beige.Id,
            "Replacement charges the retained skill using the same policy as acquisition: the reviewed build keeps reliable Yingzi and replaces the lower marginal paid judgment.");
        var originalView = OwnSkills(View(hand), beige.Id);
        var replaceOriginal = Tactic(2221, 1);
        Need(LossReviewEqual(Score(Choose(historical, originalView, Shop(1, replaceOriginal)), replaceOriginal.Id), -.5) &&
            LossReviewEqual(Score(Choose(current, originalView, Shop(1, replaceOriginal)), replaceOriginal.Id), 7.5),
            "The original-skill replacement tactic also uses the recorded policy: its weak-original threshold sees reviewed Beige rather than mixing an old retention value into the new purchase score.");

        var enemyDraw = LossReviewProgram("fixture:loss-review-enemy-draw", "afterDamageApplied", "damageSource", false,
            [new { op = "draw", target = "owner", targetRef = new { kind = "eventTarget" }, amount = 2 }]);
        Need(LossReviewEqual(LossReviewValue(enemyDraw, view, historical), 21) &&
            LossReviewEqual(LossReviewValue(enemyDraw, view, current), -21),
            "A generic renamed program proves targetRef overrides the printed Owner fallback: drawing two for the enemy is a cost, not an owner's fourteen-point income.");
        var enemyHeal = LossReviewProgram("fixture:loss-review-enemy-heal", "afterDamageApplied", "damageSource", false,
            [new { op = "recover", target = "owner", targetRef = new { kind = "eventTarget" }, amount = 1 }]);
        var woundedEnemy = view with { Players = view.Players.Select(p => p.Seat == 1 ? p with { Hp = 2 } : p).ToArray() };
        var injury = Registry.Value.Skills["classic:yiji"].Program!;
        Need(LossReviewEqual(LossReviewValue(enemyHeal, woundedEnemy, current), -21) &&
            LossReviewEqual(LossReviewValue(enemyHeal, woundedEnemy, current, [injury]), -21),
            "An owned injury-draw engine cannot turn recovery paid to the opponent into self-recovery synergy through the separate build-fit fallback.");
    }

    public static void LossReviewChoicesUseControllerEligibilityAndNetDeferredDebt()
    {
        const int current = AiDecisionPolicy.WushuangLossReviewVersion, historical = AiDecisionPolicy.WushuangStrategyVersion;
        var hand = Enumerable.Range(1, 4).Select(id => Card(id, CardKind.Slash)).ToArray();
        var view = View(hand, hp: 2);
        var zhuiji = Registry.Value.Skills["ol:zhuiji-tj"].Program!;
        Need(LossReviewEqual(LossReviewValue(zhuiji, view, historical), 35) &&
            LossReviewEqual(LossReviewValue(zhuiji, view, current), 20),
            "At two HP, Zhuiji chooses recover-one minus ending discard-two (eight), rather than adding half of two incompatible positive branches and forgetting both debts.");
        Need(LossReviewEqual(LossReviewValue(zhuiji, View(hand), historical), 17.5) &&
            LossReviewEqual(LossReviewValue(zhuiji, View(hand), current), 15) &&
            LossReviewEqual(LossReviewValue(zhuiji, View(hand, hp: 1), current), 65),
            "At full HP the best actual branch is draw-two minus lose-one HP; at one HP the valuable healing branch remains available after paying its two-card debt.");

        static object ChoiceCondition(string option) => new { kind = "choiceIs", sourceBind = "route", optionId = option };
        object[] effects =
        [
            new { op = "chooseOption", target = "owner", resultBind = "route", options = new[] { new { id = "heal" }, new { id = "income" } } },
            new { op = "recover", target = "owner", amount = 1, condition = ChoiceCondition("heal") },
            new { op = "selectOwnedCards", target = "owner", amount = 2, zones = new[] { "hand", "equipment" }, resultBind = "paid", condition = ChoiceCondition("heal") },
            new { op = "moveBoundCards", target = "owner", sourceBind = "paid", destination = "discardPile", condition = ChoiceCondition("heal") },
            new { op = "draw", target = "owner", amount = 2, condition = ChoiceCondition("income") },
            new { op = "loseHp", target = "owner", amount = 1, condition = ChoiceCondition("income") }
        ];
        var ordinary = LossReviewProgram("fixture:loss-review-net-choice", "playPhaseStarting", "owner", true, effects);
        Need(LossReviewEqual(LossReviewValue(ordinary, view, current), 20),
            "The same resource exchange expressed as ordinary program nodes gets the same best net value; branch selection and bound-card payment do not depend on Zhuiji's skill ID.");

        var hostileChoice = LossReviewProgram("fixture:loss-review-hostile-choice", "afterDamageApplied", "damageSource", false,
        [
            new { op = "chooseOption", target = "owner", chooserRef = new { kind = "eventTarget" }, resultBind = "route", options = new[] { new { id = "heal" }, new { id = "income" } } },
            new { op = "draw", target = "owner", amount = 2, condition = ChoiceCondition("income") },
            new { op = "loseHp", target = "owner", amount = 1, condition = ChoiceCondition("heal") }
        ]);
        Need(LossReviewEqual(LossReviewValue(hostileChoice, view, current), -21),
            "An opponent who controls the option takes the owner's harmful HP-loss branch; a hostile choice is not valued as if the owner can select Draw2.");
        var woundedOnly = LossReviewProgram("fixture:loss-review-eligible-target", "playPhaseStarting", "owner", false,
        [
            new { op = "selectTarget", target = "owner", targetKind = "anyWounded" },
            new { op = "recover", target = "selectedTarget", amount = 1 }
        ]);
        var onlyEnemyWounded = View(hand) with { Players = View(hand).Players.Select(p => p.Seat == 1 ? p with { Hp = 2 } : p).ToArray() };
        Need(LossReviewEqual(LossReviewValue(woundedOnly, onlyEnemyWounded, current), -35),
            "A full-HP owner is not an eligible AnyWounded beneficiary; if only the opponent is wounded the mandatory recovery is priced as the opponent's recovery.");
    }

    private static bool LossReviewEqual(double left, double right) => Math.Abs(left - right) < 1e-9;

    private static double LossReviewValue(SkillProgram program, GameSnapshot view, int policyVersion, SkillProgram[]? owned = null)
    {
        var method = typeof(SimpleAiBrain).GetMethod("ShopSkillReviewValue", BindingFlags.Static | BindingFlags.NonPublic)!;
        return (double)method.Invoke(null, [program, view.Players[0], view.Players[1], owned ?? [], 2.5d, true, policyVersion])!;
    }

    private static SkillProgram LossReviewProgram(string id, string window, string subject, bool optional, object[] effects)
    {
        var trigger = new Dictionary<string, object> { ["id"] = "value", ["window"] = window,
            ["subject"] = subject, ["optional"] = optional, ["effects"] = effects };
        if (window is "afterDamageApplied" or "damageAppliedBeforeDying") trigger["damageOccurrence"] = "perDamage";
        var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id, revision = 1, triggers = new[] { trigger } } } });
        var optionLabels = JsonSerializer.SerializeToElement(effects).EnumerateArray().Where(e => e.TryGetProperty("options", out _))
            .SelectMany(e => e.GetProperty("options").EnumerateArray()).Select(option => option.GetProperty("id").GetString()!)
            .Distinct(StringComparer.Ordinal).ToDictionary(option => option, option => option, StringComparer.Ordinal);
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "净收益", description = "独立验证公开受益人、支付和互斥分支。", optionLabels } } });
        return SkillProgramCatalog.Load(rules, presentation).Programs[id];
    }
}
