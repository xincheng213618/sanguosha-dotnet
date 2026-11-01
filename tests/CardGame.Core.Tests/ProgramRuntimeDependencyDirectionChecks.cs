using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using CardGame.Core;

internal static class ProgramRuntimeDependencyDirectionChecks
{
    internal static void Run()
    {
        var assembly = typeof(GameEngine).Assembly;
        using var pdbStream = File.OpenRead(Path.ChangeExtension(assembly.Location, ".pdb"));
        using var pdb = MetadataReaderProvider.FromPortablePdbStream(pdbStream);
        var reader = pdb.GetMetadataReader();
        var modules = ProgramRuntimeModuleCatalog.Default;
        var capabilities = ProgramCapabilityCatalog.Default;
        var owners = new Dictionary<Type, string>();
        var targets = new Dictionary<Type, Type[]>();
        var actual = new HashSet<string>(StringComparer.Ordinal);
        foreach (var module in modules.Registrations.Where(module => module.Constructor is not null))
        {
            foreach (var dependency in module.Dependencies)
            foreach (var target in Targets(dependency))
            {
                var from = Owner(module.Type);
                var to = Owner(target);
                if (to == "shared" || from == to) continue;
                actual.Add($"{from}\t{module.Type.FullName}\t{Name(dependency)}\t{to}\t{target.FullName}");
            }
        }
        using var stream = typeof(ProgramRuntimeDependencyDirectionChecks).Assembly.GetManifestResourceStream(
            "CardGame.Core.Tests.Fixtures.runtime-module-dependency-directions.txt")
            ?? throw new InvalidOperationException("The runtime dependency direction baseline is missing.");
        using var baselineReader = new StreamReader(stream);
        var rows = baselineReader.ReadToEnd().Split('\n').Select(row => row.TrimEnd('\r'))
            .Where(row => row.Length > 0 && !row.StartsWith('#')).ToArray();
        var baseline = rows.ToHashSet(StringComparer.Ordinal);
        if (baseline.Count != rows.Length) throw new InvalidOperationException("The runtime dependency baseline contains duplicate edges.");
        var added = actual.Except(baseline).Order(StringComparer.Ordinal).ToArray();
        var retired = baseline.Except(actual).Order(StringComparer.Ordinal).ToArray();
        if (added.Length != 0 || retired.Length != 0)
            throw new InvalidOperationException("Runtime dependency directions must shrink their exact baseline.\nNew forbidden edges:\n" +
                string.Join('\n', added) + "\nRemoved edges still in the baseline (delete these entries):\n" + string.Join('\n', retired));

        Type[] Targets(Type dependency)
        {
            if (targets.TryGetValue(dependency, out var cached)) return cached;
            var result = new HashSet<Type>();
            if (modules.TryGetEntry(dependency, out var entry)) result.Add(entry.Owner);
            else if (dependency.GetCustomAttribute<ProgramRuntimeEntryAttribute>() is { } alias) result.Add(alias.Owner);
            else if (ProgramRuntimeModuleCatalog.IsDeclared(dependency)) result.Add(dependency);
            else if (dependency.IsInterface)
            {
                if (capabilities.TryResolve(dependency, out var capability)) result.Add(capability.DeferredModule ?? capability.Implementation);
                else foreach (var provider in modules.Registrations.Where(provider => dependency.IsAssignableFrom(provider.Type))) result.Add(provider.Type);
            }
            if (dependency.IsConstructedGenericType)
                foreach (var argument in dependency.GetGenericArguments()) result.UnionWith(Targets(argument));
            return targets[dependency] = result.ToArray();
        }

        string Owner(Type type)
        {
            if (owners.TryGetValue(type, out var cached)) return cached;
            var methods = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Cast<MethodBase>()
                .Concat(type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
            foreach (var method in methods)
            {
                var information = reader.GetMethodDebugInformation(MetadataTokens.MethodDebugInformationHandle(method.MetadataToken & 0x00ffffff));
                IEnumerable<DocumentHandle> documents = information.Document.IsNil ? information.GetSequencePoints().Select(point => point.Document) : [information.Document];
                foreach (var document in documents.Where(document => !document.IsNil))
                {
                    var path = reader.GetString(reader.GetDocument(document).Name).Replace('\\', '/');
                    var source = path.LastIndexOf("/src/", StringComparison.Ordinal);
                    if (source < 0) continue;
                    var relative = path[(source + 5)..].Split('/');
                    if (relative[0] != "Characters" || relative[1] == "_shared") return owners[type] = "shared";
                    if (relative.Length > 3) return owners[type] = relative[1] + "/" + relative[2];
                }
            }
            throw new InvalidOperationException($"Cannot determine the actual source owner of runtime dependency '{type.FullName}' from its portable PDB.");
        }
    }

    private static string Name(Type type) => !type.IsConstructedGenericType ? type.FullName! :
        type.GetGenericTypeDefinition().FullName + "<" + string.Join(',', type.GetGenericArguments().Select(Name)) + ">";
}
