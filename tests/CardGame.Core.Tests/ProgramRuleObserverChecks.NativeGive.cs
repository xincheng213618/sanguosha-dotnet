using System.Text.Json;
using CardGame.Core;

internal static partial class ProgramRuleObserverChecks
{
    private static void NativeGiveScopeUsesTheOriginalReentrantBoundary()
    {
        var closedInsideGive = new NativeGiveScopeFixture(issueInsideEnd: false);
        closedInsideGive.IssueGive();
        Require(closedInsideGive.Pending is [TurnEndedEvent, NativeCardsGivenEvent] &&
                closedInsideGive.Gives.Read().Count == 1 &&
                closedInsideGive.Gives.GiftsForCurrentScope(0, 1, 0).Count == 0,
            "A Give accepted before a nested turn-end boundary expires even when the original Give reaches the journal after that End.");

        var issuedInsideEnd = new NativeGiveScopeFixture(issueInsideEnd: true);
        var end = new TurnEndedEvent(1, 0);
        var equalScalar = new TurnEndedEvent(1, 0);
        var scalarHash = end.GetHashCode();
        issuedInsideEnd.Runtime.AdvanceFact(end);
        Require(issuedInsideEnd.Pending is [NativeCardsGivenEvent, TurnEndedEvent] &&
                issuedInsideEnd.Gives.GiftsForCurrentScope(0, 1, 0).Single() ==
                    issuedInsideEnd.Gives.Read().Single(),
            "A Give issued after BeforeEvent35700 belongs to the new scope even while its original TurnEnded fact has not entered the journal.");
        Require(end == equalScalar && end.GetHashCode() == scalarHash,
            "The captured internal Give boundary does not change TurnEnded scalar equality or hashing.");

        var frozen = (TurnEndedEvent)CommittedEventProjection.Freeze(end);
        Require(frozen.NativeGiveHistoryCutoff == end.NativeGiveHistoryCutoff &&
                frozen == equalScalar && frozen.GetHashCode() == scalarHash &&
                JsonSerializer.Serialize(frozen) == JsonSerializer.Serialize(equalScalar),
            "Commit retains the original scope cutoff without exposing internal metadata or changing public event values.");
        var originalRejected = false;
        var committedRejected = false;
        try { end.StampNativeGiveHistoryCutoff(10); }
        catch (InvalidOperationException) { originalRejected = true; }
        try { frozen.StampNativeGiveHistoryCutoff(10); }
        catch (InvalidOperationException) { committedRejected = true; }
        Require(originalRejected && committedRejected,
            "Commit seals both the original End and its committed projection against another boundary stamp.");
    }

    // The fixture supplies only queue output and otherwise uses the real rule
    // progression stack, observer ordering, source scope capture and query.
    private sealed class NativeGiveScopeFixture : IRuleProgressionOperations, IAdvancingRuleFacts
    {
        private readonly bool _issueInsideEnd;
        private readonly ProgramEventHistoryIndex _history;
        private readonly NativeGiveScopeRuntime _scopes;
        internal List<IGameEvent> Pending { get; } = [];
        internal RuleProgressionRuntime Runtime { get; }
        internal NativeGivenCardQueries Gives { get; }
        public IEnumerable<IGameEvent> Facts => Runtime.AdvancingFacts;

        internal NativeGiveScopeFixture(bool issueInsideEnd)
        {
            _issueInsideEnd = issueInsideEnd;
            _history = new([], Pending);
            _scopes = new(new MatchNativeFactDemand(ProgramTrackedFact.NativeGive), _history, _history, this);
            Gives = new(_history, this, _scopes);
            var catalog = ProgramRuleObserverCatalog.Discover(
                Methods(typeof(NativeGiveScopeFixture)).Concat(Methods(typeof(NativeGiveScopeRuntime))));
            var observers = new ProgramRuleObservers(catalog, (method, type) => method.CreateDelegate(type,
                method.DeclaringType == typeof(NativeGiveScopeRuntime) ? _scopes : this));
            Runtime = new(observers, this);
        }

        internal void IssueGive()
        {
            var move = new CardMovementRecord(1, 1, 2, CardKind.Slash,
                CardLocation.Hand(0), CardLocation.Hand(1), new("fixture.scope-give"));
            var receipt = new NativeGiveFact(3, 4, 0, 1, 1, 0, move.Sequence)
            {
                Movements = [move],
                ScopeCutoffAtIssue = _scopes.CurrentCutoff(1, 0)
            };
            Runtime.AdvanceFact(new NativeCardsGivenEvent(3, 4, 0, 1, 1, 1, 0, move.Sequence)
                { Receipt = receipt });
        }

        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 35701)]
        private void InsertNestedFact(IGameEvent payload)
        {
            if (!_issueInsideEnd && payload is NativeCardsGivenEvent)
                Runtime.AdvanceFact(new TurnEndedEvent(1, 0));
            else if (_issueInsideEnd && payload is TurnEndedEvent)
                IssueGive();
        }

        [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 1)]
        private void After() { }
        [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 1)]
        private void BeforePublish() { }
        public void ApplySkillStateDeclarations(IGameEvent payload, SkillStateBoundaryTiming timing) { }
        public void QueueFact(IGameEvent payload) => _history.Append(payload);
        public bool BeginCharacterStateWindow() => false;
        public bool BeginHpChangedWindow() => false;
        public bool BeginCardsMovedWindow() => false;
        public void RequestStatePublication() { }
    }
}
