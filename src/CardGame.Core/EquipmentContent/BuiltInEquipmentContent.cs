namespace CardGame.Core;

/// <summary>Assembly composition, not a mutable global registration list.</summary>
public static partial class BuiltInEquipmentContent
{
    public const string WushuangShopSource = "mode:wushuang-shop";
    public const string GeneralWeaponSource = "equipment:general-weapon";

    public static EquipmentContentPackage WushuangPurchases { get; } = new("equipment:wushuang-purchases", [],
        destinationPolicies: [new(WushuangShopSource, DestroyOnEquipmentExit: true,
            RestrictInitialPlacementToOwnerEquipment: true)]);

    // Computed after the independent package initializers have completed; no
    // static field depends on declaration order across the partial files.
    public static IReadOnlyList<EquipmentContentPackage> All =>
        Array.AsReadOnly<EquipmentContentPackage>([Standard, Hulao, Shenwu, Guandu, CharacterGenerated, Shanhe, WushuangPurchases]);

    /// <summary>
    /// Builds the equipment catalogue for one match. Registered cards, conversion outputs and
    /// native generation dependencies all participate; a deck-only filter would deny generated weapons.
    /// Extra kinds cover trusted encounter opening recipes, and extra packages allow new registrations.
    /// This catalogue describes content; it does not decide which acquisition routes a mode allows.
    /// </summary>
    public static EquipmentContentRegistry CreateForMatch(ContentModeDefinition mode, ContentRegistry registry,
        IEnumerable<CardKind>? additionalKinds = null, IEnumerable<EquipmentContentPackage>? additionalPackages = null)
    {
        ArgumentNullException.ThrowIfNull(mode);
        ArgumentNullException.ThrowIfNull(registry);
        var extras = (additionalPackages ?? []).ToArray();
        var packages = All.Concat(extras).ToArray();
        var available = EquipmentContentRegistry.Create(packages);
        var required = Standard.Definitions.Select(definition => definition.Kind).ToHashSet();
        required.UnionWith(registry.Cards.Values.Where(card => card.LegacyKind is not null)
            .Select(card => card.LegacyKind!.Value));
        required.UnionWith(additionalKinds ?? []);
        // Explicitly supplied content is an intentional installation, even if
        // its future generated entities are absent from this opening deck.
        required.UnionWith(extras.SelectMany(package => package.Definitions).Select(definition => definition.Kind));
        if (mode.HulaoActivity is not null) required.UnionWith(Hulao.Definitions.Select(definition => definition.Kind));
        if (mode.ShenwuActivity is not null) required.UnionWith(Shenwu.Definitions.Select(definition => definition.Kind));
        if (mode.GuanduActivity is not null) required.UnionWith(Guandu.Definitions.Select(definition => definition.Kind));

        var operations = new HashSet<SkillProgramEffectOp>();
        foreach (var skill in registry.Skills.Values)
        {
            if (skill.Program is not { } program) continue;
            required.UnionWith(program.ViewAs.Select(rule => rule.OutputKind));
            foreach (var effect in program.Activations.SelectMany(activation => activation.Effects)
                .Concat(program.Triggers.SelectMany(trigger => trigger.Effects)))
            {
                operations.Add(effect.Op);
                if (effect.OutputKind is { } kind) required.Add(kind);
                required.UnionWith(effect.CardKinds);
            }
        }
        required.UnionWith(packages.SelectMany(package => package.OperationDependencies)
            .Where(dependency => operations.Contains(dependency.Operation)).Select(dependency => dependency.Kind));

        var builder = new EquipmentContentBuilder();
        foreach (var package in packages)
        {
            var definitions = package.Definitions.Where(definition => required.Contains(definition.Kind)).ToArray();
            var bindings = package.SkillBindings.Where(binding => required.Contains(binding.Kind) &&
                registry.Skills.ContainsKey(binding.SkillId)).ToArray();
            var policies = package.DestinationPolicies.Where(policy =>
                (policy.RequiredEquipmentKind is not { } kind || required.Contains(kind)) &&
                (package != WushuangPurchases || mode.Id == WushuangMode.ModeId)).ToArray();
            if (definitions.Length == 0 && policies.Length == 0) continue;
            builder.Add(new EquipmentContentPackage(package.Id, definitions, bindings, policies));
        }
        return builder.Build();
    }
}
