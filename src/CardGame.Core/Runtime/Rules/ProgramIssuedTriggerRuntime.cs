using System.Linq.Expressions;
using System.Reflection;

namespace CardGame.Core;

internal enum ProgramIssuedTriggerMatch
{
    AdmitWhenTrue,
    RejectWhenFalse,
    Decision
}

// Issued receipts can decide before ordinary owner, skill-instance and usage
// checks. Their precedence is a rule contract, including terminal rejections.
[AttributeUsage(AttributeTargets.Method)]
internal sealed class ProgramIssuedTriggerAttribute(int order,
    ProgramIssuedTriggerMatch match = ProgramIssuedTriggerMatch.AdmitWhenTrue) : Attribute
{
    internal int Order { get; } = order;
    internal ProgramIssuedTriggerMatch MatchKind { get; } = match;
}

internal sealed class ProgramIssuedTriggerCatalog
{
    private static readonly Lazy<ProgramIssuedTriggerCatalog> BuiltIn = new(() =>
        Discover(ProgramRuntimeModuleCatalog.Default.Methods), true);
    internal static ProgramIssuedTriggerCatalog Default => BuiltIn.Value;

    internal sealed record Rule(MethodInfo Method, int Order,
        Func<object?, ProgramTriggerCandidate, ProgramSkillWindowContext, bool?> Invoke);

    internal IReadOnlyList<Rule> Rules { get; }

    private ProgramIssuedTriggerCatalog(IEnumerable<Rule> rules) =>
        Rules = Array.AsReadOnly(rules.ToArray());

    internal static ProgramIssuedTriggerCatalog Discover(IEnumerable<MethodInfo> methods)
    {
        var rules = new SortedDictionary<int, Rule>();
        foreach (var method in methods)
        {
            if (method.GetCustomAttribute<ProgramIssuedTriggerAttribute>() is not { } registration) continue;
            if (registration.Order <= 0 || !Enum.IsDefined(registration.MatchKind) ||
                method.IsStatic && !ProgramRuntimeModuleCatalog.IsDeclaredStaticHandler(method)) throw Invalid(method);
            var rule = new Rule(method, registration.Order, Compile(method, registration.MatchKind));
            if (!rules.TryAdd(rule.Order, rule))
                throw new InvalidOperationException($"Duplicate issued trigger precedence '{rule.Order}'.");
        }
        return new(rules.Values);
    }

    private static Func<object?, ProgramTriggerCandidate, ProgramSkillWindowContext, bool?> Compile(
        MethodInfo method, ProgramIssuedTriggerMatch match)
    {
        if (method.IsGenericMethod || method.ContainsGenericParameters ||
            method.ReturnType != (match == ProgramIssuedTriggerMatch.Decision ? typeof(bool?) : typeof(bool)))
            throw Invalid(method);
        var session = Expression.Parameter(typeof(object), "session");
        var candidate = Expression.Parameter(typeof(ProgramTriggerCandidate), "candidate");
        var context = Expression.Parameter(typeof(ProgramSkillWindowContext), "context");
        var parameters = method.GetParameters();
        if (parameters.Length is < 1 or > 2 ||
            parameters.Any(parameter => parameter.IsOptional || parameter.IsOut || parameter.ParameterType.IsByRef) ||
            parameters.Select(parameter => parameter.ParameterType).Distinct().Count() != parameters.Length)
            throw Invalid(method);
        var arguments = parameters.Select(parameter => parameter.ParameterType == candidate.Type
            ? (Expression)candidate : parameter.ParameterType == context.Type ? context : throw Invalid(method));
        Expression decision = Expression.Call(method.IsStatic ? null : Expression.Convert(session, method.DeclaringType!),
            method, arguments);
        if (match != ProgramIssuedTriggerMatch.Decision)
            decision = Expression.Condition(decision,
                match == ProgramIssuedTriggerMatch.AdmitWhenTrue
                    ? Expression.Convert(Expression.Constant(true), typeof(bool?)) : Expression.Constant(null, typeof(bool?)),
                match == ProgramIssuedTriggerMatch.RejectWhenFalse
                    ? Expression.Convert(Expression.Constant(false), typeof(bool?)) : Expression.Constant(null, typeof(bool?)));
        return Expression.Lambda<Func<object?, ProgramTriggerCandidate, ProgramSkillWindowContext, bool?>>(
            decision, session, candidate, context).Compile();
    }

    private static InvalidOperationException Invalid(MethodInfo method) =>
        new($"Issued trigger '{method.DeclaringType?.Name}.{method.Name}' has an invalid registration or signature.");
}

/// <summary>Returns the first issued rule decision; null alone falls through to ordinary admission.</summary>
internal sealed class ProgramIssuedTriggerRuntime(ProgramIssuedTriggerCatalog catalog,
    Func<MethodInfo, object?> resolveTarget)
{
    internal bool? Decide(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        foreach (var rule in catalog.Rules)
            if (rule.Invoke(resolveTarget(rule.Method), candidate, context) is { } decision) return decision;
        return null;
    }
}
