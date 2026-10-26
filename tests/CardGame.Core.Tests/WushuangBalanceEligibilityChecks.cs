using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void OpeningQualityAndBalanceExclusionsPreserveNativeRulesCold()
    {
        var registry = StandardWushuangContentRegistry.Create();
        var banned = new[] { 2020, 2056, 2263 };
        Require(banned.All(id => WushuangTactics.ById[id] is { IsImplemented: true, IsAvailableInShop: false } tactic &&
                !WushuangCatalogue.CanOfferAsOpening(tactic) && tactic.SkillId is { } skill &&
                registry.Skills.ContainsKey(skill) && WushuangCatalogue.IsSkillAllowed(registry.Skills[skill])),
            "The three overpowering offers leave both production pools, while their real native definitions remain usable by explicitly configured historical fixtures.");
        Require(WushuangTactics.ById[2020] is { Name: "稳定体质", Cost: 1 } &&
                WushuangTactics.ById[2056].Cost == 4 && WushuangTactics.ById[2263].Cost == 4 &&
                new[] { 2054, 2055, 2261, 2262 }.All(id => WushuangTactics.ById[id].IsAvailableInShop) &&
                WushuangTactics.All.Where(tactic => tactic.Quality == 4).All(tactic => !WushuangCatalogue.CanOfferAsOpening(tactic)) &&
                WushuangTactics.ById[2060] is { Quality: 4, StartOnly: false, IsAvailableInShop: true },
            "Opening red quality is filtered separately from paid shopping: DrawII remains a real red shop offer; lower HP growth keeps its source price and rule.");
        Require(!WushuangCatalogue.IsSkillAllowed(registry.Skills[WushuangTactics.ById[2069].SkillId!]) &&
                new[] { "ol:wu-tu-gu", "ol:wang-shuang" }.All(id => registry.Generals.ContainsKey(id) &&
                    !registry.Modes[WushuangMode.ModeId].GeneralPoolIds!.Contains(id)) &&
                registry.Generals["ol:wu-tu-gu"].BaseHp == 15 && registry.Generals["ol:wang-shuang"].BaseHp == 8,
            "Strict armor bans still forbid binding; the two duel-specific general exclusions retain their global definitions and original HP.");

        // Use the current production offers. The shared Start helper defaults
        // to no historical preset; it never bypasses these pool restrictions.
        var (game, content) = Start(false, 33);
        Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        var opening = Shop(game);
        Require(opening.Offers.Count == 3 && opening.Offers.All(offer => offer.Kind == WushuangOfferKind.Tactic &&
                offer.Quality < 4 && offer.TacticId is { } id && WushuangCatalogue.CanOfferAsOpening(WushuangTactics.ById[id])),
            "The real shuffled opening refills three eligible choices after filtering, with no removed tier or red-quality reward.");
        Cold(game, content);
        var before = Views(game); var revision = game.Revision;
        Require(!game.Submit(new BuyWushuangOfferCommand(0, "fixture:removed-opening-2020", null, revision)).Accepted &&
                game.Revision == revision && Views(game) == before,
            "A removed opening identity cannot be forged into a purchase or change either player's prepared view.");
        Buy(game, opening.Offers[0].Id);
        Shopping(game);
        for (var page = 0; page < 3; page++)
        {
            Require(Shop(game).Offers.All(offer => offer.TacticId is not { } id ||
                    WushuangTactics.ById[id] is { IsAvailableInShop: true, StartOnly: false } && !banned.Contains(id)),
                "Actual paid shop refreshes retain the production availability and StartOnly boundaries.");
            if (page < 2 && Shop(game).Tokens >= Shop(game).RerollCost)
                Accept(game, new RerollWushuangShopCommand(0, game.Revision));
        }
        Cold(game, content);
    }
}
