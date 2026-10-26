using CardGame.Content.Standard;
using CardGame.Core;

internal static class WushuangAiNativeShopChecks
{
    private static readonly Lazy<ContentRegistry> Definitions = new(StandardWushuangContentRegistry.Create);

    public static void NativeDefenseFixedDamageAndDrunkenFistHaveConditionalPurchaseValue()
    {
        var thin = View([Card(1, CardKind.Slash)], hp: 2);
        Need(Choose(thin, Shop(3, Tactic(2197, 2))).Action == WushuangShopAction.Buy,
            "Native finite reduction must have real defensive purchase value even though its catalogue SkillId is null.");
        var fixedEnemy = Change(thin, 1, player => player with { WushuangTacticIds = [2210] });
        var search = Choose(fixedEnemy, Shop(3, Tactic(2197, 2)));
        Need(search.Action == WushuangShopAction.Reroll && search.Candidates.Where(candidate => candidate.Action == WushuangShopAction.Buy).All(candidate => candidate.Score <= 0),
            "The public fixed-two finalizer makes finite reduction worthless; this wounded owner searches for useful survival instead while retaining a purchase budget.");
        Need(Choose(View([Card(1, CardKind.Slash)]), Shop(3, Tactic(2210, 1))).Action == WushuangShopAction.Buy,
            "Fixed two upgrades a normal one-point damage plan rather than remaining an unscored native tactic.");
        var boosted = Change(View([Card(1, CardKind.Slash, Suit.Heart)]), 0, player => player with { WushuangTacticIds = [2045] });
        Need(Choose(boosted, Shop(0, Tactic(2210, 0))).Action == WushuangShopAction.Finish,
            "Fixing already boosted Heart Slash damage to two would remove its public damage gain, so even a free copy is declined.");
        var drunk = Tactic(2001, 2);
        Need(Choose(View([Card(1, CardKind.Alcohol), Card(2, CardKind.Slash)]), Shop(3, drunk)).Action == WushuangShopAction.Buy &&
            Choose(View([Card(1, CardKind.Alcohol)]), Shop(3, drunk)).Action == WushuangShopAction.Finish,
            "Drunken Fist is purchased for independent actual wine and Slash materials; a lone wine cannot pay both actions.");
    }

    public static void NativeQuotaSlotsAndOriginalReplacementRespectExistingCapacity()
    {
        var burst = View([Card(1, CardKind.Slash), Card(2, CardKind.FireSlash), Card(3, CardKind.ThunderSlash)]);
        Need(Choose(burst, Shop(3, Tactic(2226, 1))).Action == WushuangShopAction.Buy,
            "Unlocked empty skill slots give actual native extra Slash quota when the current hand can spend it.");
        var unlimited = Change(burst, 0, player => player with { Equipment = [Card(8, CardKind.Crossbow)] });
        Need(Choose(unlimited, Shop(0, Tactic(2226, 0), Tactic(2156, 0), Tactic(2282, 0))).Action == WushuangShopAction.Finish,
            "An existing Crossbow makes additive native Slash-limit goods redundant, including free ones.");
        var full = Shop(3, Tactic(2293, 1)) with { ExtraSkillIds = ["wushuang:tactic-2059", "wushuang:tactic-2060"] };
        Need(Choose(burst, full).Action == WushuangShopAction.Buy &&
            Choose(burst, full with { ExtraSkillIds = [] }).Action == WushuangShopAction.Finish,
            "A third native skill slot is valuable when both existing slots are occupied, rather than simply rewarding a slot label.");
        var weakOriginal = Change(burst, 0, player => player with { Skills = [new("", "") { ContentId = "classic:yicong" }] });
        var strongOriginal = Change(burst, 0, player => player with { Skills = [new("", "") { ContentId = "classic:yingzi" }] });
        Need(Choose(weakOriginal, Shop(3, Tactic(2221, 1))).Action == WushuangShopAction.Buy &&
            Choose(strongOriginal, Shop(3, Tactic(2221, 1))).Action == WushuangShopAction.Finish,
            "Original-skill replacement buys an option to replace a duel-useless distance skill, while preserving a genuinely valuable recurring draw skill.");
    }

    private static WushuangShopDecision Choose(GameSnapshot view, WushuangSnapshot shop) =>
        new SimpleAiBrain(0, 71, AiDecisionPolicy.WushuangStrategyVersion).ChooseWushuangShop(view, shop, Definitions.Value);
    private static WushuangSnapshot Shop(int tokens, params WushuangShopOffer[] offers) => new(0, 1, 0, 0, tokens,
        [], offers, false, 1, 0, 600_000, 1) { WindowId = 1, Phase = WushuangShopPhase.Shopping };
    private static WushuangShopOffer Tactic(int id, int cost) => new("native-" + id, "ignored", "ignored", cost,
        null, WushuangOfferKind.Tactic, 1) { TacticId = id };
    private static CardSnapshot Card(int id, CardKind kind, Suit suit = Suit.Spade) => new(id, kind, suit, 7, "ignored", "7");
    private static GameSnapshot Change(GameSnapshot view, int seat, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == seat ? change(player) : player).ToArray() };
    private static GameSnapshot View(CardSnapshot[] hand, int hp = 4) => new(null, 0, EngineStatus.AwaitingHumanPlay,
        Winner.None, 1, 0, TurnPhase.Play, 80, 0,
        [new(0, "own", false, null, false, "fixture:own", "", "", hp, 4, true, hand.Length, hand),
         new(1, "opponent", false, null, false, "fixture:other", "", "", 4, 4, true, 4, [])], null);
    private static void Need(bool condition, string why) { if (!condition) throw new InvalidOperationException(why); }
}
