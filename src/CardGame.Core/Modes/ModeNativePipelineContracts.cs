namespace CardGame.Core;

/// <summary>A match-local continuation registered in the existing native frame scheduler.</summary>
public interface IMatchModeNativeFrameProvider : IGameModeFeature
{
    IReadOnlyList<ModeNativeFrameRoute> FrameRoutes { get; }
}

/// <summary>Admission at an existing automatic phase; execution still uses ordinary native routing.</summary>
public sealed record ModeAutomaticFrameStep(NativeFrameAutomaticPhase Phase, int Order,
    NativeFrameAutomaticPending Pending = NativeFrameAutomaticPending.RequireNoneBeforeFrame,
    NativeFrameAutomaticPublish Publish = NativeFrameAutomaticPublish.AfterAdvance);

public sealed class ModeNativeFrameRoute
{
    private readonly Func<ResolutionFrame, bool> _matches;
    private readonly Action<long> _continuation;

    private ModeNativeFrameRoute(Type frameType, int order, bool conditional,
        Func<ResolutionFrame, bool> matches, Action<long> continuation, ModeAutomaticFrameStep? automatic)
    {
        if (frameType.IsAbstract || frameType.ContainsGenericParameters || order < 0)
            throw new ArgumentException("A mode native route requires a concrete frame and nonnegative order.");
        if (automatic is not null && (automatic.Order < 0 || !Enum.IsDefined(automatic.Phase) ||
            !Enum.IsDefined(automatic.Pending) || !Enum.IsDefined(automatic.Publish)))
            throw new ArgumentException("A mode automatic step requires valid native scheduler metadata.", nameof(automatic));
        FrameType = frameType;
        Order = order;
        HasCondition = conditional;
        Automatic = automatic;
        _matches = matches;
        _continuation = continuation;
    }

    public Type FrameType { get; }
    public int Order { get; }
    public bool HasCondition { get; }
    public ModeAutomaticFrameStep? Automatic { get; }

    public static ModeNativeFrameRoute For<TFrame>(int order, Action<long> continuation,
        Func<TFrame, bool>? condition = null, ModeAutomaticFrameStep? automatic = null) where TFrame : ResolutionFrame
    {
        ArgumentNullException.ThrowIfNull(continuation);
        return new(typeof(TFrame), order, condition is not null,
            frame => frame is TFrame typed && (condition is null || condition(typed)), continuation, automatic);
    }

    internal bool Matches(ResolutionFrame frame) => _matches(frame);
    internal bool TryAdvance(ResolutionFrame frame)
    {
        if (!Matches(frame)) return false;
        _continuation(frame.Id);
        return true;
    }
}

/// <summary>Exact mode answers run after native pre-handlers and share the original published choice.</summary>
public interface IMatchModePromptAnswerProvider : IGameModeFeature
{
    IReadOnlyList<ModePromptAnswerRegistration> PromptAnswers { get; }
}

public sealed class ModePromptAnswerRegistration
{
    public ModePromptAnswerRegistration(string action, Action<PromptChoice> handler)
    {
        if (string.IsNullOrWhiteSpace(action)) throw new ArgumentException("A mode prompt answer requires an action.", nameof(action));
        ArgumentNullException.ThrowIfNull(handler);
        Action = action;
        Handler = handler;
    }
    public string Action { get; }
    internal Action<PromptChoice> Handler { get; }
}
