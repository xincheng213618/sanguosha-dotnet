using System.Reflection;
using CardGame.Core;

internal static class ModeFlowExtensionChecks
{
    public static void NativeAndExternalRulesShareOrderedPipeline()
    {
        var core = new CoreObservers();
        var catalog = ProgramRuleObserverCatalog.Discover(Methods(typeof(CoreObservers)));
        ProgramRuleObservers pipeline = null!;
        var provider = new ObserverProvider
        {
            RuleObservers = new List<ModeRuleObserverRegistration>
            {
                ModeRuleObserverRegistration.ForEvent<Fact>(ProgramRulePhase.AfterEvent, 5, fact =>
                {
                    core.Trace.Add($"mode-after:{fact.Number}");
                    if (fact.Number == 1) { pipeline.BeforeEvent(new Fact(2)); pipeline.AfterEvent(new Fact(2)); }
                }),
                ModeRuleObserverRegistration.ForEvent<Fact>(ProgramRulePhase.AfterEvent, 15,
                    fact => core.Trace.Add($"mode-tail:{fact.Number}")),
                ModeRuleObserverRegistration.BeforeStatePublish(5, () => core.Trace.Add("mode-state"))
            }
        };
        var runtime = new MatchModeRuntime(provider);
        ((List<ModeRuleObserverRegistration>)provider.RuleObservers).Clear();
        pipeline = new(catalog, (method, type) => method.CreateDelegate(type, core), additionalObservers: runtime.RuleObservers);
        pipeline.BeforeEvent(new Fact(1));
        pipeline.AfterEvent(new Fact(1));
        pipeline.BeforeStatePublish();
        Require(core.Trace.SequenceEqual(["core-before:1", "mode-after:1", "core-before:2", "mode-after:2", "core-after:2",
            "mode-tail:2", "core-after:1", "mode-tail:1", "mode-state", "core-state"]),
            "External rules merge into the native priority chain, preserve nested depth-first execution and freeze provider registration lists.");
        Reject<InvalidOperationException>(() => _ = new ProgramRuleObservers(catalog,
            (method, type) => method.CreateDelegate(type, core), additionalObservers:
            [ModeRuleObserverRegistration.ForEvent<Fact>(ProgramRulePhase.AfterEvent, 10, _ => { })]),
            "A mode observer cannot reuse an active native phase/order.");
        Reject<InvalidOperationException>(() => _ = new MatchModeRuntime(new ObserverProvider
        {
            RuleObservers = [ModeRuleObserverRegistration.BeforeStatePublish(8, () => { }),
                ModeRuleObserverRegistration.BeforeStatePublish(8, () => { })]
        }), "A provider cannot install ambiguous duplicate observer priorities.");
    }

    public static void NativeAndExternalRuntimeHooksShareOrderedScheduler()
    {
        var core = new CoreHooks();
        var catalog = ProgramRuntimeHookCatalog.Discover(Methods(typeof(CoreHooks)));
        var stop = true;
        var registrations = new HookProvider
        {
            RuntimeHooks =
            [
                ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 30,
                    () => { core.Trace.Add("mode-stage-30"); return stop; }),
                ModeRuntimeHookRegistration.ForPermission(ProgramRuntimePhase.AfterOwnerDeath, 5,
                    frame => { core.Trace.Add($"mode-permission:{frame.Id}"); return true; })
            ]
        };
        var runtime = new MatchModeRuntime(registrations);
        var hooks = new ProgramRuntimeHooks(catalog, (method, type) => method.CreateDelegate(type, core), runtime.RuntimeHooks);
        Require(hooks.TryAdvance(ProgramRuntimePhase.BeforeDeclaration) && core.Trace.SequenceEqual(["core-first", "mode-stage-30"]),
            "An external encounter claims its original scheduler priority and suspends later native handlers.");
        core.Trace.Clear(); stop = false;
        Require(!hooks.TryAdvance(ProgramRuntimePhase.BeforeDeclaration) && core.Trace.SequenceEqual(["core-first", "mode-stage-30", "core-last"]),
            "A declined external claim continues the same native scheduler without a second progression loop.");
        core.Trace.Clear();
        var frame = new ProgramSkillFrame(7, 0, "test:skill", "test:binding", "test-hash", 0, [], []);
        Require(hooks.CanContinue(ProgramRuntimePhase.AfterOwnerDeath, frame) && core.Trace.SequenceEqual(["mode-permission:7"]),
            "Permission hooks receive the exact owning frame and preserve their native short-circuit phase.");
        Reject<InvalidOperationException>(() => _ = new ProgramRuntimeHooks(catalog,
            (method, type) => method.CreateDelegate(type, core),
            [ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.BeforeDeclaration, 10, () => false)]),
            "An external continuation cannot reuse an active native priority.");
        Reject<ArgumentException>(() => ModeRuntimeHookRegistration.ForAdvance(ProgramRuntimePhase.AfterOwnerDeath, 3, () => false),
            "Permission and progression signatures are distinct contracts.");
        Reject<ArgumentException>(() => ModeRuntimeHookRegistration.ForPermission(ProgramRuntimePhase.BeforeDeclaration, 3, _ => false),
            "A progression phase cannot silently become a frame-permission handler.");
        var ordered = new List<int>();
        var boundaries = new MatchModeRuntime(new Opening<int>(20, () => { ordered.Add(20); return false; }),
            new Opening<string>(10, () => { ordered.Add(10); return false; }));
        Require(!boundaries.TryBeginOpening() && ordered.SequenceEqual([10, 20]),
            "Public opening boundaries use explicit deterministic order rather than factory discovery order.");
        Reject<InvalidOperationException>(() => _ = new MatchModeRuntime(new Opening<int>(0, () => false), new Opening<string>(0, () => false)),
            "Two opening handlers must not ambiguously own the same priority.");
    }

    private sealed record Fact(int Number) : IGameEvent;
    private sealed class ObserverProvider : IMatchModeRuleObserverProvider { public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; init; } = []; }
    private sealed class HookProvider : IMatchModeRuntimeHookProvider { public IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; init; } = []; }
    private sealed class Opening<T>(int order, Func<bool> callback) : IMatchModeOpeningBoundary
    { public int Order => order; public bool TryBeginOpening() => callback(); }
    private sealed class CoreObservers
    {
        internal List<string> Trace { get; } = [];
        [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 10)] private void Before(Fact fact) => Trace.Add($"core-before:{fact.Number}");
        [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 10)] private void After(Fact fact) => Trace.Add($"core-after:{fact.Number}");
        [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 10)] private void State() => Trace.Add("core-state");
    }
    private sealed class CoreHooks
    {
        internal List<string> Trace { get; } = [];
        [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 10)]
        [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 10)]
        [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 10)]
        [ProgramRuntimeHook(ProgramRuntimePhase.BeforeJudgmentPhase, 10)]
        [ProgramRuntimeHook(ProgramRuntimePhase.BeforeRecoveryApply, 10)]
        [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDamageApply, 10)]
        private bool First() { Trace.Add("core-first"); return false; }
        [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 40)] private bool Last() { Trace.Add("core-last"); return false; }
        [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 10)]
        [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 10)]
        private bool Permission(ProgramSkillFrame frame) { Trace.Add($"core-permission:{frame.Id}"); return false; }
    }
    private static MethodInfo[] Methods(Type type) => type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
    private static void Reject<T>(Action action, string message) where T : Exception
    { try { action(); } catch (T) { return; } throw new InvalidOperationException(message); }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
