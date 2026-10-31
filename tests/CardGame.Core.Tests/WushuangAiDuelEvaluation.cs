using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

/// <summary>Opt-in paired native Wushuang opening/shop/battle evidence; never a smoke-suite registration.</summary>
internal static class WushuangAiDuelEvaluation
{
    private const int MaximumTurns = 20;
    private const double DrawUtility = WushuangMode.DrawEvaluationScore;
    private static readonly string[] SmokeGenerals = ["classic:zhao-yun", "classic:zhang-fei", "classic:guo-jia", "classic:zhang-liao"];
    private static readonly FieldInfo Brains = Field("_aiBrains"), Generated = Field("_generatedPhysicalCardIds"), Random = Field("_random");
    private static FieldInfo Field(string name) => global::FixtureRuntimeAccess.Engine.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Wushuang evaluation cannot find engine " + name);

    internal static int Run(string output, int pairs, int baseline, int? candidatePolicy, int firstPair, string profile, string? generalPoolSnapshot = null)
    {
        var candidate = candidatePolicy ?? AiDecisionPolicy.CurrentVersion;
        if (pairs is < 1 or > 120 || firstPair is < 0 or > 10000)
            throw new ArgumentOutOfRangeException(nameof(pairs), "Accept 1..120 paired seeds and first index0..10000.");
        if (!AiDecisionPolicy.IsSupported(baseline) || !AiDecisionPolicy.IsSupported(candidate))
            throw new ArgumentException("Both policies must be supported; equal versions request A/A.");
        var destination = Path.GetFullPath(output);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        var traceDirectory = Environment.GetEnvironmentVariable("CARD_AI_DUEL_TRACE") == "1" ? destination + ".traces" : null;
        if (traceDirectory is not null) Directory.CreateDirectory(traceDirectory);
        using var journal = new Journal(destination + ".journal.jsonl");
        var binaries = Directory.EnumerateFiles(AppContext.BaseDirectory, "CardGame.*.dll")
            .OrderBy(path => path, StringComparer.Ordinal).Select(path => new
            { Name = Path.GetFileNameWithoutExtension(path), Path = path,
                Sha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))) }).ToArray();
        journal.Write(new { Kind = "BatchStarted", Utc = DateTimeOffset.UtcNow, Profile = profile, BaselinePolicy = baseline,
            CandidatePolicy = candidate, FirstPair = firstPair, Pairs = pairs, PlannedGameSlots = pairs * 2,
            ClockModel = "frozen-zero: no clock commands; elapsed CPU/wall time never changes the winner", MaximumTurns,
            SurvivalLimitRule = "At20 native turns or15 minutes, greater current HP wins; equal current HP draws. MaxHP/hand/resources do not break ties.",
            DrawUtility, Binaries = binaries });
        ContentRegistry registry;
        object? pinnedPool = null;
        try
        {
            var production = StandardWushuangContentRegistry.Create();
            if (generalPoolSnapshot is not null)
            {
                if (profile != "wushuang") throw new ArgumentException("A pinned pool cannot replace the explicit smoke roster.");
                var poolBytes = File.ReadAllBytes(generalPoolSnapshot);
                var snapshot = JsonSerializer.Deserialize<PoolSnapshot>(poolBytes) ?? throw new ArgumentException("The general pool snapshot is empty.");
                var mode = production.Modes[WushuangMode.ModeId];
                if (snapshot.ModeId != WushuangMode.ModeId || snapshot.GeneralPoolIds is not { Count: >= 2 } ids ||
                    ids.Distinct(StringComparer.Ordinal).Count() != ids.Count || snapshot.GeneralCandidateCount < 1 || snapshot.GeneralCandidateCount > ids.Count ||
                    ids.Any(id => !mode.GeneralPoolIds!.Contains(id, StringComparer.Ordinal)))
                    throw new ArgumentException("A pinned pool must name distinct currently eligible production generals and a valid native candidate count.");
                registry = ContentRegistry.Build(new PinnedPoolPackage(production, snapshot));
                pinnedPool = new { Path = Path.GetFullPath(generalPoolSnapshot), Sha256 = Convert.ToHexString(SHA256.HashData(poolBytes)),
                    snapshot.SourceContentHash, snapshot.GeneralPoolIds, snapshot.GeneralCandidateCount };
            }
            else registry = profile == "wushuang-smoke" ? ContentRegistry.Build(new SmokePackage(production)) : production;
            journal.Write(new { Kind = "RegistryReady", Utc = DateTimeOffset.UtcNow, registry.ContentHash,
                ProductionContentHash = production.ContentHash, Pool = registry.Modes[WushuangMode.ModeId].GeneralPoolIds,
                WushuangCatalogue.Fingerprint, PinnedPool = pinnedPool });
        }
        catch (Exception ex)
        {
            var failed = new { Success = false, Error = Bounded(ex.ToString()), PlannedGameSlots = pairs * 2, CompletedGameSlots = 0 };
            journal.Write(new { Kind = "BatchFailed", Utc = DateTimeOffset.UtcNow, Result = failed });
            File.WriteAllText(destination, JsonSerializer.Serialize(failed, new JsonSerializerOptions { WriteIndented = true }));
            return 1;
        }
        var matches = new List<Result>(pairs * 2);
        var comparisons = new List<Pair>();
        for (var offset = 0; offset < pairs; offset++)
        {
            var index = firstPair + offset;
            var seed = 1704311 + index * 7919;
            var left = Play(registry, journal, profile, index, seed, 0, baseline, candidate, traceDirectory);
            var right = Play(registry, journal, profile, index, seed, 1, baseline, candidate, traceDirectory);
            matches.Add(left); matches.Add(right);
            var initial = left.InitialStateHash is not null && left.InitialStateHash == right.InitialStateHash;
            bool? aa = baseline == candidate && left.Success && right.Success ?
                left.FinalStateHash == right.FinalStateHash && left.DecisionHash == right.DecisionHash && left.ShopFactsHash == right.ShopFactsHash : null;
            var complete = left.Success && right.Success && initial && aa is not false;
            var pair = new Pair(index, seed, complete, initial, aa,
                complete ? (left.CandidateUtility!.Value + right.CandidateUtility!.Value) / 2 : null,
                complete ? (left.BaselineUtility!.Value + right.BaselineUtility!.Value) / 2 : null);
            comparisons.Add(pair);
            journal.Write(new { Kind = "PairCompleted", Utc = DateTimeOffset.UtcNow, Pair = pair });
            Console.WriteLine($"Wushuang {profile} pair{index}: candidate-seat0={left.WinnerSeat?.ToString() ?? left.Winner}, candidate-seat1={right.WinnerSeat?.ToString() ?? right.Winner}; {(complete ? "OK" : "FAILED")}");
        }
        var completeIndices = comparisons.Where(p => p.Complete).Select(p => p.Index).ToHashSet();
        var balanced = matches.Where(m => completeIndices.Contains(m.Index)).ToArray();
        var deltas = comparisons.Where(p => p.Complete).Select(p => p.CandidateMeanUtility!.Value - p.BaselineMeanUtility!.Value).ToArray();
        var coverage = new
        {
            OpeningChoices = matches.Sum(m => m.Shop?.OpeningChoices ?? 0), Purchases = matches.Sum(m => m.Shop?.Purchases ?? 0),
            MultiplePurchaseWindows = matches.Sum(m => m.Shop?.MultiplePurchaseWindows ?? 0),
            PaidRefreshes = matches.Sum(m => m.Shop?.PaidRefreshes ?? 0), FreeRefreshes = matches.Sum(m => m.Shop?.FreeRefreshes ?? 0),
            LockChanges = matches.Sum(m => m.Shop?.LockChanges ?? 0), BattleGames = matches.Count(m => m.Shop?.BattleStarted == true)
        };
        var summary = new
        {
            Profile = profile, BaselinePolicy = baseline, CandidatePolicy = candidate, HostPolicy = baseline, AAControl = baseline == candidate,
            FirstPair = firstPair, LastPairInclusive = firstPair + pairs - 1, PlannedGames = pairs * 2, Games = matches.Count,
            SuccessfulGames = matches.Count(m => m.Success), FailedGames = matches.Count(m => !m.Success),
            CompletePairs = comparisons.Count(p => p.Complete), InitialStateMismatches = comparisons.Count(p => !p.SameInitialState),
            AAMismatches = comparisons.Count(p => p.AAInvariant is false), MaximumTurns, DrawUtility,
            SurvivalLimitAuditFailures = matches.Count(m => m.SurvivalLimitCurrentHpVerified is false || m.EndConditionVerified is false),
            CompletePairedGames = new
            {
                Games = balanced.Length, CandidateWins = balanced.Count(m => m.WinnerSeat == m.CandidateSeat),
                BaselineWins = balanced.Count(m => m.WinnerSeat == 1 - m.CandidateSeat), Draws = balanced.Count(m => m.Winner == nameof(Winner.Draw)),
                CandidateLosses = balanced.Count(m => m.WinnerSeat == 1 - m.CandidateSeat), BaselineLosses = balanced.Count(m => m.WinnerSeat == m.CandidateSeat),
                CandidateWinRate = balanced.Length == 0 ? (double?)null : balanced.Count(m => m.WinnerSeat == m.CandidateSeat) / (double)balanced.Length,
                BaselineWinRate = balanced.Length == 0 ? (double?)null : balanced.Count(m => m.WinnerSeat == 1 - m.CandidateSeat) / (double)balanced.Length,
                DrawRate = balanced.Length == 0 ? (double?)null : balanced.Count(m => m.Winner == nameof(Winner.Draw)) / (double)balanced.Length,
                CandidateMeanUtility = balanced.Length == 0 ? (double?)null : balanced.Average(m => m.CandidateUtility!.Value),
                BaselineMeanUtility = balanced.Length == 0 ? (double?)null : balanced.Average(m => m.BaselineUtility!.Value),
                MeanTurns = balanced.Length == 0 ? (double?)null : balanced.Average(m => m.Turns),
                MeanPairedUtilityDifference = deltas.Length == 0 ? (double?)null : deltas.Average(),
                ApproximatePaired95PercentInterval = PairedInterval(deltas)
            },
            CompletePairedGamesByEndCondition = balanced.GroupBy(m => m.EndCondition).OrderBy(g => g.Key).Select(g => new
            {
                EndCondition = g.Key, Games = g.Count(), CandidateWins = g.Count(m => m.WinnerSeat == m.CandidateSeat),
                BaselineWins = g.Count(m => m.WinnerSeat == 1 - m.CandidateSeat), Draws = g.Count(m => m.Winner == nameof(Winner.Draw)),
                CandidateMeanUtility = g.Average(m => m.CandidateUtility!.Value), BaselineMeanUtility = g.Average(m => m.BaselineUtility!.Value),
                MeanTurns = g.Average(m => m.Turns), MeanCandidateFinalHp = g.Average(m => m.FinalPlayers.Single(p => p.SeatId == m.CandidateSeat).Hp),
                MeanBaselineFinalHp = g.Average(m => m.FinalPlayers.Single(p => p.SeatId == 1 - m.CandidateSeat).Hp)
            }).ToArray(),
            Coverage = coverage, MeanGameMilliseconds = matches.Average(m => m.Milliseconds),
            Limit = "Only complete paired native games enter primary rates. Win1/draw0.2/loss0; utility is not win rate. " +
                "At the20-turn limit, greater current HP wins and equal HP draws; this survival result enters overall rates but is separated from elimination. " +
                "Limit outcomes must match both recorded final HP values; faults, safety-loop emergency draws and interrupted starts never score. " +
                "The clock is frozen at zero elapsed: this experiment does not estimate production15-minute outcomes or UI decision latency. " +
                "Opening-shop equality uses initial state, native page seed/content and first refresh facts, not copied private offer pages. " +
                "Smoke uses a named small production-general pool; full profile uses the current eligible production pool or its explicitly pinned eligible production snapshot. Neither proves every general. " +
                "Native wall time is diagnostic only. Mixed brains are test-only and not replayable checkpoints. Shop operation counts describe actual choices; zero lock/refresh coverage is a visible gap, not a forced action."
        };
        File.WriteAllText(destination, JsonSerializer.Serialize(new
        {
            Method = "Two native wushuang:solo games per exact seed/roster/hand/deck/RNG: candidate seats0 then1, opponent always baseline. " +
                "Both legs use the same baseline host gates and native seed*397 XOR(seat+1) brain seed. Only ordinary seat views reach brains. " +
                "Constructor snapshot/hash precedes brain injection; hidden deck order and per-seat native shop seeds are trusted audit data only. " +
                "Native20-turn survival outcomes compare current HP only, independently audited and reported separately from elimination. " +
                "Final physical inventory must equal initial IDs union new native registered generated IDs, including destroyed equipment retained OutsideGame.",
            registry.ContentHash, PinnedPool = pinnedPool, Binaries = binaries, Packages = registry.Packages, Mode = registry.Modes[WushuangMode.ModeId], Summary = summary,
            Pairs = comparisons, Matches = matches
        }, new JsonSerializerOptions { WriteIndented = true }));
        journal.Write(new { Kind = "BatchCompleted", Utc = DateTimeOffset.UtcNow, Summary = summary });
        Console.WriteLine(JsonSerializer.Serialize(summary));
        return comparisons.All(p => p.Complete) ? 0 : 1;
    }

    // The production participant host is required for independent NPC invitations.
    // With exactly two seats each invitation has one recipient; no worker race
    // chooses a winner. The host receives detached ordinary actor views only.
    private static async Task<CommandResult> StartAndRunHosted(GameEngine game)
    {
        using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(45));
        await using var host = ParticipantPromptHost.CreateHeadless(game);
        var started = await host.SubmitAsync(new StartGameCommand(), deadline.Token);
        if (started.Accepted) await host.RunToBoundaryAsync(deadline.Token);
        return started;
    }

    private static Result Play(ContentRegistry registry, Journal journal, string profile, int index, int seed,
        int candidateSeat, int baseline, int candidate, string? traceDirectory = null)
    {
        var identity = new { Profile = profile, Index = index, Seed = seed, CandidateSeat = candidateSeat, BaselinePolicy = baseline, CandidatePolicy = candidate };
        journal.Write(new { Kind = "MatchStarted", Utc = DateTimeOffset.UtcNow, Match = identity });
        GameEngine? game = null;
        var result = new Result { Profile = profile, Index = index, Seed = seed, CandidateSeat = candidateSeat, HostPolicy = baseline };
        using var watch = new MatchWatch(journal, identity);
        var elapsed = Stopwatch.StartNew();
        try
        {
            game = GameEngine.CreateStandard(new GameOptions { Seed = seed, PlayerCount = 2, ModeId = WushuangMode.ModeId,
                HumanSeat = -1, HumanRole = null, HumanTeamId = null, UseInteractiveSetup = false,
                ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                AiPolicyVersion = baseline, MaxTurns = MaximumTurns }, registry);
            watch.Attach(game);
            var initial = game.CreateSnapshot(-1, true);
            var inventory = game.CreateCardZoneDiagnostics().Select(c => c.CardId).Order().ToArray();
            var initialGenerated = NativeGenerated(game);
            if (initial.Players.Count != 2 || initial.Players.Any(p => p.IsHuman) || inventory.Distinct().Count() != inventory.Length)
                throw new InvalidOperationException("Wushuang evaluation requires exactly two native AI seats and unique original entities.");
            var roster = initial.Players.Select(p => new Seat(p.Seat, p.GeneralId, p.TeamId!, p.Hp, p.MaxHp,
                p.Hand.Select(c => c.Id).ToArray(), p.Seat == candidateSeat ? candidate : baseline,
                unchecked(seed * 397) ^ (p.Seat + 1),
                unchecked(seed * 397 ^ p.Seat * 7919 ^ 104729))).ToArray();
            result = result with { InitialStateHash = StateHash(game, registry.ContentHash), Roster = roster, InitialPhysicalCards = inventory.Length };
            journal.Write(new { Kind = "InitialSetup", Utc = DateTimeOffset.UtcNow, Match = identity, result.InitialStateHash,
                Roster = roster, InitialElapsedClockMilliseconds = 0, TrustedLocalOnly = true });
            var brains = Brains.GetValue(game) as Dictionary<int, SimpleAiBrain> ?? throw new InvalidOperationException("Unknown native AI map.");
            if (brains.Count != 2 || game.CreateCheckpoint().Options.AiPolicyVersion != baseline)
                throw new InvalidOperationException("A paired leg must retain its baseline host and exact native AI map.");
            foreach (var seat in roster) brains[seat.SeatId] = new SimpleAiBrain(seat.SeatId,
                seat.AiBrainSeed, seat.Policy, includeJudgmentMaterials: false);
            watch.Stage = "NativeOpeningShopAndBattle";
            var started = StartAndRunHosted(game).GetAwaiter().GetResult();
            result = result with { StartAccepted = started.Accepted };
            if (!started.Accepted) throw new InvalidOperationException(started.Error?.Message ?? "Native Start rejected.");
            watch.Stage = "FinalAudit";
            var final = game.CreateSnapshot(-1, true);
            var finalPlayers = final.Players.OrderBy(p => p.Seat).Select(p => new TerminalSeat(p.Seat, p.TeamId, p.Hp, p.MaxHp, p.IsAlive)).ToArray();
            result = result with { FinalPlayers = finalPlayers, Status = final.Status.ToString(), Winner = final.Winner.ToString(),
                RawWinnerSeat = final.WinnerSeat, WinnerTeamId = final.WinnerTeamId, Turns = final.TurnNumber };
            var generated = NativeGenerated(game).Except(initialGenerated).Order().ToArray();
            var actual = game.CreateCardZoneDiagnostics();
            var expected = inventory.Concat(generated).Order().ToArray();
            var conserved = expected.SequenceEqual(actual.Select(c => c.CardId).Order()) && actual.Select(c => c.CardId).Distinct().Count() == expected.Length;
            var settled = game.ResolutionStack.Count == 0 && final.ProcessingCardCount == 0;
            var facts = game.Events.Select(e => e.Payload).ToArray();
            var shop = AuditShop(game, facts);
            var safetyDraw = game.Log.Any(l => l.Message.Contains("规则循环超过安全上限", StringComparison.Ordinal));
            var clockUntouched = !facts.OfType<WushuangClockAdvancedEvent>().Any() && !facts.OfType<WushuangClockExpiredEvent>().Any() &&
                game.WushuangRemainingMilliseconds == WushuangMode.TimeLimitMilliseconds;
            var winningTeamSeats = final.Players.Where(p => final.WinnerTeamId is not null && p.TeamId == final.WinnerTeamId)
                .Select(p => p.Seat).ToArray();
            int? winnerSeat = final.Winner == Winner.Draw ? null : final.WinnerSeat ??
                (winningTeamSeats.Length == 1 ? winningTeamSeats[0] : (int?)null);
            var terminated = final.Status == EngineStatus.Completed && final.Winner is not Winner.None &&
                (final.Winner == Winner.Draw ? final.WinnerSeat is null && final.WinnerTeamId is null :
                    winnerSeat is 0 or 1 && winningTeamSeats.Length == 1 && winnerSeat == winningTeamSeats[0]);
            var draw = final.Winner == Winner.Draw;
            var survivalLogs = game.Log.Where(l => l.Type == "WushuangSurvivalLimit").ToArray();
            var expiration = facts.OfType<WushuangClockExpiredEvent>().ToArray();
            var turnLimit = survivalLogs.Length == 1 && expiration.Length == 0 &&
                survivalLogs[0].Message.StartsWith($"达到最大回合数 {MaximumTurns}，", StringComparison.Ordinal);
            var endCondition = safetyDraw ? "EmergencySafetyGuard" : expiration.Length > 0 ? "TimeLimitCurrentHp" :
                turnLimit ? "TurnLimitCurrentHp" : survivalLogs.Length > 0 ? "UnrecognizedSurvivalLimit" :
                winnerSeat is { } champion && finalPlayers.Any(p => p.SeatId != champion && !p.IsAlive) ? "Elimination" :
                draw ? "NaturalDraw" : "OtherNativeWin";
            var atSurvivalLimit = endCondition is "TurnLimitCurrentHp" or "TimeLimitCurrentHp";
            bool? hpVerified = atSurvivalLimit ? finalPlayers.Length == 2 &&
                (draw ? finalPlayers[0].Hp == finalPlayers[1].Hp : winnerSeat is { } hpWinner &&
                    finalPlayers.Single(p => p.SeatId == hpWinner).Hp > finalPlayers.Single(p => p.SeatId != hpWinner).Hp) : null;
            var endConditionVerified = endCondition != "UnrecognizedSurvivalLimit" &&
                (!turnLimit || final.TurnNumber == MaximumTurns) && (expiration.Length == 0 ||
                    expiration.Length == 1 && expiration[0].TotalMilliseconds == WushuangMode.TimeLimitMilliseconds && survivalLogs.Length == 1);
            var success = terminated && conserved && settled && clockUntouched && !safetyDraw && endConditionVerified && hpVerified is not false;
            result = result with
            {
                Success = success, Status = final.Status.ToString(), Winner = final.Winner.ToString(), WinnerSeat = winnerSeat,
                RawWinnerSeat = final.WinnerSeat, WinnerTeamId = final.WinnerTeamId,
                Turns = final.TurnNumber, FinalStateHash = StateHash(game, registry.ContentHash), DecisionHash = DecisionHash(game), ShopFactsHash = FactsHash(facts),
                EndCondition = endCondition, EndConditionVerified = endConditionVerified, SurvivalLimitCurrentHpVerified = hpVerified,
                DrawKind = draw ? endCondition : null,
                InventoryPreserved = conserved, Settled = settled, ClockUntouched = clockUntouched, SafetyGuardDraw = safetyDraw,
                FinalPhysicalCards = actual.Count, NewGeneratedPhysicalCards = generated.Length, Shop = shop,
                CandidateUtility = success ? draw ? DrawUtility : winnerSeat == candidateSeat ? 1d : 0d : null,
                BaselineUtility = success ? draw ? DrawUtility : winnerSeat == 1 - candidateSeat ? 1d : 0d : null,
                FailureKind = success ? null : safetyDraw ? "EmergencySafetyGuard" : !terminated ? "NotCompleted" :
                    hpVerified is false || !endConditionVerified ? "SurvivalLimitAudit" :
                    !conserved ? "InventoryAudit" : !settled ? "UnsettledResolution" : "UnexpectedClockMovement",
                Error = success ? null : $"Native audit failed: terminal={terminated}, inventory={conserved}, settled={settled}, frozenClock={clockUntouched}, safetyDraw={safetyDraw}, end={endCondition}/{endConditionVerified}, limitHp={hpVerified}.",
                Events = game.Events.Count, Thoughts = game.AiThoughts.Count
            };
        }
        catch (Exception ex)
        {
            result = result with { Error = Bounded(ex.ToString()), FailureKind = watch.Stage == "Constructing" ? "SetupException" :
                watch.Stage == "FinalAudit" ? "AuditException" : "NativeCommandException", Status = game is null ? "NotCreated" :
                global::FixtureRuntimeAccess.Engine.GetField("_status", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(game)?.ToString() ?? "Unknown" };
            if (game is not null)
                try
                {
                    journal.Write(new { Kind = "TerminalFaultContext", Utc = DateTimeOffset.UtcNow, Match = identity,
                        Frames = game.ResolutionStack.TakeLast(4).Select(f => new { f.Id, f.Kind, f.Step }).ToArray(),
                        PrivateLocalOnly = true });
                }
                catch { /* Never replace the original match failure with a secondary diagnostic error. */ }
        }
        elapsed.Stop();
        result = result with { Milliseconds = Math.Round(elapsed.Elapsed.TotalMilliseconds, 2) };
        watch.Stop();
        if (traceDirectory is not null && game is not null)
        {
            // Trusted local diagnostics only. These records never reach either AI;
            // a terminal snapshot is not a reconstruction of a decision's input view.
            var path = Path.Combine(traceDirectory, $"pair-{index}-candidate-{candidateSeat}.json");
            using var trace = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            JsonSerializer.Serialize(trace, new
            {
                TrustedLocalOnly = true, Match = identity, Result = result,
                Thoughts = game.AiThoughts, Log = game.Log, Movements = game.CardMovements,
                Events = game.Events.Select(e => new { e.Sequence, Kind = e.Payload.GetType().Name,
                    Fact = JsonSerializer.SerializeToElement(e.Payload, e.Payload.GetType()) }).ToArray()
            });
        }
        journal.Write(new { Kind = "MatchCompleted", Utc = DateTimeOffset.UtcNow, Match = identity, Result = result });
        return result;
    }

    private static ShopAudit AuditShop(GameEngine game, IGameEvent[] facts)
    {
        var generation = NativeGenerated(game);
        foreach (var destroyed in facts.OfType<WushuangShopEquipmentDestroyedEvent>())
        {
            var created = facts.OfType<WushuangShopEquipmentCreatedEvent>().Where(e => e.CardId == destroyed.CardId).ToArray();
            var moves = game.CardMovements.Where(m => m.Sequence == destroyed.MovementSequence && m.CardId == destroyed.CardId &&
                m.From.Zone == CardZoneKind.Equipment && m.To == CardLocation.OutsideGame).ToArray();
            if (created.Length != 1 || moves.Length != 1 || !generation.Contains(destroyed.CardId) ||
                facts.OfType<WushuangShopEquipmentDestroyedEvent>().Count(e => e.CardId == destroyed.CardId) != 1)
                throw new InvalidOperationException("A destroyed shop equipment requires its single native creation and exact real departure.");
        }
        var firstRefresh = facts.OfType<WushuangShopRefreshedEvent>().GroupBy(e => e.Seat).Select(g => g.First()).ToArray();
        if (firstRefresh.Length != 2 || firstRefresh.Any(e => e.RefreshNumber != 1))
            throw new InvalidOperationException("Both native opening pages must start at the same recorded first refresh epoch.");
        var turn = 0;
        var purchases = new Dictionary<(int Turn, int Seat), int>();
        var transcript = new List<object>();
        foreach (var e in facts)
        {
            if (e is TurnStartedEvent started) turn = started.TurnNumber;
            if (e is WushuangOfferBoughtEvent bought && turn > 0)
                purchases[(turn, bought.Seat)] = purchases.GetValueOrDefault((turn, bought.Seat)) + 1;
            if (e is WushuangOfferBoughtEvent or WushuangShopRefreshedEvent or WushuangShopLockChangedEvent or
                WushuangTacticAcquiredEvent or WushuangFreeRerollsChangedEvent)
                transcript.Add(new { Turn = turn, Kind = e.GetType().Name, Fact = JsonSerializer.SerializeToElement(e, e.GetType()) });
        }
        return new(facts.OfType<WushuangTacticAcquiredEvent>().Count(e => e.IsOpening),
            facts.OfType<WushuangOfferBoughtEvent>().Count(), purchases.Count(p => p.Value > 1),
            facts.OfType<WushuangTokensChangedEvent>().Count(e => e.Reason == "refresh"),
            facts.OfType<WushuangFreeRerollsChangedEvent>().Count(e => e.Reason == "refresh" && e.Count >= 0),
            facts.OfType<WushuangShopLockChangedEvent>().Count(), facts.OfType<WushuangShopEquipmentCreatedEvent>().Count(),
            facts.OfType<WushuangShopEquipmentDestroyedEvent>().Count(), facts.OfType<TurnStartedEvent>().Any(), transcript.ToArray());
    }

    private static HashSet<int> NativeGenerated(GameEngine game) => Generated.GetValue(game) is HashSet<int> ids
        ? ids.ToHashSet() : throw new InvalidOperationException("Unknown native entity-generation provenance.");
    private static string StateHash(GameEngine game, string contentHash) => Hash(new
    {
        ContentHash = contentHash,
        Snapshot = SnapshotJson.Serialize(game.CreateSnapshot(-1, true)),
        Zones = game.CreateCardZoneDiagnostics().OrderBy(c => c.CardId).ToArray(),
        Shops = Enumerable.Range(0, 2).Select(game.CreateWushuangSnapshot).ToArray(),
        RandomState = Random.GetValue(game)?.GetType().GetProperty("State")?.GetValue(Random.GetValue(game))
    });
    private static string DecisionHash(GameEngine game) => Hash(game.AiThoughts.Select(t => new { t.Sequence, t.ActorSeat, t.TurnNumber, t.Decision }).ToArray());
    private static string FactsHash(IEnumerable<IGameEvent> facts) => Hash(facts.Where(e => e.GetType().Name.StartsWith("Wushuang", StringComparison.Ordinal))
        .Select(e => new { Kind = e.GetType().Name, Fact = JsonSerializer.SerializeToElement(e, e.GetType()) }).ToArray());
    private static string Hash<T>(T value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value))));
    private static string Bounded(string text) => text.Length <= 16384 ? text : text[..16384] + "\n[truncated at16384 characters]";
    private static double[]? PairedInterval(double[] values)
    {
        if (values.Length < 2) return null;
        var mean = values.Average();
        var half = 1.96 * Math.Sqrt(values.Sum(v => (v - mean) * (v - mean)) / (values.Length * (values.Length - 1d)));
        return [mean - half, mean + half];
    }

    private sealed record PoolSnapshot(string ModeId, string SourceContentHash, IReadOnlyList<string> GeneralPoolIds, int GeneralCandidateCount);
    private sealed class PinnedPoolPackage(ContentRegistry source, PoolSnapshot snapshot) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ai-wushuang-pinned-pool", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.CopyModeFeatureFactoriesFrom(source);
            foreach (var d in source.Cards.Values) b.AddCard(d);
            foreach (var d in source.Skills.Values) b.AddSkill(d);
            foreach (var d in source.Generals.Values) b.AddGeneral(d);
            foreach (var d in source.Decks.Values) b.AddDeck(d);
            foreach (var d in source.Modes.Values) b.AddMode(d.Id == WushuangMode.ModeId
                ? d with { GeneralPoolIds = snapshot.GeneralPoolIds.ToArray(), GeneralCandidateCount = snapshot.GeneralCandidateCount } : d);
        }
    }

    private sealed class SmokePackage(ContentRegistry source) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ai-wushuang-smoke", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.CopyModeFeatureFactoriesFrom(source);
            foreach (var d in source.Cards.Values) b.AddCard(d);
            foreach (var d in source.Skills.Values) b.AddSkill(d);
            foreach (var d in source.Generals.Values) b.AddGeneral(d);
            foreach (var d in source.Decks.Values) b.AddDeck(d);
            var mode = source.Modes[WushuangMode.ModeId];
            var pool = SmokeGenerals.Where(id => mode.GeneralPoolIds!.Contains(id, StringComparer.Ordinal)).ToArray();
            if (pool.Length != SmokeGenerals.Length)
                throw new ArgumentException("The named Wushuang smoke roster no longer satisfies actual production eligibility; revise this explicit profile before evaluation.");
            foreach (var d in source.Modes.Values) b.AddMode(d.Id == WushuangMode.ModeId ? d with { GeneralPoolIds = pool, GeneralCandidateCount = pool.Length } : d);
        }
    }

    private sealed class Journal : IDisposable
    {
        private readonly FileStream stream;
        private readonly StreamWriter writer;
        private readonly object gate = new();
        internal Journal(string path) { stream = new(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read); writer = new(stream, new UTF8Encoding(false)); }
        internal void Write(object value) { lock (gate) { writer.WriteLine(JsonSerializer.Serialize(value)); writer.Flush(); stream.Flush(true); } }
        public void Dispose() { lock (gate) { writer.Dispose(); stream.Dispose(); } }
    }
    private sealed class MatchWatch : IDisposable
    {
        private readonly Journal journal;
        private readonly object identity;
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly Timer timer;
        private GameEngine? game;
        private int samples;
        internal string Stage = "Constructing";
        internal MatchWatch(Journal journal, object identity) { this.journal = journal; this.identity = identity; timer = new(_ => Sample(), null, 5000, 5000); }
        internal void Attach(GameEngine value) => Volatile.Write(ref game, value);
        private void Sample()
        {
            if (Interlocked.Increment(ref samples) > 240) { timer.Change(Timeout.Infinite, Timeout.Infinite); return; }
            try
            {
                var g = Volatile.Read(ref game);
                // Approximate trusted scalar diagnostics only. Never project a view,
                // enumerate mutable zones or submit clock commands from this timer.
                var type = typeof(GameEngine);
                object? Read(string name) => g is null ? null : type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(g);
                journal.Write(new { Kind = "Progress", Utc = DateTimeOffset.UtcNow, Match = identity, Stage,
                    ElapsedMilliseconds = clock.ElapsedMilliseconds, Turn = Read("_turnNumber"), CurrentSeat = Read("_currentSeat"),
                    MovementSequence = Read("_movementSequence"), ThoughtSequence = Read("_thoughtSequence"),
                    Limit = "Approximate uncommitted scalar observation; not an outcome, replay snapshot or brain input." });
            }
            catch { /* Diagnostics must not affect the game command. */ }
        }
        internal void Stop() { using var done = new ManualResetEvent(false); if (timer.Dispose(done)) done.WaitOne(); }
        public void Dispose() => timer.Dispose();
    }
    private sealed record Seat(int SeatId, string GeneralId, string TeamId, int InitialHp, int InitialMaxHp,
        IReadOnlyList<int> InitialHandCardIds, int Policy, int AiBrainSeed, int NativeOpeningPageSeed);
    private sealed record TerminalSeat(int SeatId, string? TeamId, int Hp, int MaxHp, bool IsAlive);
    private sealed record ShopAudit(int OpeningChoices, int Purchases, int MultiplePurchaseWindows, int PaidRefreshes,
        int FreeRefreshes, int LockChanges, int EquipmentCreated, int EquipmentDestroyed, bool BattleStarted, IReadOnlyList<object> Choices);
    private sealed record Pair(int Index, int Seed, bool Complete, bool SameInitialState, bool? AAInvariant,
        double? CandidateMeanUtility, double? BaselineMeanUtility);
    private sealed record Result
    {
        public string Profile { get; init; } = "";
        public int Index { get; init; }
        public int Seed { get; init; }
        public int CandidateSeat { get; init; }
        public int HostPolicy { get; init; }
        public IReadOnlyList<Seat> Roster { get; init; } = [];
        public IReadOnlyList<TerminalSeat> FinalPlayers { get; init; } = [];
        public string? InitialStateHash { get; init; }
        public string? FinalStateHash { get; init; }
        public string? DecisionHash { get; init; }
        public string? ShopFactsHash { get; init; }
        public bool Success { get; init; }
        public string Status { get; init; } = "NotCreated";
        public string Winner { get; init; } = "None";
        public int? RawWinnerSeat { get; init; }
        public string? WinnerTeamId { get; init; }
        public int? WinnerSeat { get; init; }
        public int Turns { get; init; }
        public double Milliseconds { get; init; }
        public bool StartAccepted { get; init; }
        public bool InventoryPreserved { get; init; }
        public bool Settled { get; init; }
        public bool ClockUntouched { get; init; }
        public bool SafetyGuardDraw { get; init; }
        public string? EndCondition { get; init; }
        public bool? EndConditionVerified { get; init; }
        public bool? SurvivalLimitCurrentHpVerified { get; init; }
        public string? DrawKind { get; init; }
        public int InitialPhysicalCards { get; init; }
        public int FinalPhysicalCards { get; init; }
        public int NewGeneratedPhysicalCards { get; init; }
        public double? CandidateUtility { get; init; }
        public double? BaselineUtility { get; init; }
        public ShopAudit? Shop { get; init; }
        public int Events { get; init; }
        public int Thoughts { get; init; }
        public string? FailureKind { get; init; }
        public string? Error { get; init; }
    }
}
