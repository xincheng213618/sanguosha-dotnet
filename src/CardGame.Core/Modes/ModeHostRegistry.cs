namespace CardGame.Core;

/// <summary>
/// Explicit, match-local dedicated host registrations. Resolving a host freezes
/// the whitelist; neither registrations nor instances are shared across games.
/// </summary>
public sealed class ModeHostRegistry
{
    private readonly Dictionary<Type, Func<object>> _factories = [];
    private readonly Dictionary<Type, object> _instances = [];
    private readonly HashSet<Type> _constructing = [];
    private bool _frozen;

    public ModeHostRegistry Register<T>(Func<T> factory) where T : class
    {
        ArgumentNullException.ThrowIfNull(factory);
        ValidateContract(typeof(T));
        if (_frozen) throw new InvalidOperationException("Mode host registrations are frozen after the first resolution.");
        if (!_factories.TryAdd(typeof(T), () => factory()!))
            throw new InvalidOperationException($"Duplicate dedicated mode host '{typeof(T).FullName}'.");
        return this;
    }

    public T Get<T>() where T : class => Resolve(typeof(T)) is T host ? host :
        throw new NotSupportedException($"No dedicated mode host is registered for '{typeof(T).FullName}'.");

    internal object? Resolve(Type contract)
    {
        ValidateContract(contract);
        _frozen = true;
        if (_instances.TryGetValue(contract, out var host)) return host;
        if (!_factories.TryGetValue(contract, out var factory)) return null;
        if (!_constructing.Add(contract)) throw new InvalidOperationException($"Cyclic dedicated mode host '{contract.FullName}'.");
        try
        {
            host = factory();
            ValidateHost(contract, host);
            _instances.Add(contract, host);
            return host;
        }
        finally { _constructing.Remove(contract); }
    }

    internal static void ValidateContract(Type contract)
    {
        if (!contract.IsInterface || contract.ContainsGenericParameters ||
            typeof(IEnumerable<ResolutionFrame>).IsAssignableFrom(contract))
            throw new ArgumentException("A mode feature requires a dedicated host interface, not an implementation or a resolution-stack collection.", nameof(contract));
    }

    internal static void ValidateHost(Type contract, object? host)
    {
        if (host is null || !contract.IsInstanceOfType(host) || host is GameEngine or CharacterState or IEnumerable<ResolutionFrame>)
            throw new NotSupportedException($"No dedicated mode host is available for '{contract.FullName}'.");
    }
}
