using System.Reflection;
using CardGame.Core;

internal static class MatchModeRuntimeChecks
{
    public static void ScopedObserversAreNotBoundInOtherMatches()
    {
        var methods = typeof(ObserverFixture).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        var catalog = ProgramRuleObserverCatalog.Discover(methods);
        var ordinary = new ObserverFixture();
        var scopedBindings = 0;
        var ordinaryObservers = new ProgramRuleObservers(catalog, (method, type) =>
        {
            Require(method.Name != nameof(ObserverFixture.OnlySolo), "An ordinary match must never bind the optional feature.");
            return method.CreateDelegate(type, ordinary);
        }, "fixture:identity");
        var solo = new ObserverFixture();
        var soloObservers = new ProgramRuleObservers(catalog, (method, type) =>
        {
            if (method.Name == nameof(ObserverFixture.OnlySolo)) scopedBindings++;
            return method.CreateDelegate(type, solo);
        }, "fixture:solo");
        ordinaryObservers.BeforeEvent(new GameStartedEvent(2));
        ordinaryObservers.AfterEvent(new GameStartedEvent(2));
        ordinaryObservers.BeforeStatePublish();
        soloObservers.BeforeEvent(new GameStartedEvent(2));
        soloObservers.AfterEvent(new GameStartedEvent(2));
        soloObservers.BeforeStatePublish();
        Require(scopedBindings == 1 && ordinary.Trace.SequenceEqual(new[] { "before", "after", "state" }) &&
            solo.Trace.SequenceEqual(new[] { "before", "solo", "after", "state" }),
            "A shared immutable observer catalog must activate only each match's rules in their original order.");
    }

    private sealed class ObserverFixture
    {
        internal List<string> Trace { get; } = [];
        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 1)]
        private void Before() => Trace.Add("before");
        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 2, "fixture:solo")]
        internal void OnlySolo() => Trace.Add("solo");
        [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 1)]
        private void After() => Trace.Add("after");
        [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 1)]
        private void State() => Trace.Add("state");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
