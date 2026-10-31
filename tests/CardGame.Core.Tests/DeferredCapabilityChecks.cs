using System.Reflection;
using CardGame.Core;

internal static class DeferredCapabilityChecks
{
    public static void TypedFactoriesDeferCyclesAndRetainRefOutAndMatchIdentity()
    {
        var modules = new ProgramRuntimeModuleCatalog([typeof(First), typeof(Second)]);
        var capabilities = new ProgramCapabilityCatalog([typeof(FirstPort), typeof(SecondPort)]);
        var value = new ValueStore(5);
        var match = new MatchRuntime(modules, capabilities, new NoEngineAdapters(),
            [new(typeof(IValueStore), value)]);
        var other = new MatchRuntime(modules, capabilities, new NoEngineAdapters(),
            [new(typeof(IValueStore), new ValueStore(20))]);
        var first = match.Capabilities.Get<IFirst>();
        Require(ModuleCount(match) == 0 && ReferenceEquals(first, match.Capabilities.Get<IFirst>()),
            "Resolving typed ports caches only their factories; their mutually dependent modules stay unconstructed.");
        Require(first.Read() == 6 && ModuleCount(match) == 2 && match.Capabilities.Get<ISecond>().ReadFirst() == 6,
            "The first invocation constructs each owner once and permits a later call back through the original typed port.");
        var input = 3;
        Require(first.Combine(ref input, out var total) && input == 8 && total == 13,
            "Deferred ports preserve the caller's actual ref storage, output value and return value.");
        first.Number = 7;
        Require(value.Number == 7 && first.Read() == 8 && other.Capabilities.Get<IFirst>().Read() == 21 &&
                !ReferenceEquals(match.GetModule(typeof(First)), other.GetModule(typeof(First))),
            "A ref return reaches the actual state owner while factories and module instances remain isolated per match.");
        Reject(() => new ProgramCapabilityCatalog([typeof(WrongFactoryPort)]));
        Reject(() => new ProgramCapabilityCatalog([typeof(UndeclaredTargetPort)]));
        CheckSharedEntries();
    }

    private static void CheckSharedEntries()
    {
        var modules = new ProgramRuntimeModuleCatalog([typeof(SharedFirst), typeof(SharedSecond)]);
        var capabilities = new ProgramCapabilityCatalog([]);
        var value = new ValueStore(5);
        var match = new MatchRuntime(modules, capabilities, new NoEngineAdapters(), [new(typeof(IValueStore), value)]);
        var other = new MatchRuntime(modules, capabilities, new NoEngineAdapters(), [new(typeof(IValueStore), new ValueStore(20))]);
        var first = match.GetDeferredEntry<FirstEntry, SharedFirst>(FirstEntry.Create);
        Require(ModuleCount(match) == 0 && ReferenceEquals(first, match.GetDeferredEntry<FirstEntry, SharedFirst>(FirstEntry.Create)),
            "A shared entry is cached without constructing either mutually dependent module.");
        var argumentSawModules = -1;
        int Argument() { argumentSawModules = ModuleCount(match); return 1; }
        Require(first.Read(Argument()) == 7 && argumentSawModules == 0 && ModuleCount(match) == 2,
            "Call arguments are evaluated before shared entries construct their target and its deferred dependencies.");
        var second = (SharedSecond)match.GetModule(typeof(SharedSecond));
        Require(ReferenceEquals(second.First, first) && second.ReadFirst() == 6,
            "Injection and the kernel entry getter share one entry object, including a deferred call back to the original module.");
        var input = 3;
        Require(first.Combine(ref input, out var total) && input == 8 && total == 13,
            "Shared entries forward the original ref/out storage without per-consumer adapters.");
        first.Number = 7;
        Require(value.Number == 7 && first.Read() == 8 &&
                other.GetDeferredEntry<FirstEntry, SharedFirst>(FirstEntry.Create).Read() == 21 &&
                !ReferenceEquals(first, other.GetDeferredEntry<FirstEntry, SharedFirst>(FirstEntry.Create)),
            "Shared entry references reach actual state and remain isolated between matches.");
    }

    private interface IFirst : IProgramRuntimeOperations
    {
        int Read();
        bool Combine(ref int input, out int total);
        ref int Number { get; }
    }
    private interface ISecond : IProgramRuntimeOperations
    {
        int Read();
        int ReadFirst();
        bool Combine(ref int input, out int total);
        ref int Number { get; }
    }
    private interface IValueStore : IProgramRuntimeOperations { ref int Number { get; } }
    private sealed class ValueStore(int initial) : IValueStore
    {
        private int _number = initial;
        public ref int Number => ref _number;
    }
    [ProgramRuntimeModule]
    private sealed class SharedFirst(SecondEntry second)
    {
        internal int Read(int offset) => second.Read() + 1 + offset;
        internal bool Combine(ref int input, out int total) => second.Combine(ref input, out total);
        internal ref int Number => ref second.Number;
    }
    [ProgramRuntimeModule]
    private sealed class SharedSecond(FirstEntry first, IValueStore value)
    {
        internal FirstEntry First => first;
        internal int Read() => value.Number;
        internal int ReadFirst() => first.Read();
        internal bool Combine(ref int input, out int total)
        { input += value.Number; total = input + value.Number; return true; }
        internal ref int Number => ref value.Number;
    }
    [ProgramRuntimeEntry(typeof(SharedFirst))]
    private sealed class FirstEntry(Func<SharedFirst> resolve)
    {
        internal static FirstEntry Create(Func<SharedFirst> resolve) => new(resolve);
        internal int Read(int offset = 0) => resolve().Read(offset);
        internal bool Combine(ref int input, out int total) => resolve().Combine(ref input, out total);
        internal ref int Number => ref resolve().Number;
    }
    [ProgramRuntimeEntry(typeof(SharedSecond))]
    private sealed class SecondEntry(Func<SharedSecond> resolve)
    {
        internal int Read() => resolve().Read();
        internal bool Combine(ref int input, out int total) => resolve().Combine(ref input, out total);
        internal ref int Number => ref resolve().Number;
    }
    [ProgramRuntimeModule]
    private sealed class First(ISecond second)
    {
        internal int Read() => second.Read() + 1;
        internal bool Combine(ref int input, out int total) => second.Combine(ref input, out total);
        internal ref int Number => ref second.Number;
    }
    [ProgramRuntimeModule]
    private sealed class Second(IFirst first, IValueStore value)
    {
        internal int Read() => value.Number;
        internal int ReadFirst() => first.Read();
        internal bool Combine(ref int input, out int total)
        { input += value.Number; total = input + value.Number; return true; }
        internal ref int Number => ref value.Number;
    }
    [ProgramCapability(typeof(First))]
    private sealed class FirstPort(Func<First> target) : IFirst
    {
        public int Read() => target().Read();
        public bool Combine(ref int input, out int total) => target().Combine(ref input, out total);
        public ref int Number => ref target().Number;
    }
    [ProgramCapability(typeof(Second))]
    private sealed class SecondPort(Func<Second> target) : ISecond
    {
        public int Read() => target().Read();
        public int ReadFirst() => target().ReadFirst();
        public bool Combine(ref int input, out int total) => target().Combine(ref input, out total);
        public ref int Number => ref target().Number;
    }
    [ProgramCapability(typeof(First))]
    private sealed class WrongFactoryPort(GameEngine engine) : IFirst
    {
        public int Read() => throw new NotSupportedException();
        public bool Combine(ref int input, out int total) => throw new NotSupportedException();
        public ref int Number => throw new NotSupportedException();
    }
    [ProgramCapability(typeof(ValueStore))]
    private sealed class UndeclaredTargetPort(Func<ValueStore> target) : IValueStore
    { public ref int Number => ref target().Number; }
    private sealed class NoEngineAdapters : IMatchRuntimeAdapterFactory
    {
        public object CreateAdapter(ProgramCapabilityCatalog.Registration registration) => throw new Exception("Unexpected engine adapter.");
        public Delegate BindCoreHandler(MethodInfo method, Type delegateType) => throw new Exception("Unexpected engine binding.");
        public object CoreHandlerTarget(MethodInfo method) => throw new Exception("Unexpected engine target.");
    }
    private static int ModuleCount(MatchRuntime match)
    {
        var modules = typeof(MatchRuntime).GetField("_modules", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(match)!;
        return ((System.Collections.IDictionary)typeof(ProgramRuntimeModuleRegistry)
            .GetField("_instances", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(modules)!).Count;
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("An invalid deferred capability was accepted.");
    }
}
