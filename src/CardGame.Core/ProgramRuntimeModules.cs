using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CardGame.Core;

// Explicit registration prevents unrelated classes from becoming runtime hosts.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class ProgramRuntimeModuleAttribute : Attribute { }

// Trusted, narrow engine operations may be injected alongside existing read-only
// selection queries. This marker grants no access to engine state by itself.
internal interface IProgramRuntimeOperations { }

internal sealed class ProgramRuntimeModuleCatalog
{
    private static readonly Lazy<ProgramRuntimeModuleCatalog> BuiltIn = new(() => new(
        typeof(GameEngine).Assembly.GetTypes(), typeof(GameEngine).GetMethods(BindingFlags.Instance |
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)), true);
    internal static ProgramRuntimeModuleCatalog Default => BuiltIn.Value;
    internal sealed record Registration(Type Type, ConstructorInfo? Constructor, IReadOnlyList<Type> Dependencies);
    private readonly IReadOnlyDictionary<Type, Registration> _registrations;
    private readonly IReadOnlyDictionary<Type, ProgramRuntimeEntryRegistration> _entries;
    internal IReadOnlyList<Registration> Registrations { get; }
    internal IReadOnlyList<MethodInfo> Methods { get; }

    internal ProgramRuntimeModuleCatalog(IEnumerable<Type> types, IEnumerable<MethodInfo>? legacyMethods = null)
    {
        ArgumentNullException.ThrowIfNull(types);
        var registrations = new List<Registration>();
        var methods = new List<MethodInfo>(legacyMethods ?? []);
        var engineContracts = new Dictionary<Type, bool>();
        var entries = new Dictionary<Type, ProgramRuntimeEntryRegistration>();
        foreach (var type in types.Select(type => type ??
                     throw new InvalidOperationException("A runtime module type cannot be null."))
                     .OrderBy(type => type.FullName, StringComparer.Ordinal))
        {
            if (!IsDeclared(type)) continue;
            if (!type.IsClass || !type.IsSealed || type.IsGenericType || type.ContainsGenericParameters ||
                type.IsNested && type.DeclaringType == typeof(GameEngine))
                throw Invalid(type, "requires an independent, sealed, non-generic type");
            if (type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly).Any(field => !field.IsLiteral))
                throw Invalid(type, "must not retain nonconstant static fields");
            var declaredMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            if (declaredMethods.Any(ExposesEngine) || type.GetFields(BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Any(field => ContainsEngine(field.FieldType)))
                throw Invalid(type, "must not expose GameEngine through rule fields, methods or callbacks");
            ConstructorInfo? constructor = null;
            Type[] dependencies = [];
            if (type.IsAbstract)
            {
                // A static module may retain constants, but never match state.
            }
            else
            {
                var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (constructors is not [var only]) throw Invalid(type, "requires exactly one constructor");
                constructor = only;
                dependencies = only.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                if (dependencies.Distinct().Count() != dependencies.Length || dependencies.Any(dependency =>
                        !IsDedicatedDependency(dependency)) ||
                    only.GetParameters().Any(parameter => parameter.IsOptional || parameter.IsOut))
                    throw Invalid(type, "constructor dependencies must be dedicated contracts or declared shared entries");
                if (dependencies.Any(ContractExposesEngine))
                    throw Invalid(type, "dedicated dependency contracts must not expose GameEngine or engine callbacks");
            }
            registrations.Add(new(type, constructor, Array.AsReadOnly(dependencies)));
            methods.AddRange(declaredMethods);
        }
        Registrations = Array.AsReadOnly(registrations.ToArray());
        _registrations = new ReadOnlyDictionary<Type, Registration>(registrations.ToDictionary(item => item.Type));
        _entries = new ReadOnlyDictionary<Type, ProgramRuntimeEntryRegistration>(entries);
        Methods = Array.AsReadOnly(methods.ToArray());

        bool IsDedicatedDependency(Type dependency)
        {
            if (dependency.IsDefined(typeof(ProgramRuntimeEntryAttribute), false) ||
                dependency.IsConstructedGenericType && dependency.GetGenericTypeDefinition() == typeof(RuntimeEntries.DeferredRuntime<>))
            {
                if (!entries.ContainsKey(dependency)) entries.Add(dependency, ProgramRuntimeEntryRegistration.For(dependency));
                return true;
            }
            return dependency.IsInterface && !dependency.ContainsGenericParameters &&
                !typeof(ISkillProgramExecutionHost).IsAssignableFrom(dependency) &&
                !typeof(ISkillProgramEffectHost).IsAssignableFrom(dependency) &&
                dependency != typeof(IProgramSelectionQuery) && dependency != typeof(IProgramRuntimeOperations) &&
                (typeof(IProgramSelectionQuery).IsAssignableFrom(dependency) ||
                    typeof(IProgramRuntimeOperations).IsAssignableFrom(dependency));
        }

        bool ContractExposesEngine(Type contract)
        {
            if (engineContracts.TryGetValue(contract, out var result)) return result;
            result = ContainsEngine(contract) || contract.GetInterfaces().Append(contract).Any(current => current.GetMethods(
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly).Any(ExposesEngine)) ||
                contract.IsClass && contract.GetFields(BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Any(field => ContainsEngine(field.FieldType));
            engineContracts.Add(contract, result);
            return result;
        }
    }

    private static bool ExposesEngine(MethodInfo method) => ContainsEngine(method.ReturnType) ||
        method.GetParameters().Any(parameter => ContainsEngine(parameter.ParameterType));

    private static bool ContainsEngine(Type type) => ContainsEngine(type, new HashSet<Type>());

    private static bool ContainsEngine(Type type, HashSet<Type> visited)
    {
        if (type == typeof(GameEngine)) return true;
        if (!visited.Add(type)) return false;
        if (type.HasElementType && ContainsEngine(type.GetElementType()!, visited)) return true;
        if (type.IsGenericType && type.GetGenericArguments().Any(argument => ContainsEngine(argument, visited))) return true;
        return typeof(Delegate).IsAssignableFrom(type) && type.GetMethod("Invoke") is { } invoke &&
            (ContainsEngine(invoke.ReturnType, visited) ||
             invoke.GetParameters().Any(parameter => ContainsEngine(parameter.ParameterType, visited)));
    }

    internal static bool IsDeclared(Type? type) => type?.IsDefined(typeof(ProgramRuntimeModuleAttribute), false) == true;
    internal static bool IsDeclaredStaticHandler(MethodInfo method) => method.IsStatic && IsDeclared(method.DeclaringType);
    internal bool TryGetEntry(Type type, out ProgramRuntimeEntryRegistration registration) =>
        _entries.TryGetValue(type, out registration!);
    internal Registration Get(Type type) => _registrations.TryGetValue(type, out var registration) ? registration :
        throw Invalid(type, "is not registered as an independent runtime module");
    private static InvalidOperationException Invalid(Type type, string reason) =>
        new($"Program runtime module '{type.FullName}' {reason}.");
}

// One registry per game; neither these instances nor bound delegates enter a
// checkpoint. Pending state remains on existing resolution frames.
internal sealed class ProgramRuntimeModuleRegistry(ProgramRuntimeModuleCatalog catalog, Func<Type, object> resolve)
{
    private readonly Dictionary<Type, object> _instances = new();
    private readonly HashSet<Type> _constructing = new();
    private readonly Dictionary<(MethodInfo Method, Type DelegateType), Delegate> _handlers = new();

    internal object Get(Type type)
    {
        if (_instances.TryGetValue(type, out var instance)) return instance;
        var registration = catalog.Get(type);
        if (registration.Constructor is null) throw new InvalidOperationException("A static runtime module has no instance.");
        if (!_constructing.Add(type)) throw new InvalidOperationException($"Cyclic runtime module dependency '{type.FullName}'.");
        try
        {
            var arguments = registration.Dependencies.Select(dependency =>
            {
                var value = resolve(dependency);
                if (!dependency.IsInstanceOfType(value) || value is GameEngine)
                    throw new InvalidOperationException($"Runtime dependency '{dependency.FullName}' resolved an invalid host.");
                return value;
            }).ToArray();
            try { instance = registration.Constructor.Invoke(arguments); }
            catch (TargetInvocationException failure) when (failure.InnerException is { } cause)
            { ExceptionDispatchInfo.Capture(cause).Throw(); throw; }
            _instances.Add(type, instance);
            return instance;
        }
        finally { _constructing.Remove(type); }
    }

    internal Delegate Bind(MethodInfo method, Type delegateType, Func<MethodInfo, Type, Delegate> bindCore)
    {
        var key = (method, delegateType);
        if (_handlers.TryGetValue(key, out var handler)) return handler;
        if (method.IsStatic) handler = method.CreateDelegate(delegateType);
        else if (method.DeclaringType == typeof(GameEngine)) handler = bindCore(method, delegateType);
        else
        {
            _ = catalog.Get(method.DeclaringType!);
            handler = DeferredRuntimeHandler.Create(method, delegateType,
                () => method.CreateDelegate(delegateType, Get(method.DeclaringType!)));
        }
        _handlers.Add(key, handler);
        return handler;
    }

    internal Delegate Bind(MethodInfo method, Type delegateType, object legacyHost) =>
        Bind(method, delegateType, (coreMethod, type) => coreMethod.CreateDelegate(type, legacyHost));
}
