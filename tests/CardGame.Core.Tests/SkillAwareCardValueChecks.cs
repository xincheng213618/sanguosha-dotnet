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
