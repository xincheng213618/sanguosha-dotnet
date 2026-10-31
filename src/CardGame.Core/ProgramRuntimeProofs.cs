using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core;

internal enum ProgramDyingProofPhase { Runtime = 1 }

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class ProgramDyingProofAttribute(ProgramDyingProofPhase phase, int order, int group = 200) : Attribute
{
    internal ProgramDyingProofPhase Phase { get; } = phase;
    internal int Order { get; } = order;
    internal int Group { get; } = group;
}

internal sealed class ProgramRuntimeProofCatalog
{
    internal IReadOnlyDictionary<ProgramDyingProofPhase, IReadOnlyList<IReadOnlyList<ProgramRuntimeRegistration>>> Dying { get; }

    private ProgramRuntimeProofCatalog(
        Dictionary<ProgramDyingProofPhase, SortedDictionary<int, SortedDictionary<int, ProgramRuntimeRegistration>>> dying)
    {
        Dying = new ReadOnlyDictionary<ProgramDyingProofPhase, IReadOnlyList<IReadOnlyList<ProgramRuntimeRegistration>>>(
            dying.ToDictionary(p => p.Key, p => (IReadOnlyList<IReadOnlyList<ProgramRuntimeRegistration>>)Array.AsReadOnly(
                p.Value.Values.Select(group => (IReadOnlyList<ProgramRuntimeRegistration>)Array.AsReadOnly(group.Values.ToArray())).ToArray())));
    }

    internal static ProgramRuntimeProofCatalog Discover(IEnumerable<MethodInfo> methods)
    {
        var dying = Enum.GetValues<ProgramDyingProofPhase>().ToDictionary(p => p,
            _ => new SortedDictionary<int, SortedDictionary<int, ProgramRuntimeRegistration>>());
        foreach (var method in methods)
        {
            foreach (var attribute in method.GetCustomAttributes<ProgramDyingProofAttribute>())
            {
                if (!Enum.IsDefined(attribute.Phase) || attribute.Group <= 0)
                    throw new InvalidOperationException($"Runtime proof '{method.Name}' requires a valid phase and positive group.");
                if (!dying[attribute.Phase].TryGetValue(attribute.Group, out var group))
                    dying[attribute.Phase].Add(attribute.Group, group = new());
                Add(group, attribute.Order, method, typeof(Func<bool>), []);
            }
        }
        return new(dying);
    }

    private static void Add(SortedDictionary<int, ProgramRuntimeRegistration> group, int order,
        MethodInfo method, Type delegateType, Type[] parameters)
    {
        if (order <= 0) throw new InvalidOperationException($"Runtime proof '{method.Name}' requires a positive order.");
        if (method.IsAbstract || method.IsGenericMethod || method.ContainsGenericParameters || method.ReturnType != typeof(bool) ||
            !method.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters) ||
            method.GetParameters().Any(p => p.IsOptional || p.IsOut || p.ParameterType.IsByRef))
            throw new InvalidOperationException($"Runtime proof '{method.Name}' has an invalid handler signature.");
        if (!group.TryAdd(order, new(method, delegateType)))
            throw new InvalidOperationException($"Duplicate runtime proof order '{order}' at '{method.Name}'.");
    }

}

// Bind only the proof phases reached by this match. The shared catalog contains
// method metadata; these delegates and their cache belong to one engine only.
internal sealed class ProgramRuntimeProofs(ProgramRuntimeProofCatalog catalog, Func<ProgramRuntimeRegistration, Delegate> bind)
{
    private readonly Dictionary<ProgramDyingProofPhase, Func<bool>[][]> _dying = new();

    internal bool IsDying(ProgramDyingProofPhase phase)
    {
        if (!_dying.TryGetValue(phase, out var groups))
            _dying.Add(phase, groups = catalog.Dying[phase].Select(group => group.Select(row => (Func<bool>)bind(row)).ToArray()).ToArray());
        var result = false;
        foreach (var group in groups)
        {
            var matched = false;
            foreach (var handler in group) if (handler()) { matched = true; break; }
            // Each old |= statement evaluated its RHS even after an earlier
            // group matched; only the OR inside that RHS short-circuited.
            result |= matched;
        }
        return result;
    }
}

internal sealed partial class ProgramRuntimeHooks
{
    private readonly ProgramRuntimeProofs _proofs;
    internal bool IsDying(ProgramDyingProofPhase phase) => _proofs.IsDying(phase);
}
