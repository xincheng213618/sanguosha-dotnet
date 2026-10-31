#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramNativeDrawReturnsDefinitions
{
internal static readonly Lazy<IReadOnlyDictionary<SkillProgramEffectOp, MethodInfo>> ProgramNativeDrawReturns = new(() =>
{
    var returns = new Dictionary<SkillProgramEffectOp, MethodInfo>();
    foreach (var method in global::CardGame.Core.GameEngine.RuntimeModuleMethods)
    {
        if (method.GetCustomAttribute<ProgramNativeDrawReturnAttribute>()is not { } registration)
            continue;
        if (registration.Operations.Count == 0 || method.IsStatic && !ProgramRuntimeModuleCatalog.IsDeclaredStaticHandler(method) || method.IsGenericMethod || method.ReturnType != typeof(void) || !method.GetParameters().Select(p => p.ParameterType).SequenceEqual([typeof(ProgramSkillFrame), typeof(NativeDrawOutcome)]))
            throw new InvalidOperationException("A native program draw return has an invalid signature.");

        foreach (var op in registration.Operations)
            if (!Enum.IsDefined(op) || !returns.TryAdd(op, method))
                throw new InvalidOperationException("A native program draw return has an unknown or duplicate operation.");
    }

    return new ReadOnlyDictionary<SkillProgramEffectOp, MethodInfo>(returns);
}, true);
}
