using CardGame.Core;

internal static partial class WushuangAiShopChecks
{
    public static void DyingWindowsDoNotOutbidReliableDrawAsRoutineIncome()
    {
        var reliable = BuildSkillOffer("reliable-draw", "classic:yingzi", 1);
        var dying = BuildSkillOffer("dying-draw", "ol:juejing", 1);
        var healthy = Brain().ChooseWushuangShop(View([]), Shop(2, reliable, dying), Registry.Value);
        Need(healthy.OfferId == reliable.Id && healthy.Candidates.Single(c => c.OfferId == dying.Id).Score <
            healthy.Candidates.Single(c => c.OfferId == reliable.Id).Score,
            "Drawing on entering and surviving dying is conditional rescue value, rather than two reliable draw events every expected future turn.");
        var emergency = Brain().ChooseWushuangShop(View([Card(1, CardKind.Peach)], hp: 1), Shop(2, dying), Registry.Value);
        Need(emergency.Candidates.Single(c => c.OfferId == dying.Id).Score > healthy.Candidates.Single(c => c.OfferId == dying.Id).Score,
            "A genuine low-HP owner with a visible self-rescue retains higher dying-window value; the fix does not erase the executable skill.");
    }

    public static void SameOwnerNewAndReplacedSkillsRecomputeRecoveryAndBurstPurchases()
    {
        var brain = Brain(); var sameOwner = View([]);
        var defense = CardOffer("defense", CardKind.Dodge, 1); var peach = CardOffer("recover", CardKind.Peach, 1);
        var reservePage = Shop(1, defense, peach);
        Need(brain.ChooseWushuangShop(sameOwner, reservePage, Registry.Value).OfferId == defense.Id &&
            brain.ChooseWushuangShop(OwnSkills(sameOwner, "classic:yiji"), reservePage, Registry.Value).OfferId == peach.Id &&
            brain.ChooseWushuangShop(OwnSkills(sameOwner, "boundary:pojun"), reservePage, Registry.Value).OfferId == defense.Id,
            "The same full-HP general first buys defense, then reserves a Peach after acquiring per-damage Draw2, and recalculates after replacing that skill; names and printed descriptions do not select the plan.");
        var slashHand = View([Card(2, CardKind.Slash)]);
        var wine = CardOffer("burst-wine", CardKind.Alcohol, 1); var burstPage = Shop(1, defense, wine);
        Need(brain.ChooseWushuangShop(slashHand, burstPage, Registry.Value).OfferId == defense.Id &&
            brain.ChooseWushuangShop(OwnSkills(slashHand, "boundary:pojun"), burstPage, Registry.Value).OfferId == wine.Id &&
            brain.ChooseWushuangShop(OwnSkills(slashHand, "classic:yiji"), burstPage, Registry.Value).OfferId == defense.Id,
            "Acquiring executable Slash damage plus target-card holding makes the independent actual wine/Slash pair preferable; removing it immediately removes that purchase synergy.");
        var fixedDamage = OwnSkills(slashHand, "boundary:pojun") with { Players = OwnSkills(slashHand, "boundary:pojun").Players
            .Select(player => player.Seat == 0 ? player with { WushuangTacticIds = [2210] } : player).ToArray() };
        Need(brain.ChooseWushuangShop(fixedDamage, burstPage, Registry.Value).OfferId == defense.Id,
            "Even a burst build does not buy wine for additional damage when its public fixed-two rule removes that marginal gain.");
    }

    public static void ReplacementNetValueRetainsCurrentIndependentBurstMaterials()
    {
        var offer = BuildSkillOffer("replace-with-draw", "classic:yingzi", 1);
        var page = Shop(2, offer) with { ExtraSkillIds = ["boundary:pojun", "wushuang:tactic-2060"] };
        var sameOwner = OwnSkills(View([Card(1, CardKind.Slash), Card(2, CardKind.Alcohol)]), "boundary:pojun", "wushuang:tactic-2060");
        var ready = Brain().ChooseWushuangShop(sameOwner, page, Registry.Value);
        Need(ready.Action == WushuangShopAction.Buy && ready.OfferId == offer.Id && ready.Score > 0 &&
            ready.ReplaceSkillId == "boundary:pojun",
            "Current burst synergy is a replacement cost, not an absolute ban: a recurring draw upgrade can still be bought when its complete net value remains positive, while retaining the more valuable two-card draw engine.");
        var withoutSlash = OwnSkills(View([Card(2, CardKind.Alcohol)]), "boundary:pojun", "wushuang:tactic-2060");
        var changed = Brain().ChooseWushuangShop(withoutSlash, page, Registry.Value);
        Need(changed.Action == WushuangShopAction.Buy && changed.ReplaceSkillId == "boundary:pojun",
            "Without an executable Slash component the same offered draw can exceed the replaced skill's actual current synergy; the build reader does not permanently lock a preferred skill.");
        Need(ready.Candidates.Single(candidate => candidate.ReplaceSkillId == "boundary:pojun").Score <
            changed.Candidates.Single(candidate => candidate.ReplaceSkillId == "boundary:pojun").Score &&
            ready.Candidates.Single(candidate => candidate.ReplaceSkillId == "wushuang:tactic-2060").Score < 0,
            "The actual replacement margin must charge more for losing a current independent Slash/control combination than for the same skill with no Slash; it must not silently sacrifice the stronger retained draw engine.");
    }

    private static WushuangShopOffer BuildSkillOffer(string id, string skill, int cost) =>
        new(id, "ignored label", "ignored prose", cost, skill, WushuangOfferKind.Skill, 1);
}
