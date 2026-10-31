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
