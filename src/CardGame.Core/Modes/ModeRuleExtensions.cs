namespace CardGame.Core;

/// <summary>Explicit callbacks for the existing rule pipeline, bound to this match's feature instance.</summary>
public interface IMatchModeRuleObserverProvider
{
    IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; }
}

public sealed class ModeRuleObserverRegistration
{
    private ModeRuleObserverRegistration(ProgramRulePhase phase, int order, Action<IGameEvent?> callback)
    {
        if (!Enum.IsDefined(phase) || order <= 0)
            throw new ArgumentException("A mode rule observer requires a valid phase and positive explicit order.");
        Phase = phase;
        Order = order;
        Callback = callback;
    }
    public ProgramRulePhase Phase { get; }
    public int Order { get; }
    internal Action<IGameEvent?> Callback { get; }

    public static ModeRuleObserverRegistration ForEvent<TEvent>(ProgramRulePhase phase, int order, Action<TEvent> callback)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(callback);
        if (phase == ProgramRulePhase.BeforeStatePublish)
            throw new ArgumentException("A state-publication observer cannot declare an event parameter.", nameof(phase));
        return new(phase, order, fact => { if (fact is TEvent typed) callback(typed); });
    }

    public static ModeRuleObserverRegistration BeforeStatePublish(int order, Action callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        return new(ProgramRulePhase.BeforeStatePublish, order, _ => callback());
    }
}

/// <summary>Continuation claims at the existing scheduler phases; priority is shared with native rules.</summary>
public interface IMatchModeRuntimeHookProvider
{
    IReadOnlyList<ModeRuntimeHookRegistration> RuntimeHooks { get; }
}

public sealed class ModeRuntimeHookRegistration
{
    private ModeRuntimeHookRegistration(ProgramRuntimePhase phase, int order, Delegate callback)
    {
        if (!Enum.IsDefined(phase) || order <= 0)
            throw new ArgumentException("A mode runtime hook requires a valid phase and positive explicit order.");
        Phase = phase;
        Order = order;
        Callback = callback;
    }
    public ProgramRuntimePhase Phase { get; }
    public int Order { get; }
    internal Delegate Callback { get; }

    public static ModeRuntimeHookRegistration ForAdvance(ProgramRuntimePhase phase, int order, Func<bool> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        if (ProgramRuntimeHookCatalog.IsPermission(phase))
            throw new ArgumentException("A permission phase requires its exact program-frame parameter.", nameof(phase));
        return new(phase, order, callback);
    }

    public static ModeRuntimeHookRegistration ForPermission(ProgramRuntimePhase phase, int order, Func<ProgramSkillFrame, bool> callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        if (!ProgramRuntimeHookCatalog.IsPermission(phase))
            throw new ArgumentException("A scheduler-advance phase does not accept a program-frame parameter.", nameof(phase));
        return new(phase, order, callback);
    }
}
