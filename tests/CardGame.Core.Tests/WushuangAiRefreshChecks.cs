using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    public static void PaidRefreshFollowsMissingBuildComponentsAndPreservesPurchaseBudget()
    {
        var plain = View([Card(1, CardKind.Slash)]);
        var missingWine = OwnSkills(plain, "boundary:pojun");
        var page = Shop(3);
        Need(Brain().ChooseWushuangShop(plain, page, Registry.Value).Action == WushuangShopAction.Finish &&
            Brain().ChooseWushuangShop(missingWine, page, Registry.Value).Action == WushuangShopAction.Reroll,
            "The same empty page and three tokens trigger paid search only when the current owned Slash build lacks a useful component.");
        var brain = Brain();
        Need(brain.ChooseWushuangShop(missingWine, page, Registry.Value).Action == WushuangShopAction.Reroll &&
            brain.ChooseWushuangShop(missingWine, page with { Tokens = 2 }, Registry.Value).Action == WushuangShopAction.Reroll &&
            brain.ChooseWushuangShop(missingWine, page with { Tokens = 1 }, Registry.Value).Action == WushuangShopAction.Finish,
            "Two searches consume the available search budget while preserving the last token for a purchase.");
        var ready = OwnSkills(View([Card(1, CardKind.Slash), Card(2, CardKind.Alcohol)]), "boundary:pojun");
        Need(Brain().ChooseWushuangShop(ready, page, Registry.Value).Action == WushuangShopAction.Finish,
            "An already executable Slash/wine build does not spend a small balance searching for duplicate components.");
        var emergency = View([], hp: 1);
        Need(Brain().ChooseWushuangShop(emergency, Shop(2), Registry.Value).Action == WushuangShopAction.Reroll &&
            Brain().ChooseWushuangShop(emergency, Shop(1), Registry.Value).Action == WushuangShopAction.Finish &&
            Brain().ChooseWushuangShop(View([Card(7, CardKind.Peach)], hp: 1), Shop(2), Registry.Value).Action == WushuangShopAction.Finish,
            "A wounded owner can search for survival while preserving a purchase token, then stops after acquiring a usable rescue card.");
        Need(Brain().ChooseWushuangShop(missingWine, page with { RemainingTurns = 0 }, Registry.Value).Action == WushuangShopAction.Finish &&
            Brain().ChooseWushuangShop(missingWine, page with { RemainingMilliseconds = 4_000 }, Registry.Value).Action == WushuangShopAction.Finish,
            "Paid searching stops when the current HP result is about to close the match.");
    }

    public static void UsefulUnaffordableUpgradesLockBeforeSearchAndExpireWithoutProgress()
    {
        var view = OwnSkills(View([]), "classic:yicong", "wushuang:tactic-2059");
        var upgrade = new WushuangShopOffer("valuable-upgrade", "irrelevant", "irrelevant", 4,
            "wushuang:tactic-2060", WushuangOfferKind.Skill, 1);
        var shop = Shop(1, upgrade) with { ExtraSkillIds = ["classic:yicong", "wushuang:tactic-2059"] };
        var brain = Brain();
        Need(brain.ChooseWushuangShop(view, shop, Registry.Value).Action == WushuangShopAction.Lock,
            "Even with full skill slots, a fitting positive-value replacement three tokens away is locked rather than lost to a paid search.");
        Need(brain.ChooseWushuangShop(view with { TurnNumber = 3 }, shop with { Tokens = 3, IsLocked = true, WindowId = 3 }, Registry.Value).Action == WushuangShopAction.Finish,
            "The same still-useful locked upgrade is retained while real income closes its price gap.");
        var bought = brain.ChooseWushuangShop(view with { TurnNumber = 4 }, shop with { Tokens = 4, IsLocked = true, WindowId = 4 }, Registry.Value);
        Need(bought.Action == WushuangShopAction.Buy && bought.OfferId == upgrade.Id && bought.ReplaceSkillId == "classic:yicong",
            "Once the actual token balance covers the price, the original locked ID is bought with its currently legal weakest-skill replacement.");
        brain = Brain();
        brain.ChooseWushuangShop(view, shop, Registry.Value);
        Need(brain.ChooseWushuangShop(view with { TurnNumber = 6 }, shop with { IsLocked = true, WindowId = 6 }, Registry.Value).Action == WushuangShopAction.Lock,
            "A locked page with no income progress is explicitly unlocked after its bounded saving horizon.");
        Need(Brain().ChooseWushuangShop(view, shop with { RemainingTurns = 1 }, Registry.Value).Action == WushuangShopAction.Finish,
            "The last actor turn cannot justify locking an unaffordable long-term upgrade.");
    }
}
