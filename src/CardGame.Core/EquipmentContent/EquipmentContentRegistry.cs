using System.Collections.ObjectModel;

namespace CardGame.Core;

/// <summary>Equipment-owned skills, including their existing stable grant identities.</summary>
public sealed record EquipmentSkillBinding(CardKind Kind, string SkillId, string GrantPrefix)
{
    public string CreateGrantId(int cardId) => $"{GrantPrefix}:{cardId}";
}

/// <summary>Equipment created implicitly by a native program operation rather than an output-kind node.</summary>
public sealed record EquipmentOperationDependency(SkillProgramEffectOp Operation, CardKind Kind);

/// <summary>Immutable equipment content; registration never captures a running match.</summary>
public sealed class EquipmentContentPackage
{
    public EquipmentContentPackage(string id, IEnumerable<EquipmentDefinition> definitions,
        IEnumerable<EquipmentSkillBinding>? skillBindings = null,
        IEnumerable<EquipmentDestinationPolicy>? destinationPolicies = null,
        IEnumerable<EquipmentOperationDependency>? operationDependencies = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(definitions);
        Id = id;
        Definitions = Array.AsReadOnly(definitions.ToArray());
        SkillBindings = Array.AsReadOnly((skillBindings ?? []).ToArray());
        DestinationPolicies = Array.AsReadOnly((destinationPolicies ?? []).ToArray());
        OperationDependencies = Array.AsReadOnly((operationDependencies ?? []).ToArray());
    }

    public string Id { get; }
    public IReadOnlyList<EquipmentDefinition> Definitions { get; }
    public IReadOnlyList<EquipmentSkillBinding> SkillBindings { get; }
    public IReadOnlyList<EquipmentDestinationPolicy> DestinationPolicies { get; }
    public IReadOnlyList<EquipmentOperationDependency> OperationDependencies { get; }
}

/// <summary>A match-local builder. Identical shared definitions may be composed; conflicting rules fail.</summary>
public sealed class EquipmentContentBuilder
{
    private readonly Dictionary<CardKind, EquipmentDefinition> _definitions = [];
    private readonly Dictionary<CardKind, EquipmentSkillBinding> _bindings = [];
    private readonly Dictionary<string, EquipmentDestinationPolicy> _policies = new(StringComparer.Ordinal);
    private readonly HashSet<string> _packageIds = new(StringComparer.Ordinal);

    public EquipmentContentBuilder Add(EquipmentContentPackage package)
    {
        ArgumentNullException.ThrowIfNull(package);
        foreach (var definition in package.Definitions) AddDefinition(definition);
        foreach (var binding in package.SkillBindings) AddSkillBinding(binding);
        foreach (var policy in package.DestinationPolicies) AddDestinationPolicy(policy);
        _packageIds.Add(package.Id);
        return this;
    }

    public EquipmentContentBuilder AddDefinition(EquipmentDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentException.ThrowIfNullOrWhiteSpace(definition.DisplayName);
        if (!Enum.IsDefined(definition.Slot) || definition.WeaponAttackRange is < 0)
            throw new ArgumentException("An equipment definition has an invalid slot or attack range.", nameof(definition));
        AddUnique(_definitions, definition.Kind, definition, "equipment definition");
        return this;
    }

    public EquipmentContentBuilder AddSkillBinding(EquipmentSkillBinding binding)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentException.ThrowIfNullOrWhiteSpace(binding.SkillId);
        ArgumentException.ThrowIfNullOrWhiteSpace(binding.GrantPrefix);
        AddUnique(_bindings, binding.Kind, binding, "equipment skill binding");
        return this;
    }

    public EquipmentContentBuilder AddDestinationPolicy(EquipmentDestinationPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        ArgumentException.ThrowIfNullOrWhiteSpace(policy.SourceId);
        AddUnique(_policies, policy.SourceId, policy, "equipment destination policy");
        return this;
    }

    public EquipmentContentRegistry Build()
    {
        if (_bindings.Keys.Any(kind => !_definitions.ContainsKey(kind)))
            throw new InvalidOperationException("An equipment skill binding requires its registered equipment definition.");
        if (_policies.Values.Any(policy => policy.RequiredEquipmentKind is { } kind && !_definitions.ContainsKey(kind)))
            throw new InvalidOperationException("An equipment destination policy requires its registered equipment definition.");
        return new(_definitions, _bindings, _policies, _packageIds);
    }

    private static void AddUnique<TKey, TValue>(Dictionary<TKey, TValue> target, TKey key, TValue value, string category)
        where TKey : notnull
    {
        if (target.TryGetValue(key, out var existing))
        {
            if (!EqualityComparer<TValue>.Default.Equals(existing, value))
                throw new InvalidOperationException($"Conflicting {category} registration '{key}'.");
            return;
        }
        target.Add(key, value);
    }
}

/// <summary>Only content assembled for this registry is available; no process-wide registrations are mutated.</summary>
public sealed class EquipmentContentRegistry
{
    internal EquipmentContentRegistry(Dictionary<CardKind, EquipmentDefinition> definitions,
        Dictionary<CardKind, EquipmentSkillBinding> bindings,
        Dictionary<string, EquipmentDestinationPolicy> policies, IEnumerable<string> packageIds)
    {
        Definitions = new ReadOnlyDictionary<CardKind, EquipmentDefinition>(new Dictionary<CardKind, EquipmentDefinition>(definitions));
        SkillBindings = new ReadOnlyDictionary<CardKind, EquipmentSkillBinding>(new Dictionary<CardKind, EquipmentSkillBinding>(bindings));
        DestinationPolicies = new ReadOnlyDictionary<string, EquipmentDestinationPolicy>(new Dictionary<string, EquipmentDestinationPolicy>(policies, StringComparer.Ordinal));
        PackageIds = Array.AsReadOnly(packageIds.Order(StringComparer.Ordinal).ToArray());
        Implemented = Array.AsReadOnly(definitions.Values.OrderBy(definition => definition.Slot)
            .ThenBy(definition => definition.Kind).ToArray());
    }

    public IReadOnlyDictionary<CardKind, EquipmentDefinition> Definitions { get; }
    public IReadOnlyDictionary<CardKind, EquipmentSkillBinding> SkillBindings { get; }
    public IReadOnlyDictionary<string, EquipmentDestinationPolicy> DestinationPolicies { get; }
    public IReadOnlyList<string> PackageIds { get; }
    public IReadOnlyList<EquipmentDefinition> Implemented { get; }

    public static EquipmentContentRegistry Create(params EquipmentContentPackage[] packages) => Create((IEnumerable<EquipmentContentPackage>)packages);

    public static EquipmentContentRegistry Create(IEnumerable<EquipmentContentPackage> packages)
    {
        ArgumentNullException.ThrowIfNull(packages);
        var builder = new EquipmentContentBuilder();
        foreach (var package in packages) builder.Add(package);
        return builder.Build();
    }

    public bool IsEquipment(CardKind kind) => Definitions.ContainsKey(kind);
    public bool TryGet(CardKind kind, out EquipmentDefinition? definition) => Definitions.TryGetValue(kind, out definition);
    public EquipmentDefinition Get(CardKind kind) => Definitions.TryGetValue(kind, out var definition) ? definition :
        throw new ArgumentOutOfRangeException(nameof(kind), kind, "No equipment content is registered for this card kind.");
    public EquipmentSkillBinding? GetSkillBinding(CardKind kind) => SkillBindings.GetValueOrDefault(kind);

    public CardLocation NormalizeDestination(EquipmentDestinationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Origin is not { } origin || !DestinationPolicies.TryGetValue(origin.SourceId, out var policy))
            return context.To;
        if (!IsEquipment(context.Card.Kind))
            throw new InvalidOperationException("Equipment provenance requires a registered equipment definition.");
        return policy.Normalize(context);
    }
}
