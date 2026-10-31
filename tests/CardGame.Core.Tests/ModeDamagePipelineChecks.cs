using System.Reflection;
using CardGame.Core;

internal static class ModeDamagePipelineChecks
{
    public static void AdditiveRulesPreserveNativeOrderAndMatchIsolation()
    {
        var catalog = ProgramDamageHookCatalog.Discover(typeof(NativeFixture).GetMethods(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly), _ => typeof(Func<Attack, int>));
        var trace = new List<string>();
        var native = new NativeFixture(trace);
        ModeDamageFacts? observed = null;
        var registrations = new List<ModeDamageRuleRegistration>
        {
            ModeDamageRuleRegistration.Additive(20, facts => { observed = facts; trace.Add("mode:20"); return 2; })
        };
        var hooks = Hooks(native, registrations);
        registrations.Clear();
        var attack = new Attack();
        Require(hooks.AddAmounts(attack, int.MaxValue) == unchecked(int.MaxValue + 2) &&
            trace.SequenceEqual(["native:10", "mode:20", "native:30"]) &&
            observed is { ResolutionId: 77, SourceSeat: 2, TargetSeat: 3, CardUserSeat: 1,
                EffectiveCardKind: CardKind.Slash, Nature: DamageNature.Fire, IsChainPropagation: false, IsSourceLess: false },
            "Mode providers share the original unchecked additive fold and receive scalar facts after earlier native handlers have run.");
        var secondTrace = new List<string>();
        var second = Hooks(new NativeFixture(secondTrace),
            [ModeDamageRuleRegistration.Additive(20, _ => { secondTrace.Add("other:20"); return 4; })]);
        Require(second.AddAmounts(new Attack(), 3) == 7 && secondTrace.SequenceEqual(["native:10", "other:20", "native:30"]) &&
            trace.SequenceEqual(["native:10", "mode:20", "native:30"]),
            "Shared native metadata cannot bind an optional rule or its closure to another match.");
        Reject(() => Hooks(native, [ModeDamageRuleRegistration.Additive(10, _ => 0)]));
        Reject(() => Hooks(native, [ModeDamageRuleRegistration.Additive(20, _ => 0), ModeDamageRuleRegistration.Additive(20, _ => 1)]));
        var failure = new InvalidOperationException("original mode damage failure");
        trace.Clear();
        var failing = Hooks(native, [ModeDamageRuleRegistration.Additive(20, _ => throw failure)]);
        try { _ = failing.AddAmounts(new Attack(), 0); throw new Exception("A mode damage failure was swallowed."); }
        catch (InvalidOperationException error) when (ReferenceEquals(error, failure))
        { Require(trace.SequenceEqual(["native:10"]), "A direct mode failure keeps the original exception and stops later native hooks."); }

        var firstCalls = 0;
        var installed = new List<ModeDamageRuleRegistration>
            { ModeDamageRuleRegistration.Additive(1000001, _ => ++firstCalls) };
        var runtime = new MatchModeRuntime(new Feature(installed));
        runtime.ValidateNativePorts();
        installed.Clear();
        var factsSnapshot = new ModeDamageFacts(78, 0, 1, 0, CardKind.FireSlash, DamageNature.Fire, true, false);
        Require(runtime.DamageRules.Single().Amount(factsSnapshot) == 1 &&
            runtime.DamageRules is System.Collections.IList { IsReadOnly: true },
            "Mode composition freezes provider arrays before any live handler is invoked.");
        var otherCalls = 0;
        var other = new MatchModeRuntime(new Feature([ModeDamageRuleRegistration.Additive(1000001, _ => ++otherCalls)]));
        other.ValidateNativePorts();
        Require(other.DamageRules.Single().Amount(factsSnapshot) == 1 && firstCalls == 1 && otherCalls == 1 &&
            typeof(ModeDamageFacts).GetProperties().All(property => property.PropertyType.IsValueType),
            "Installed damage facts expose scalar values only, while optional providers remain separate per match.");
        var nativeOrder = CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime.NativeDamageHookCatalog.Phases[ProgramDamagePhase.AdditiveAmount].First().Order;
        Reject(() => new MatchModeRuntime(new Feature([ModeDamageRuleRegistration.Additive(nativeOrder, _ => 0)]))
            .ValidateNativePorts());
        Reject(() => new MatchModeRuntime(new Feature([ModeDamageRuleRegistration.Additive(1000001, _ => 0),
            ModeDamageRuleRegistration.Additive(1000001, _ => 0)])).ValidateNativePorts());

        var natureTrace = new List<string>();
        var nature = new MatchModeRuntime(new LastNature(20, natureTrace), new FirstNature(natureTrace));
        nature.ValidateNativePorts();
        Require(nature.TransformDamageNature(DamageNature.Normal) == DamageNature.Thunder &&
            natureTrace.SequenceEqual(["first", "last"]),
            "Nature transformations use their explicit original boundary order without manufacturing an attack or paid receipt.");
        Reject(() => new MatchModeRuntime(new LastNature(10, []), new FirstNature([])).ValidateNativePorts());

        ProgramDamageHooks Hooks(NativeFixture target, IReadOnlyList<ModeDamageRuleRegistration> rules) => new(catalog,
            row => row.Method.CreateDelegate(row.DelegateType, target), rules, value =>
            {
                var source = (Attack)value;
                return new(77, source.SourceSeat, 3, 1, CardKind.Slash, DamageNature.Fire, false, false);
            });
    }

    private sealed class Attack { internal int SourceSeat = 1; }
    private sealed class NativeFixture(List<string> trace)
    {
        [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 10)]
        private int First(Attack attack) { trace.Add("native:10"); attack.SourceSeat = 2; return 1; }
        [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 30)]
        private int Last(Attack attack) { trace.Add("native:30"); return -1; }
    }
    private sealed class Feature(IReadOnlyList<ModeDamageRuleRegistration> rules) : IMatchModeDamageRuleProvider
    { public IReadOnlyList<ModeDamageRuleRegistration> DamageRules { get; } = rules; }
    private sealed class FirstNature(List<string> trace) : IMatchModeDamageNaturePolicy
    {
        public int Order => 10;
        public DamageNature TransformDamageNature(DamageNature nature)
        { trace.Add("first"); return nature == DamageNature.Normal ? DamageNature.Fire : nature; }
    }
    private sealed class LastNature(int order, List<string> trace) : IMatchModeDamageNaturePolicy
    {
        public int Order { get; } = order;
        public DamageNature TransformDamageNature(DamageNature nature)
        { trace.Add("last"); return nature == DamageNature.Fire ? DamageNature.Thunder : nature; }
    }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Ambiguous native/mode damage orders were accepted.");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
