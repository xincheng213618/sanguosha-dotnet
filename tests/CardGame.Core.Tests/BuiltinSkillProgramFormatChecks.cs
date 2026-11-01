using System.Reflection;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BuiltinSkillProgramFormatChecks
{
    public static void EmbeddedAndExternalFormsPreserveAllContentHashes()
    {
        var assembly = typeof(StandardContentPackage).Assembly;
        const string prefix = "CardGame.Content.Standard.SkillPrograms.";
        const string suffix = ".rules.json";
        var resources = assembly.GetManifestResourceNames().Where(name =>
            name.StartsWith(prefix, StringComparison.Ordinal) && name.EndsWith(suffix, StringComparison.Ordinal))
            .Order(StringComparer.Ordinal).ToArray();
        Need(resources.Length > 0, "The check must compare actual built-in rule resources.");
        var current = new Dictionary<(string Id, string Hash), SkillProgram>();
        var external = new Dictionary<(string Id, string Hash), SkillProgram>();
        string? sampleRules = null, samplePresentation = null;
        var programCount = 0;
        foreach (var resource in resources)
        {
            var rules = Read(resource);
            var presentation = Read(resource[..^suffix.Length] + ".presentation.json");
            var root = JsonNode.Parse(rules)!.AsObject();
            Need(!root.ContainsKey("schemaVersion"), resource + " must use the current embedded format.");
            var builtin = SkillProgramCatalog.LoadCurrent(rules, presentation);
            root["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion;
            var versioned = SkillProgramCatalog.Load(root.ToJsonString(), presentation);
            Need(builtin.Programs.Keys.Order(StringComparer.Ordinal).SequenceEqual(
                versioned.Programs.Keys.Order(StringComparer.Ordinal)), resource + " changed skill identities.");
            foreach (var (id, program) in builtin.Programs)
            {
                var other = versioned.Programs[id];
                Need(!ReferenceEquals(program, other) && program.GameplayHash == other.GameplayHash &&
                    program.RuntimeVersion == other.RuntimeVersion && program.MinimumRulesVersion == other.MinimumRulesVersion,
                    resource + ":" + id + " must be independently parsed with identical gameplay identity.");
                current.TryAdd((id, program.GameplayHash), program);
                external.TryAdd((id, other.GameplayHash), other);
                programCount++;
            }
            sampleRules ??= rules;
            samplePresentation ??= presentation;
        }

        var envelope = JsonNode.Parse(sampleRules!)!.AsObject();
        envelope["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion;
        Reject(() => SkillProgramCatalog.LoadCurrent(envelope.ToJsonString(), samplePresentation!));
        envelope["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion - 1;
        Reject(() => SkillProgramCatalog.LoadCurrent(envelope.ToJsonString(), samplePresentation!));
        Reject(() => SkillProgramCatalog.Load(envelope.ToJsonString(), samplePresentation!));
        Reject(() => SkillProgramCatalog.Load(sampleRules!, samplePresentation!));

        var registries = typeof(StandardContentRegistry).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.ReturnType == typeof(ContentRegistry) && method.GetParameters().Length == 0)
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .Select(method => (Name: method.Name, Registry: (ContentRegistry)method.Invoke(null, null)!)).ToList();
        registries.Add(("Showcase", ComposedSkillContentRegistry.CreateShowcase()));
        registries.Add(("ShowcaseAllActivities", ComposedSkillContentRegistry.CreateShowcase(
            includeActivities: true, includeBossActivities: true, includeHulaoActivities: true, includeShenwuActivities: true,
            includeNianActivities: true, includeHellActivities: true, includeWenheActivities: true,
            includeDragonBoatActivities: true, includeGuanduActivities: true)));
        registries.Add(("Wushuang", StandardWushuangContentRegistry.Create()));
        registries.Add(("ShanhePlayerRoster", StandardShanheCampaignCatalog.PlayerRosterRegistry));
        registries.Add(("ShanheEncounter", StandardShanheCampaignCatalog.CreateBattleRegistry(new(
            73, 1, "fixture:format-hash", "classic:cao-cao", 4, 0, 0, 0, 0, [], [], [],
            [new ShanheCombatantDefinition("classic:guan-yu")], [], new string('a', 64)))));
        var embeddedIds = current.Keys.Select(key => key.Id).ToHashSet(StringComparer.Ordinal);
        foreach (var (name, registry) in registries)
        {
            var builtinSkills = ReplacePrograms(registry, current);
            var externalSkills = ReplacePrograms(registry, external);
            Need(Fingerprint(registry, registry.Skills) == registry.ContentHash &&
                Fingerprint(registry, builtinSkills) == registry.ContentHash &&
                Fingerprint(registry, externalSkills) == registry.ContentHash,
                name + " must retain its real content hash in both independently parsed formats.");
            Console.WriteLine($"  {name}: {registry.ContentHash}");
        }
        var probe = registries[0].Registry;
        var changed = probe.Skills.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.Ordinal);
        var first = changed.Values.First(skill => skill.Program is not null);
        changed[first.Id] = first with { Program = null };
        Need(Fingerprint(probe, changed) != probe.ContentHash, "The fingerprint comparison must read supplied programs, not a cached registry hash.");
        Console.WriteLine($"  Compared {resources.Length} embedded bundles, {programCount} skill programs and {registries.Count} real registries.");

        string Read(string name)
        {
            using var stream = assembly.GetManifestResourceStream(name) ?? throw new InvalidOperationException("Missing resource " + name);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        Dictionary<string, ContentSkillDefinition> ReplacePrograms(ContentRegistry registry,
            IReadOnlyDictionary<(string Id, string Hash), SkillProgram> parsed) =>
            registry.Skills.ToDictionary(pair => pair.Key, pair =>
            {
                var definition = pair.Value;
                if (definition.Program is not { } program) return definition;
                // Inline/generated programs do not use the embedded rules envelope.
                if (!embeddedIds.Contains(program.Id)) return definition;
                Need(parsed.TryGetValue((program.Id, program.GameplayHash), out var replacement),
                    definition.Id + " must resolve to the independently parsed embedded program.");
                Need(!ReferenceEquals(program, replacement), definition.Id + " must not reuse the embedded catalog cache.");
                return definition with { Program = replacement };
            }, StringComparer.Ordinal);
    }

    private static string Fingerprint(ContentRegistry registry, IReadOnlyDictionary<string, ContentSkillDefinition> skills)
    {
        // Reuse both production fingerprint layers, retaining exact equipment-package ownership.
        var type = typeof(ContentRegistry);
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
        var contentHash = type.GetMethod("ComputeContentHash", flags)!.Invoke(null,
            [registry.Packages, registry.Cards, skills, registry.Generals, registry.Decks, registry.Modes]);
        var equipment = type.GetField("_extensionEquipmentRegistrations", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(registry);
        return (string)type.GetMethod("ExtendEquipmentContentHash", flags)!.Invoke(null, [contentHash, equipment])!;
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("The strict external envelope and current embedded format must remain distinct loading boundaries.");
    }
}
