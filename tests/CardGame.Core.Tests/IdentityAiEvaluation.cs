using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

/// <summary>
/// Identity-only policy experiment. One all-baseline control is reused by the
/// three camp-specific comparisons at each fixed seed and table size. Only the
/// focal camp changes policy; its opponents remain baseline in both games.
/// Trusted roles are used for assignment/audit, never as decision inputs.
/// </summary>
internal static class IdentityAiEvaluation
{
    private static readonly string[] Camps = ["LordAndLoyalists", "Rebels", "Renegade"];
    private static readonly FieldInfo BrainsField = global::FixtureRuntimeAccess.Engine.GetField(
        "_aiBrains", BindingFlags.Instance | BindingFlags.NonPublic) ??
        throw new InvalidOperationException("Identity evaluation cannot find the engine AI map.");
    private static readonly FieldInfo GeneratedCardsField = global::FixtureRuntimeAccess.Engine.GetField(
        "_generatedPhysicalCardIds", BindingFlags.Instance | BindingFlags.NonPublic) ??
        throw new InvalidOperationException("Identity evaluation cannot find native physical-card generation provenance.");
    private const int MaximumTurns = 240;

    public static int Run(string output, int seeds = 12,
        int baselinePolicy = AiDecisionPolicy.TacticalPlanningVersion,
        int? candidatePolicy = null, int firstSeed = 0, string profile = "standard",
        int? players = null, string? focalCamp = null)
    {
        var candidate = candidatePolicy ?? AiDecisionPolicy.CurrentVersion;
        if (seeds is < 1 or > 240)
            throw new ArgumentOutOfRangeException(nameof(seeds), "Identity evaluation accepts 1 to 240 seeds per table size.");
        if (firstSeed is < 0 or > 10000)
            throw new ArgumentOutOfRangeException(nameof(firstSeed), "The first seed index must be between 0 and 10000.");
        if (!AiDecisionPolicy.IsSupported(baselinePolicy) || !AiDecisionPolicy.IsSupported(candidate))
            throw new ArgumentException("Both identity policies must be supported; equal policies run the A/A control.");
        if (players is not (null or 5 or 8))
            throw new ArgumentOutOfRangeException(nameof(players), "Select 5, 8, or null for both table sizes.");
        var tableSizes = players is { } selectedPlayers ? new[] { selectedPlayers } : new[] { 5, 8 };
        string[] selectedCamps = focalCamp is null || focalCamp.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? Camps : focalCamp.Equals("control", StringComparison.OrdinalIgnoreCase) ? []
            : Camps.Where(camp => camp.Equals(focalCamp, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (selectedCamps.Length == 0 && !string.Equals(focalCamp, "control", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Select all, control, LordAndLoyalists, Rebels, or Renegade.", nameof(focalCamp));
        profile = profile.ToLowerInvariant();
        var registry = profile switch
        {
            "standard" or "active" => StandardContentRegistry.CreateWithActiveSkillsAndTeamModes(),
            "classic" => StandardContentRegistry.CreateWithClassicGenerals(),
            _ => throw new ArgumentException("Identity evaluation profile must be standard, active or classic.", nameof(profile))
        };
        var matches = new List<MatchResult>(seeds * 8);
        var comparisons = new List<Comparison>(seeds * 6);
        var destination = Path.GetFullPath(output);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        using var journal = new EvaluationJournal(destination + ".journal.jsonl");
        journal.Write(new { Kind = "BatchStarted", Utc = DateTimeOffset.UtcNow, Profile = profile,
            BaselinePolicy = baselinePolicy, CandidatePolicy = candidate, HostPolicy = baselinePolicy,
            FirstSeedIndex = firstSeed, SeedsPerTableSize = seeds, TableSizes = tableSizes, FocalCamps = selectedCamps,
            PlannedGameSlots = seeds * tableSizes.Length * (1 + selectedCamps.Length),
            PlannedComparisonSlots = seeds * tableSizes.Length * selectedCamps.Length, registry.ContentHash,
            Limit = "Trusted local diagnostics only; partial or interrupted slots are not completed matches or draws. " +
                "Progress samples are approximate uncommitted scalars, never decision inputs or replay snapshots." });
        var batchWatch = Stopwatch.StartNew();
        foreach (var count in tableSizes)
        for (var offset = 0; offset < seeds; offset++)
        {
            var index = firstSeed + offset;
            var seed = checked(1_327_111 + index * 7919 + count * 101);
            var mode = ModeId(profile, count);
            var control = Play(registry, profile, mode, index, seed, count, baselinePolicy, candidate, null, journal);
            matches.Add(control);
            foreach (var camp in selectedCamps)
            {
                var upgraded = Play(registry, profile, mode, index, seed, count, baselinePolicy, candidate, camp, journal);
                matches.Add(upgraded);
                var sameSetup = control.InitialStateHash is not null &&
                    control.InitialStateHash == upgraded.InitialStateHash;
                bool? aaSame = candidate == baselinePolicy && control.Success && upgraded.Success
                    ? control.FinalStateHash == upgraded.FinalStateHash && control.DecisionHash == upgraded.DecisionHash
                    : null;
                var complete = control.Success && upgraded.Success && sameSetup && (aaSame is not false);
                comparisons.Add(new Comparison(profile, mode, count, index, seed, camp,
                    complete, sameSetup, aaSame, control.Success, upgraded.Success,
                    control.Winner, upgraded.Winner,
                    complete ? Points(control.Winner, camp) : null,
                    complete ? Points(upgraded.Winner, camp) : null,
                    complete ? Points(upgraded.Winner, camp) - Points(control.Winner, camp) : null,
                    control.Diagnostics, upgraded.Diagnostics));
                journal.Write(new { Kind = "ComparisonCompleted", Utc = DateTimeOffset.UtcNow, Comparison = comparisons[^1] });
            }
            var local = matches.TakeLast(1 + selectedCamps.Length).ToArray();
            Console.WriteLine($"Identity {count} seats {offset + 1}/{seeds} (index {index}, seed {seed}): " +
                string.Join(", ", local.Select(match => $"{match.FocalCamp ?? "control"}={match.Winner}" +
                    (match.Success ? "" : " [FAILED]"))));
        }
        batchWatch.Stop();

        var strata = comparisons.GroupBy(pair => (pair.Players, pair.Camp)).Select(group =>
        {
            var complete = group.Where(pair => pair.Complete).ToArray();
            var deltas = complete.Select(pair => pair.PointDelta!.Value).ToArray();
            var mean = deltas.Length == 0 ? (double?)null : deltas.Average();
            var standardError = deltas.Length < 2 ? (double?)null :
                Math.Sqrt(deltas.Sum(value => Math.Pow(value - mean!.Value, 2)) / (deltas.Length * (deltas.Length - 1d)));
            return new
            {
                Players = group.Key.Players,
                Camp = group.Key.Camp,
                AttemptedPairs = group.Count(),
                CompletePairs = complete.Length,
                ControlFailures = group.Count(pair => !pair.ControlSuccess),
                CandidateFailures = group.Count(pair => !pair.CandidateSuccess),
                InitialStateMismatches = group.Count(pair => !pair.SameInitialState),
                AAMismatches = group.Count(pair => pair.AAInvariant == false),
                BaselineWins = complete.Count(pair => pair.ControlWinner == group.Key.Camp),
                CandidateWins = complete.Count(pair => pair.CandidateWinner == group.Key.Camp),
                BaselineDraws = complete.Count(pair => pair.ControlWinner == nameof(Winner.Draw)),
                CandidateDraws = complete.Count(pair => pair.CandidateWinner == nameof(Winner.Draw)),
                ImprovedPairs = complete.Count(pair => pair.PointDelta > 0),
                RegressedPairs = complete.Count(pair => pair.PointDelta < 0),
                TiedPairs = complete.Count(pair => pair.PointDelta == 0),
                BaselineWinRate = complete.Length == 0 ? (double?)null :
                    Math.Round(complete.Count(pair => pair.ControlWinner == group.Key.Camp) / (double)complete.Length, 4),
                CandidateWinRate = complete.Length == 0 ? (double?)null :
                    Math.Round(complete.Count(pair => pair.CandidateWinner == group.Key.Camp) / (double)complete.Length, 4),
                MeanPairedPointDelta = mean is null ? (double?)null : Math.Round(mean.Value, 4),
                ApproximatePaired95PercentInterval = standardError is null ? null : new[]
                {
                    Math.Round(Math.Max(-1d, mean!.Value - 1.96d * standardError.Value), 4),
                    Math.Round(Math.Min(1d, mean.Value + 1.96d * standardError.Value), 4)
                },
                BaselineDiagnostics = SumDiagnostics(complete.Select(pair => pair.ControlDiagnostics)),
                CandidateDiagnostics = SumDiagnostics(complete.Select(pair => pair.CandidateDiagnostics))
            };
        }).ToArray();
        var summary = new
        {
            BaselinePolicy = baselinePolicy,
            CandidatePolicy = candidate,
            HostPolicy = baselinePolicy,
            Profile = profile,
            AAControl = candidate == baselinePolicy,
            SeedsPerTableSize = seeds,
            FirstSeedIndex = firstSeed,
            LastSeedIndexInclusive = firstSeed + seeds - 1,
            TableSizes = tableSizes,
            FocalCamps = selectedCamps,
            PlannedGameSlots = seeds * tableSizes.Length * (1 + selectedCamps.Length),
            PlannedComparisonSlots = seeds * tableSizes.Length * selectedCamps.Length,
            Journal = journal.Path,
            Setups = seeds * tableSizes.Length,
            Games = matches.Count,
            SuccessfulGames = matches.Count(match => match.Success),
            FailedGames = matches.Count(match => !match.Success),
            CompleteComparisons = comparisons.Count(pair => pair.Complete),
            InitialStateMismatches = comparisons.Count(pair => !pair.SameInitialState),
            AAMismatches = comparisons.Count(pair => pair.AAInvariant == false),
            TotalMilliseconds = Math.Round(batchWatch.Elapsed.TotalMilliseconds, 2),
            MeanGameMilliseconds = Math.Round(matches.Average(match => match.Milliseconds), 2),
            MaximumGameMilliseconds = Math.Round(matches.Max(match => match.Milliseconds), 2),
            MaximumObservedTurns = matches.Max(match => match.Turns),
            Strata = strata,
            Limit = "Each camp is compared with the same baseline opposition, not with a changed opposing coalition. " +
                "The shared control is reused by three camp comparisons; those comparisons are correlated. " +
                "Do not pool identity camps or table sizes into a single zero-sum win rate. " +
                "The paired interval is a normal approximation and weak for small samples; profile-specific roster-pool results do not prove strength for every general. " +
                "Only seat-specific SimpleAiBrain policy changes: shared fixed skill heuristics and host-policy gates remain baseline. " +
                "A/A hashes cover trusted snapshots, physical-card locations/order and recorded selected decisions, not every internal automatic branch or RNG state. " +
                "Mixed-policy injection exists only in this test fixture, so these games are not replayable checkpoints."
        };
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(destination, JsonSerializer.Serialize(new
        {
            Method = "At every fixed seed and size, run one all-baseline control plus one candidate upgrade for each of LordAndLoyalists/Rebels/Renegade. " +
                "All opposing seats keep the baseline policy. Noninteractive constructor setup fixes roles, generals and hands before brain replacement. " +
                "Initial/final state hashes combine the trusted snapshot with physical-card locations and zone order, including the hidden draw pile. " +
                "Both versions use the baseline host policy and exact engine brain seeds, includeJudgmentMaterials=false. " +
                "Only normal player-view decision inputs reach brains; trusted roles are used solely to assign policies and audit outcomes. " +
                "A/A requires final snapshot and selected-decision hashes to match the control. " +
                "Final inventory must equal initial IDs union newly registered native generated IDs, each exactly once; " +
                "completion, processing area and resolution stack are audited after every game and failures remain in the report.",
            SeedFormula = "1327111 + seedIndex * 7919 + players * 101",
            MaximumTurns,
            ContentHash = registry.ContentHash,
            Packages = registry.Packages.Select(package => new { package.Id, Version = package.Version.ToString() }).ToArray(),
            Modes = tableSizes.Select(count => registry.Modes[ModeId(profile, count)])
                .Select(mode => new { mode.Id, mode.DeckId, GeneralPoolCount = mode.GeneralPoolIds?.Count,
                    GeneralSelectionPoolCount = mode.GeneralSelectionPoolIds?.Count, mode.RoleCounts }).ToArray(),
            Summary = summary,
            Comparisons = comparisons,
            Matches = matches
        }, jsonOptions));
        journal.Write(new { Kind = "BatchCompleted", Utc = DateTimeOffset.UtcNow, Summary = summary });
        Console.WriteLine(JsonSerializer.Serialize(summary, jsonOptions));
        Console.WriteLine($"Saved identity policy evidence: {destination}");
        return matches.All(match => match.Success) && comparisons.All(pair => pair.Complete) ? 0 : 1;
    }

    private static MatchResult Play(ContentRegistry registry, string profile, string mode, int index, int seed,
        int count, int baseline, int candidate, string? focalCamp, EvaluationJournal journal)
    {
        using var progress = new MatchProgressProbe(journal, new MatchIdentity(profile, mode, count, index, seed,
            baseline, candidate, focalCamp));
        GameEngine? game = null;
        GameSnapshot? initial = null;
        GameSnapshot? final = null;
        string? initialHash = null, finalHash = null, decisionHash = null, error = null;
        IReadOnlyList<SeatSetup> roster = [];
        var inventory = Array.Empty<int>();
        var initialGenerated = new HashSet<int>();
        var accepted = false;
        var inventoryPreserved = false;
        var settled = false;
        var watch = new Stopwatch();
        IdentityDiagnostics? diagnostics = null;
        var events = 0;
        var thoughts = 0;
        var finalPhysicalCards = 0;
        var newGeneratedCards = 0;
        try
        {
            game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = seed, PlayerCount = count, ModeId = mode, HumanSeat = -1, HumanRole = null,
                UseInteractiveSetup = false, AiPolicyVersion = baseline, MaxTurns = MaximumTurns
            }, registry);
            progress.Attach(game);
            initial = game.CreateSnapshot(-1, true);
            var initialZones = game.CreateCardZoneDiagnostics();
            initialHash = StateHash(initial, initialZones);
            inventory = initialZones.Select(card => card.CardId).Order().ToArray();
            initialGenerated = GeneratedCards(game).ToHashSet();
            if (inventory.Distinct().Count() != inventory.Length)
                throw new InvalidOperationException("Initial identity inventory has duplicate physical cards.");
            if (!initialGenerated.IsSubsetOf(inventory))
                throw new InvalidOperationException("Native initial generated-card provenance has no physical card.");
            var brains = BrainsField.GetValue(game) as Dictionary<int, SimpleAiBrain> ??
                throw new InvalidOperationException("Identity evaluation found an unexpected AI map.");
            if (brains.Count != count || initial.Players.Any(player => player.IsHuman || !brains.ContainsKey(player.Seat)))
                throw new InvalidOperationException("Every identity evaluation seat must be AI controlled.");
            roster = initial.Players.Select(player =>
            {
                var role = player.Role ?? throw new InvalidOperationException("Trusted identity setup omitted a role.");
                var camp = Camp(role);
                var policy = camp == focalCamp ? candidate : baseline;
                var brain = new SimpleAiBrain(player.Seat, unchecked(seed * 397) ^ (player.Seat + 1),
                    policy, includeJudgmentMaterials: false);
                brains[player.Seat] = brain;
                if (!ReferenceEquals(brains[player.Seat], brain))
                    throw new InvalidOperationException("Identity policy replacement did not reach the engine map.");
                return new SeatSetup(player.Seat, player.GeneralId, role.ToString(), camp, policy,
                    player.Hp, player.MaxHp, player.Hand.Select(card => card.Id).ToArray());
            }).ToArray();
            if (game.CreateCheckpoint().Options.AiPolicyVersion != baseline ||
                focalCamp is not null && !roster.Any(seat => seat.Camp == focalCamp) ||
                roster.Any(seat => seat.Policy != (seat.Camp == focalCamp ? candidate : baseline)))
                throw new InvalidOperationException("Only the focal identity camp may change policy under an identical host policy.");
            progress.InitialSetup(initialHash, roster);
            watch.Start();
            progress.Stage = "Playing";
            var started = game.Submit(new StartGameCommand());
            accepted = started.Accepted;
            if (!accepted) error = BoundedText("Start rejected: " + JsonSerializer.Serialize(started.Error));
        }
        catch (Exception exception) { error = BoundedError(exception); }
        finally { watch.Stop(); }

        try
        {
            progress.Stage = "FinalAudit";
            if (game is not null)
            {
                final = game.CreateSnapshot(-1, true);
                var finalZones = game.CreateCardZoneDiagnostics();
                finalHash = StateHash(final, finalZones);
                var finalInventory = finalZones.Select(card => card.CardId).Order().ToArray();
                var generated = GeneratedCards(game);
                var newlyGenerated = generated.Except(initialGenerated).ToArray();
                var expectedInventory = inventory.Concat(newlyGenerated).Order().ToArray();
                finalPhysicalCards = finalInventory.Length;
                newGeneratedCards = newlyGenerated.Length;
                inventoryPreserved = initial is not null && initialGenerated.IsSubsetOf(generated) &&
                    expectedInventory.SequenceEqual(finalInventory) &&
                    finalInventory.Distinct().Count() == expectedInventory.Length;
                settled = game.ResolutionStack.Count == 0 && final.ProcessingCardCount == 0;
                decisionHash = SelectedDecisionHash(game.AiThoughts);
                events = game.Events.Count;
                thoughts = game.AiThoughts.Count;
                if (initial is not null && roster.Count == count)
                    diagnostics = AuditIdentity(game, initial, final, roster);
                if (final.Status == EngineStatus.Completed && final.Winner is not
                    (Winner.LordAndLoyalists or Winner.Rebels or Winner.Renegade or Winner.Draw))
                    error = BoundedText((error is null ? "" : error + "\n") +
                        $"Completed identity match has unexpected winner {final.Winner}.");
            }
        }
        catch (Exception exception) { error = BoundedText((error is null ? "" : error + "\n") + "Diagnostic failure: " + exception); }
        var success = error is null && accepted && final?.Status == EngineStatus.Completed && inventoryPreserved && settled;
        var result = new MatchResult(profile, mode, count, index, seed, focalCamp, baseline, initialHash, finalHash,
            decisionHash, roster, success, final?.Status.ToString() ?? "NotCreated", final?.Winner.ToString() ?? "None",
            final?.TurnNumber ?? 0, Math.Round(watch.Elapsed.TotalMilliseconds, 2), accepted, inventoryPreserved,
            settled, inventory.Length, finalPhysicalCards, newGeneratedCards, events, thoughts, diagnostics, error);
        if (!success && game is not null) progress.TerminalFaultContext(game);
        progress.Completed(result);
        return result;
    }

    private static IdentityDiagnostics AuditIdentity(GameEngine game, GameSnapshot initial, GameSnapshot final,
        IReadOnlyList<SeatSetup> roster)
    {
        var roles = roster.ToDictionary(seat => seat.Seat, seat => Enum.Parse<Role>(seat.Role));
        var living = initial.Players.Where(player => player.IsAlive).Select(player => player.Seat).ToHashSet();
        var lord = roles.Single(pair => pair.Value == Role.Lord).Key;
        var renegade = roles.Single(pair => pair.Value == Role.Renegade).Key;
        var lordKillsLoyalists = 0;
        var rebelOnRebelDeaths = 0;
        var prematureRenegadeLordKills = 0;
        var lordDamageByLoyalists = 0;
        var rebelDamageToLord = 0;
        var rebelDamageElsewhere = 0;
        var reachedLastTwo = false;
        foreach (var envelope in game.Events)
        {
            if (envelope.Payload is PlayerDiedEvent death)
            {
                if (death.KillerSeat is { } killer && roles.TryGetValue(killer, out var role))
                {
                    if (role == Role.Lord && roles[death.VictimSeat] == Role.Loyalist) lordKillsLoyalists++;
                    if (role == Role.Rebel && roles[death.VictimSeat] == Role.Rebel) rebelOnRebelDeaths++;
                    if (role == Role.Renegade && death.VictimSeat == lord && living.Count > 2)
                        prematureRenegadeLordKills++;
                }
                living.Remove(death.VictimSeat);
                if (living.Count <= 2 && living.Contains(renegade)) reachedLastTwo = true;
            }
            if (envelope.Payload is DamageAppliedEvent { SourceLess: false } damage &&
                roles.TryGetValue(damage.SourceSeat, out var sourceRole))
            {
                if (sourceRole == Role.Loyalist && damage.TargetSeat == lord) lordDamageByLoyalists += damage.Amount;
                if (sourceRole == Role.Rebel)
                {
                    if (damage.TargetSeat == lord) rebelDamageToLord += damage.Amount;
                    else rebelDamageElsewhere += damage.Amount;
                }
            }
        }
        return new(lordKillsLoyalists, rebelOnRebelDeaths, prematureRenegadeLordKills,
            lordDamageByLoyalists, rebelDamageToLord, rebelDamageElsewhere, reachedLastTwo,
            final.Players.Single(player => player.Seat == lord).IsAlive,
            final.Players.Single(player => player.Seat == renegade).IsAlive);
    }

    private static object SumDiagnostics(IEnumerable<IdentityDiagnostics?> source)
    {
        var values = source.OfType<IdentityDiagnostics>().ToArray();
        return new
        {
            Games = values.Length,
            LordKilledLoyalists = values.Sum(value => value.LordKilledLoyalists),
            RebelOnRebelDeaths = values.Sum(value => value.RebelOnRebelDeaths),
            RenegadePrematureLordKills = values.Sum(value => value.RenegadePrematureLordKills),
            LordDamageByLoyalists = values.Sum(value => value.LordDamageByLoyalists),
            RebelDamageToLord = values.Sum(value => value.RebelDamageToLord),
            RebelDamageElsewhere = values.Sum(value => value.RebelDamageElsewhere),
            RenegadeReachedLastTwoGames = values.Count(value => value.RenegadeReachedLastTwo),
            LordAliveAtEndGames = values.Count(value => value.LordAliveAtEnd),
            RenegadeAliveAtEndGames = values.Count(value => value.RenegadeAliveAtEnd)
        };
    }

    private static string ModeId(string profile, int count) => profile switch
    {
        "standard" => $"identity:standard-{count}",
        "active" => $"identity:active-skills-{count}",
        "classic" => $"identity:classic-{count}",
        _ => throw new ArgumentOutOfRangeException(nameof(profile))
    };
    private static string Camp(Role role) => role switch
    {
        Role.Lord or Role.Loyalist => "LordAndLoyalists",
        Role.Rebel => "Rebels",
        Role.Renegade => "Renegade",
        _ => throw new ArgumentOutOfRangeException(nameof(role))
    };
    private static double Points(string winner, string camp) => winner == camp ? 1d : winner == nameof(Winner.Draw) ? .5d : 0d;
    private static string Hash(string text) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
    private static string StateHash(GameSnapshot snapshot, IReadOnlyList<CardZoneDiagnostic> zones) =>
        Hash(SnapshotJson.Serialize(snapshot) + "\n" + JsonSerializer.Serialize(zones.OrderBy(card => card.CardId)));
    private static string BoundedError(Exception exception) => BoundedText(exception.ToString());
    private static string BoundedText(string text) => text.Length <= 16384 ? text : text[..16384] + "\n[truncated at 16384 characters]";
    private static IReadOnlySet<int> GeneratedCards(GameEngine game) =>
        GeneratedCardsField.GetValue(game) as IReadOnlySet<int> ??
        throw new InvalidOperationException("Identity evaluation found an unexpected native generated-card set.");
    private static string SelectedDecisionHash(IReadOnlyList<AiThoughtRecord> thoughts)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (var thought in thoughts)
            hash.AppendData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
                { thought.Sequence, thought.TurnNumber, thought.ActorSeat, thought.Decision }) + "\n"));
        return Convert.ToHexString(hash.GetHashAndReset());
    }

    private sealed class EvaluationJournal : IDisposable
    {
        private readonly object _gate = new();
        private readonly FileStream _stream;
        private readonly StreamWriter _writer;
        internal string Path { get; }

        internal EvaluationJournal(string path)
        {
            Path = path;
            // A journal is evidence, not a resume request. Never erase an older
            // interrupted run that happens to use the same output basename.
            _stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_stream, new UTF8Encoding(false), bufferSize: 4096, leaveOpen: true);
        }

        internal void Write(object value)
        {
            var line = JsonSerializer.Serialize(value);
            lock (_gate)
            {
                _writer.WriteLine(line);
                _writer.Flush();
                _stream.Flush(flushToDisk: true);
            }
        }

        public void Dispose()
        {
            lock (_gate) { _writer.Dispose(); _stream.Dispose(); }
        }
    }

    private sealed record MatchIdentity(string Profile, string Mode, int Players, int Index, int Seed,
        int BaselinePolicy, int CandidatePolicy, string? FocalCamp);

    private sealed class MatchProgressProbe : IDisposable
    {
        private const int MaximumProgressSamples = 256;
        private static readonly Dictionary<string, MemberInfo> DiagnosticFields = new[]
        {
            "_turnNumber", "_currentSeat", "_phase", "_status", "_thoughtSequence", "_resolutionSequence",
            "_movementSequence", "_pendingEvents", "_cardMovements", "_resolutionStack", "_aiThoughts", "_pendingDecisionBacking"
        }.ToDictionary(name => name, name => (MemberInfo?)FixtureRuntimeAccess.Engine.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) ??
            (name == "_resolutionSequence" ? FixtureRuntimeAccess.Engine.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic) : null) ??
            throw new InvalidOperationException("Identity progress diagnostics cannot find " + name));
        private readonly EvaluationJournal _journal;
        private readonly MatchIdentity _identity;
        private readonly Stopwatch _watch = Stopwatch.StartNew();
        private readonly Process _process = Process.GetCurrentProcess();
        private readonly Timer _timer;
        private GameEngine? _game;
        private string _stage = "Constructing";
        private string? _samplingError;
        private int _samples;
        private bool _completed;
        private bool _stopped;
        internal string Stage { set => Volatile.Write(ref _stage, value); }

        internal MatchProgressProbe(EvaluationJournal journal, MatchIdentity identity)
        {
            _journal = journal; _identity = identity;
            journal.Write(new { Kind = "MatchStarted", Utc = DateTimeOffset.UtcNow, Match = identity });
            _timer = new Timer(_ => Sample(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }

        internal void Attach(GameEngine game) { Volatile.Write(ref _game, game); Stage = "InitialAudit"; }

        internal void InitialSetup(string? hash, IReadOnlyList<SeatSetup> roster) =>
            _journal.Write(new { Kind = "InitialSetup", Utc = DateTimeOffset.UtcNow, Match = _identity,
                InitialStateHash = hash, Roster = roster, TrustedLocalOnly = true });

        private static object? ReadDiagnostic(GameEngine game, string name) => DiagnosticFields[name] switch
        {
            FieldInfo field => field.GetValue(game),
            PropertyInfo property => property.GetValue(game),
            _ => throw new InvalidOperationException("Unsupported identity progress diagnostic " + name)
        };

        private void Sample()
        {
            try
            {
                if (Interlocked.Increment(ref _samples) > MaximumProgressSamples)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _journal.Write(new { Kind = "ProgressSampleCapReached", Utc = DateTimeOffset.UtcNow, Match = _identity,
                        MaximumProgressSamples, Limit = "Sampling stops; this does not stop, complete or score the match." });
                    return;
                }
                var game = Volatile.Read(ref _game);
                _process.Refresh();
                // The engine may be in its one long command. Read only bounded
                // diagnostic scalars and immutable last-record references; never
                // project a view, enumerate mutable zones, call rules or lock it.
                // Samples need not describe an atomic/committed game state.
                object? Read(string name) => game is null ? null : ReadDiagnostic(game, name);
                var frames = Read("_resolutionStack") as IReadOnlyList<ResolutionFrame>;
                var thoughts = Read("_aiThoughts") as IList;
                var pending = Read("_pendingDecisionBacking") as PendingDecision;
                var last = thoughts is { Count: > 0 } ? thoughts[thoughts.Count - 1] as AiThoughtRecord : null;
                var top = frames is { Count: > 0 } ? frames[frames.Count - 1] as ResolutionFrame : null;
                var decision = last?.Decision;
                _journal.Write(new { Kind = "Progress", Utc = DateTimeOffset.UtcNow, Match = _identity,
                    Stage = Volatile.Read(ref _stage), ElapsedMilliseconds = _watch.ElapsedMilliseconds,
                    ProcessCpuMilliseconds = Math.Round(_process.TotalProcessorTime.TotalMilliseconds, 2),
                    _process.WorkingSet64, Turn = Read("_turnNumber"), CurrentSeat = Read("_currentSeat"),
                    Phase = Read("_phase")?.ToString(), Status = Read("_status")?.ToString(),
                    ThoughtSequence = Read("_thoughtSequence"), ResolutionSequence = Read("_resolutionSequence"),
                    MovementSequence = Read("_movementSequence"), PendingEvents = (Read("_pendingEvents") as IList)?.Count,
                    MovementRecords = (Read("_cardMovements") as IList)?.Count, ResolutionDepth = frames?.Count,
                    TopFrameId = top?.Id, TopFrameKind = top?.Kind.ToString(),
                    PendingDecisionKind = pending?.Kind.ToString(), PendingActorSeat = pending?.PlayerSeat,
                    LastDecision = decision is { Length: > 600 } ? decision[..600] : decision,
                    LastDecisionTurn = last?.TurnNumber,
                    Limit = "Approximate trusted local observation of uncommitted work; not a snapshot, outcome or decision input." });
            }
            catch (Exception exception)
            {
                // A concurrent list cursor may change between Count and index.
                // Sampling must neither fault nor synchronize the game command.
                Volatile.Write(ref _samplingError, BoundedText(exception.GetType().Name + ": " + exception.Message));
            }
        }

        private void StopSampling()
        {
            if (_stopped) return;
            _timer.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _stopped = true;
        }

        internal void TerminalFaultContext(GameEngine game)
        {
            try
            {
                // Play and its final audit have stopped. This synchronous read
                // records the actual failed boundary, even for sub-5s failures.
                StopSampling();
                const int jsonCharacterLimit = 32768;
                var pending = ReadDiagnostic(game, "_pendingDecisionBacking") as PendingDecision;
                var frames = game.ResolutionStack.TakeLast(6).Select(frame =>
                {
                    var program = frame as ProgramSkillFrame;
                    var window = frame as ProgramLifecycleTriggerWindowFrame;
                    return new
                    {
                        FrameType = frame.GetType().Name, frame.Id, Kind = frame.Kind.ToString(), Step = frame.Step.ToString(),
                        Program = program is null ? null : new
                        {
                            program.OwnerSeat, SkillId = Limit(program.SkillId), ActivationId = Limit(program.ActivationId),
                            TriggerId = Limit(program.TriggerId), SkillInstanceId = Limit(program.SkillInstanceId),
                            program.InstructionIndex, GameplayHash = Limit(program.GameplayHash),
                            Window = program.WindowContext?.Window.ToString(), ParentFrameId = program.WindowContext?.ParentFrameId,
                            SelectedCards = program.SelectedCardIds.Take(8).ToArray(),
                            SelectedTargets = program.SelectedTargetSeats.Take(8).ToArray(),
                            HasPendingMovementContinuation = program.PendingMovementContinuation is not null
                        },
                        Lifecycle = window is null ? null : new
                        {
                            window.OwnerSeat, Window = window.Window.ToString(), Continuation = window.Continuation.ToString(),
                            window.CandidateIndex, CandidateCount = window.Candidates.Count
                        }
                    };
                }).ToArray();
                var context = new
                {
                    Stage = Volatile.Read(ref _stage),
                    PendingDecision = pending is null ? null : new
                    {
                        Kind = pending.Kind.ToString(), pending.PlayerSeat, pending.PromptId,
                        SkillId = Limit(pending.SkillPrompt?.SkillId), pending.RequiredCardCount,
                        Choices = pending.Choices.Take(16).Select(choice => new
                        {
                            Id = Limit(choice.Id.ToString()), Cards = choice.Cards.Take(8).ToArray(),
                            Targets = choice.Targets.Take(8).ToArray(),
                            Parameters = choice.Parameters.Take(16).ToDictionary(pair => pair.Key,
                                pair => Limit(pair.Value, 512)),
                            ParameterCount = choice.Parameters.Count
                        }).ToArray(),
                        ChoiceCount = pending.Choices.Count
                    },
                    ResolutionDepth = game.ResolutionStack.Count, Frames = frames
                };
                var json = JsonSerializer.Serialize(context, new JsonSerializerOptions { MaxDepth = 12 });
                if (json.Length <= jsonCharacterLimit)
                    _journal.Write(new { Kind = "TerminalFaultContext", Utc = DateTimeOffset.UtcNow, Match = _identity,
                        TrustedLocalOnly = true, Context = context, JsonDepthLimit = 12,
                        JsonCharacterLimit = jsonCharacterLimit, Truncated = false });
                else
                    _journal.Write(new { Kind = "TerminalFaultContext", Utc = DateTimeOffset.UtcNow, Match = _identity,
                        TrustedLocalOnly = true, Frames = frames, PendingKind = pending?.Kind.ToString(),
                        ContextJsonPrefix = json[..jsonCharacterLimit], JsonDepthLimit = 12,
                        JsonCharacterLimit = jsonCharacterLimit, Truncated = true });
            }
            catch (Exception exception)
            {
                // Terminal diagnostics are secondary evidence. The original
                // MatchResult.Error and outcome must never be replaced by them.
                Volatile.Write(ref _samplingError, "TerminalFaultContext: " +
                    BoundedText(exception.GetType().Name + ": " + exception.Message));
            }

            static string? Limit(string? text, int length = 256) =>
                text is { } value && value.Length > length ? value[..length] : text;
        }

        internal void Completed(MatchResult result)
        {
            StopSampling();
            _journal.Write(new { Kind = "MatchCompleted", Utc = DateTimeOffset.UtcNow, Match = _identity, Result = result,
                SamplingError = Volatile.Read(ref _samplingError) });
            _completed = true;
            Console.WriteLine($"Identity match {result.Players} seats index {result.Index} {result.FocalCamp ?? "control"}: " +
                $"{result.Winner} [{(result.Success ? "OK" : "FAILED")}] {result.Milliseconds:0.##} ms");
        }

        public void Dispose()
        {
            StopSampling();
            if (!_completed)
                _journal.Write(new { Kind = "MatchAbandoned", Utc = DateTimeOffset.UtcNow, Match = _identity,
                    Stage = Volatile.Read(ref _stage), SamplingError = Volatile.Read(ref _samplingError),
                    Limit = "No final audited result was produced; this slot has no win/draw score." });
            _process.Dispose();
        }
    }

    private sealed record SeatSetup(int Seat, string GeneralId, string Role, string Camp, int Policy,
        int InitialHp, int InitialMaxHp, IReadOnlyList<int> InitialHandCardIds);
    private sealed record IdentityDiagnostics(int LordKilledLoyalists, int RebelOnRebelDeaths,
        int RenegadePrematureLordKills, int LordDamageByLoyalists, int RebelDamageToLord,
        int RebelDamageElsewhere, bool RenegadeReachedLastTwo, bool LordAliveAtEnd, bool RenegadeAliveAtEnd);
    private sealed record MatchResult(string Profile, string Mode, int Players, int Index, int Seed,
        string? FocalCamp, int HostPolicy, string? InitialStateHash, string? FinalStateHash, string? DecisionHash,
        IReadOnlyList<SeatSetup> Roster, bool Success, string Status, string Winner, int Turns,
        double Milliseconds, bool StartAccepted, bool InventoryPreserved, bool Settled, int InitialPhysicalCards,
        int FinalPhysicalCards, int NewGeneratedPhysicalCards,
        int Events, int Thoughts, IdentityDiagnostics? Diagnostics, string? Error);
    private sealed record Comparison(string Profile, string Mode, int Players, int Index, int Seed,
        string Camp, bool Complete, bool SameInitialState, bool? AAInvariant, bool ControlSuccess,
        bool CandidateSuccess, string ControlWinner, string CandidateWinner, double? ControlPoints,
        double? CandidatePoints, double? PointDelta, IdentityDiagnostics? ControlDiagnostics,
        IdentityDiagnostics? CandidateDiagnostics);
}
