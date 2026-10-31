using System.Linq.Expressions;
using System.Reflection;

namespace CardGame.Core;

// These are distinct admission stages, not interchangeable issued-trigger rules.
// In particular, a receipt exception only bypasses its owner/instance check.
internal enum ProgramTriggerProofPhase
{
    OwnerException, InstanceException, HpChangeOwner, MovementOwner, NativeWindow,
    ForeignDraw, ForeignDrawEnd, DeadOwnerEnding, GameStart, DesignatedTarget, LegacyWindow
}

internal enum ProgramTriggerProofMatch { AdmitWhenTrue, Decision }

internal readonly record struct CandidateTriggerProofRequest(ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context);
internal readonly record struct HpTriggerProofRequest(CharacterState Owner, SkillProgramTrigger Trigger,
    ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context, HpChangeContext Change);
internal readonly record struct MovementTriggerProofRequest(CharacterState Owner, SkillProgramTrigger Trigger,
    ProgramInstructionFeatures Features, ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context, CardMovementBatchContext Batch);
internal readonly record struct NativeWindowTriggerProofRequest(CharacterState Owner, ProgramTriggerCandidate Candidate,
    ProgramSkillWindowContext Context, SkillProgramTrigger Trigger);
internal readonly record struct DeadOwnerTriggerProofRequest(ProgramTriggerCandidate Candidate);
internal readonly record struct GameStartTriggerProofRequest(ProgramLifecycleTriggerWindowFrame Frame);

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class ProgramTriggerProofAttribute : Attribute
{
    internal ProgramTriggerProofAttribute(ProgramTriggerProofPhase phase, int order,
        ProgramTriggerProofMatch match = ProgramTriggerProofMatch.AdmitWhenTrue)
    { Phase = phase; Order = order; MatchKind = match; }

    internal ProgramTriggerProofAttribute(ProgramTriggerProofPhase phase, int order, SkillProgramTriggerWindow window)
        : this(phase, order) => Window = window;

    internal ProgramTriggerProofPhase Phase { get; }
    internal int Order { get; }
    internal ProgramTriggerProofMatch MatchKind { get; }
    internal SkillProgramTriggerWindow? Window { get; }
}

internal sealed class ProgramTriggerProofCatalog
{
    private static readonly Lazy<ProgramTriggerProofCatalog> BuiltIn = new(() =>
        Discover(ProgramRuntimeModuleCatalog.Default.Methods), true);
    internal static ProgramTriggerProofCatalog Default => BuiltIn.Value;

    internal sealed record Rule(MethodInfo Method, int Order, Delegate Invoke, SkillProgramTriggerWindow? Window);
    private readonly IReadOnlyDictionary<ProgramTriggerProofPhase, IReadOnlyList<Rule>> _phases;

    private ProgramTriggerProofCatalog(Dictionary<ProgramTriggerProofPhase, IReadOnlyList<Rule>> phases) => _phases = phases;

    internal IReadOnlyList<Rule> Rules<TRequest>(ProgramTriggerProofPhase phase) where TRequest : struct
    {
        if (RequestType(phase) != typeof(TRequest))
            throw new InvalidOperationException($"Trigger proof stage '{phase}' requires '{RequestType(phase).Name}'.");
        return _phases.GetValueOrDefault(phase) ?? [];
    }

    internal static ProgramTriggerProofCatalog Discover(IEnumerable<MethodInfo> methods)
    {
        var phases = new Dictionary<ProgramTriggerProofPhase, SortedDictionary<int, Rule>>();
        foreach (var method in methods)
        foreach (var registration in method.GetCustomAttributes<ProgramTriggerProofAttribute>())
        {
            if (!Enum.IsDefined(registration.Phase) || registration.Order <= 0 || !Enum.IsDefined(registration.MatchKind) ||
                method.IsStatic && !ProgramRuntimeModuleCatalog.IsDeclaredStaticHandler(method) ||
                registration.Window is { } window && (!Enum.IsDefined(window) || registration.Phase != ProgramTriggerProofPhase.NativeWindow))
                throw Invalid(method);
            if (!phases.TryGetValue(registration.Phase, out var rules)) phases.Add(registration.Phase, rules = new());
            if (!rules.TryAdd(registration.Order, new(method, registration.Order, Compile(method, registration), registration.Window)))
                throw new InvalidOperationException($"Duplicate trigger proof order '{registration.Phase}:{registration.Order}'.");
        }
        return new(phases.ToDictionary(pair => pair.Key,
            pair => (IReadOnlyList<Rule>)Array.AsReadOnly(pair.Value.Values.ToArray())));
    }

    private static Type RequestType(ProgramTriggerProofPhase phase) => phase switch
    {
        ProgramTriggerProofPhase.HpChangeOwner => typeof(HpTriggerProofRequest),
        ProgramTriggerProofPhase.MovementOwner => typeof(MovementTriggerProofRequest),
        ProgramTriggerProofPhase.NativeWindow or ProgramTriggerProofPhase.DesignatedTarget => typeof(NativeWindowTriggerProofRequest),
        ProgramTriggerProofPhase.DeadOwnerEnding => typeof(DeadOwnerTriggerProofRequest),
        ProgramTriggerProofPhase.GameStart => typeof(GameStartTriggerProofRequest),
        ProgramTriggerProofPhase.OwnerException or ProgramTriggerProofPhase.InstanceException or
            ProgramTriggerProofPhase.ForeignDraw or ProgramTriggerProofPhase.ForeignDrawEnd or
            ProgramTriggerProofPhase.LegacyWindow => typeof(CandidateTriggerProofRequest),
        _ => throw new InvalidOperationException($"Unknown trigger proof stage '{phase}'.")
    };

    private static Delegate Compile(MethodInfo method, ProgramTriggerProofAttribute registration)
    {
        if (method.IsGenericMethod || method.ContainsGenericParameters || method.ReturnType !=
            (registration.MatchKind == ProgramTriggerProofMatch.Decision ? typeof(bool?) : typeof(bool))) throw Invalid(method);
        var requestType = RequestType(registration.Phase);
        var session = Expression.Parameter(typeof(object), "session");
        var request = Expression.Parameter(requestType, "request");
        var parameters = method.GetParameters();
        if (parameters.Length == 0 || parameters.Any(parameter => parameter.IsOptional || parameter.IsOut || parameter.ParameterType.IsByRef) ||
            parameters.Select(parameter => parameter.ParameterType).Distinct().Count() != parameters.Length) throw Invalid(method);
        var properties = requestType.GetProperties().ToDictionary(property => property.PropertyType);
        var arguments = parameters.Select(parameter => parameter.ParameterType == requestType ? (Expression)request :
            properties.TryGetValue(parameter.ParameterType, out var property) ? Expression.Property(request, property) : throw Invalid(method));
        Expression decision = Expression.Call(method.IsStatic ? null : Expression.Convert(session, method.DeclaringType!), method, arguments);
        if (registration.MatchKind == ProgramTriggerProofMatch.AdmitWhenTrue)
            decision = Expression.Condition(decision, Expression.Convert(Expression.Constant(true), typeof(bool?)), Expression.Constant(null, typeof(bool?)));
        return Expression.Lambda(typeof(Func<,,>).MakeGenericType(typeof(object), requestType, typeof(bool?)), decision, session, request).Compile();
    }

    private static InvalidOperationException Invalid(MethodInfo method) =>
        new($"Trigger proof '{method.DeclaringType?.Name}.{method.Name}' has an invalid stage, registration or signature.");
}

// Metadata retains neither a game nor a request. Each decision uses the original
// participants and current state; only null falls through, including to generic defaults.
internal sealed class ProgramTriggerProofRuntime(ProgramTriggerProofCatalog catalog, Func<MethodInfo, object?> resolveTarget)
{
    internal bool? Decide<TRequest>(ProgramTriggerProofPhase phase, TRequest request) where TRequest : struct
    {
        foreach (var rule in catalog.Rules<TRequest>(phase))
        {
            // An unrelated window must not even instantiate another native owner.
            if (rule.Window is { } window && request is NativeWindowTriggerProofRequest native && native.Context.Window != window) continue;
            if (((Func<object?, TRequest, bool?>)rule.Invoke)(resolveTarget(rule.Method), request) is { } decision) return decision;
        }
        return null;
    }
}
