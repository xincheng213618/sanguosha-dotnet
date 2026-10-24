using CardGame.Core;

internal static class IdentityStrategyAiChecks
{
    public static void RebelsConcentrateOnLordWithoutRevivingBlockedAttacks()
    {
        var slashCard = Card(10, CardKind.Slash);
        var dismantle = Card(11, CardKind.Dismantlement);
        var weapon = Card(90, CardKind.QilinBow);
        var view = Scene(Role.Rebel, slashCard, dismantle);
        view = Change(view, 2, player => player with { Equipment = [weapon], HandCount = 0 });
        var lordAttack = Slash(slashCard.Id, 1);
        var side = new LegalAction(LegalActionKind.Dismantlement, dismantle.Id, 2, "remove side weapon", TargetCardId: weapon.Id);
        var actions = new[] { side, lordAttack, End };
        var old = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(view, actions, 1);
        var current = Brain().ChoosePlay(view, actions, 1);
        Need(old.Action == side && current.Action == lordAttack,
            "An exposed Lord should outweigh a distracting healthy loyalist's weapon for an already profitable Rebel attack.");
        Need(current.Thought.Candidates.Single(candidate => candidate.Action == lordAttack).Reason.Contains("主公", StringComparison.Ordinal),
            "The changed priority must explain the public identity objective.");

        var blocked = Change(view, 1, player => player with { Equipment = [Card(91, CardKind.RenwangShield)] });
        var noRevival = Brain().ChoosePlay(blocked, actions, 1);
        Need(noRevival.Action == side && Score(noRevival.Thought, lordAttack) < 0,
            "Rebel Lord priority must not revive a black Slash that the shared armor scorer already rejected.");
        var reused = Brain();
        Need(reused.ChoosePlay(view, [side, End], 1).Action == side &&
            reused.ChoosePlay(view, actions, 2).Action == lordAttack &&
            reused.ChoosePlay(blocked, actions, 3).Action == side,
            "Published Lord-attack availability must be recalculated on every decision when either actions or public defenses change.");
    }

    public static void LordSafetyAndLoyalistRescueReserve()
    {
        var peach = Card(10, CardKind.Peach);
        var slash = Card(11, CardKind.Slash);
        var lord = Change(Scene(Role.Lord, peach, slash), 0, player => player with { Hp = 2 });
        var heal = new LegalAction(LegalActionKind.Peach, peach.Id, 0, "heal self");
        var attack = Slash(slash.Id, 2);
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(lord, [attack, heal, End], 1).Action == attack &&
            Brain().ChoosePlay(lord, [attack, heal, End], 1).Action == heal,
            "The Lord should recover dangerous public HP before an ordinary nonlethal attack.");

        var draw = Card(11, CardKind.DrawTwo);
        var loyal = Change(Change(Scene(Role.Loyalist, peach, draw), 0, player => player with { Hp = 2 }),
            1, player => player with { Hp = 1 });
        var replenish = new LegalAction(LegalActionKind.DrawTwo, draw.Id, 0, "draw cards");
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(loyal, [heal, replenish, End], 1).Action == heal &&
            Brain().ChoosePlay(loyal, [heal, replenish, End], 1).Action == replenish,
            "A still-living Loyalist should retain its only physical Peach for the one-HP Lord rather than spending it for nonurgent self-healing.");
        var criticalSelf = Change(loyal, 0, player => player with { Hp = 1 });
        Need(Brain().ChoosePlay(criticalSelf, [heal, replenish, End], 1).Action == heal,
            "Preserving a rescue must not stop the Loyalist from first saving its own last HP.");
        var highMaximum = Change(loyal, 0, player => player with { MaxHp = 8 });
        Need(Brain().ChoosePlay(highMaximum, [heal, End], 1).Action == End,
            "A larger printed self-heal value must not defeat the last-Peach reservation for an endangered Lord.");
        var sparePeach = Change(loyal, 0, player => player with
        {
            Hand = player.Hand.Append(Card(12, CardKind.Peach)).ToArray(), HandCount = 3
        });
        Need(Brain().ChoosePlay(sparePeach, [heal, replenish, End], 1).Action == heal,
            "Two physical Peaches leave another rescue available, so the reservation cannot become a blanket refusal to heal.");
    }

    public static void RenegadeBalancesPublicCampsAndTransitionsToFinalDuel()
    {
        var card = Card(10, CardKind.Slash);
        var view = Scene(Role.Renegade, card);
        view = Change(Change(Change(Change(view, 1, player => player with { Hp = 4, HandCount = 5 }),
            2, player => player with { Hp = 5, MaxHp = 5, HandCount = 5, Equipment = [Card(90, CardKind.QilinBow)] }),
            3, player => player with { Hp = 1 }), 4, player => player with { Hp = 1 });
        var loyalist = Slash(card.Id, 2);
        var rebel = Slash(card.Id, 3);
        var lord = Slash(card.Id, 1);
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(view, [loyalist, rebel, lord, End], 1).Action == rebel &&
            Brain().ChoosePlay(view, [loyalist, rebel, lord, End], 1).Action == loyalist,
            "With a healthy dominant Lord side and weakened Rebels, the Renegade should reduce the stronger side instead of handing it the table.");

        var endangered = Change(view, 1, player => player with { Hp = 1 });
        var protect = Brain().ChoosePlay(endangered, [loyalist, rebel, lord, End], 1);
        Need(protect.Action == rebel && Score(protect.Thought, lord) < 0,
            "The Renegade must suppress likely Rebels and preserve the endangered Lord until the final duel.");
        var finalDuel = view with
        {
            Players = view.Players.Select(player => player with { IsAlive = player.Seat is 0 or 1 }).ToArray()
        };
        Need(Brain().ChoosePlay(finalDuel, [lord, End], 1).Action == lord,
            "When only the Renegade and Lord remain, the previously protected Lord becomes the actual winning target.");
        var lastRebel = Change(view, 4, player => player with { IsAlive = false });
        Need(Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(lastRebel, [rebel, End], 1).Action == rebel &&
            Brain().ChoosePlay(lastRebel, [rebel, End], 1).Action == End,
            "If the only currently published attack would finish the publicly last Rebel and leave a dominant Lord-plus-Loyalist side, the Renegade must actually retain it rather than merely reduce a still-positive score.");
    }

    public static void RescueBeliefsProtectLordAndPreserveWeakSide()
    {
        var peach = Card(10, CardKind.Peach);
        var view = Scene(Role.Renegade, peach, Card(11, CardKind.Slash));
        view = Change(Change(Change(Change(view, 1, player => player with { Hp = 4, HandCount = 5 }),
            2, player => player with { Hp = 4, HandCount = 5 }),
            3, player => player with { Hp = 0 }), 4, player => player with { IsAlive = false });
        var before = SnapshotJson.Serialize(view);
        var actual = new Card(peach.Id, peach.Kind, peach.Suit, peach.Rank);
        Need(!Brain(AiDecisionPolicy.TacticalPlanningVersion).ChooseDyingResponse(view, 3, [actual], 1).UsePeach &&
            Brain().ChooseDyingResponse(view, 3, [actual], 1).UsePeach,
            "A safe Renegade can preserve the clearly weaker last Rebel instead of letting a dominant Lord-plus-Loyalist side win the balance immediately.");
        var lordAtRisk = Change(view, 1, player => player with { Hp = 1 });
        Need(!Brain().ChooseDyingResponse(lordAtRisk, 3, [actual], 1).UsePeach,
            "Camp balancing must not consume the Lord's protection when the Lord is endangered.");
        var lordDying = Change(view, 1, player => player with { Hp = 0 });
        Need(Brain().ChooseDyingResponse(lordDying, 1, [actual], 1).UsePeach,
            "Before the final duel, saving the Lord remains mandatory to the Renegade's own win condition.");
        var finalDuel = lordDying with
        {
            Players = lordDying.Players.Select(player => player with { IsAlive = player.Seat is 0 or 1 }).ToArray()
        };
        Need(!Brain().ChooseDyingResponse(finalDuel, 1, [actual], 1).UsePeach && SnapshotJson.Serialize(view) == before,
            "The final-duel victory must not be undone by rescue, and none of the choices may mutate the original player view.");
    }

    public static void IdentityObjectivesKeepPublicInputsAndOtherModesStable()
    {
        var card = Card(10, CardKind.Slash);
        var view = Scene(Role.Lord, card);
        view = view with { Players = view.Players.Select(player => player.Seat == 0
            ? player : player with { Role = null, IsRoleRevealed = false }).ToArray() };
        var first = Slash(card.Id, 2);
        var other = Slash(card.Id, 3);
        var actions = new[] { first, other, End };
        var before = SnapshotJson.Serialize(view);
        var brain = Brain();
        brain.ObserveSlash(2, 0, 0, Role.Lord);
        brain.ObserveSlash(2, 0, 0, Role.Lord);
        var calls = 0;
        var choice = brain.ChoosePlay(view, actions, 1, (action, target) =>
        {
            Need(actions.Contains(action) && action.TargetSeat == target,
                "Identity scoring may query current published attacks only, never a projected target or forged future action.");
            calls++;
            var victim = view.Players.Single(player => player.Seat == target);
            return PublicDamagePrediction.Calculate(victim.Hp, victim.Armor, 1, maxHp: victim.MaxHp);
        });
        Need(choice.Action == first && calls > 0 && before == SnapshotJson.Serialize(view) &&
            view.Players.Where(player => player.Seat != 0).All(player => player.Hand.Count == 0 && player.Role is null),
            "Only published hostile evidence should focus the Lord's defense; hidden identities and hand faces stay absent.");

        var team = Scene(Role.Rebel, card) with { ModeKind = ContentModeKind.Team };
        team = team with { Players = team.Players.Select(player => player with
            { TeamId = player.Seat == 0 ? "blue" : "red" }).ToArray() };
        var old = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(team, [first, other, End], 1);
        var current = Brain().ChoosePlay(team, [first, other, End], 1);
        Need(old.Thought.Candidates.SequenceEqual(current.Thought.Candidates),
            "Identity-only objectives must preserve the public-team scorer and its seeded tie breaking.");
        var friendly = Scene(Role.Rebel, card);
        var allyAttack = Slash(card.Id, 3);
        var oldAlly = Brain(AiDecisionPolicy.TacticalPlanningVersion).ChoosePlay(friendly, [allyAttack, End], 1);
        var newAlly = Brain().ChoosePlay(friendly, [allyAttack, End], 1);
        Need(newAlly.Action == End && Score(oldAlly.Thought, allyAttack) == Score(newAlly.Thought, allyAttack),
            "An already rejected attack on a known Rebel ally must retain its safety rejection.");
    }

    public static void IdentityNullificationAndRescueMatchCampObjectives()
    {
        var counter = Card(10, CardKind.Nullification);
        var peach = Card(11, CardKind.Peach);
        var view = Scene(Role.Renegade, counter, peach);
        view = Change(Change(Change(Change(view, 1, player => player with { Hp = 4, HandCount = 5 }),
            2, player => player with { Hp = 5, MaxHp = 5, HandCount = 5,
                Equipment = [Card(90, CardKind.QilinBow)] }),
            3, player => player with { Hp = 1 }), 4, player => player with { IsAlive = false });
        var before = SnapshotJson.Serialize(view);
        foreach (var kind in new[] { CardKind.Duel, CardKind.FireAttack,
                     CardKind.BarbarianAssault, CardKind.ArrowBarrage })
        {
            Need(Counter(view, kind, false, AiDecisionPolicy.TacticalPlanningVersion).CardId is null &&
                Counter(view, kind, false).CardId == counter.Id,
                "The current counterspell window should protect the one-HP last Rebel that the Renegade deliberately preserves; v6 must retain its old pass.");
            Need(Counter(view, kind, true, AiDecisionPolicy.TacticalPlanningVersion).CardId == counter.Id &&
                Counter(view, kind, true).CardId is null,
                "The reverse counterspell chain must not restore potential lethal damage to the same protected Rebel.");
        }
        Need(Counter(view, CardKind.PeachGarden, false, AiDecisionPolicy.TacticalPlanningVersion).CardId == counter.Id &&
            Counter(view, CardKind.PeachGarden, false).CardId is null &&
            Counter(view, CardKind.PeachGarden, true, AiDecisionPolicy.TacticalPlanningVersion).CardId is null &&
            Counter(view, CardKind.PeachGarden, true).CardId == counter.Id,
            "The same protected target's recovery should be allowed or restored, rather than being countered after preserving its life.");
        var full = Change(view, 3, player => player with { Hp = player.MaxHp });
        Need(SameCandidates(Counter(full, CardKind.PeachGarden, false).Thought,
                Counter(full, CardKind.PeachGarden, false, AiDecisionPolicy.TacticalPlanningVersion).Thought),
            "Peach Garden has zero recovery value for a full-HP protected target.");
        var endangered = Change(view, 1, player => player with { Hp = 1 });
        Need(Counter(endangered, CardKind.Duel, false).CardId is null &&
            Counter(endangered, CardKind.PeachGarden, false).CardId == counter.Id,
            "An endangered Lord ends the protection of the last Rebel: allow pressure and oppose the Rebel's recovery.");
        var three = Change(view, 2, player => player with { IsAlive = false });
        Need(Counter(three, CardKind.Duel, false).CardId is null,
            "The final Lord-Rebel-Renegade stage must still permit eliminating the Rebel and progressing to a duel.");
        var team = view with { ModeKind = ContentModeKind.Team,
            Players = view.Players.Select(player => player with
                { TeamId = player.Seat == 0 ? "blue" : "red" }).ToArray() };
        Need(SameCandidates(Counter(team, CardKind.Duel, false).Thought,
                Counter(team, CardKind.Duel, false, AiDecisionPolicy.TacticalPlanningVersion).Thought),
            "The identity counterspell adjustment must preserve public-team policies exactly.");

        var late = Scene(Role.Lord, peach, Card(12, CardKind.Slash));
        late = late with
        {
            Players = late.Players.Concat(Enumerable.Range(5, 3).Select(seat => late.Players[4] with
                    { Seat = seat, Name = "seat " + seat }))
                .Select(player => player.Seat == 0 ? player : player.Seat <= 3
                    ? player with { Role = null, IsRoleRevealed = false, Hp = player.Seat == 1 ? 1 : 4 }
                    : player with { Role = Role.Rebel, IsRoleRevealed = true, IsAlive = false }).ToArray(),
            IdentityComposition = new(1, 2, 4, 1)
        };
        var pressure = Slash(12, 1);
        Need(Brain().ChoosePlay(late, [pressure, End], 1).Action == pressure,
            "After all Rebels are publicly dead, equally plausible remaining Renegades must receive pressure rather than form permanent allies.");
        var dying = Change(late, 1, player => player with { Hp = 0 });
        Need(!Brain().ChooseDyingResponse(dying, 1,
                [new Card(peach.Id, peach.Kind, peach.Suit, peach.Rank)], 1).UsePeach &&
            SnapshotJson.Serialize(view) == before,
            "A positive-hostility endgame contender must not be rescued merely because its quota-conditioned Loyalist probability also exceeds .6, and scoring must not change the player view.");

        (int? CardId, AiThoughtRecord Thought) Counter(GameSnapshot snapshot, CardKind kind, bool nullified,
            int policy = AiDecisionPolicy.IdentityStrategyVersion) =>
            Brain(policy).ChooseNullification(snapshot, kind, 2, 3, nullified, nullified ? 1 : 0, [counter.Id], 1);
    }

    private static readonly LegalAction End = new(LegalActionKind.EndPlay, null, null, "end");
    private static SimpleAiBrain Brain(int policy = AiDecisionPolicy.IdentityStrategyVersion) => new(0, 227, policy);
    private static bool SameCandidates(AiThoughtRecord left, AiThoughtRecord right) =>
        System.Text.Json.JsonSerializer.Serialize(left.Candidates) ==
        System.Text.Json.JsonSerializer.Serialize(right.Candidates);
    private static double Score(AiThoughtRecord thought, LegalAction action) =>
        thought.Candidates.Single(candidate => candidate.Action == action).Score;
    private static CardSnapshot Card(int id, CardKind kind) =>
        new(id, kind, Suit.Spade, 7, CardCatalog.Get(kind).DisplayName, "7");
    private static LegalAction Slash(int card, int target) => new(LegalActionKind.Slash, card, target, "Slash seat " + target)
        { SlashTraits = new(false, false, DamageNature.Normal) };
    private static GameSnapshot Change(GameSnapshot view, int seat, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == seat ? change(player) : player).ToArray() };
    private static GameSnapshot Scene(Role role, params CardSnapshot[] cards)
    {
        var roles = role switch
        {
            Role.Lord => new[] { role, Role.Loyalist, Role.Rebel, Role.Rebel, Role.Renegade },
            Role.Loyalist => new[] { role, Role.Lord, Role.Rebel, Role.Rebel, Role.Renegade },
            Role.Rebel => new[] { role, Role.Lord, Role.Loyalist, Role.Rebel, Role.Renegade },
            _ => new[] { role, Role.Lord, Role.Loyalist, Role.Rebel, Role.Rebel }
        };
        return new(null, 0, EngineStatus.AwaitingHumanPlay, Winner.None, 1, 0, TurnPhase.Play, 80, 0,
            roles.Select((identity, seat) => new PlayerSnapshot(seat, "seat " + seat, seat == 0, identity, true,
                "fixture:identity-" + seat, "seat " + seat, "", 4, 4, true,
                seat == 0 ? cards.Length : 0, seat == 0 ? cards : [])
            { Skills = [], SecondarySkills = [], Equipment = [], Judgment = [] }).ToArray(), null)
            { ModeKind = ContentModeKind.Identity, IdentityComposition = new(1, 1, 2, 1) };
    }
    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
