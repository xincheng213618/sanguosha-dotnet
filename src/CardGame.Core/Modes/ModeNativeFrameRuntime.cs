using System.Collections.ObjectModel;

namespace CardGame.Core;

/// <summary>One immutable merged view of native and installed-mode routes, without another progression loop.</summary>
internal sealed class ModeNativeFrameRuntime
{
    private sealed record Route(Type FrameType, int Order, bool HasCondition,
        Func<GameEngine, ResolutionFrame, bool> Advance);
    internal sealed record AutomaticRegistration(NativeFrameAutomaticStepMetadata Step,
        Func<GameEngine, ResolutionFrame, bool> Matches);
    private readonly IReadOnlyList<Route> _routes;
    private readonly IReadOnlyDictionary<Type, Route[]> _byFrameType;
    internal IReadOnlyDictionary<NativeFrameAutomaticPhase, IReadOnlyList<AutomaticRegistration>> Automatic { get; }

    internal ModeNativeFrameRuntime(NativeFrameRouteCatalog native, NativeFrameAutomaticStepCatalog automatic,
        IEnumerable<ModeNativeFrameRoute> modeRoutes)
    {
        ArgumentNullException.ThrowIfNull(modeRoutes);
        var installed = modeRoutes.ToArray();
        if (installed.Any(route => route is null)) throw new InvalidOperationException("A mode native route cannot be null.");
        var routes = native.Registrations.Select(route => new Route(route.FrameType, route.Order,
                route.Condition is not null, route.TryAdvance))
            .Concat(installed.Select(route => new Route(route.FrameType, route.Order, route.HasCondition,
                (_, frame) => route.TryAdvance(frame)))).OrderBy(route => route.Order).ToArray();
        if (routes.GroupBy(route => (route.FrameType, route.Order)).Any(group => group.Count() > 1) ||
            routes.Any(route => !route.HasCondition && routes.Any(later => !ReferenceEquals(later, route) &&
                route.Order <= later.Order && route.FrameType.IsAssignableFrom(later.FrameType))))
            throw new InvalidOperationException("Native and mode frame routes require unambiguous ordered ownership.");
        _routes = Array.AsReadOnly(routes);
        _byFrameType = new ReadOnlyDictionary<Type, Route[]>(routes.Select(route => route.FrameType).Distinct()
            .ToDictionary(type => type, type => routes.Where(route => route.FrameType.IsAssignableFrom(type)).ToArray()));

        var steps = automatic.Registrations.Values.SelectMany(items => items)
            .Select(item => new AutomaticRegistration(item.Step, item.Matches))
            .Concat(installed.Where(route => route.Automatic is not null).Select(route =>
                new AutomaticRegistration(new(route.Automatic!.Phase, route.Automatic.Order,
                    route.Automatic.Pending, route.Automatic.Publish), (_, frame) => route.Matches(frame)))).ToArray();
        if (steps.GroupBy(item => (item.Step.Phase, item.Step.Order)).Any(group => group.Count() > 1))
            throw new InvalidOperationException("Native and mode automatic steps require unique orders within each scheduler phase.");
        Automatic = new ReadOnlyDictionary<NativeFrameAutomaticPhase, IReadOnlyList<AutomaticRegistration>>(
            Enum.GetValues<NativeFrameAutomaticPhase>().ToDictionary(phase => phase,
                phase => (IReadOnlyList<AutomaticRegistration>)Array.AsReadOnly(steps
                    .Where(item => item.Step.Phase == phase).OrderBy(item => item.Step.Order).ToArray())));
    }

    internal bool TryAdvance(GameEngine engine, ResolutionFrame frame)
    {
        var candidates = _byFrameType.TryGetValue(frame.GetType(), out var known) ? known : _routes;
        foreach (var route in candidates)
            if (route.Advance(engine, frame)) return true;
        return false;
    }
}
