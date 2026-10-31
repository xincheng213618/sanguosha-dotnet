using System.Reflection;
using CardGame.Core;

internal static class ProgramRuleObserverChecks
{
    public static void RegistrationPreservesOrderReentrancyAndIsolation()
    {
        var methods = Methods(typeof(ObserverFixture));
        var catalog = ProgramRuleObserverCatalog.Discover(methods.Reverse());
        var first = new ObserverFixture(catalog);
        var second = new ObserverFixture(catalog);
        first.Raise(new TurnStartedEvent(1, 0));
        Require(first.Trace.SequenceEqual(new[]
        {
            "first:1", "first:2", "sync", "last:TurnStartedEvent", "queued:TurnStartedEvent", "after:2",
            "sync", "last:TurnStartedEvent", "queued:TurnStartedEvent", "after:1"
        }), "Nested facts must completely settle in explicit registration order across the queue boundary.");
        Require(second.Trace.Count == 0, "Shared registration metadata must not retain another match's bound callbacks.");

        second.Raise(new GameStartedEvent(2));
        second.Observers.BeforeStatePublish();
        Require(second.Trace.SequenceEqual(new[] { "sync", "last:GameStartedEvent", "queued:GameStartedEvent", "sync" }),
            "Typed event callbacks must filter facts while parameterless publication rules run only in their registered phases.");
        Require(catalog.Phases.Values.All(items => items is IList<ProgramRuleObserverRegistration> list && list.IsReadOnly),
            "Published static registration lists must be immutable.");

        first.Trace.Clear();
        first.Failure = new InvalidOperationException("fixture rule failure");
        try
        {
            first.Raise(new GameStartedEvent(2));
            throw new InvalidOperationException("Expected the rule failure to propagate before queueing the fact.");
        }
        catch (InvalidOperationException error) when (ReferenceEquals(error, first.Failure))
        {
            Require(first.Trace.SequenceEqual(new[] { "sync", "last:GameStartedEvent" }),
                "A failing pre-fact rule must preserve its exception and prevent the queue and post-fact rules.");
        }

        // Discover the actual built-ins too: a dropped phase or duplicate order
        // must fail at construction, before a command mutates match state.
        _ = ProgramRuleObserverCatalog.Discover(ProgramRuntimeModuleCatalog.Default.Methods);
    }

    private static MethodInfo[] Methods(Type type) => type.GetMethods(
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class ObserverFixture
    {
        internal List<string> Trace { get; } = [];
        internal InvalidOperationException? Failure { get; set; }
        internal ProgramRuleObservers Observers { get; }

        internal ObserverFixture(ProgramRuleObserverCatalog catalog) =>
            Observers = new(catalog, (method, type) => method.CreateDelegate(type, this));

        internal void Raise(IGameEvent payload)
        {
            Observers.BeforeEvent(payload);
            Trace.Add($"queued:{payload.GetType().Name}");
            Observers.AfterEvent(payload);
        }

        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 20)]
        private void Last(IGameEvent payload)
        {
            Trace.Add($"last:{payload.GetType().Name}");
            if (Failure is { } error) throw error;
        }

        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 10)]
        private void First(TurnStartedEvent started)
        {
            Trace.Add($"first:{started.TurnNumber}");
            if (started.TurnNumber == 1) Raise(started with { TurnNumber = 2 });
        }

        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 15)]
        [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 10)]
        private void Synchronize() => Trace.Add("sync");

        [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 10)]
        private void After(TurnStartedEvent ended) => Trace.Add($"after:{ended.TurnNumber}");
    }
}
