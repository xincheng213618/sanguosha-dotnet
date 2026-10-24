using System.Collections.ObjectModel;

namespace CardGame.Core;

// Match-local optional features share the engine's command transaction and
// resolution stack. The runtime owns their composition, never another engine.
public interface IMatchModeCommandHandler
{
    IReadOnlyList<Type> CommandTypes { get; }
    CommandResult Submit(GameCommand command);
}

public interface IMatchModeOpeningBoundary
{
    int Order => 0;
    bool TryBeginOpening();
}

public interface IMatchModeTurnIntermission
{
    int Order => 0;
    bool TryBeginIntermission(int endedSeat, bool skipped);
}

internal sealed partial class MatchModeRuntime
{
    internal static MatchModeRuntime Empty { get; } = new();
    private readonly IReadOnlyDictionary<Type, object> _features;
    private readonly IReadOnlyDictionary<Type, IMatchModeCommandHandler> _commands;
    private readonly IMatchModeOpeningBoundary[] _opening;
    private readonly IMatchModeTurnIntermission[] _intermissions;
    internal IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
    internal IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }

    internal MatchModeRuntime(params object[] features)
    {
        ArgumentNullException.ThrowIfNull(features);
        features = features.ToArray();
        var byType = new Dictionary<Type, object>();
        var commands = new Dictionary<Type, IMatchModeCommandHandler>();
        foreach (var feature in features)
        {
            ArgumentNullException.ThrowIfNull(feature);
            if (!byType.TryAdd(feature.GetType(), feature))
                throw new InvalidOperationException($"Duplicate match feature '{feature.GetType().FullName}'.");
            if (feature is not IMatchModeCommandHandler handler) continue;
            foreach (var commandType in handler.CommandTypes ?? throw new InvalidOperationException("A mode command handler returned no command registrations."))
            {
                if (commandType is null || commandType.IsAbstract || !typeof(GameCommand).IsAssignableFrom(commandType))
                    throw new InvalidOperationException("A mode command handler requires concrete game-command types.");
                if (!commands.TryAdd(commandType, handler))
                    throw new InvalidOperationException($"Duplicate mode command handler '{commandType.FullName}'.");
            }
        }
        _features = new ReadOnlyDictionary<Type, object>(byType);
        _commands = new ReadOnlyDictionary<Type, IMatchModeCommandHandler>(commands);
        _opening = features.OfType<IMatchModeOpeningBoundary>().OrderBy(item => item.Order).ToArray();
        _intermissions = features.OfType<IMatchModeTurnIntermission>().OrderBy(item => item.Order).ToArray();
        if (_opening.Any(item => item.Order < 0) || _opening.GroupBy(item => item.Order).Any(group => group.Count() > 1))
            throw new InvalidOperationException("Mode opening boundaries require unique nonnegative explicit orders.");
        if (_intermissions.Any(item => item.Order < 0) || _intermissions.GroupBy(item => item.Order).Any(group => group.Count() > 1))
            throw new InvalidOperationException("Mode intermissions require unique nonnegative explicit orders.");
        var observers = features.OfType<IMatchModeRuleObserverProvider>().SelectMany(provider => provider.RuleObservers ??
            throw new InvalidOperationException("A mode rule provider returned no observer registrations.")).ToArray();
        var hooks = features.OfType<IMatchModeRuntimeHookProvider>().SelectMany(provider => provider.RuntimeHooks ??
            throw new InvalidOperationException("A mode hook provider returned no runtime registrations.")).ToArray();
        if (observers.Any(item => item is null) || observers.GroupBy(item => (item.Phase, item.Order)).Any(group => group.Count() > 1))
            throw new InvalidOperationException("Mode rule observers require unique phase/order registrations.");
        if (hooks.Any(item => item is null) || hooks.GroupBy(item => (item.Phase, item.Order)).Any(group => group.Count() > 1))
            throw new InvalidOperationException("Mode runtime hooks require unique phase/order registrations.");
        RuleObservers = Array.AsReadOnly(observers.OrderBy(item => item.Phase).ThenBy(item => item.Order).ToArray());
        RuntimeHooks = Array.AsReadOnly(hooks.OrderBy(item => item.Phase).ThenBy(item => item.Order).ToArray());
    }

    internal T? Find<T>() where T : class
    {
        if (_features.TryGetValue(typeof(T), out var feature)) return (T)feature;
        var matches = _features.Values.OfType<T>().Take(2).ToArray();
        if (matches.Length > 1) throw new InvalidOperationException($"Multiple mode features implement the unique contract '{typeof(T).FullName}'.");
        return matches.FirstOrDefault();
    }

    internal bool TrySubmit(GameCommand command, out CommandResult? result)
    {
        if (!_commands.TryGetValue(command.GetType(), out var handler))
        {
            result = null;
            return false;
        }
        result = handler.Submit(command);
        return true;
    }

    internal bool TryBeginOpening()
    {
        foreach (var feature in _opening)
            if (feature.TryBeginOpening()) return true;
        return false;
    }

    internal bool TryBeginIntermission(int endedSeat, bool skipped)
    {
        foreach (var feature in _intermissions)
            if (feature.TryBeginIntermission(endedSeat, skipped)) return true;
        return false;
    }
}
