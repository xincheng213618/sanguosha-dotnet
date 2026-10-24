using System.Collections.ObjectModel;

namespace CardGame.Core;

// Match-local optional features share the engine's command transaction and
// resolution stack. The runtime owns their composition, never another engine.
internal interface IMatchModeCommandHandler
{
    IReadOnlyList<Type> CommandTypes { get; }
    CommandResult Submit(GameCommand command);
}

internal interface IMatchModeOpeningBoundary
{
    bool TryBeginOpening();
}

internal interface IMatchModeTurnIntermission
{
    bool TryBeginIntermission(int endedSeat, bool skipped);
}

internal sealed partial class MatchModeRuntime
{
    internal static MatchModeRuntime Empty { get; } = new();
    private readonly IReadOnlyDictionary<Type, object> _features;
    private readonly IReadOnlyDictionary<Type, IMatchModeCommandHandler> _commands;
    private readonly IMatchModeOpeningBoundary[] _opening;
    private readonly IMatchModeTurnIntermission[] _intermissions;

    internal MatchModeRuntime(params object[] features)
    {
        ArgumentNullException.ThrowIfNull(features);
        var byType = new Dictionary<Type, object>();
        var commands = new Dictionary<Type, IMatchModeCommandHandler>();
        foreach (var feature in features)
        {
            ArgumentNullException.ThrowIfNull(feature);
            if (!byType.TryAdd(feature.GetType(), feature))
                throw new InvalidOperationException($"Duplicate match feature '{feature.GetType().FullName}'.");
            if (feature is not IMatchModeCommandHandler handler) continue;
            foreach (var commandType in handler.CommandTypes)
            {
                if (commandType is null || commandType.IsAbstract || !typeof(GameCommand).IsAssignableFrom(commandType))
                    throw new InvalidOperationException("A mode command handler requires concrete game-command types.");
                if (!commands.TryAdd(commandType, handler))
                    throw new InvalidOperationException($"Duplicate mode command handler '{commandType.FullName}'.");
            }
        }
        _features = new ReadOnlyDictionary<Type, object>(byType);
        _commands = new ReadOnlyDictionary<Type, IMatchModeCommandHandler>(commands);
        _opening = features.OfType<IMatchModeOpeningBoundary>().ToArray();
        _intermissions = features.OfType<IMatchModeTurnIntermission>().ToArray();
    }

    internal T? Find<T>() where T : class =>
        _features.TryGetValue(typeof(T), out var feature) ? (T)feature : null;

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
