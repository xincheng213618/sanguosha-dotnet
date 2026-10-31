using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class CardMovementPipelineChecks
{
    public static void BatchCoordinatorPreservesEnumerationSettlementAndFailureBoundaries()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var match = (MatchRuntime)typeof(GameEngine).GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = match.Capabilities.Get<IProgramRuntimeFrames>();
        var parent = new ProgramSkillFrame(frames.NextId(), 0, "fixture:batch-parent", "run", "hash", 0, [], [])
        { PendingMovementContinuation = new(0, 0, null) };
        frames.Push(parent);
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck([new Card(1, CardKind.Slash, Suit.Spade, 7), new Card(2, CardKind.Dodge, Suit.Heart, 8)]);
        zones.MoveAll(CardLocation.DrawPile, CardLocation.Hand(0));
        var store = new CardMovementBatchStore();
        var facts = new BatchFacts(frames) { TurnNumber = 3, SetupComplete = true };
        var calls = new List<string>();
        ProgramSkillFrame? destinationOrigin = null;
        var pipeline = new BatchPipeline
        {
            OnBegin = batch =>
            {
                Require(store.ActiveBatchId is null, "Begin metadata must run before the batch becomes active.");
                calls.Add("begin");
                batch.WenhePoisonRound = 6;
            },
            OnObserve = (batch, movement) =>
            {
                Require(store.ActiveBatchId is null, "Committed observations must run after popping the active batch.");
                calls.Add("observe");
            },
            OnCapture = (batch, movements) =>
            {
                calls.Add("facts");
                zones.Move(2, CardLocation.Hand(0), CardLocation.Hand(1));
            },
            OnComplete = (batch, completed) =>
            {
                calls.Add("complete");
                Require(completed.SourceCounts.Single().CountAfter == 1 && completed.DestinationCounts!.Single().CountAfter == 2,
                    "Source after-counts precede fact hooks; destination after-counts follow them.");
                return completed with { MovementTiming = new(0, TurnPhase.Draw, 1) };
            }
        };
        var coordinator = new CardMovementBatchCoordinator(frames, zones, facts, store, pipeline,
            match.Capabilities.Get<IEquipmentMovementCapture>());
        IEnumerable<CardLocation> Sources()
        {
            Require(frames.NextId() == parent.Id + 1, "Source enumeration must precede batch identity allocation.");
            yield return CardLocation.Hand(0);
            yield return CardLocation.Hand(0);
        }
        IEnumerable<CardLocation> Destinations()
        {
            destinationOrigin = new(frames.NextId(), 1, "fixture:batch-destination", "run", "hash", 0, [], []);
            Require(destinationOrigin.Id == parent.Id + 3, "Destination enumeration must follow batch identity allocation.");
            frames.Push(destinationOrigin);
            facts.TurnNumber = 4;
            zones.Move(1, CardLocation.Hand(0), CardLocation.Hand(1));
            yield return CardLocation.Hand(1);
        }
        var batch = coordinator.Begin(Sources(), Destinations());
        Require(batch.Id == parent.Id + 2 && batch.ParentFrameId == parent.Id && batch.AwaitingProgramFrameId == parent.Id &&
                ReferenceEquals(batch.OriginProgram, destinationOrigin) && batch.TurnNumber == 3 &&
                batch.SourceCountsBefore.Single().Value == 2 && batch.DestinationCountsBefore.Single().Value == 1 &&
                batch.WenhePoisonRound == 6 && store.ActiveBatchId == batch.Id,
            "Begin must preserve interleaved input enumeration, original identities, counts and origin read positions.");
        try { batch.WenhePoisonRound = 7; throw new Exception("Start metadata remained writable after begin."); }
        catch (InvalidOperationException) { }
        var movements = new List<CardMovementRecord>
        {
            new(1, 3, 1, CardKind.Slash, CardLocation.Hand(0), CardLocation.Hand(1), CardMoveReasons.Draw)
        };
        coordinator.Complete(batch, movements, true);
        var completed = store.PendingBatches.Single();
        movements.Clear();
        Require(calls.SequenceEqual(["begin", "observe", "facts", "complete"]) && completed.Movements.Count == 1 &&
                completed.MovementTiming?.PhaseActorSeat == 1 && store.ActiveBatchId is null,
            "Completion must preserve ordered hooks, the folded context and the frozen movement list.");

        pipeline.OnBegin = _ => { };
        calls.Clear();
        facts.SetupComplete = false;
        var setup = coordinator.Begin([], []);
        coordinator.Complete(setup, completed.Movements, true);
        Require(calls.SequenceEqual(["observe"]) && store.PendingBatches.Count == 1,
            "Committed observations must still run before the setup guard without publishing another batch.");
        var outer = coordinator.Begin([], []);
        var inner = coordinator.Begin([], []);
        Require(inner.ParentBatchId == outer.Id, "Nested batches must use the original store's active parent.");
        try { coordinator.Complete(outer, [], false); throw new Exception("A mismatched active batch was accepted."); }
        catch (InvalidOperationException) { }
        Require(store.ActiveBatchId == outer.Id, "A mismatched completion must pop before it throws.");
        coordinator.Complete(outer, [], false);
        Require(store.ActiveBatchId is null && store.PendingBatches.Count == 1,
            "Aborted batches must unwind without observations, fact captures or publication.");
    }

    public static void OrderedProvidersPreserveFirstMatchAndHandledEmptyIndexes()
    {
        BatchCoordinatorPreservesEnumerationSettlementAndFailureBoundaries();
        var replacedPhases = new[]
        {
            ProgramPipelinePhase.CollectMovementCandidates,
            ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate,
            ProgramPipelinePhase.CreateMovementContext,
            ProgramPipelinePhase.IsProgramMovementSettlement,
            ProgramPipelinePhase.SelectLegacyMovementIndexes,
            ProgramPipelinePhase.SelectLegacyDiscardIndexes
        };
        var productionMethods = ProgramRuntimeModuleCatalog.Default.Methods;
        var fixtureMethods = typeof(Fixture).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
        var methods = productionMethods.Where(method => !method.GetCustomAttributes<ProgramPipelineHookAttribute>()
                .Any(attribute => replacedPhases.Contains(attribute.Phase)))
            .Concat(fixtureMethods).ToArray();
        var catalog = ProgramPipelineHookCatalog.Discover(methods);
        var reversed = ProgramPipelineHookCatalog.Discover(methods.Reverse());
        foreach (var phase in replacedPhases)
            Require(catalog.Phases[phase].Select(item => item.Method)
                    .SequenceEqual(reversed.Phases[phase].Select(item => item.Method)),
                "Explicit movement-provider order must be independent of metadata enumeration order.");

        var content = StandardContentRegistry.Create();
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, content);
        var hooks = new ProgramPipelineHooks(catalog, game);
        var trigger = content.Skills.Values.SelectMany(skill => skill.Program?.Triggers ?? []).First();
        var batch = new CardMovementBatchContext(10, null, null, 1, [], []);
        var window = new CardsMovedTriggerWindowFrame(batch.Id, batch, []);
        var collected = new List<ProgramTriggerCandidate>();
        hooks.CollectMovementCandidates(batch, collected);
        Require(collected.Select(candidate => candidate.BindingId).SequenceEqual(["first", "second"]),
            "Movement collectors must append in explicit order, preserving the shared candidate list.");

        foreach (var binding in new[] { "first", "second", "none" })
        {
            var candidate = Candidate(binding);
            Require(hooks.ExcludeOrdinaryMovementCandidate(candidate, trigger) == (binding != "none"),
                "Ordinary movement exclusions must stop at the first true provider and default to false.");
            var context = hooks.CreateMovementContext(window, candidate);
            Require(binding == "none" ? context is null : context?.Amount == (binding == "first" ? 1 : 2),
                "Movement contexts must use the first non-null provider, with null allowing later providers.");
            var frame = new ProgramSkillFrame(binding == "first" ? 1 : binding == "second" ? 2 : 3,
                0, "fixture:movement-pipeline", "run", "hash", 0, [], []);
            Require(hooks.IsProgramMovementSettlement(frame, batch) == (binding != "none"),
                "Program movement settlement must stop at its first true proof and default to false.");
            AssertIndexes(hooks.SelectLegacyMovementIndexes(batch, candidate, trigger, CardLocation.Hand(0)), binding);
            AssertIndexes(hooks.SelectLegacyDiscardIndexes(batch, candidate, trigger), binding);
        }

    }

    private static ProgramTriggerCandidate Candidate(string binding) =>
        new(0, "fixture:movement-pipeline", binding, "fixture-instance", "hash", 0);

    private static void AssertIndexes(int[]? indexes, string binding) =>
        Require(binding == "none" ? indexes is null : binding == "first" ? indexes is { Length: 0 } :
                indexes is [7],
            "A null index result must permit the next provider; an empty array must count as handled and stop it.");

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class BatchFacts(IProgramRuntimeFrames frames) : ICardMovementBatchFacts
    {
        public int TurnNumber { get; set; }
        public bool SetupComplete { get; set; }
        public Winner Winner => Winner.None;
        public EngineStatus Status => EngineStatus.Running;
        public ProgramSkillFrame? LastProgram => frames.OfType<ProgramSkillFrame>().LastOrDefault();
    }

    private sealed class BatchPipeline : ICardMovementBatchPipeline
    {
        public Action<CardMovementBatchBuilder>? OnBegin { get; set; }
        public Action<CardMovementBatchBuilder, CardMovementRecord>? OnObserve { get; init; }
        public Action<CardMovementBatchBuilder, IReadOnlyList<CardMovementRecord>>? OnCapture { get; init; }
        public Func<CardMovementBatchBuilder, CardMovementBatchContext, CardMovementBatchContext>? OnComplete { get; init; }
        public void Begin(CardMovementBatchBuilder batch) => OnBegin?.Invoke(batch);
        public void ObserveCommitted(CardMovementBatchBuilder batch, CardMovementRecord movement) => OnObserve?.Invoke(batch, movement);
        public void CaptureFacts(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => OnCapture?.Invoke(batch, movements);
        public CardMovementBatchContext Complete(CardMovementBatchBuilder batch, CardMovementBatchContext completed) =>
            OnComplete?.Invoke(batch, completed) ?? completed;
    }

    [ProgramRuntimeModule]
    private static class Fixture
    {
        [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 100)]
        private static void ZCollectFirst(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) =>
            result.Add(Candidate("first"));

        [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 200)]
        private static void ACollectSecond(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result)
        {
            Require(result is [var first] && first.BindingId == "first", "The earlier collector must have run first.");
            result.Add(Candidate("second"));
        }

        [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 100)]
        private static bool ZExcludeFirst(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) =>
            candidate.BindingId == "first";

        [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 200)]
        private static bool AExcludeSecond(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
        {
            if (candidate.BindingId == "first") throw new InvalidOperationException("A handled exclusion reached a later provider.");
            return candidate.BindingId == "second";
        }

        [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 100)]
        private static ProgramSkillWindowContext? ZContextFirst(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) =>
            candidate.BindingId == "first" ? new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, Amount: 1) : null;

        [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 200)]
        private static ProgramSkillWindowContext? AContextSecond(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
        {
            if (candidate.BindingId == "first") throw new InvalidOperationException("A handled context reached a later provider.");
            return candidate.BindingId == "second" ? new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, Amount: 2) : null;
        }

        [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 100)]
        private static bool ZSettlementFirst(ProgramSkillFrame frame, CardMovementBatchContext batch) => frame.Id == 1;

        [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 200)]
        private static bool ASettlementSecond(ProgramSkillFrame frame, CardMovementBatchContext batch)
        {
            if (frame.Id == 1) throw new InvalidOperationException("A proved settlement reached a later provider.");
            return frame.Id == 2;
        }

        [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyMovementIndexes, 100)]
        private static int[]? ZMovementFirst(CardMovementBatchContext batch, ProgramTriggerCandidate candidate,
            SkillProgramTrigger trigger, CardLocation location) => candidate.BindingId == "first" ? [] : null;

        [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyMovementIndexes, 200)]
        private static int[]? AMovementSecond(CardMovementBatchContext batch, ProgramTriggerCandidate candidate,
            SkillProgramTrigger trigger, CardLocation location) => LaterIndexes(candidate);

        [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 100)]
        private static int[]? ZDiscardFirst(CardMovementBatchContext batch, ProgramTriggerCandidate candidate,
            SkillProgramTrigger trigger) => candidate.BindingId == "first" ? [] : null;

        [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 200)]
        private static int[]? ADiscardSecond(CardMovementBatchContext batch, ProgramTriggerCandidate candidate,
            SkillProgramTrigger trigger) => LaterIndexes(candidate);

        private static int[]? LaterIndexes(ProgramTriggerCandidate candidate)
        {
            if (candidate.BindingId == "first") throw new InvalidOperationException("A handled empty index result reached a later provider.");
            return candidate.BindingId == "second" ? [7] : null;
        }
    }
}
