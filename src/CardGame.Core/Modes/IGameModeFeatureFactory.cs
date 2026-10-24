namespace CardGame.Core;

/// <summary>A match-local mode service constructed by an explicitly installed content package.</summary>
public interface IGameModeFeature { }

/// <summary>Executable registration metadata; factories must not retain a running match.</summary>
public interface IGameModeFeatureFactory
{
    string Id { get; }
    string ModeId { get; }
    IGameModeFeature Create(ModeFeatureCreationContext context);
}

/// <summary>Immutable setup plus dedicated host contracts, without exposing the game engine.</summary>
public sealed class ModeFeatureCreationContext(int seed, ContentRegistry contentRegistry, Func<Type, object?> resolveHost)
{
    private readonly Func<Type, object?> _resolveHost = resolveHost ?? throw new ArgumentNullException(nameof(resolveHost));
    public int Seed { get; } = seed;
    public ContentRegistry ContentRegistry { get; } = contentRegistry ?? throw new ArgumentNullException(nameof(contentRegistry));

    public T GetHost<T>() where T : class
    {
        if (!typeof(T).IsInterface)
            throw new ArgumentException("A mode feature requires a dedicated host interface.", nameof(T));
        var host = _resolveHost(typeof(T));
        return host is T typed && host is not GameEngine ? typed :
            throw new NotSupportedException($"No dedicated mode host is available for '{typeof(T).FullName}'.");
    }
}
