using System.Reflection;
using CardGame.Core;

internal static class ProgramRuntimeProofChecks
{
    public static void OrderedTypedAdmissionAndDyingGroups()
    {
        var catalog = ProgramRuntimeProofCatalog.Discover(Methods(typeof(Fixture)).Reverse());
        var a = new Fixture(catalog) { Match = true };
        var b = new Fixture(catalog);
        Need(a.Bound.Count == 0 && b.Bound.Count == 0, "Unused continuation handlers remain unbound.");
        Need(a.Proofs.IsDying(ProgramDyingProofPhase.Runtime) &&
            a.Trace.SequenceEqual(["first", "later-group"]) && b.Trace.Count == 0,
            "A matched group short-circuits while later groups still run, with per-match bindings.");
        Need(!b.Proofs.IsDying(ProgramDyingProofPhase.Runtime) &&
            b.Trace.SequenceEqual(["first", "second", "later-group"]),
            "An unmatched group visits all of its handlers.");
        a.Trace.Clear();
        a.Failure = new InvalidOperationException("continuation failure");
        try { a.Proofs.IsDying(ProgramDyingProofPhase.Runtime); throw new Exception("Expected failure."); }
        catch (InvalidOperationException error) when (ReferenceEquals(error, a.Failure))
        { Need(a.Trace.SequenceEqual(["first"]), "Failures stop dispatch without reflection wrapping."); }
        Need(catalog.Dying.Values.All(groups =>
                groups is IList<IReadOnlyList<ProgramRuntimeRegistration>> list && list.IsReadOnly &&
                groups.All(rows => rows is IList<ProgramRuntimeRegistration> group && group.IsReadOnly)),
            "Shared handler groups remain immutable.");
        Reject(typeof(DuplicateOrder), "Duplicate runtime proof order");
        Reject(typeof(WrongSignature), "invalid handler signature");
        Reject(typeof(OptionalArgument), "invalid handler signature");
        Reject(typeof(WrongPhase), "valid phase");
        Reject(typeof(WrongGroup), "positive group");
        static void Reject(Type type, string expected)
        {
            try { _ = ProgramRuntimeProofCatalog.Discover(Methods(typeof(Fixture)).Concat(Methods(type))); }
            catch (InvalidOperationException error) when (error.Message.Contains(expected, StringComparison.Ordinal)) { return; }
            throw new InvalidOperationException($"Did not reject {type.Name}: {expected}");
        }
    }

    private static MethodInfo[] Methods(Type type) => type.GetMethods(BindingFlags.Instance | BindingFlags.Static |
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture
    {
        internal List<string> Trace { get; } = [];
        internal List<string> Bound { get; } = [];
        internal bool Match { get; init; }
        internal Exception? Failure { get; set; }
        internal ProgramRuntimeProofs Proofs { get; }
        internal Fixture(ProgramRuntimeProofCatalog catalog) => Proofs = new(catalog, row =>
        {
            Bound.Add(row.Method.Name);
            return row.Method.CreateDelegate(row.DelegateType, this);
        });
        [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 10, group: 10)]
        private bool First()
        {
            Trace.Add("first");
            if (Failure is { } error) throw error;
            return Match;
        }
        [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 20, group: 10)]
        private bool Second() { Trace.Add("second"); return false; }
        [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 10, group: 20)]
        private bool LaterGroup() { Trace.Add("later-group"); return false; }
    }
    private sealed class DuplicateOrder { [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 10, group: 10)] private bool Bad() => false; }
    private sealed class WrongSignature { [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 999)] private int Bad() => 0; }
    private sealed class OptionalArgument { [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 999)] private bool Bad(int value = 0) => false; }
    private sealed class WrongPhase { [ProgramDyingProof((ProgramDyingProofPhase)999, 10)] private bool Bad() => false; }
    private sealed class WrongGroup { [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 999, group: 0)] private bool Bad() => false; }
}
