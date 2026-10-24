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

    public static void ControlledHostsAndWildcardFactoryQueries()
    {
        var created = 0;
        var hosts = new ModeHostRegistry().Register<IHost>(() => { created++; return new Host(); });
        var registry = ContentRegistry.Build(new WildcardPackage());
        var options = new GameOptions { Seed = 19 };
        var mode = new ContentModeDefinition("test:custom-mode", "测试", 4, 4, new Dictionary<string, int>());
        var context = new ModeFeatureCreationContext(options, mode, registry, hosts);
        Require(ReferenceEquals(context.Options, options) && ReferenceEquals(context.ModeDefinition, mode) && context.Seed == 19 &&
            ReferenceEquals(context.GetHost<IHost>(), context.GetHost<IHost>()) && created == 1 &&
            ReferenceEquals(context.GetHost<IHost>(), hosts.Get<IHost>()),
            "Factories receive actual match configuration and reuse one explicitly registered host instance per match.");
        var otherHosts = new ModeHostRegistry().Register<IHost>(() => new Host());
        Require(!ReferenceEquals(hosts.Get<IHost>(), otherHosts.Get<IHost>()), "A host cache never crosses match boundaries.");
        Reject<InvalidOperationException>(() => hosts.Register<IOtherHost>(() => new OtherHost()),
            "The host whitelist freezes before a feature can resolve native services.");
        Reject<InvalidOperationException>(() => new ModeHostRegistry().Register<IHost>(() => new Host()).Register<IHost>(() => new Host()),
            "Duplicate host contracts cannot replace each other's native implementation.");
        var cyclic = new ModeHostRegistry();
        cyclic.Register<IHost>(() => cyclic.Get<IHost>());
        Reject<InvalidOperationException>(() => cyclic.Get<IHost>(), "Cyclic host construction fails explicitly.");
        Reject<ArgumentException>(() => new ModeHostRegistry().Register<IReadOnlyList<ResolutionFrame>>(() => Array.Empty<ResolutionFrame>()),
            "The resolution stack itself is not a dedicated mode host.");
        var oldCalls = 0;
        var legacy = new ModeFeatureCreationContext(19, registry, _ => { oldCalls++; return new Host(); });
        Require(ReferenceEquals(legacy.GetHost<IHost>(), legacy.GetHost<IHost>()) && oldCalls == 1 &&
            legacy.Options is null && legacy.ModeDefinition is null,
            "The existing three-argument factory context remains compatible and now caches its dedicated resolver result.");
        Require(registry.ModeFeatureFactories.Single().ModeId == ModeFeatureTargets.AllModes,
            "A shared factory explicitly targets custom profiles without a hard-coded mode id or automatic assembly discovery.");
        Reject<InvalidOperationException>(() => ContentRegistry.Build(new WildcardPackage(repeat: true)),
            "The read-only presence query does not weaken strict duplicate Add validation.");
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
        [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 10)]
        [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 10)] private void Invariant(ProgramSkillFrame frame) { }
    }
    private interface IHost { }
    private interface IOtherHost { }
    private sealed class Host : IHost { }
    private sealed class OtherHost : IOtherHost { }
    private sealed class Feature : IGameModeFeature { }
    private sealed class SharedFactory : IGameModeFeatureFactory
    {
        public string Id => "test:shared-mode-runtime";
        public string ModeId => ModeFeatureTargets.AllModes;
        public IGameModeFeature Create(ModeFeatureCreationContext context) => new Feature();
    }
    private sealed class WildcardPackage(bool repeat = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:shared-factory", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            Require(!builder.HasModeFeatureFactory("test:shared-mode-runtime"), "An unregistered shared factory must be absent.");
            builder.AddModeFeatureFactory(new SharedFactory());
            Require(builder.HasModeFeatureFactory("test:shared-mode-runtime"), "The production scoped builder forwards the presence query.");
            if (repeat) builder.AddModeFeatureFactory(new SharedFactory());
        }
    }
    private static MethodInfo[] Methods(Type type) => type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
    private static void Reject<T>(Action action, string message) where T : Exception
    { try { action(); } catch (T) { return; } throw new InvalidOperationException(message); }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
