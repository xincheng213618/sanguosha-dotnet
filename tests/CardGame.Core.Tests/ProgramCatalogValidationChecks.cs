using System.Reflection;
using System.Reflection.Emit;
using CardGame.Core;

internal static class ProgramCatalogValidationChecks
{
    private const BindingFlags DeclaredMethods = BindingFlags.Instance | BindingFlags.Static |
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    private static readonly List<string> Trace = [];
    private static readonly InvalidOperationException Failure = new("original catalog validator failure");

    public static void MetadataCandidatesPreserveCatalogContractsAndFallbacks()
    {
        var core = typeof(ProgramCatalogValidations).Assembly;
        var coreTypes = core.GetTypes();
        var original = Reflect(coreTypes);
        var expected = original.Where(IsCandidate).ToArray();
        var discovered = ProgramCatalogValidationMethodDiscovery.Find(core).ToArray();
        SameMethods(expected, discovered,
            "The real Core metadata candidate sequence retains original type/method order and open generic definitions.");
        var reflectionCatalog = ProgramCatalogValidations.Discover(original);
        var metadataCatalog = ProgramCatalogValidations.Discover(discovered);
        Need(reflectionCatalog.Registrations.Count > 0 &&
            Rows(reflectionCatalog).SequenceEqual(Rows(metadataCatalog)) &&
            Rows(reflectionCatalog).SequenceEqual(Rows(ProgramCatalogValidations.Default)),
            "The complete real default catalog keeps every validator, exact method identity and explicit validation order.");

        var physical = typeof(ProgramCatalogValidationChecks).Assembly;
        var types = physical.GetTypes();
        MethodInfo[] candidates;
        using (var image = File.OpenRead(physical.Location))
        {
            Need(ProgramCatalogValidationMethodDiscovery.TryRead(physical.ManifestModule, types, image, out candidates),
                "A physical test assembly uses the same PE discovery boundary as production.");
            Need(image.CanRead, "Metadata discovery leaves ownership of the supplied image stream with its caller.");
        }
        SameMethods(Reflect(types).Where(IsCandidate).ToArray(), candidates,
            "Test-assembly discovery includes direct declarations and inherited virtual candidates in their original order.");
        CheckDeclarations(candidates);

        using (var invalid = new MemoryStream([0, 1, 2, 3]))
            Need(!ProgramCatalogValidationMethodDiscovery.TryRead(core.ManifestModule, coreTypes, invalid, out var invalidCandidates) &&
                invalidCandidates.Length == 0, "A malformed PE produces no partial method catalog.");
        using (var otherImage = File.OpenRead(physical.Location))
            Need(!ProgramCatalogValidationMethodDiscovery.TryRead(core.ManifestModule, coreTypes, otherImage, out var foreignCandidates) &&
                foreignCandidates.Length == 0, "A valid image with another module MVID cannot supply any loaded-module tokens.");
        using (var coreImage = File.OpenRead(core.Location))
            Need(!ProgramCatalogValidationMethodDiscovery.TryRead(core.ManifestModule,
                [reflectionCatalog.Registrations[0].Method.DeclaringType!, typeof(ProgramCatalogValidationChecks)], coreImage,
                out var partialCandidates) && partialCandidates.Length == 0,
                "A foreign type discovered after a valid owner cannot leak a partially resolved candidate list.");

        var memory = Assembly.Load(File.ReadAllBytes(physical.Location));
        Need(memory.Location.Length == 0, "The memory-loaded fixture exercises the actual missing-Location boundary.");
        SameMethods(Reflect(memory.GetTypes()), ProgramCatalogValidationMethodDiscovery.Find(memory).ToArray(),
            "A memory-loaded assembly falls back to the entire original reflection sequence, including unmarked methods.");
        var dynamic = DynamicAssembly();
        var dynamicMethods = Reflect(dynamic.GetTypes());
        var dynamicDiscovered = ProgramCatalogValidationMethodDiscovery.Find(dynamic).ToArray();
        SameMethods(dynamicMethods, dynamicDiscovered,
            "A dynamic assembly retains its original reflection path without requiring a physical image.");
        var dynamicCatalog = ProgramCatalogValidations.Discover(dynamicDiscovered);
        Need(dynamicCatalog.Registrations is [{ Order: 70 }] &&
            Rows(ProgramCatalogValidations.Discover(dynamicMethods)).SequenceEqual(Rows(dynamicCatalog)),
            "Dynamic fallback still binds and validates its real static declaration.");
        CheckExecutionOrderAndFailures();
    }

    private static void CheckDeclarations(MethodInfo[] candidates)
    {
        var multiple = Method(typeof(OrderedValidators), nameof(OrderedValidators.EachProgram));
        Need(candidates.Count(method => Key(method) == Key(multiple)) == 1 &&
            multiple.GetCustomAttributes<ProgramCatalogValidationAttribute>().Count() == 2 &&
            !ProgramRuntimeModuleCatalog.IsDeclared(typeof(OrderedValidators)),
            "Multiple declarations resolve their MethodDef once without requiring an unrelated runtime-host marker.");
        var validators = ProgramCatalogValidations.Discover(Methods(typeof(OrderedValidators)).Reverse());
        Need(validators.Registrations.Select(row => row.Order).SequenceEqual([10, 20, 30, 40]),
            "All attributes on one static method remain separate ordered validations.");

        foreach (var reference in new[]
        {
            Method(typeof(InvalidValidators), nameof(InvalidValidators.Generic)),
            Method(typeof(GenericOwner<>), nameof(GenericOwner<int>.Validate)),
            Method(typeof(InstanceValidator), nameof(InstanceValidator.Validate)),
            Method(typeof(InheritedValidator), nameof(InheritedValidator.Validate)),
            Method(typeof(InvalidValidators), nameof(InvalidValidators.WrongSignature)),
            Method(typeof(InvalidValidators), nameof(InvalidValidators.ZeroOrder))
        })
        {
            var found = candidates.Single(method => Key(method) == Key(reference));
            Need(found.IsGenericMethodDefinition == reference.IsGenericMethodDefinition &&
                found.ContainsGenericParameters == reference.ContainsGenericParameters,
                "Metadata resolution cannot close or erase a generic method or declaring type before validation.");
            Expect<InvalidOperationException>(() => ProgramCatalogValidations.Discover([found]));
        }
        var inherited = candidates.Single(method => Key(method) ==
            Key(Method(typeof(InheritedValidator), nameof(InheritedValidator.Validate))));
        Need(!inherited.IsDefined(typeof(ProgramCatalogValidationAttribute), inherit: false) &&
            inherited.IsDefined(typeof(ProgramCatalogValidationAttribute), inherit: true),
            "An unannotated override is retained so the original inherited-attribute/static-only boundary can reject it.");
        var duplicate = Method(typeof(InvalidValidators), nameof(InvalidValidators.DuplicateOrder));
        var first = Method(typeof(OrderedValidators), nameof(OrderedValidators.Before));
        Expect<InvalidOperationException>(() => ProgramCatalogValidations.Discover([first, duplicate]));
        Expect<InvalidOperationException>(() => ProgramCatalogValidations.Discover([first, first]));
        Need(ProgramCatalogValidations.Discover([]).Registrations.Count == 0,
            "An empty catalog remains legal at the original Discover boundary.");
        Expect<ArgumentNullException>(() => ProgramCatalogValidationMethodDiscovery.Find(null!).ToArray());
        Expect<NullReferenceException>(() => ProgramCatalogValidations.Discover(null!));
        Expect<ArgumentNullException>(() => ProgramCatalogValidations.Discover([null!]));
    }

    private static void CheckExecutionOrderAndFailures()
    {
        var loaded = SkillProgramCatalog.LoadCurrent("""
            {"skills":[
              {"id":"fixture:catalog-first","revision":1,"activations":[{"id":"run","minCards":0,"maxCards":0,
                "minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                "effects":[{"op":"draw","target":"owner","amount":1}]}]},
              {"id":"fixture:catalog-second","revision":1,"activations":[{"id":"run","minCards":0,"maxCards":0,
                "minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                "effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
            """, $$$$"""
            {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{
              "fixture:catalog-first":{"name":"First","description":"Catalog order fixture"},
              "fixture:catalog-second":{"name":"Second","description":"Catalog order fixture"}}}
            """);
        var programs = new Dictionary<string, SkillProgram>
        {
            ["fixture:catalog-second"] = loaded.Programs["fixture:catalog-second"],
            ["fixture:catalog-first"] = loaded.Programs["fixture:catalog-first"]
        };
        try
        {
            Trace.Clear();
            ProgramCatalogValidations.Discover(Methods(typeof(OrderedValidators)).Reverse()).Validate(programs);
            Need(Trace.SequenceEqual(["before", "fixture:catalog-second", "fixture:catalog-first", "after",
                    "fixture:catalog-second", "fixture:catalog-first"]),
                "Validation remains validator-first, and every per-program validator uses the original programs.Values order.");
            Trace.Clear();
            OriginalFailure(() => ProgramCatalogValidations.Discover(
                [Method(typeof(ThrowingValidators), nameof(ThrowingValidators.Catalog)),
                 Method(typeof(ThrowingValidators), nameof(ThrowingValidators.AfterFailure))]).Validate(programs));
            Need(Trace.Count == 0, "A catalog validator failure is not wrapped or followed by another validation.");
            OriginalFailure(() => ProgramCatalogValidations.Discover(
                [Method(typeof(ThrowingValidators), nameof(ThrowingValidators.Program)),
                 Method(typeof(ThrowingValidators), nameof(ThrowingValidators.AfterFailure))]).Validate(programs));
            Need(Trace.SequenceEqual(["fixture:catalog-second"]),
                "The per-program delegate propagates the original first failure without visiting a later program.");
        }
        finally { Trace.Clear(); }
    }

    private static Assembly DynamicAssembly()
    {
        // The marker is internal to Core; this fixture uses the existing friend
        // identity so reflection must actually observe its emitted declaration.
        var assembly = AssemblyBuilder.DefineDynamicAssembly(new("CardGame.Core.Tests"),
            AssemblyBuilderAccess.RunAndCollect);
        var module = assembly.DefineDynamicModule("CatalogValidationFixture");
        var owner = module.DefineType("CatalogValidationFixture", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
        var validate = owner.DefineMethod("Validate", MethodAttributes.Public | MethodAttributes.Static, typeof(void),
            [typeof(IReadOnlyDictionary<string, SkillProgram>)]);
        var constructor = typeof(ProgramCatalogValidationAttribute).GetConstructor(BindingFlags.Instance | BindingFlags.Public |
            BindingFlags.NonPublic, null, [typeof(int)], null)!;
        validate.SetCustomAttribute(new CustomAttributeBuilder(constructor, [70]));
        validate.GetILGenerator().Emit(OpCodes.Ret);
        _ = owner.CreateType();
        return assembly;
    }

    private static bool IsCandidate(MethodInfo method) => method.IsVirtual ||
        method.IsDefined(typeof(ProgramCatalogValidationAttribute), inherit: true);
    private static MethodInfo[] Reflect(IReadOnlyList<Type> types) => types.SelectMany(Methods).ToArray();
    private static MethodInfo[] Methods(Type type) => type.GetMethods(DeclaredMethods);
    private static MethodInfo Method(Type type, string name) => type.GetMethod(name, DeclaredMethods)!;
    private static (Guid Module, int Token) Key(MethodInfo method) => (method.Module.ModuleVersionId, method.MetadataToken);
    private static IEnumerable<(int Order, Guid Module, int Token)> Rows(ProgramCatalogValidations catalog) =>
        catalog.Registrations.Select(row => (row.Order, row.Method.Module.ModuleVersionId, row.Method.MetadataToken));

    private static void SameMethods(MethodInfo[] expected, MethodInfo[] actual, string message) =>
        Need(expected.Select(method => (Key(method), method.IsStatic, method.IsVirtual, method.IsGenericMethodDefinition,
                method.ContainsGenericParameters)).SequenceEqual(actual.Select(method => (Key(method), method.IsStatic,
                method.IsVirtual, method.IsGenericMethodDefinition, method.ContainsGenericParameters))), message);
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Expect<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException) { return; }
        throw new InvalidOperationException($"Catalog discovery did not retain its {typeof(TException).Name} boundary.");
    }
    private static void OriginalFailure(Action action)
    {
        try { action(); }
        catch (InvalidOperationException error) when (ReferenceEquals(error, Failure)) { return; }
        throw new InvalidOperationException("A catalog validator replaced or wrapped its original exception.");
    }

    private static class OrderedValidators
    {
        [ProgramCatalogValidation(10)] internal static void Before(IReadOnlyDictionary<string, SkillProgram> programs) => Trace.Add("before");
        [ProgramCatalogValidation(20), ProgramCatalogValidation(40)]
        internal static void EachProgram(string id, SkillProgram program) => Trace.Add(id);
        [ProgramCatalogValidation(30)] internal static void After(IReadOnlyDictionary<string, SkillProgram> programs) => Trace.Add("after");
    }
    private static class InvalidValidators
    {
        [ProgramCatalogValidation(50)] internal static void Generic<T>(IReadOnlyDictionary<string, SkillProgram> programs) { }
        [ProgramCatalogValidation(51)] internal static int WrongSignature(IReadOnlyDictionary<string, SkillProgram> programs) => 0;
        [ProgramCatalogValidation(0)] internal static void ZeroOrder(IReadOnlyDictionary<string, SkillProgram> programs) { }
        [ProgramCatalogValidation(10)] internal static void DuplicateOrder(IReadOnlyDictionary<string, SkillProgram> programs) { }
    }
    private static class GenericOwner<T>
    { [ProgramCatalogValidation(52)] internal static void Validate(IReadOnlyDictionary<string, SkillProgram> programs) { } }
    private sealed class InstanceValidator
    { [ProgramCatalogValidation(53)] public void Validate(IReadOnlyDictionary<string, SkillProgram> programs) { } }
    private class InheritedBase
    { [ProgramCatalogValidation(54)] public virtual void Validate(IReadOnlyDictionary<string, SkillProgram> programs) { } }
    private sealed class InheritedValidator : InheritedBase
    { public override void Validate(IReadOnlyDictionary<string, SkillProgram> programs) { } }
    private static class ThrowingValidators
    {
        [ProgramCatalogValidation(60)] internal static void Catalog(IReadOnlyDictionary<string, SkillProgram> programs) => throw Failure;
        [ProgramCatalogValidation(61)] internal static void Program(string id, SkillProgram program) { Trace.Add(id); throw Failure; }
        [ProgramCatalogValidation(70)] internal static void AfterFailure(IReadOnlyDictionary<string, SkillProgram> programs) => Trace.Add("late");
    }
}
