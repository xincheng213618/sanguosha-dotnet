using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace CardGame.Core;

// The built-in catalog needs only marked methods and virtual methods that can
// inherit a marker. Discover still owns every attribute and signature decision.
internal static class ProgramCatalogValidationMethodDiscovery
{
    private const BindingFlags DeclaredMethods = BindingFlags.Static | BindingFlags.Instance |
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    internal static IEnumerable<MethodInfo> Find(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        // Preserve the original assembly/type loading boundary even when a PE
        // image is unavailable or the optional index cannot be read.
        var types = assembly.GetTypes();
        try
        {
            if (assembly.GetModules() is [var module] && !string.IsNullOrEmpty(assembly.Location))
            {
                using var image = File.OpenRead(assembly.Location);
                if (TryRead(module, types, image, out var methods)) return methods;
            }
        }
        catch (Exception error) when (error is not OutOfMemoryException and not StackOverflowException)
        {
            // File-backed metadata is an optimization, never a requirement for
            // the original reflection discovery (including dynamic assemblies).
        }
        return types.SelectMany(type => type.GetMethods(DeclaredMethods));
    }

    internal static bool TryRead(Module module, IReadOnlyList<Type> types, Stream image,
        out MethodInfo[] methods)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(image);
        methods = [];
        try
        {
            if (module.Assembly.GetModules() is not [var only] || only != module) return false;
            using var pe = new PEReader(image, PEStreamOptions.PrefetchMetadata | PEStreamOptions.LeaveOpen);
            var reader = pe.GetMetadataReader();
            if (reader.GetGuid(reader.GetModuleDefinition().Mvid) != module.ModuleVersionId) return false;

            var marker = typeof(ProgramCatalogValidationAttribute);
            var constructor = marker.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single();
            var markerType = (TypeDefinitionHandle)MetadataTokens.EntityHandle(marker.MetadataToken);
            var constructors = new Dictionary<EntityHandle, bool>();
            var direct = new HashSet<MethodDefinitionHandle>();
            foreach (var handle in reader.CustomAttributes)
            {
                var attribute = reader.GetCustomAttribute(handle);
                if (attribute.Parent.Kind == HandleKind.MethodDefinition && IsMarkerConstructor(attribute.Constructor))
                    direct.Add((MethodDefinitionHandle)attribute.Parent);
            }

            var selected = new List<MethodInfo>();
            foreach (var type in types)
            {
                if (type.Module != module || MetadataTokens.EntityHandle(type.MetadataToken).Kind != HandleKind.TypeDefinition)
                    return false;
                Type[]? genericArguments = null;
                var argumentsRead = false;
                // GetTypes preserves type order; MethodDef order preserves each
                // type's original declared-method order without reflecting all
                // of its unmarked, nonvirtual methods.
                foreach (var handle in reader.GetTypeDefinition((TypeDefinitionHandle)MetadataTokens.EntityHandle(type.MetadataToken)).GetMethods())
                {
                    var definition = reader.GetMethodDefinition(handle);
                    if ((definition.Attributes & MethodAttributes.Virtual) == 0 && !direct.Contains(handle)) continue;
                    if (reader.StringComparer.Equals(definition.Name, ".ctor") ||
                        reader.StringComparer.Equals(definition.Name, ".cctor")) continue;
                    if (!argumentsRead)
                    {
                        genericArguments = type.IsGenericType ? type.GetGenericArguments() : null;
                        argumentsRead = true;
                    }
                    if (module.ResolveMethod(MetadataTokens.GetToken(handle), genericArguments, null) is not MethodInfo method)
                        return false;
                    // ResolveMethod can return a self-instantiated generic
                    // method; GetMethods returns its original definition.
                    if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
                        method = method.GetGenericMethodDefinition();
                    selected.Add(method);
                }
            }
            methods = selected.ToArray();
            return true;

            bool IsMarkerConstructor(EntityHandle handle)
            {
                if (constructors.TryGetValue(handle, out var known)) return known;
                var result = false;
                if (handle.Kind == HandleKind.MethodDefinition)
                    result = marker.Module == module && MetadataTokens.GetToken(handle) == constructor.MetadataToken &&
                        reader.GetMethodDefinition((MethodDefinitionHandle)handle).GetDeclaringType() == markerType;
                else if (handle.Kind == HandleKind.MemberReference)
                {
                    var reference = reader.GetMemberReference((MemberReferenceHandle)handle);
                    var couldMatch = marker.Module == module && reference.Parent == markerType;
                    if (reference.Parent.Kind == HandleKind.TypeReference)
                    {
                        var parent = reader.GetTypeReference((TypeReferenceHandle)reference.Parent);
                        couldMatch = reader.StringComparer.Equals(parent.Name, marker.Name) &&
                            reader.StringComparer.Equals(parent.Namespace, marker.Namespace!);
                    }
                    if (couldMatch)
                        result = module.ResolveMethod(MetadataTokens.GetToken(handle)) is ConstructorInfo resolved &&
                            resolved.DeclaringType == marker && resolved.MetadataToken == constructor.MetadataToken;
                }
                else throw new BadImageFormatException("Unsupported custom-attribute constructor handle.");
                constructors.Add(handle, result);
                return result;
            }
        }
        catch (Exception error) when (error is not OutOfMemoryException and not StackOverflowException)
        {
            // An incomplete candidate list must never reach Discover. Its
            // original validation runs only after the full fallback is chosen.
            methods = [];
            return false;
        }
    }
}
