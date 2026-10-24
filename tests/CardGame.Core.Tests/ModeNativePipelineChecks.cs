using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ModeNativePipelineChecks
{
    public static void OptionalRegistrationsShareNativeOrderingAndFreezePerMatch()
    {
        var engine = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, StandardContentRegistry.Create());
        var methods = typeof(NativeFixture).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
        var native = new NativeFrameRouteCatalog(methods);
        var automatic = new NativeFrameAutomaticStepCatalog(native, methods);
        var continued = new List<long>();
        var mode = ModeNativeFrameRoute.For<ProgramSkillFrame>(20, continued.Add, frame => frame.OwnerSeat == 1,
            new(NativeFrameAutomaticPhase.AfterOpening, 20, NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.None));
        var merged = new ModeNativeFrameRuntime(native, automatic, [mode]);
        var frame = new ProgramSkillFrame(71, 0, "fixture:mode-native", "run", "hash", 0, [], []);
        Selected(() => merged.TryAdvance(engine, frame), "first");
        Require(continued.Count == 0, "An earlier matching native route keeps ownership ahead of the optional mode route.");
        Require(merged.TryAdvance(engine, frame with { OwnerSeat = 1 }) && continued.SequenceEqual([71L]),
            "The installed mode receives the original frame id after an earlier conditional native route declines.");
        Selected(() => merged.TryAdvance(engine, frame with { OwnerSeat = 2 }), "fallback");
        Require(!merged.TryAdvance(engine, new DeferredTurnEndFrame(72, 0, 1, false, [])),
            "A typed optional route cannot consume an unrelated native frame.");
        var steps = merged.Automatic[NativeFrameAutomaticPhase.AfterOpening];
        Require(steps.Select(step => step.Step.Order).SequenceEqual([20, 30]) &&
            steps[0].Step is { Pending: NativeFrameAutomaticPending.Ignore, Publish: NativeFrameAutomaticPublish.None } &&
            steps[0].Matches(engine, frame with { OwnerSeat = 1 }) && !steps[0].Matches(engine, frame) &&
            steps[1].Matches(engine, frame with { OwnerSeat = 1 }) && continued.Count == 1,
            "Merged automatic admission retains phase/order/pending/publication metadata and never executes a continuation.");
        Reject(() => new ModeNativeFrameRuntime(native, automatic,
            [ModeNativeFrameRoute.For<ProgramSkillFrame>(10, _ => { }, _ => true)]));
        Reject(() => new ModeNativeFrameRuntime(native, automatic,
            [ModeNativeFrameRoute.For<ProgramSkillFrame>(20, _ => { })]));
        Reject(() => new ModeNativeFrameRuntime(native, automatic,
            [ModeNativeFrameRoute.For<ProgramSkillFrame>(40, _ => { }, _ => true)]));
        Reject(() => new ModeNativeFrameRuntime(native, automatic,
            [ModeNativeFrameRoute.For<ProgramSkillFrame>(20, _ => { }, _ => true,
                new(NativeFrameAutomaticPhase.AfterOpening, 30))]));

        var calls = new List<string>();
        var choice = Choice("fixture-mode-answer");
        var answerHost = new AnswerHost(calls);
        var catalog = ProgramInteractionCatalog.Discover(typeof(AnswerHost)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        var expectedFailure = new InvalidOperationException("original-mode-answer-failure");
        ModePromptAnswerRegistration[] answers =
        [
            new("fixture-mode-answer", selected =>
            {
                Require(ReferenceEquals(selected, choice), "A mode answer receives the original published choice without projecting it again.");
                calls.Add("mode");
            }),
            new("fixture-mode-failure", _ => throw expectedFailure)
        ];
        var router = new ProgramInteractionRouter(catalog, (method, type) => method.CreateDelegate(type, answerHost),
            _ => calls.Add("activation"), answers);
        router.Resolve(choice);
        Require(calls.SequenceEqual(["before", "mode"]), "Native pre-answer handlers run before a mode's ordinary exact action.");
        calls.Clear();
        answerHost.Stop = true;
        router.Resolve(choice);
        Require(calls.SequenceEqual(["before"]), "A native pre-answer claim prevents every ordinary mode answer.");
        answerHost.Stop = false;
        foreach (var (action, last) in new[] { ("fixture-native", "native"), ("fixture-prefix:value", "prefix"), ("skip", "activation") })
        {
            calls.Clear(); router.Resolve(Choice(action));
            Require(calls.SequenceEqual(["before", last]), "Native exact/prefix actions and activation fallback retain their original layer.");
        }
        try { router.Resolve(Choice("fixture-mode-failure")); throw new Exception("The mode failure was swallowed."); }
        catch (InvalidOperationException failure)
        { Require(ReferenceEquals(failure, expectedFailure), "Mode answer failures propagate without reflection wrappers or fallback."); }
        foreach (var action in new[] { "fixture-native", "fixture-prefix:value", "activate", "skip" })
            Reject(() => new ProgramInteractionRouter(catalog, (method, type) => method.CreateDelegate(type, answerHost),
                _ => { }, [new(action, _ => { })]));
        Reject(() => catalog.FreezeModeAnswers([new("duplicate", _ => { }), new("duplicate", _ => { })]));

        var perMatchCalls = new List<long>();
        var routes = new List<ModeNativeFrameRoute> { ModeNativeFrameRoute.For<OptionalFrame>(71901, perMatchCalls.Add) };
        var mutableAnswers = new List<ModePromptAnswerRegistration> { new("fixture-frozen-mode-answer", _ => { }) };
        var runtime = new MatchModeRuntime(new Feature(routes, mutableAnswers));
        runtime.ValidateNativePorts();
        routes.Clear(); mutableAnswers.Clear();
        Require(runtime.NativeFrames.TryAdvance(engine, new OptionalFrame(81)) && perMatchCalls.SequenceEqual([81L]) &&
            runtime.PromptAnswers.Single().Action == "fixture-frozen-mode-answer" &&
            runtime.PromptAnswers is System.Collections.IList { IsReadOnly: true },
            "Composition freezes provider collections before callers can remove routes or answers from the installed match.");
        var otherCalls = new List<long>();
        var other = new MatchModeRuntime(new Feature([ModeNativeFrameRoute.For<OptionalFrame>(71901, otherCalls.Add)], []));
        other.ValidateNativePorts();
        Require(other.NativeFrames.TryAdvance(engine, new OptionalFrame(82)) && otherCalls.SequenceEqual([82L]) &&
            perMatchCalls.SequenceEqual([81L]), "The shared metadata never binds another match to an earlier mode continuation.");
        Reject(() => new MatchModeRuntime(new Feature(
            [ModeNativeFrameRoute.For<ProgramSkillFrame>(210, _ => { })], [])).ValidateNativePorts());
        Reject(() => new MatchModeRuntime(new Feature([], [new("select-subset", _ => { })])).ValidateNativePorts());
        var nativeStep = NativeFrameAutomaticStepCatalog.Default.Registrations.Values.SelectMany(items => items).First().Step;
        Reject(() => new MatchModeRuntime(new Feature([ModeNativeFrameRoute.For<OptionalFrame>(71901, _ => { },
            automatic: new(nativeStep.Phase, nativeStep.Order))], [])).ValidateNativePorts());
    }

    private sealed record OptionalFrame(long FrameId) : ResolutionFrame(FrameId,
        ResolutionFrameKind.DeferredTurnEnd, ResolutionFrameStep.ResolvingEffect);
    private sealed class Feature(IReadOnlyList<ModeNativeFrameRoute> routes, IReadOnlyList<ModePromptAnswerRegistration> answers)
        : IMatchModeNativeFrameProvider, IMatchModePromptAnswerProvider
    {
        public IReadOnlyList<ModeNativeFrameRoute> FrameRoutes { get; } = routes;
        public IReadOnlyList<ModePromptAnswerRegistration> PromptAnswers { get; } = answers;
    }
    private sealed class NativeSelected(string name) : Exception
    { internal string Name { get; } = name; }
    [ProgramRuntimeModule]
    private static class NativeFixture
    {
        private static bool First(ProgramSkillFrame frame) => frame.OwnerSeat == 0;
        [NativeFrameRoute(typeof(ProgramSkillFrame), 10, typeof(NativeFixture), nameof(First))]
        private static void Select(long frameId) => throw new NativeSelected("first");
        [NativeFrameRoute(typeof(ProgramSkillFrame), 30)]
        [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 30,
            NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.None)]
        private static void Fallback(long frameId) => throw new NativeSelected("fallback");
    }
    private sealed class AnswerHost(List<string> calls)
    {
        internal bool Stop;
        [ProgramAnswerBefore(1)]
        private bool Before(PromptChoice choice) { calls.Add("before"); return Stop; }
        [ProgramAnswer("fixture-native")]
        private void Native(PromptChoice choice) => calls.Add("native");
        [ProgramAnswerPrefix("fixture-prefix:")]
        private void Prefix(PromptChoice choice) => calls.Add("prefix");
    }
    private static PromptChoice Choice(string action) => new(new(action), action, [], [],
        new Dictionary<string, string> { ["program-action"] = action });
    private static void Selected(Action action, string name)
    {
        try { action(); }
        catch (NativeSelected selected) when (selected.Name == name) { return; }
        throw new InvalidOperationException("The native continuation order changed.");
    }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Ambiguous optional native registrations were accepted.");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
