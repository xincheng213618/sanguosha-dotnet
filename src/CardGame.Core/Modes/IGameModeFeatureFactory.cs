namespace CardGame.Core;

/// <summary>A match-local mode service constructed by an explicitly installed content package.</summary>
public interface IGameModeFeature { }

public static class ModeFeatureTargets
{
    public const string AllModes = "*";
}

/// <summary>Executable registration metadata; factories must not retain a running match.</summary>
public interface IGameModeFeatureFactory
{
    string Id { get; }
    string ModeId { get; }
    IGameModeFeature Create(ModeFeatureCreationContext context);
}

/// <summary>Immutable setup plus dedicated host contracts, without exposing the game engine.</summary>
public sealed class ModeFeatureCreationContext
{
    private readonly Func<Type, object?> _resolveHost;
    private readonly Dictionary<Type, object> _resolvedHosts = [];
    private readonly HashSet<Type> _constructing = [];

    public ModeFeatureCreationContext(int seed, ContentRegistry contentRegistry, Func<Type, object?> resolveHost)
    {
        Seed = seed;
        ContentRegistry = contentRegistry ?? throw new ArgumentNullException(nameof(contentRegistry));
        _resolveHost = resolveHost ?? throw new ArgumentNullException(nameof(resolveHost));
    }

    public ModeFeatureCreationContext(GameOptions options, ContentModeDefinition modeDefinition,
        ContentRegistry contentRegistry, ModeHostRegistry hosts)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        ModeDefinition = modeDefinition ?? throw new ArgumentNullException(nameof(modeDefinition));
        ContentRegistry = contentRegistry ?? throw new ArgumentNullException(nameof(contentRegistry));
        ArgumentNullException.ThrowIfNull(hosts);
        Seed = options.Seed;
        _resolveHost = hosts.Resolve;
    }

    public int Seed { get; }
    public ContentRegistry ContentRegistry { get; }
    public GameOptions? Options { get; }
    public ContentModeDefinition? ModeDefinition { get; }

    public T GetHost<T>() where T : class
    {
        var contract = typeof(T);
        ModeHostRegistry.ValidateContract(contract);
        if (_resolvedHosts.TryGetValue(contract, out var cached)) return (T)cached;
        if (!_constructing.Add(contract)) throw new InvalidOperationException($"Cyclic mode feature host '{contract.FullName}'.");
        try
        {
            var host = _resolveHost(contract);
            ModeHostRegistry.ValidateHost(contract, host);
            _resolvedHosts.Add(contract, host!);
            return (T)host!;
        }
        finally { _constructing.Remove(contract); }
    }
}
