using CardGame.Core;

internal static class RebelStrategyAiChecks
{
    public static void CoordinationWaitsForEvidenceWithoutSuppressingLordOrKnownEnemies()
    {
        var slash = Card(10, CardKind.Slash);
        var view = Scene(slash);
        view = view with { Players = view.Players.Select(player => player.Seat is 0 or 1
            ? player : player with { Role = null }).ToArray() };
        var blind = Slash(slash.Id, 2);
        var old = Brain(AiDecisionPolicy.IdentityStrategyVersion);
        var current = Brain(AiDecisionPolicy.RebelCoordinationVersion);
        Need(old.ChoosePlay(view, [blind, End], 1, Predict(view)).Action == blind &&
            current.ChoosePlay(view, [blind, End], 1, Predict(view)).Action == End,
            "With three other Rebels among six unknowns, an unobserved neighbour must not receive a blind positive damage floor.");
        var lord = Slash(slash.Id, 1);
        Need(current.ChoosePlay(view, [blind, lord, End], 2, Predict(view)).Action == lord,
            "The published Lord remains an immediate winning target while unknown-neighbour damage is reserved.");
        current.ObserveSlash(2, 0, 1, Role.Rebel);
        Need(current.ChoosePlay(view, [blind, End], 3, Predict(view)).Action == blind,
            "A real directed attack on this publicly known-to-self Rebel supplies enough contrary evidence to resume pressure.");
        var five = view with { Players = view.Players.Take(5).ToArray(), IdentityComposition = new(1, 1, 2, 1) };
        Need(Same(Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(five, [blind, End], 1, Predict(five)).Thought,
            Brain(AiDecisionPolicy.RebelCoordinationVersion).ChoosePlay(five, [blind, End], 1, Predict(five)).Thought),
            "A five-player opening with only one Rebel among three unknowns keeps the original identity policy's useful pressure.");
    }

    public static void PressureDiscountsOnlyNewObjectiveAndKeepsRealDamageSafety()
    {
        var slash = Card(10, CardKind.Slash);
        var draw = Card(11, CardKind.DrawTwo);
        var view = Change(Scene(slash, draw), 1, player => player with { HandCount = 8 });
        var hit = Slash(slash.Id, 1);
        var actions = new[] { hit, new LegalAction(LegalActionKind.DrawTwo, draw.Id, 0, "draw"), End };
        var old = Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(view, actions, 1, Predict(view)).Thought;
        var current = Brain(AiDecisionPolicy.RebelPressureVersion).ChoosePlay(view, actions, 1, Predict(view));
        Need(Score(current.Thought, hit) >= Score(old, hit),
            "Unknown responses discount only the new winning objective, never the already priced attack and material value.");
        var immune = Brain(AiDecisionPolicy.RebelPressureVersion).ChoosePlay(view, actions, 1,
            (_, seat) => PublicDamagePrediction.Calculate(view.Players[seat].Hp, 0, 0, maxHp: 4));
        Need(immune.Action != hit && Score(immune.Thought, hit) <= 0,
            "Stronger winning-objective pressure still cannot revive a publicly ineffective attack.");
        var blocked = Change(view, 1, player => player with { Equipment = [Card(90, CardKind.RenwangShield)] });
        Need(Brain(AiDecisionPolicy.RebelPressureVersion).ChoosePlay(blocked, [hit, End], 1, Predict(blocked)).Action == End,
            "The new pressure policy preserves the real armor rejection before target objectives.");
        var groupView = Change(Change(Scene(Card(20, CardKind.ArrowBarrage)), 4,
            player => player with { Hp = 1, HandCount = 0 }), 1, player => player with { HandCount = 0 });
        groupView = groupView with { Players = groupView.Players.Select(player => player.Seat is 5 or 6
            ? player with { IsAlive = false, Hp = 0 } : player).ToArray() };
        var group = new LegalAction(LegalActionKind.ArrowBarrage, 20, null, "arrows");
        Need(Brain(AiDecisionPolicy.RebelPressureVersion).ChoosePlay(groupView, [group, End], 1, Predict(groupView)).Action == End,
            "The pressure policy protects an exposed one-HP known ally when the Lord would survive.");
        groupView = Change(groupView, 1, player => player with { Hp = 1 });
        Need(Brain(AiDecisionPolicy.RebelPressureVersion).ChoosePlay(groupView, [group, End], 1, Predict(groupView)).Action == group,
            "An actual public Lord-finishing opportunity retains priority over the incidental allied loss.");
    }

    public static void LordPressurePricesEffectiveDamageAndExposure()
    {
        var slash = Card(10, CardKind.Slash);
        var draw = Card(11, CardKind.DrawTwo);
        var view = Scene(slash, draw);
        var hit = Slash(slash.Id, 1);
        var replenish = new LegalAction(LegalActionKind.DrawTwo, draw.Id, 0, "draw two");
        var actions = new[] { hit, replenish, End };
        PublicDamagePrediction Zero(LegalAction action, int seat) =>
            PublicDamagePrediction.Calculate(view.Players[seat].Hp, 0, 0, maxHp: view.Players[seat].MaxHp);
        Need(Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(view, actions, 1, Zero).Action == hit &&
             Brain().ChoosePlay(view, actions, 1, Zero).Action == replenish,
            "A public zero-effect attack must stop consuming Rebel resources; the original identity policy retains its fixed Lord priority.");
        var armored = Change(view, 1, player => player with { Armor = 3 });
        Need(Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(armored, actions, 1, Predict(armored)).Action == hit &&
             Brain().ChoosePlay(armored, actions, 1, Predict(armored)).Action == replenish,
            "One point absorbed only by Armor is bounded stripping value, so replenishing should precede spending every available attack.");
        var exposed = Change(view, 1, player => player with { HandCount = 0 });
        Need(Brain().ChoosePlay(exposed, actions, 1, Predict(exposed)).Action == hit,
            "A genuinely exposed Lord still receives effective winning-objective pressure.");
        var defended = Change(view, 1, player => player with
            { HandCount = 20, Equipment = [Card(91, CardKind.BaguaFormation)] });
        Need(Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(defended, actions, 1, Predict(defended)).Action == hit &&
             Brain().ChoosePlay(defended, actions, 1, Predict(defended)).Action == replenish,
            "Public Bagua plus a large hand reduces immediate exposure; it must not be treated as guaranteed HP damage merely because this player is the Lord.");
    }

    public static void GroupPressureProtectsAlliesUnlessItCanFinishLord()
    {
        var barrage = Card(10, CardKind.ArrowBarrage);
        var view = Scene(barrage);
        view = Change(Change(view, 1, player => player with { HandCount = 0 }), 4,
            player => player with { Hp = 1, HandCount = 6 });
        view = view with { Players = view.Players.Select(player => player.Seat is 5 or 6
            ? player with { IsAlive = false, Hp = 0 } : player).ToArray() };
        var group = new LegalAction(LegalActionKind.ArrowBarrage, barrage.Id, null, "arrows");
        Need(Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(view, [group, End], 1, Predict(view)).Action == group &&
             Brain().ChoosePlay(view, [group, End], 1, Predict(view)).Action == End,
            "Positive total group pressure must not override a publicly lethal hit on a confident Rebel ally while the Lord survives.");
        var winning = Change(view, 1, player => player with { Hp = 1 });
        Need(Brain().ChoosePlay(winning, [group, End], 1, Predict(winning)).Action == group,
            "An actual public opportunity to finish the Lord takes precedence over incidental allied losses; the rule must not prohibit every group attack near a low-HP ally.");
    }

    public static void SideFocusAndPreparationKeepNativeSafetyRejections()
    {
        var slash = Card(10, CardKind.Slash);
        var remove = Card(11, CardKind.Dismantlement);
        var view = Change(Change(Scene(slash, remove), 2, player => player with { HandCount = 5 }),
            3, player => player with { HandCount = 0 });
        var defended = Slash(slash.Id, 2);
        var exposed = Slash(slash.Id, 3);
        Need(Brain().ChoosePlay(view, [defended, exposed, End], 1, Predict(view)).Action == exposed,
            "Without a published Lord attack, concentrate on the publicly exposed opposing defender rather than spreading attacks across equally healthy targets.");
        var armor = Card(90, CardKind.RenwangShield);
        var blocked = Change(view, 1, player => player with { Equipment = [armor] });
        var hitLord = Slash(slash.Id, 1);
        var dismantle = new LegalAction(LegalActionKind.Dismantlement, remove.Id, 1, "remove Lord defense", TargetCardId: armor.Id);
        Need(Brain().ChoosePlay(blocked, [hitLord, dismantle, End], 1, Predict(blocked)).Action == dismantle,
            "Reuse the existing independent-material armor preparation rather than reviving the blocked black Slash.");
        var noPreparation = Brain().ChoosePlay(blocked, [hitLord, End], 1, Predict(blocked));
        Need(noPreparation.Action == End && Score(noPreparation.Thought, hitLord) < 0,
            "The new role objective must leave an already rejected black Slash negative.");
        var ally = Slash(slash.Id, 4);
        var oldAlly = Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(view, [ally, End], 1);
        var newAlly = Brain().ChoosePlay(view, [ally, End], 1);
        Need(newAlly.Action == End && Same(oldAlly.Thought, newAlly.Thought),
            "A real Rebel ally keeps its exact prior safety rejection, including the candidate reason.");
        var team = view with { ModeKind = ContentModeKind.Team, Players = view.Players.Select(player => player with
            { TeamId = player.Seat == 0 ? "blue" : "red" }).ToArray() };
        Need(Same(Brain(AiDecisionPolicy.IdentityStrategyVersion).ChoosePlay(team, [defended, exposed, End], 1).Thought,
                Brain().ChoosePlay(team, [defended, exposed, End], 1).Thought),
            "The new Rebel policy must preserve public-team candidates exactly.");
    }

    public static void RescueReserveUsesConfidenceWithoutAbandoningKnownAllies()
    {
        var peach = Card(10, CardKind.Peach);
        var view = Scene(peach);
        view = view with { Players = view.Players.Select(player => player.Seat is 0 or 1 or 4 ? player : player with
            { Role = null, IsRoleRevealed = false }).ToArray() };
        view = Change(Change(view, 0, player => player with { Hp = 1 }), 2, player => player with { Hp = 0 });
        var old = Brain(AiDecisionPolicy.IdentityStrategyVersion);
        var current = Brain();
        old.ObserveSlash(2, 1, 1, Role.Lord);
        current.ObserveSlash(2, 1, 1, Role.Lord);
        old.ObserveDuel(2, 4, 1, Role.Rebel);
        current.ObserveDuel(2, 4, 1, Role.Rebel);
        var physical = new Card(peach.Id, peach.Kind, peach.Suit, peach.Rank);
        Need(old.ChooseDyingResponse(view, 2, [physical], 1).UsePeach &&
             !current.ChooseDyingResponse(view, 2, [physical], 1).UsePeach,
            "At one HP the Rebel reserves its last physical Peach when the recipient is probable rather than publicly certain.");
        var known = Change(view, 2, player => player with { Role = Role.Rebel, IsRoleRevealed = true });
        Need(Brain().ChooseDyingResponse(known, 2, [physical], 1).UsePeach,
            "The reserve must not become a blanket refusal to save a publicly known camp ally.");
        var safe = Change(view, 0, player => player with { Hp = 3 });
        Need(current.ChooseDyingResponse(safe, 2, [physical], 2).UsePeach,
            "The same public evidence is sufficient to help the probable ally when the rescuer is safe.");
    }

    private static readonly LegalAction End = new(LegalActionKind.EndPlay, null, null, "end");
    private static SimpleAiBrain Brain(int policy = AiDecisionPolicy.RebelStrategyVersion) => new(0, 531, policy);
    private static CardSnapshot Card(int id, CardKind kind) => new(id, kind, Suit.Spade, 7, kind.ToString(), "7");
    private static LegalAction Slash(int card, int target) => new(LegalActionKind.Slash, card, target, "Slash " + target)
        { SlashTraits = new(false, false, DamageNature.Normal) };
    private static GameSnapshot Change(GameSnapshot view, int seat, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == seat ? change(player) : player).ToArray() };
    private static Func<LegalAction, int, PublicDamagePrediction> Predict(GameSnapshot view) => (_, seat) =>
        PublicDamagePrediction.Calculate(view.Players[seat].Hp, view.Players[seat].Armor, 1, maxHp: view.Players[seat].MaxHp);
    private static double Score(AiThoughtRecord thought, LegalAction action) => thought.Candidates.Single(candidate => candidate.Action == action).Score;
    private static bool Same(AiThoughtRecord left, AiThoughtRecord right) =>
        System.Text.Json.JsonSerializer.Serialize(left.Candidates) == System.Text.Json.JsonSerializer.Serialize(right.Candidates);
    private static GameSnapshot Scene(params CardSnapshot[] hand)
    {
        var roles = new[] { Role.Rebel, Role.Lord, Role.Loyalist, Role.Loyalist, Role.Rebel, Role.Rebel, Role.Rebel, Role.Renegade };
        return new(null, 0, EngineStatus.AwaitingHumanPlay, Winner.None, 1, 0, TurnPhase.Play, 80, 0,
            roles.Select((role, seat) => new PlayerSnapshot(seat, "seat " + seat, seat == 0, role, true,
                "fixture:rebel-" + seat, "general", "", 4, 4, true, seat == 0 ? hand.Length : 0,
                seat == 0 ? hand : [])).ToArray(), null)
        { ModeKind = ContentModeKind.Identity, IdentityComposition = new(1, 2, 4, 1) };
    }
    private static void Need(bool condition, string why)
    {
        if (!condition) throw new InvalidOperationException(why);
    }
}
