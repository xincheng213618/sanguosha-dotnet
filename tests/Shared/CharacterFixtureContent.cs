using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

/// <summary>Loads only the requested character manifests for small behavior fixtures.</summary>
internal static class CharacterFixtureContent
{
    private const string ResourcePrefix = "CardGame.Content.Standard.Characters.";
    private static readonly Assembly ContentAssembly = typeof(StandardContentPackage).Assembly;
    private static readonly Type CatalogType = ContentAssembly.GetType("CardGame.Content.Standard.CharacterContentCatalog", true)!;
    private static readonly MethodInfo LoadCatalog = CatalogType.GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic)!;
    private static readonly MethodInfo RegisterCatalog = CatalogType.GetMethod("Register", BindingFlags.Instance | BindingFlags.NonPublic)!;
    private static readonly ConcurrentDictionary<string, Lazy<Selection>> Selections = new(StringComparer.Ordinal);

    internal static void Register(IContentRegistryBuilder builder, params string[] generalIds)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(generalIds);
        if (generalIds.Length == 0 || generalIds.Any(id => string.IsNullOrWhiteSpace(id) || !id.Contains(':')))
            throw new ArgumentException("A fixture must select explicit general ids.", nameof(generalIds));
        var ids = generalIds.Distinct(StringComparer.Ordinal).OrderBy(id => id, StringComparer.Ordinal).ToArray();
        var key = string.Join('\n', ids);
        var selection = Selections.GetOrAdd(key, _ => new Lazy<Selection>(() => Load(ids),
            LazyThreadSafetyMode.ExecutionAndPublication)).Value;
        foreach (var packageId in selection.PackageIds)
            Invoke(RegisterCatalog, selection.Catalog, [builder, packageId]);
    }

    internal static SkillProgram SuppressionProgram(string id, int hp, SkillProgram? original = null)
    {
        var rules = $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":{{JsonSerializer.Serialize(id)}},"revision":1,
          "skillStates":[{"target":"owner","action":"suppress",
            "match":{"filter":["and",["==",["number","skill.character"],1],["==",["number","skill.isSource"],0]]},
            "while":["==",["hp","owner"],{{hp}}]}]}]}
        """;
        var presentations = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = id, description = id } } });
        var declared = SkillProgramCatalog.Load(rules, presentations).Programs[id];
        if (original is null) return declared;
        var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(
            original.GameplayHash + "\n" + declared.GameplayHash))).ToLowerInvariant();
        return new(original.Id, original.Revision, hash, original.RuntimeVersion, original.MinimumRulesVersion,
            original.Modifiers, original.ViewAs, original.Activations, original.Triggers, original.Contributions,
            original.CardIdentities, original.BooleanStates, original.DamageModifiers, original.CardPolicies,
            original.LordSkillProjection, original.CannotChallengeDeclarations, original.HpSkillGrants,
            original.EquipmentSuitSkillGrants, original.SkillStates.Concat(declared.SkillStates).ToArray());
    }

    private static Selection Load(IReadOnlyList<string> generalIds)
    {
        var sources = new List<(string Name, string Json)>();
        var packageIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in generalIds)
        {
            var name = ResourcePrefix + id.Replace(':', '-') + ".character.json";
            using var stream = ContentAssembly.GetManifestResourceStream(name) ??
                throw new InvalidOperationException($"No embedded character manifest exists for fixture general '{id}'.");
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            using var document = JsonDocument.Parse(json);
            if (!document.RootElement.GetProperty("generals").EnumerateArray()
                    .Any(general => general.GetProperty("definition").GetProperty("id").GetString() == id))
                throw new InvalidOperationException($"Character fixture resource '{name}' does not declare '{id}'.");
            packageIds.Add(document.RootElement.GetProperty("packageId").GetString()!);
            sources.Add((name, json));
        }
        return new Selection(Invoke(LoadCatalog, null, [sources])!,
            Array.AsReadOnly(packageIds.OrderBy(id => id, StringComparer.Ordinal).ToArray()));
    }

    private static object? Invoke(MethodInfo method, object? target, object?[] arguments)
    {
        try { return method.Invoke(target, arguments); }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        { ExceptionDispatchInfo.Capture(exception.InnerException).Throw(); throw; }
    }

    private sealed record Selection(object Catalog, IReadOnlyList<string> PackageIds);
}
