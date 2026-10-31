using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class IdentityBeliefAiChecks
{

    public static void NativeSnapshotsPublishOnlyPublicCompositionAndViewerRoles()
    {
        const string customMode = "fixture:identity-composition-six";
        var standard = StandardContentRegistry.CreateWithTeamModes();
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardTeamModePackage(),
            new CompositionModePackage(standard.Modes["identity:standard-8"] with
            {
                Id = customMode, Name = "Six-player public identity composition", MinPlayers = 6, MaxPlayers = 6,
                RoleCounts = new Dictionary<string, int>
                {
                    [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 2,
                    [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1
                }
            }));
        foreach (var modeId in new[] { "identity:standard-5", "identity:standard-8", customMode })
        {
            var mode = registry.Modes[modeId];
            var players = mode.RoleCounts.Values.Sum();
            var expected = new PublicIdentityComposition(mode.RoleCounts.GetValueOrDefault(nameof(Role.Lord)),
                mode.RoleCounts.GetValueOrDefault(nameof(Role.Loyalist)), mode.RoleCounts.GetValueOrDefault(nameof(Role.Rebel)),
                mode.RoleCounts.GetValueOrDefault(nameof(Role.Renegade)));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = 531, PlayerCount = players, ModeId = modeId, HumanSeat = 0, HumanRole = null,
                HumanSeats = Enumerable.Range(0, players).ToArray(), UseInteractiveSetup = false,
                AdvanceAfterHumanCommands = false, AiPolicyVersion = AiDecisionPolicy.IdentityStrategyVersion
            }, registry);
            var started = game.Submit(new StartGameCommand());
            Need(started.Accepted, started.Error?.ToString() ?? "Native identity opening was rejected.");
            var trusted = game.CreateSnapshot(-1, revealAll: true);
            var lordSeat = trusted.Players.Single(player => player.Role == Role.Lord).Seat;
            Need(trusted.Players.All(player => player.IsAlive) && trusted.IdentityComposition == expected,
                "The real opening must project the selected public mode's exact role composition, including custom table sizes.");
            foreach (var role in new[] { Role.Lord, Role.Loyalist, Role.Rebel, Role.Renegade })
                Need(trusted.Players.Count(player => player.Role == role) == mode.RoleCounts.GetValueOrDefault(role.ToString()),
                    "The public composition must match the actual native role allocation.");
            foreach (var viewer in Enumerable.Range(0, players))
            {
                var view = game.CreateSnapshot(viewer);
                Need(view.IdentityComposition == expected && view.Players.All(player =>
                    player.Role == (player.Seat == viewer || player.Seat == lordSeat
                        ? trusted.Players.Single(actual => actual.Seat == player.Seat).Role : null)),
                    "Every native viewer receives the same public quota while only their own role and the Lord are revealed.");
                var unknown = view.Players.Where(player => player.Role is null).ToArray();
                var brain = new SimpleAiBrain(viewer, 531, AiDecisionPolicy.IdentityStrategyVersion);
                foreach (var role in new[] { Role.Lord, Role.Loyalist, Role.Rebel, Role.Renegade })
                    Near(P(brain, view, unknown[0].Seat, role),
                        (mode.RoleCounts.GetValueOrDefault(role.ToString()) - view.Players.Count(player => player.Role == role)) /
                        (double)unknown.Length);
            }
            Need(game.CreateSnapshot(-1).Players.All(player => player.Seat == lordSeat ? player.Role == Role.Lord : player.Role is null),
                "An unprivileged observer must not acquire hidden identities from public composition metadata.");
        }

        var team = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 531, PlayerCount = 4, ModeId = "team:standard-2v2", HumanRole = null,
            HumanTeamId = "team:blue",
            AdvanceAfterHumanCommands = false, AiPolicyVersion = AiDecisionPolicy.IdentityStrategyVersion
        }, registry);
        var teamStarted = team.Submit(new StartGameCommand());
        Need(teamStarted.Accepted, teamStarted.Error?.ToString() ?? "Native public-team opening was rejected.");
        Need(Enumerable.Range(0, 4).All(viewer => team.CreateSnapshot(viewer).IdentityComposition is null) &&
             team.CreateSnapshot(-1, revealAll: true).IdentityComposition is null,
            "Public-team snapshots must not advertise identity quotas, even in trusted diagnostics.");
    }

    public static void DetachedParticipantBeliefsPreserveHistoryWithoutSharingObservations()
    {
        var opening = View(8, Role.Lord);
        var original = Brain();
        original.ObserveSlash(1, 4, 0, null);
        original.ObserveSlash(2, 0, 0, Role.Lord);
        var revealed = Change(opening, 4, player => player with { Role = Role.Rebel, IsAlive = false });
        var originalValue = P(original, revealed, 1, Role.Rebel);
        var detached = original.ForkParticipantDecision();
        var control = original.ForkParticipantDecision();
        Near(P(detached, revealed, 1, Role.Rebel), originalValue);
        Near(P(control, revealed, 1, Role.Rebel), originalValue);
        Need(originalValue < P(Brain(), revealed, 1, Role.Rebel),
            "The fork comparison must include retrospective evidence, not only an empty observer history.");

        detached.ObserveSlash(1, 3, 0, null);
        var later = Change(revealed, 3, player => player with { Role = Role.Loyalist, IsAlive = false });
        Need(P(detached, later, 1, Role.Rebel) > P(original, later, 1, Role.Rebel),
            "A detached observer's later unresolved attack is visible only in its own subsequent inference.");
        Near(P(original, later, 1, Role.Rebel), P(control, later, 1, Role.Rebel));

        var final = Change(later, 5, player => player with { Role = Role.Rebel, IsAlive = false });
        var detachedBefore = P(detached, final, 1, Role.Rebel);
        original.ObserveSlash(1, 5, 0, null);
        Need(P(original, final, 1, Role.Rebel) < P(control, final, 1, Role.Rebel),
            "The original observer must independently retain new retrospective evidence after forking.");
        Near(P(detached, final with { Revision = final.Revision + 1 }, 1, Role.Rebel), detachedBefore);
    }

    public static void PublicCompositionAndDirectedEvidenceConditionJointBeliefs()
    {
        var view = View(8, Role.Lord);
        var brain = Brain();
        var original = SnapshotJson.Serialize(view);
        Near(P(brain, view, 1, Role.Rebel), 4d / 7);
        Near(P(brain, view, 1, Role.Loyalist), 2d / 7);
        Near(P(brain, view, 1, Role.Renegade), 1d / 7);
        Near(P(brain, view, 1, Role.Lord), 0d);
        Near(P(brain, view, 0, Role.Lord), 1d);

        brain.ObserveGroupAttack(1, 0, 0, Role.Lord);
        for (var target = 2; target < 8; target++) brain.ObserveGroupAttack(1, target, 0, null);
        Near(P(brain, view, 1, Role.Rebel), 4d / 7);
        Need(brain.RebelSuspicion.GetValueOrDefault(1) == 0,
            "An AOE's mandatory Lord recipient must not establish directed Rebel intent, even across its whole target list.");

        brain.ObserveSlash(2, 0, 0, Role.Lord);
        Need(P(brain, view, 2, Role.Rebel) > .85 && P(brain, view, 2, Role.Rebel) < .99 &&
             P(brain, view, 1, Role.Loyalist) > 2d / 7,
            "A directed attack on the public Lord is strong evidence but not hidden-role certainty; quotas redistribute the remaining camp mass.");
        Near(view.Players.Sum(player => P(brain, view, player.Seat, Role.Rebel)), 4d);
        Near(view.Players.Sum(player => P(brain, view, player.Seat, Role.Loyalist)), 2d);
        Near(view.Players.Sum(player => P(brain, view, player.Seat, Role.Renegade)), 1d);
        var first = P(brain, view, 2, Role.Rebel);
        Near(P(brain, view, 2, Role.Rebel), first);
        Need(SnapshotJson.Serialize(view) == original,
            "Joint belief calculation and cached repeats must leave the original viewer snapshot unchanged.");

        var historical = Brain(AiDecisionPolicy.TacticalPlanningVersion);
        historical.ObserveGroupAttack(1, 0, 0, Role.Lord);
        Need(historical.RebelSuspicion.GetValueOrDefault(1) == 3,
            "Policy six retains its recorded per-target AOE evidence rather than silently adopting the identity-policy change.");
    }

    public static void PublicRoleRevelationRevisesEvidenceWithoutInventingEndgameRoles()
    {
        var opening = View(5, Role.Lord);
        var observed = Brain();
        observed.ObserveSlash(1, 4, 0, null);
        var revealed = Change(opening, 4, player => player with { Role = Role.Rebel, IsAlive = false });
        var control = Brain();
        Need(P(observed, revealed, 1, Role.Rebel) < P(control, revealed, 1, Role.Rebel) &&
             P(observed, revealed, 1, Role.Loyalist) > P(control, revealed, 1, Role.Loyalist),
            "A now-public Rebel death can clarify earlier public attacks without having supplied the role before it became visible.");

        var late = Change(revealed, 3, player => player with { Role = Role.Rebel, IsAlive = false });
        var neutral = Brain();
        Near(P(neutral, late, 1, Role.Renegade), .5d);
        Near(P(neutral, late, 2, Role.Renegade), .5d);
        Near(Hostility(neutral, late, 1), Hostility(neutral, late, 2));
        Need(Hostility(neutral, late, 1) > 0,
            "When Rebels are gone, tied Loyalist/Renegade mixtures remain unresolved and actionable; neither seat becomes an arbitrary certain enemy or a permanent ally.");
        neutral.ObserveSlash(1, 0, 0, Role.Lord);
        Need(P(neutral, late, 1, Role.Renegade) > P(neutral, late, 2, Role.Renegade) &&
             P(neutral, late, 1, Role.Renegade) < 1d && Hostility(neutral, late, 2) < -15d,
            "New public evidence may focus strategic pressure while preserving the less likely remaining ally and fractional role inference.");
        var last = Change(late, 2, player => player with { Role = Role.Loyalist, IsAlive = false });
        Near(P(neutral, last, 1, Role.Renegade), 1d);
        Near(P(neutral, last, 2, Role.Loyalist), 1d);
        Need(Hostility(neutral, last, 2) == -100d,
            "Only publicly revealed composition can force the last hidden role; a genuinely known Loyalist stays protected.");
    }

    public static void EvidenceCanReverseAndMissingCompositionRemainsUncertain()
    {
        var view = Change(View(8, Role.Lord), 7, player => player with { Role = Role.Rebel, IsAlive = false });
        var brain = Brain();
        for (var index = 0; index < 6; index++) brain.ObserveSlash(1, 0, 0, Role.Lord);
        var hostile = P(brain, view, 1, Role.Rebel);
        for (var index = 0; index < 12; index++) brain.ObserveDuel(1, 7, 0, Role.Rebel);
        var revised = P(brain, view, 1, Role.Rebel);
        Need(hostile > .9d && revised < .15d && revised < hostile &&
             brain.RebelSuspicion.Values.All(value => value is >= -6d and <= 6d),
            "Contradictory observed behaviour must reverse an old accusation, including on the same cached immutable view, rather than create a permanent identity label.");

        var absent = view with { IdentityComposition = null };
        var malformed = view with { IdentityComposition = new(1, -1, int.MaxValue, 1) };
        foreach (var current in new[] { absent, malformed })
        {
            var total = new[] { Role.Lord, Role.Loyalist, Role.Rebel, Role.Renegade }
                .Sum(role => P(brain, current, 2, role));
            Near(total, 1d);
            Need(P(brain, current, 2, Role.Rebel) is > 0d and < 1d &&
                 P(brain, current, 2, Role.Loyalist) is > 0d and < 1d,
                "Older views without valid public composition use finite uncertain priors instead of failing or inventing role certainty.");
        }
        var unknown = view.Players[2];
        var changedFaces = Change(view, 2, player => player with
        {
            Name = "different label", GeneralId = "fixture:different-visible-general",
            Hand = [new(901, CardKind.Peach, Suit.Heart, 2, "untrusted foreign face", "2")]
        });
        foreach (var role in new[] { Role.Loyalist, Role.Rebel, Role.Renegade })
            Near(P(brain, view, unknown.Seat, role), P(brain, changedFaces, unknown.Seat, role));
    }

    private static SimpleAiBrain Brain(int policy = AiDecisionPolicy.IdentityStrategyVersion) => new(0, 531, policy);

    private static GameSnapshot View(int players, Role ownRole)
    {
        var composition = players == 5 ? new PublicIdentityComposition(1, 1, 2, 1) : new PublicIdentityComposition(1, 2, 4, 1);
        return new(null, 0, EngineStatus.AwaitingHumanPlay, Winner.None, 1, 0, TurnPhase.Play, 80, 0,
            Enumerable.Range(0, players).Select(seat => new PlayerSnapshot(seat, $"seat {seat}", seat == 0,
                seat == 0 ? ownRole : null, seat == 0 && ownRole == Role.Lord,
                $"fixture:general-{seat}", "general", "", 4, 4, true, 4, [])).ToArray(), null)
            { ModeKind = ContentModeKind.Identity, IdentityComposition = composition };
    }

    private static GameSnapshot Change(GameSnapshot view, int seat, Func<PlayerSnapshot, PlayerSnapshot> change) =>
        view with { Players = view.Players.Select(player => player.Seat == seat ? change(player) : player).ToArray() };

    private static double P(SimpleAiBrain brain, GameSnapshot view, int seat, Role role) =>
        Invoke(brain, "GetIdentityRoleProbability", view, view.Players.Single(player => player.Seat == seat), role);

    private static double Hostility(SimpleAiBrain brain, GameSnapshot view, int seat) =>
        Invoke(brain, "GetIdentityHostility", view, view.Players.Single(player => player.Seat == 0).Role!.Value,
            view.Players.Single(player => player.Seat == seat));

    private static double Invoke(SimpleAiBrain brain, string name, params object[] arguments)
    {
        var method = typeof(SimpleAiBrain).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic) ??
            throw new InvalidOperationException("Missing identity policy query " + name);
        return (double)method.Invoke(brain, arguments)!;
    }

    private static void Near(double actual, double expected)
    {
        Need(double.IsFinite(actual) && Math.Abs(actual - expected) < 1e-9,
            $"Expected probability/utility {expected:R}, actual {actual:R}.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class CompositionModePackage(ContentModeDefinition mode) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:identity-composition", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder) => builder.AddMode(mode);
    }
}
