using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// This is a source index for existing name filters, not a claim of complete coverage.
internal static class InvariantVerificationIndex
{
    internal sealed record Result(string[] CoreFilters, string[] UnmatchedVersions, VersionMatch[] Matches);
    internal sealed record VersionMatch(string Version, string[] CoreFilters, ClassMatch[] Classes);
    internal sealed record ClassMatch(string Class, string[] Identifiers, string[] CoreFilters);
    private sealed record Registration(string Name, string Class);

    internal static string[] FindCoreFilters(string root, IReadOnlyList<string> versions) => Build(root, versions).CoreFilters;

    internal static Result Build(string root, IReadOnlyList<string> versions)
    {
        root = Path.GetFullPath(root);
        var testRoot = Path.Combine(root, "tests", "CardGame.Core.Tests");
        var registrations = ReadRegistrations(Path.Combine(testRoot, "Program.cs"));
        var registeredClasses = registrations.Select(r => r.Class).ToHashSet(StringComparer.Ordinal);
        var identities = versions.Select(version => Path.GetRelativePath(root,
                Path.GetFullPath(Path.Combine(root, version))).Replace('\\', '/').TrimEnd('/'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToDictionary(version => version, version => ReadIdentifiers(Path.Combine(root, version)), StringComparer.OrdinalIgnoreCase);
        var typeOwners = identities.Values.SelectMany(identity => identity.Types).GroupBy(type => type, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);
        var identifiers = identities.ToDictionary(pair => pair.Key,
            pair => pair.Value.Strings.Concat(pair.Value.Types.Where(type => typeOwners[type] == 1))
                .ToHashSet(StringComparer.Ordinal), StringComparer.OrdinalIgnoreCase);
        var allIds = identifiers.Values.SelectMany(ids => ids).ToHashSet(StringComparer.Ordinal);
        var hits = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        foreach (var path in Directory.EnumerateFiles(testRoot, "*.cs", SearchOption.AllDirectories).Where(NotBuildPath))
        {
            var syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot();
            foreach (var declaration in syntax.DescendantNodes().OfType<TypeDeclarationSyntax>()
                .Where(type => registeredClasses.Contains(type.Identifier.ValueText)))
            {
                var name = declaration.Identifier.ValueText;
                if (!hits.TryGetValue(name, out var found)) hits[name] = found = new(StringComparer.Ordinal);
                // Nested package/fixture declarations must not make every check in
                // their containing source file look character-specific.
                foreach (var member in declaration.Members.Where(member => member is not BaseTypeDeclarationSyntax and not DelegateDeclarationSyntax))
                foreach (var token in member.DescendantTokens())
                {
                    if (token.IsKind(SyntaxKind.IdentifierToken) && allIds.Contains(token.ValueText)) found.Add(token.ValueText);
                    if (token.Value is string literal)
                        foreach (var value in LiteralValues(literal))
                            if (allIds.Contains(value)) found.Add(value);
                }
            }
        }
        var matches = identifiers.Select(pair =>
        {
            var classes = hits.Select(hit => new ClassMatch(hit.Key, hit.Value.Intersect(pair.Value, StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray(),
                    registrations.Where(r => r.Class == hit.Key).Select(r => r.Name).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray()))
                .Where(hit => hit.Identifiers.Length > 0).OrderBy(hit => hit.Class, StringComparer.Ordinal).ToArray();
            return new VersionMatch(pair.Key, classes.SelectMany(hit => hit.CoreFilters).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray(), classes);
        }).OrderBy(match => match.Version, StringComparer.Ordinal).ToArray();
        return new(matches.SelectMany(match => match.CoreFilters).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray(),
            matches.Where(match => match.CoreFilters.Length == 0).Select(match => match.Version).ToArray(), matches);
    }

    private static Registration[] ReadRegistrations(string path) => CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot()
        .DescendantNodes().OfType<VariableDeclaratorSyntax>().Single(variable => variable.Identifier.ValueText == "tests")
        .Initializer!.Value.DescendantNodes().OfType<TupleExpressionSyntax>()
        .Where(tuple => tuple.Arguments.Count == 2 && tuple.Arguments[0].Expression is LiteralExpressionSyntax literal &&
            literal.IsKind(SyntaxKind.StringLiteralExpression) && tuple.Arguments[1].Expression is MemberAccessExpressionSyntax)
        .Select(tuple => new Registration(((LiteralExpressionSyntax)tuple.Arguments[0].Expression).Token.ValueText,
            ((MemberAccessExpressionSyntax)tuple.Arguments[1].Expression).Expression.DescendantTokens()
                .Last(token => token.IsKind(SyntaxKind.IdentifierToken)).ValueText)).ToArray();

    private static (HashSet<string> Strings, HashSet<string> Types) ReadIdentifiers(string directory)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        var types = new HashSet<string>(StringComparer.Ordinal);
        var content = Path.Combine(directory, "Content");
        if (Directory.Exists(content))
            foreach (var path in Directory.EnumerateFiles(content, "*.character.json", SearchOption.AllDirectories))
            {
                using var document = JsonDocument.Parse(File.ReadAllText(path));
                if (document.RootElement.TryGetProperty("generals", out var generals))
                    foreach (var general in generals.EnumerateArray())
                        if (general.TryGetProperty("definition", out var definition) && definition.TryGetProperty("id", out var id))
                            result.Add(id.GetString()!);
            }
        var skills = Path.Combine(directory, "Skills");
        if (Directory.Exists(skills))
            foreach (var path in Directory.EnumerateFiles(skills, "*.rules.json", SearchOption.AllDirectories))
            {
                using var document = JsonDocument.Parse(File.ReadAllText(path));
                if (document.RootElement.TryGetProperty("skills", out var definitions))
                    foreach (var definition in definitions.EnumerateArray())
                        if (definition.TryGetProperty("id", out var id)) result.Add(id.GetString()!);
            }
        var runtime = Path.Combine(directory, "Runtime");
        if (Directory.Exists(runtime))
            foreach (var path in Directory.EnumerateFiles(runtime, "*.cs", SearchOption.AllDirectories).Where(NotBuildPath))
                foreach (var type in CSharpSyntaxTree.ParseText(File.ReadAllText(path)).GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
                    // Short nested types such as Host, Facts and Receipt are not
                    // useful character identities when matching syntax tokens.
                    if ((!type.Modifiers.Any(SyntaxKind.PartialKeyword) || type is ClassDeclarationSyntax &&
                         type.Identifier.ValueText.StartsWith("Character", StringComparison.Ordinal) &&
                         type.Identifier.ValueText.EndsWith("Runtime", StringComparison.Ordinal)) &&
                        type.Identifier.ValueText.Length >= 12 && type.Identifier.ValueText is not
                        ("ProgramSkillFrame" or "CharacterState" or "CardUseFrame" or "CardMovementRecord" or "ResolutionFrame" or
                         "DamageFrame" or "SkillProgramDefinition" or "SkillProgramEffect" or "ProgramSkillWindowContext"))
                        types.Add(type.Identifier.ValueText);
        return (result, types);
    }

    private static IEnumerable<string> LiteralValues(string literal)
    {
        yield return literal;
        var trimmed = literal.TrimStart();
        if (trimmed.Length == 0 || trimmed[0] is not ('{' or '[')) yield break;
        JsonDocument? document;
        try { document = JsonDocument.Parse(literal); }
        catch (JsonException) { yield break; }
        using (document)
            foreach (var value in JsonStrings(document.RootElement)) yield return value;
    }

    private static IEnumerable<string> JsonStrings(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.String) yield return element.GetString()!;
        else if (element.ValueKind == JsonValueKind.Array)
            foreach (var child in element.EnumerateArray())
                foreach (var value in JsonStrings(child)) yield return value;
        else if (element.ValueKind == JsonValueKind.Object)
            foreach (var property in element.EnumerateObject())
                foreach (var value in JsonStrings(property.Value)) yield return value;
    }

    private static bool NotBuildPath(string path) => !path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        .Any(part => part is "bin" or "obj" or ".artifacts");
}
