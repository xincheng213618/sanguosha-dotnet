using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WushuangAiShopChecks
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
        Need(brain.ChooseWushuangShop(view with { TurnNumber = 4 }, saving with { WindowId = 4, IsLocked = true }, Registry.Value).Action == WushuangShopAction.Lock,
            "A page that remains unaffordable beyond two turns is explicitly unlocked.");
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
