using System.Reflection;
using System.Text.Json;
using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    public static void DeferredPhaseShopPricesActualRecipientDebtAndKeepsReliableSlots()
    {
        var hand = Enumerable.Range(1, 4).Select(id => Card(id, CardKind.Slash)).ToArray();
        var program = Registry.Value.Skills["wushuang:skill-3331"].Program!;
        var low = View(hand, hp: 2); var healthy = View(hand);
        Need(Math.Abs(DeferredValue(program, low, 12) - 35) < 1e-9 &&
            Math.Abs(DeferredValue(program, low, 13) - 14) < 1e-9 &&
            Math.Abs(DeferredValue(program, healthy, 13) - 24.5) < 1e-9,
            "Policy12 keeps the historical 35; policy13 deducts the actual target's future Draw2 or marginal HandLimit-2 instead of pricing both immediate branches as free income.");
        var alias = DeferredAliasProgram();
        Need(Math.Abs(DeferredValue(alias, low, 13) - DeferredValue(program, low, 13)) < 1e-9 &&
            Math.Abs(DeferredValue(alias, healthy, 13) - DeferredValue(program, healthy, 13)) < 1e-9,
            "The phase exchange is recognized from native operations and the immediate selected-target effect, independently of the skill identity or display text.");
        var keji = Registry.Value.Skills["classic:keji"].Program!;
        Need(Math.Abs(DeferredValue(program, low, 13, [keji]) - DeferredValue(program, low, 13)) < 1e-9 &&
            DeferredValue(program, low with { Players = low.Players.Select(p => p.Seat == 0 ? p with { HandLimit = 100 } : p).ToArray() }, 13) >
                DeferredValue(program, low, 13),
            "A conditional discard-skip capability does not promise a free penalty exemption; genuinely published spare capacity lowers the actual marginal cost.");
        var offer = BuildSkillOffer("native-phase-exchange", program.Id, 2);
        var late = Shop(2, offer) with { RemainingTurns = 1 };
        var old = new SimpleAiBrain(0, 71, 12).ChooseWushuangShop(healthy, late, Registry.Value);
        var current = new SimpleAiBrain(0, 71, 13).ChooseWushuangShop(healthy, late, Registry.Value);
        Need(old.Action == WushuangShopAction.Buy && current.Action == WushuangShopAction.Finish &&
            old.Candidates.Single(c => c.OfferId == offer.Id).Score > 0 &&
            current.Candidates.Single(c => c.OfferId == offer.Id).Score < 0,
            "At the last shopping horizon the old unpriced branch still spends both tokens, while the reviewed exchange cannot recover its price and preserves the budget.");
        var full = OwnSkills(low, "classic:yingzi", "boundary:yingzi");
        var slots = Shop(2, offer) with { ExtraSkillIds = ["classic:yingzi", "boundary:yingzi"] };
        var before = SnapshotJson.Serialize(full);
        Need(new SimpleAiBrain(0, 71, 12).ChooseWushuangShop(full, slots, Registry.Value).Action == WushuangShopAction.Buy &&
            new SimpleAiBrain(0, 71, 13).ChooseWushuangShop(full, slots, Registry.Value).Action != WushuangShopAction.Buy &&
            before == SnapshotJson.Serialize(full),
            "The same net pricing also applies to replacement: a constrained two-HP build retains reliable draw instead of replacing it with an overvalued phase exchange.");
        var finisher = low with { Players = low.Players.Select(p => p.Seat == 1 ? p with { Hp = 1 } : p).ToArray() };
        Need(DeferredValue(program, finisher, 13) > DeferredValue(program, low, 13),
            "A public one-HP opponent has a real immediate finishing opportunity; a target who cannot survive the loss is not promised a later Draw2.");
    }

    private static double DeferredValue(SkillProgram program, GameSnapshot view, int policy, SkillProgram[]? owned = null) =>
        (double)typeof(SimpleAiBrain).GetMethod("ShopSkillReviewValue", BindingFlags.NonPublic | BindingFlags.Static)!
            .Invoke(null, [program, view.Players[0], view.Players[1], owned ?? [], 2.5d, true, policy])!;

    private static SkillProgram DeferredAliasProgram()
    {
        const string id = "fixture:renamed-native-phase-exchange";
        object Activation(string name, string op, int amount) => new { id = name, minCards = 0, maxCards = 0,
            minTargets = 1, maxTargets = 1, targetKind = "anyLiving", usesPerTurn = (int?)null, usesPerPhase = 1,
            effects = new object[] { new { op = "armWushuangYinglue", target = "owner" },
                new { op, target = "selectedTarget", amount } } };
        var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id, revision = 1, activations = new[] {
                Activation("lose-hp-next-draw", "loseHp", 1), Activation("draw-next-handlimit", "draw", 2) } } } });
        var labels = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "延迟交换", description = "独立身份验证。" } } });
        return SkillProgramCatalog.Load(rules, labels).Programs[id];
    }
}

internal static partial class WushuangModeChecks
{
    public static void DeferredPhaseHintsDriveActualTargetsAndNativePaymentCold()
    {
        foreach (var policy in new[] { AiDecisionPolicy.WushuangLossReviewVersion, AiDecisionPolicy.WushuangRefinementVersion })
        {
            var (game, registry) = Start(false, 227, skills: ["wushuang:skill-3331"], initialHp: 4, aiQuota: 0);
            var initial = game.CreateCheckpoint();
            game = GameReplay.Replay(initial.Options with { AiPolicyVersion = policy }, initial.Commands, registry);
            Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            Buy(game, Shop(game).Offers[0].Id); Shopping(game);
            Accept(game, new FinishWushuangShopCommand(0, game.Revision)); Play(game);
            var view = game.CreateSnapshot(0);
            var loss = game.GetHumanLegalActions().Single(action => action.ProgramSkillId == "wushuang:skill-3331" &&
                action.ProgramActivationId == "lose-hp-next-draw");
            var draw = game.GetHumanLegalActions().Single(action => action.ProgramSkillId == "wushuang:skill-3331" &&
                action.ProgramActivationId == "draw-next-handlimit");
            var brain = new SimpleAiBrain(0, 71, policy);
            Require(brain.ChooseActiveSkillTargets(view, loss).SequenceEqual([policy == 12 ? 1 : 0]) &&
                brain.ChooseActiveSkillTargets(view, draw).SequenceEqual([0]),
                "Actual native action summaries make the new policy choose its own recoverable HP-for-next-draw exchange rather than financing a healthy opponent, while Draw2 goes to the owner.");
            if (policy == AiDecisionPolicy.WushuangLossReviewVersion)
            {
                Require(loss.ProgramAiHint!.TargetNextDrawBonus == 0 && draw.ProgramAiHint!.TargetNextDiscardHandLimitPenalty == 0 &&
                    !JsonSerializer.Serialize(loss.ProgramAiHint).Contains("TargetNextDrawBonus", StringComparison.Ordinal),
                    "Policy12 keeps both its original action and the previous serialized hint shape.");
                continue;
            }
            Require(loss.ProgramAiHint!.TargetNextDrawBonus == 2 && draw.ProgramAiHint!.TargetNextDiscardHandLimitPenalty == 2,
                "Only the owning policy13 Wushuang actor receives the public deferred-phase counts, without private future cards.");
            var low = view with { Players = view.Players.Select(p => p.Seat == 0 ? p with { Hp = 2 } : p).ToArray() };
            var end = new LegalAction(LegalActionKind.EndPlay, null, null, "end");
            Require(new SimpleAiBrain(0, 71, 13).ChoosePlay(low, [loss, end], 1).Action.Kind == LegalActionKind.EndPlay,
                "At two HP the exchange has no positive safe recipient, so a legal active skill does not win merely from its base activation priority.");
            var last = view with { Wushuang = view.Wushuang! with { RemainingTurns = 0 } };
            Require(new SimpleAiBrain(0, 71, 13).ChooseActiveSkillTargets(last, loss).SequenceEqual([1]) &&
                new SimpleAiBrain(0, 71, 13).ChooseActiveSkillTargets(view with { Wushuang = null }, loss).SequenceEqual([1]),
                "At the final own play phase an opponent cannot redeem a next-turn draw; outside Wushuang the historical general target policy is retained.");
            var lethal = view with { Players = view.Players.Select(p => p.Seat == 0 ? p with {
                    Hand = [new(9001, CardKind.Slash, Suit.Spade, 7, "visible Slash", "7")], HandCount = 1 } :
                    p with { Hp = 1, Hand = [], HandCount = 0 }).ToArray() };
            var slash = new LegalAction(LegalActionKind.Slash, 9001, 1, "ordinary lethal Slash");
            var payment = PublicDamagePrediction.Calculate(1, 0, 1, maxHp: lethal.Players[1].MaxHp);
            payment = payment with { Wushuang = new(payment, null, false, false, 0, 1) };
            Require(new SimpleAiBrain(0, 71, 13).ChoosePlay(lethal, [slash, loss, end], 1, (_, _) => payment)
                    .Action.ProgramSkillId == loss.ProgramSkillId &&
                new SimpleAiBrain(0, 71, 13).ChooseActiveSkillTargets(lethal, loss).SequenceEqual([1]),
                "Immediate direct HP-loss lethal retains its previous finishing priority above spending a respondable ordinary Slash; the deferred draw debt is not charged after the public lethal branch.");
            var beforeHp = view.Players[0].Hp;
            Accept(game, new UseProgramSkillCommand(0, loss.ProgramSkillId!, loss.ProgramActivationId!, [], [0], game.Revision, P(game)!.PromptId));
            Play(game);
            Require(game.CreateSnapshot(0).Players[0].Hp == beforeHp - 1 &&
                E<WushuangYinglueArmedEvent>(game).ToArray() is [{ TargetSeat: 0, Phase: TurnPhase.Draw, Amount: 2 }],
                "The chosen actual target pays one real HP and owns the one native future Draw2 grant.");
            Cold(game, registry);
        }
    }
}
