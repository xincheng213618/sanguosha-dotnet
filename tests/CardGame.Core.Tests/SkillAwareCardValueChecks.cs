using CardGame.Core;

internal static class SkillAwareCardValueChecks
{
    public static void ReservesLastRescueAndSkillResponseMaterials()
    {
        var alcohol = Card(1, CardKind.Alcohol, Suit.Spade);
        var slash = Card(2, CardKind.Slash, Suit.Heart);
        var wounded = View(1, alcohol, slash);
        var payment = Payment(alcohol, slash);
        Need(Brain(5).ChooseActiveSkillCards(wounded, payment).SequenceEqual([alcohol.Id]),
            "Policy five must keep its recorded static-cost choice of Alcohol.");
        Need(Brain().ChooseActiveSkillCards(wounded, payment).SequenceEqual([slash.Id]),
            "At one HP an ordinary skill fee must not consume the only self-rescue when another legal material exists.");
        var healthy = WithOwner(wounded, self => self with { Hp = 4 });
        Need(Brain().ChooseActiveSkillCards(healthy, payment).SequenceEqual([alcohol.Id]),
            "The same material ranking must change with current HP rather than permanently inflating Alcohol's price.");

        var black = Card(3, CardKind.Slash, Suit.Club);
        var red = Card(4, CardKind.Slash, Suit.Heart);
        var response = WithOwner(View(2, black, red), self => self with
        {
            Skills = [new("fixture response", "Own visible black material may respond as Dodge")
            {
                ViewAsOpportunities = [new(CardKind.Dodge, [], [Suit.Spade, Suit.Club], false, true)]
            }]
        });
        var responsePayment = Payment(black, red);
        Need(Brain(5).ChooseActiveSkillCards(response, responsePayment).SequenceEqual([black.Id]) &&
             Brain().ChooseActiveSkillCards(response, responsePayment).SequenceEqual([red.Id]),
            "The new policy must retain the only material of its own visible response conversion, while the old policy keeps its original tie order.");
        var spare = Card(5, CardKind.Slash, Suit.Spade);
        var withSpare = WithOwner(response, self => self with
            { Hand = [black, red, spare], HandCount = 3 });
        Need(Brain().ChooseActiveSkillCards(withSpare, responsePayment).SequenceEqual([red.Id]),
            "Two physical conversion materials are alternatives, rather than treating multiple capabilities on one card as independent stock.");
    }

    public static void PricesSpareResponsesAndKeepsSelectionConstraints()
    {
        var peach = Card(1, CardKind.Peach, Suit.Heart);
        var firstDodge = Card(2, CardKind.Dodge, Suit.Spade);
        var secondDodge = Card(3, CardKind.Dodge, Suit.Club);
        var view = View(2, peach, firstDodge, secondDodge);
        var payment = Payment(peach, firstDodge, secondDodge);
        Need(Brain(5).ChooseActiveSkillCards(view, payment).SequenceEqual([peach.Id]) &&
             Brain().ChooseActiveSkillCards(view, payment).SequenceEqual([firstDodge.Id]),
            "A wounded owner should pay a spare Dodge before the sole Peach while retaining the other Dodge.");

        var alcohol = Card(4, CardKind.Alcohol, Suit.Spade);
        var slash = Card(5, CardKind.Slash, Suit.Spade);
        var duel = Card(6, CardKind.Duel, Suit.Heart);
        view = View(1, alcohol, slash, duel);
        var distinct = Payment(alcohol, slash, duel) with
        {
            MinCardCount = 2, MaxCardCount = 2, SelectedCardsDistinctSuits = true
        };
        var selected = Brain().ChooseActiveSkillCards(view, distinct);
        Need(selected.ToHashSet().SetEquals([slash.Id, duel.Id]) &&
             selected.Select(id => view.Players[0].Hand.Single(card => card.Id == id).Suit).Distinct().Count() == 2,
            "Dynamic payment selection must preserve the issued different-suit constraint while keeping the only Alcohol.");
        var same = distinct with { SelectedCardsDistinctSuits = false, SelectedCardsSameSuit = true };
        Need(Brain().ChooseActiveSkillCards(view, same).ToHashSet().SetEquals([alcohol.Id, slash.Id]),
            "Reserve preferences cannot invent an unavailable same-suit payment; the only legal pair remains selected.");
        var ordered = payment with { ProgramAiHint = new(0, 0, 0, 0, 0, 0, false, true)
            { PreferPindianInputOrder = true }, SelectableCardIds = [secondDodge.Id, firstDodge.Id, peach.Id] };
        view = View(2, peach, firstDodge, secondDodge);
        Need(Brain().ChooseActiveSkillCards(view, ordered).SequenceEqual([secondDodge.Id]),
            "A published Pindian input order must continue to override generic payment value sorting.");
    }

    public static void PreservesConvertibleDefenseWithoutRevivingRejectedActions()
    {
        var black = Card(1, CardKind.Slash, Suit.Club);
        var red = Card(2, CardKind.Slash, Suit.Heart);
        var view = WithOwner(View(2, black, red), self => self with
        {
            Skills = [new("fixture response", "Black material may respond as Dodge")
            { ViewAsOpportunities = [new(CardKind.Dodge, [], [Suit.Spade, Suit.Club], false, true)] }]
        });
        var blackAttack = new LegalAction(LegalActionKind.Slash, black.Id, 1, "black attack");
        var redAttack = new LegalAction(LegalActionKind.Slash, red.Id, 1, "red attack");
        var end = new LegalAction(LegalActionKind.EndPlay, null, null, "end");
        Need(Brain(5).ChoosePlay(view, [blackAttack, redAttack, end], 1).Action == blackAttack &&
             Brain().ChoosePlay(view, [blackAttack, redAttack, end], 1).Action == redAttack,
            "Ordinary attacks must prefer the otherwise equal card that does not spend the only skill-based defense.");

        var friend = view with { Players = view.Players.Select(player => player.Seat == 1
            ? player with { Role = Role.Rebel, IsRoleRevealed = true } : player).ToArray() };
        var choice = Brain().ChoosePlay(friend, [redAttack, end], 1);
        Need(choice.Action == end && choice.Thought.Candidates.Single(candidate => candidate.Action == redAttack).Score < 0,
            "An overflow-card discount cannot turn a rejected friendly attack into a useful action.");

        var dodge = Card(7, CardKind.Dodge, Suit.Spade);
        var conversionView = View(2, dodge);
        var converted = new LegalAction(LegalActionKind.Slash, dodge.Id, 1, "Dodge as Slash", CardKind.Slash);
        var prior = Brain(5).ChoosePlay(conversionView, [converted, end], 1).Thought.Candidates.Single(candidate => candidate.Action == converted);
        var current = Brain().ChoosePlay(conversionView, [converted, end], 1).Thought.Candidates.Single(candidate => candidate.Action == converted);
        Need(!current.Reason.Contains("机会成本", StringComparison.Ordinal) && current.Score <= prior.Score + .001d,
            "The existing last-Dodge conversion reserve must remain charged once, without an extra generic opportunity-cost charge or a refund.");
    }

    public static void DuelReadinessUsesRemainingResponsesAndPublicRisk()
    {
        var duel = Card(1, CardKind.Duel, Suit.Heart);
        var draw = Card(2, CardKind.DrawTwo, Suit.Heart);
        var attack = new LegalAction(LegalActionKind.Duel, duel.Id, 1, "native Duel");
        var drawAction = new LegalAction(LegalActionKind.DrawTwo, draw.Id, 0, "draw first");
        var end = new LegalAction(LegalActionKind.EndPlay, null, null, "end");
        var view = View(1, duel, draw);
        view = view with { Players = view.Players.Select(player => player.Seat == 1
            ? player with { HandCount = 6 } : player).ToArray() };
        var before = SnapshotJson.Serialize(view);
        Need(Brain(5).ChoosePlay(view, [attack, drawAction, end], 1).Action == attack &&
             Brain().ChoosePlay(view, [attack, drawAction, end], 1).Action == drawAction &&
             Brain().ChoosePlay(view, [attack, end], 1).Action == end,
            "One HP and no remaining Slash responses should reject a risky Duel into six public cards, naturally choosing an available draw instead.");
        var first = Brain().ChoosePlay(view, [attack, drawAction, end], 1);
        var second = Brain().ChoosePlay(view, [attack, drawAction, end], 1);
        Need(first.Thought.Candidates.SequenceEqual(second.Thought.Candidates) && before == SnapshotJson.Serialize(view),
            "The bounded risk estimate must be deterministic and leave its original viewer snapshot unchanged.");

        var empty = view with { Players = view.Players.Select(player => player.Seat == 1
            ? player with { HandCount = 0 } : player).ToArray() };
        Need(Brain().ChoosePlay(empty, [attack, drawAction, end], 1).Action == attack &&
             DuelScore(empty, 6) == DuelScore(empty, 5),
            "A publicly empty opponent must not create imaginary Slash-response risk or a blanket draw-first preference.");
        var stocked = WithOwner(view, self => self with
        {
            Hand = new[] { duel }.Concat(Enumerable.Range(10, 6).Select(id => Card(id, CardKind.Slash, Suit.Spade))).ToArray(),
            HandCount = 7
        });
        Need(Brain().ChoosePlay(stocked, [attack, end], 1).Action == attack &&
             DuelScore(stocked, 6) == DuelScore(stocked, 5),
            "Enough distinct own Slash entities to answer every public enemy card should remove the readiness penalty.");
        var armored = WithOwner(view, self => self with { Armor = 5 });
        Need(Brain().ChoosePlay(armored, [attack, end], 1).Action == attack &&
             DuelScore(armored, 6) > DuelScore(view, 6),
            "A one-HP owner with five public Armor must not receive the unprotected one-HP critical-risk penalty.");
        var undefinedHp = WithOwner(view, self => self with { Hp = 0, HpUndefined = true });
        Need(DuelScore(undefinedHp, 6) == DuelScore(armored, 6),
            "Undefined HP cannot be interpreted as a known zero-HP survival reserve.");

        var spentConversion = WithOwner(view, self => self with
        {
            Hand = [duel], HandCount = 1,
            Skills = [new("Duel-only Slash conversion", "The actual Duel can respond as Slash only while unspent")
            { ViewAsOpportunities = [new(CardKind.Slash, [CardKind.Duel], [], false, true)] }]
        });
        spentConversion = spentConversion with { Players = spentConversion.Players.Select(player => player.Seat == 1
            ? player with { HandCount = 4 } : player).ToArray() };
        var spent = Brain().ChoosePlay(spentConversion, [attack, end], 1);
        Need(spent.Action == end && spent.Thought.Candidates.Single(candidate => candidate.Action == attack).Reason.Contains("响应储备 0", StringComparison.Ordinal),
            "The physical Duel being used cannot simultaneously count as its owner's available view-as Slash response.");
        var withConversions = view with { Players = view.Players.Select(player => player.Seat == 1
            ? player with { Skills = [new("visible response", "Visible extra Slash-response capability")
                { ViewAsOpportunities = [new(CardKind.Slash, [CardKind.Dodge], [], false, true)] }] } : player).ToArray() };
        Need(DuelScore(withConversions, 6) < DuelScore(view, 6),
            "A publicly visible response conversion should increase the response prior without consulting the opponent's hand faces.");
        var invalidFriendly = view with { Players = view.Players.Select(player => player.Seat == 1
            ? player with { Role = Role.Rebel } : player).ToArray() };
        Need(Brain().ChoosePlay(invalidFriendly, [attack, end], 1).Action == end &&
             DuelScore(invalidFriendly, 6) == DuelScore(invalidFriendly, 5),
            "The readiness model must leave an already rejected friendly Duel unchanged.");

        double DuelScore(GameSnapshot state, int policy) => Brain(policy).ChoosePlay(state, [attack, end], 1)
            .Thought.Candidates.Single(candidate => candidate.Action == attack).Score;
    }

    private static SimpleAiBrain Brain(int policy = AiDecisionPolicy.TacticalPlanningVersion) => new(0, 31, policy);

    private static LegalAction Payment(params CardSnapshot[] cards) =>
        new(LegalActionKind.UseProgramSkill, null, null, "fixture payment", MinCardCount: 1, MaxCardCount: 1)
        {
            ProgramSkillId = "fixture:skill-value", ProgramActivationId = "pay",
            ProgramAiHint = new(1, 0, 0, 0, 0, 0, false, true),
            SelectableCardIds = cards.Select(card => card.Id).ToArray()
        };

    private static GameSnapshot View(int hp, params CardSnapshot[] cards) =>
        new(null, 0, EngineStatus.AwaitingHumanPlay, Winner.None, 1, 0, TurnPhase.Play, 80, 0,
        [new(0, "owner", true, Role.Rebel, true, "fixture:owner", "owner", "", hp, 4, true, cards.Length, cards),
         new(1, "enemy", false, Role.Lord, true, "fixture:enemy", "enemy", "", 4, 4, true, 0, [])], null);

    private static GameSnapshot WithOwner(GameSnapshot view, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == 0 ? change(player) : player).ToArray() };

    private static CardSnapshot Card(int id, CardKind kind, Suit suit) => new(id, kind, suit, 7,
        CardCatalog.Get(kind).DisplayName, "7");

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
