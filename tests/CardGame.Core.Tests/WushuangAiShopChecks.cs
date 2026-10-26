using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    private static readonly Lazy<ContentRegistry> Registry = new(StandardWushuangContentRegistry.Create);

    public static void EmergencyOpeningAndHandSynergyUseExecutableBenefits()
    {
        var view = View([], hp: 1);
        var peach = CardOffer("heal", CardKind.Peach, 1);
        var draw = Tactic(2060, 2);
        var shop = Shop(3, peach, draw);
        Need(Brain().ChooseWushuangShop(view, shop, Registry.Value).OfferId == peach.Id,
            "An actual immediate Peach at one HP takes priority over buying a future two-card draw engine.");
        var opening = Shop(0, Tactic(2135, 0), Tactic(2059, 0)) with { Phase = WushuangShopPhase.OpeningTactic };
        Need(Brain().ChooseWushuangShop(View([Card(1, CardKind.Slash), Card(2, CardKind.Dodge)]), opening, Registry.Value).OfferId == Tactic(2059, 0).Id,
            "The initial tactic is selected by executable recurring draw rather than the price/quality of surplus hand capacity.");
        var bow = CardOffer("bow", CardKind.Crossbow, 0);
        var armor = CardOffer("armor", CardKind.BaguaFormation, 0);
        var burst = View([Card(1, CardKind.Slash), Card(2, CardKind.FireSlash), Card(3, CardKind.ThunderSlash)]);
        Need(Brain().ChooseWushuangShop(burst, Shop(0, bow, armor), Registry.Value).OfferId == bow.Id &&
            Brain().ChooseWushuangShop(View([Card(1, CardKind.Slash)]), Shop(0, bow, armor), Registry.Value).OfferId == armor.Id,
            "The same published Crossbow beats armor only when this owner's actual hand can use the extra Slash capacity.");
    }

    public static void FullSlotsReplaceWeakSkillsAndRejectRedundantCapabilities()
    {
        var view = OwnSkills(View([]), "classic:yicong", "wushuang:tactic-2059");
        var upgrade = new WushuangShopOffer("engine", "unimportant label", "unimportant prose", 2,
            "wushuang:tactic-2060", WushuangOfferKind.Skill, 1);
        var shop = Shop(3, upgrade) with { ExtraSkillIds = ["classic:yicong", "wushuang:tactic-2059"] };
        var choice = Brain().ChooseWushuangShop(view, shop, Registry.Value);
        Need(choice.Action == WushuangShopAction.Buy && choice.ReplaceSkillId == "classic:yicong" && choice.OfferId == upgrade.Id,
            "A full shop replaces the duel-useless distance skill, preserving the existing recurring draw skill.");
        var original = shop with { ExtraSkillIds = ["wushuang:tactic-2059", "wushuang:tactic-2060"], ReplaceableOriginalSkillIds = ["classic:yicong"] };
        var better = upgrade with { Id = "stronger", SkillId = "wushuang:tactic-2208" };
        choice = Brain().ChooseWushuangShop(OwnSkills(View([]), "classic:yicong", "wushuang:tactic-2059", "wushuang:tactic-2060"),
            original with { Offers = [better] }, Registry.Value);
        Need(choice.Action == WushuangShopAction.Buy && choice.ReplaceSkillId == "classic:yicong",
            "Original-general replacement is a candidate only when the owning shop explicitly permits that skill ID.");
        var redundant = View([Card(1, CardKind.Slash), Card(2, CardKind.Slash)]) with
        { Players = View([Card(1, CardKind.Slash), Card(2, CardKind.Slash)]).Players.Select(p => p.Seat == 0 ? p with { Equipment = [Card(8, CardKind.Crossbow)] } : p).ToArray() };
        choice = Brain().ChooseWushuangShop(redundant, Shop(0, Tactic(2209, 0), Tactic(2009, 0)), Registry.Value);
        Need(choice.Action == WushuangShopAction.Finish,
            "Existing unlimited Slash capacity makes both the additive and suit-only Slash-limit upgrades have no marginal benefit, even when free.");
    }

    public static void RefreshAndSavingsHaveFiniteProgressBounds()
    {
        var view = View([]); var brain = Brain(); var free = Shop(0) with { FreeRerolls = 3, RerollCost = 0 };
        Need(brain.ChooseWushuangShop(view, free, Registry.Value).Action == WushuangShopAction.Reroll &&
            brain.ChooseWushuangShop(view, free with { FreeRerolls = 2 }, Registry.Value).Action == WushuangShopAction.Reroll &&
            brain.ChooseWushuangShop(view, free with { FreeRerolls = 1 }, Registry.Value).Action == WushuangShopAction.Finish,
            "Only two free refreshes are spent in the same owning window, regardless of how many a tactic grants.");
        brain = Brain(); var paid = Shop(7);
        Need(brain.ChooseWushuangShop(view, paid, Registry.Value).Action == WushuangShopAction.Reroll &&
            brain.ChooseWushuangShop(view, paid with { Tokens = 6 }, Registry.Value).Action == WushuangShopAction.Finish &&
            Brain().ChooseWushuangShop(view, paid with { Tokens = 4 }, Registry.Value).Action == WushuangShopAction.Finish,
            "Paid search occurs once per window and preserves four tokens for a later purchase.");
        brain = Brain(); var saving = Shop(5, Tactic(2060, 6));
        Need(brain.ChooseWushuangShop(view, saving, Registry.Value).Action == WushuangShopAction.Lock,
            "A useful published engine one token away can be saved rather than paid-refreshing it away.");
        Need(brain.ChooseWushuangShop(view, saving with { IsLocked = true }, Registry.Value).Action == WushuangShopAction.Finish,
            "An already locked page finishes rather than toggling its lock repeatedly in one window.");
        Need(brain.ChooseWushuangShop(view with { TurnNumber = 6 }, saving with { WindowId = 6, IsLocked = true }, Registry.Value).Action == WushuangShopAction.Lock,
            "A page that remains unaffordable beyond four actor turns is explicitly unlocked.");
    }

    public static void DecisionsUseOnlyOwnPublishedAffordableOffersAndAreDeterministic()
    {
        var view = View([Card(1, CardKind.Slash)]);
        var shop = Shop(1, CardOffer("affordable", CardKind.Dodge, 1), CardOffer("expensive", CardKind.DrawTwo, 9));
        var first = Brain(3).ChooseWushuangShop(view, shop, Registry.Value);
        var changedForeign = view with { Seed = 987654, Players = view.Players.Select(p => p.Seat == 0 ? p : p with
        { Hand = [Card(99, CardKind.Peach)], GeneralId = "unrevealed", GeneralName = "different label" }).ToArray() };
        var second = Brain(9123).ChooseWushuangShop(changedForeign, shop, Registry.Value);
        Need(JsonSerializer.Serialize(first) == JsonSerializer.Serialize(second) && first.Action == WushuangShopAction.Buy &&
            first.Candidates.Where(c => c.Action == WushuangShopAction.Buy).All(c => shop.Offers.Any(o => o.Id == c.OfferId && o.Cost <= shop.Tokens)),
            "Own published offer IDs and tokens determine legal candidates; foreign hand faces, labels and brain/game seeds do not enter this deterministic shop score.");
        Need(Brain().ChooseWushuangShop(view, shop with { Seat = 1 }, Registry.Value).Action == WushuangShopAction.Finish,
            "A foreign private shop is rejected instead of being inspected for a purchase.");
    }

    public static void PublicPressureFavorsDefenseButLatePurchasesStillSeekUsefulProgress()
    {
        var hand = new[] { Card(1, CardKind.Slash), Card(2, CardKind.FireSlash), Card(3, CardKind.ThunderSlash) };
        var page = Shop(0, CardOffer("burst", CardKind.Crossbow, 0), CardOffer("defend", CardKind.BaguaFormation, 0));
        var weak = View(hand, hp: 2) with { Players = View(hand, hp: 2).Players.Select(p => p.Seat == 1
            ? p with { WushuangTacticIds = [2059, 2060] } : p).ToArray() };
        Need(Brain().ChooseWushuangShop(weak, page, Registry.Value).OfferId == "defend",
            "A visibly healthier opponent with more public tactics raises the value of actual sustainable armor when the owner is weak.");
        var winning = View(hand) with { Players = View(hand).Players.Select(p => p.Seat == 1 ? p with { Hp = 1 } : p).ToArray() };
        Need(Brain().ChooseWushuangShop(winning, page, Registry.Value).OfferId == "burst",
            "A healthy owner with three real Slashes still buys a usable offensive engine against a wounded opponent rather than always dragging the duel toward a draw.");
        var late = Shop(1, Tactic(2286, 1), CardOffer("ready-defense", CardKind.Dodge, 1)) with { RemainingMilliseconds = 20_000 };
        Need(Brain().ChooseWushuangShop(View([]), late, Registry.Value).OfferId == "ready-defense",
            "Near the draw deadline, a usable defense card beats income that lacks time to repay its purchase.");
        Need(Brain().ChooseWushuangShop(View([]), late with { RemainingMilliseconds = 600_000, RemainingTurns = 1 }, Registry.Value).OfferId == "ready-defense",
            "The published native turn limit also discounts income that cannot repay before HP decides the winner.");
    }

    public static void NativeAiMultiplePurchasesResumeThePaidAcquisitionParentCold()
    {
        var definitions = ContentRegistry.Build(new NativeShopFixture(Registry.Value));
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(12) with
        { AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false, AiPolicyVersion = AiDecisionPolicy.WushuangStrategyVersion, MaxTurns = 8 }, definitions);
        Submit(game, new StartGameCommand());
        Reach(game, () => Pending(game)?.Kind == DecisionKind.SelectGeneral);
        Submit(game, new SelectGeneralCommand(0, NativeShopFixture.Human, game.Revision, Pending(game)!.PromptId));
        Reach(game, () => game.CreateWushuangSnapshot(0) is { CanShop: true, Phase: WushuangShopPhase.Shopping });
        var window = game.ResolutionStack.OfType<WushuangIntermissionFrame>().Single();
        var page = game.CreateWushuangSnapshot(1)!.Offers;
        var acquired = page.SingleOrDefault(o => o.TacticId == 2295) ??
            throw new InvalidOperationException("The native fixture acquisition page changed: " + JsonSerializer.Serialize(page));
        Need(page.Any(o => o.Kind == WushuangOfferKind.Card && o.Cost == 0),
            "The fixed native seed publishes a paid category-card acquisition and another useful free good on the same ordinary AI page.");
        var before = game.Events.Count;
        Submit(game, new FinishWushuangShopCommand(0, game.Revision));
        Reach(game, () => Pending(game)?.SkillPrompt?.SkillId == NativeShopFixture.Watch &&
            game.ResolutionStack.OfType<WushuangIntermissionFrame>().Any(f => !f.Opening && f.Acquisition is { Seat: 1, TacticId: 2295 }));
        var parent = game.ResolutionStack.OfType<WushuangIntermissionFrame>().Single();
        var child = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.WushuangDeckGain is not null);
        Need(parent.Id == window.Id && parent.SeatCursor == 1 && parent.AiShopActions == 1 &&
            parent.Acquisition is { AdvanceSeat: false } receipt && receipt.OfferId == acquired!.Id &&
            child.OwnerSeat == 1 && child.WindowContext?.ParentFrameId == parent.Id && child.WushuangDeckGain!.CardIds.Count == 4 &&
            game.CardMovements.Count(m => m.To == CardLocation.Hand(1) && m.Reason.Value == "wushuang.obtain-deck-cards") == 4 &&
            Facts<WushuangOfferBoughtEvent>(game).Count(e => e.OfferId == acquired.Id) == 1 &&
            game.Events.Skip(before).Select(e => e.Payload).OfType<WushuangTokensChangedEvent>()
                .Where(e => e.Seat == 1 && e.Reason == "purchase").Select(e => e.Delta).SequenceEqual([-acquired.Cost]),
            "The native v11 purchase pays once and owns all four real acquired entities before its own pending movement observer, while retaining the same AI shop parent.");
        game = Restore(game);
        Reach(game, () => game.ResolutionStack.All(f => f.Id != window.Id));
        var bought = game.Events.Skip(before).Select(e => e.Payload).OfType<WushuangOfferBoughtEvent>()
            .Where(e => e.Seat == 1).ToArray();
        var payments = game.Events.Skip(before).Select(e => e.Payload).OfType<WushuangTokensChangedEvent>()
            .Where(e => e.Seat == 1 && e.Reason == "purchase").ToArray();
        Need(bought.Length >= 2 && bought[0].OfferId == acquired!.Id && bought.Skip(1).Any(e => page.Any(o => o.Id == e.OfferId)) &&
            bought.Select(e => e.OfferId).Distinct(StringComparer.Ordinal).Count() == bought.Length &&
            payments.Length == bought.Count(e => e.Cost > 0) && payments.Sum(e => e.Delta) == -bought.Sum(e => e.Cost) &&
            Facts<WushuangTacticAcquiredEvent>(game).Count(e => e.Seat == 1 && e.TacticId == 2295) == 1 &&
            Facts<WushuangDeckCardsObtainedEvent>(game).Count(e => e.ProgramFrameId == child.Id && e.Count == 4) == 1 &&
            Facts<ProgramBindingResolvedEvent>(game).Count(e => e.FrameId == child.Id && e.Completed) == 1,
            "Cold continuation completes the original acquired instruction once, then buys another actual published good in the same window with one payment per purchase.");
        _ = Restore(game);

        GameEngine Restore(GameEngine original)
        {
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), definitions);
            Need(State(original) == State(restored), "Cold replay preserves both views, the exact owning acquisition frame, decisions, movements and paid shop history.");
            return restored;
        }
        static string State(GameEngine g) => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 2).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))).ToArray(),
            Frames = g.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), g.CardMovements,
            Events = g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            Commands = CommandJson.Serialize(g.AcceptedCommands)
        });
        static IEnumerable<T> Facts<T>(GameEngine g) where T : IGameEvent => g.Events.Select(e => e.Payload).OfType<T>();
        static PendingDecision? Pending(GameEngine g) => g.CreateSnapshot(0).PendingDecision ?? g.CreateSnapshot(1).PendingDecision;
        static void Submit(GameEngine g, GameCommand command)
        { var result = g.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native AI shop command rejected."); }
        static void Reach(GameEngine g, Func<bool> done)
        {
            for (var step = 0; step < 100; step++)
            {
                if (done()) return;
                var p = Pending(g);
                if (p is { PlayerSeat: 1 }) Submit(g, new AdvanceOneStepCommand(g.Revision));
                else if (g.CreateWushuangSnapshot(0) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic } opening)
                    Submit(g, new BuyWushuangOfferCommand(0, opening.Offers[0].Id, null, g.Revision));
                else if (p is { Kind: DecisionKind.PlayCard }) Submit(g, new EndPlayPhaseCommand(0, g.Revision, p.PromptId));
                else if (p is { Kind: DecisionKind.ProgramTrigger })
                    Submit(g, new AnswerPromptCommand(0, p.PromptId, p.Choices.First(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, g.Revision));
                else Submit(g, new AdvanceOneStepCommand(g.Revision));
            }
            throw new InvalidOperationException("Native AI shop missed its bounded parent/child boundary: " + JsonSerializer.Serialize(new
            { Prompt = Pending(g), Shop = g.CreateWushuangSnapshot(1), Frames = g.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }), Last = g.Events.TakeLast(6).Select(e => e.Payload.ToString()) }));
        }
    }

    private sealed class NativeShopFixture(ContentRegistry publicDefinitions) : IGameContentPackage
    {
        internal const string Human = "fixture:wushuang-ai-shop-human", Watch = "fixture:wushuang-ai-shop-watch";
        private const string Quiet = "fixture:wushuang-ai-shop-quiet", Ai = "fixture:wushuang-ai-shop-peer", Deck = "fixture:wushuang-ai-shop-deck";
        public PackageManifest Manifest { get; } = new("fixture:wushuang-ai-shop", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            builder.CopyModeFeatureFactoriesFrom(publicDefinitions);
            foreach (var card in publicDefinitions.Cards.Values) builder.AddCard(card);
            foreach (var id in new[] { "wushuang:tactic-2294", "wushuang:tactic-2295" }) builder.AddSkill(publicDefinitions.Skills[id]);
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"real-acquired-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["wushuang.obtain-deck-cards"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-child","options":[{"id":"continue"}]}]}]},
              {"id":"{{{Quiet}}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":30,"priority":0}],"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
            ]}
            """;
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Watch] = new { name = "取得子窗", description = "观察真实取得移动。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [Quiet] = new { name = "安静对手", description = "固定原生商店验证。" }
                }
            }));
            foreach (var id in new[] { Watch, Quiet }) builder.AddSkill(new(id, id, id)
            { Program = programs.Programs[id], ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Human, "商店观察者", "", null, "qun", BaseHp: 30));
            builder.AddGeneral(new(Ai, "商店对手", "", Quiet, "qun", BaseHp: 30, AdditionalSkillIds: [Watch]));
            builder.AddDeck(new(Deck, "Small native acquisition deck", 4, 0, [])
            { PhysicalCards = Enumerable.Range(0, 160).Select(i => new ContentDeckPhysicalCard(i % 2 == 0 ? "standard:draw_two" : "standard:dodge", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(WushuangMode.ModeId, "无双 AI 商店验证", 2, 2, new Dictionary<string, int>(), Deck, 2,
                [Human, Ai], ContentModeKind.Team, new Dictionary<string, int> { [WushuangMode.PlayerTeamId] = 1, [WushuangMode.AiTeamId] = 1 }));
        }
    }

    private static SimpleAiBrain Brain(int seed = 1) => new(0, seed, AiDecisionPolicy.WushuangStrategyVersion);
    private static WushuangSnapshot Shop(int tokens, params WushuangShopOffer[] offers) => new(0, 1, 0, 0,
        tokens, [], offers, false, 1, 0, 600_000, 1) { Phase = WushuangShopPhase.Shopping, WindowId = 1 };
    private static WushuangShopOffer Tactic(int id, int cost)
    {
        var tactic = WushuangTactics.ById[id];
        return new("tactic-" + id, tactic.Name, tactic.Description, cost, null, WushuangOfferKind.Tactic, 1)
        { TacticId = id, Quality = tactic.Quality };
    }
    private static WushuangShopOffer CardOffer(string id, CardKind kind, int cost) =>
        new(id, "irrelevant", "irrelevant", cost, null, WushuangOfferKind.Card, 1) { CardKind = kind };
    private static CardSnapshot Card(int id, CardKind kind) => new(id, kind, Suit.Spade, 7, "irrelevant", "7");
    private static GameSnapshot OwnSkills(GameSnapshot view, params string[] ids) => view with
    { Players = view.Players.Select(p => p.Seat == 0 ? p with { Skills = ids.Select(id => new GeneralSkillDefinition("", "") { ContentId = id }).ToArray() } : p).ToArray() };
    private static GameSnapshot View(CardSnapshot[] hand, int hp = 4) => new(null, 0, EngineStatus.AwaitingHumanPlay,
        Winner.None, 1, 0, TurnPhase.Play, 70, 0,
        [new(0, "own", false, null, false, "fixture:own", "", "", hp, 4, true, hand.Length, hand),
         new(1, "opponent", false, null, false, "fixture:other", "", "", 4, 4, true, 4, [])], null);
    private static void Need(bool value, string reason) { if (!value) throw new InvalidOperationException(reason); }
}
