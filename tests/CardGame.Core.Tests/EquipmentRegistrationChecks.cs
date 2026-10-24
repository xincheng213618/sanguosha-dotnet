using System.Reflection;
using CardGame.Core;

internal static class EquipmentRegistrationChecks
{
    private const string Skill = "test:registered-equipment-skill";
    private static readonly EquipmentDefinition Weapon = new(CardKind.Xuanjian, "测试玄剑", EquipmentSlot.Weapon,
        "本内容包的装备定义。", WeaponAttackRange: 6);
    private static readonly EquipmentDefinition Armor = new(CardKind.LionBelt, "测试护具", EquipmentSlot.Armor,
        "本内容包的第二件装备。", BlocksBlackSlash: true);

    public static void FrozenCompositionAndCompleteFingerprint()
    {
        var baseline = Build(Pack());
        var reversed = Build(new("test:registered-weapons", [Armor, Weapon],
            [new(CardKind.Xuanjian, Skill, "test:equipment-grant")],
            [new("test:equipment-origin", DestroyOnEquipmentExit: true, RequiredEquipmentKind: CardKind.Xuanjian)],
            [new(SkillProgramEffectOp.PlaceNamedWeapon, CardKind.Xuanjian)]));
        Require(baseline.ContentHash == reversed.ContentHash &&
            baseline.EquipmentPackages.Single().Definitions.Select(item => item.Kind)
                .SequenceEqual(baseline.EquipmentPackages.Single().Definitions.Select(item => item.Kind).Order()) &&
            baseline.EquipmentPackages is IList<EquipmentContentPackage> { IsReadOnly: true } &&
            baseline.EquipmentPackages.Single().Definitions is IList<EquipmentDefinition> { IsReadOnly: true },
            "The production scoped builder registers immutable equipment content in stable order, independent of input order.");

        EquipmentDefinition[] changedDefinitions =
        [
            Weapon with { WeaponAttackRange = 7 }, Weapon with { DisplayName = "另一名称" },
            Weapon with { Description = "另一规则说明" }, Weapon with { Slot = EquipmentSlot.Treasure },
            Weapon with { AttackRangeBonus = 1 }, Weapon with { SlashLimitBonus = 1 },
            Weapon with { OutgoingDistanceModifier = -1 }, Weapon with { IncomingDistanceModifier = 1 },
            Weapon with { DrawCountBonus = 1 }, Weapon with { IgnoresArmor = true },
            Weapon with { BlocksBlackSlash = true }
        ];
        Require(changedDefinitions.All(definition => Build(Pack(definition)).ContentHash != baseline.ContentHash) &&
            Build(Pack(armor: Armor with { Kind = CardKind.NationalStyleRobe })).ContentHash != baseline.ContentHash,
            "Every explicit equipment definition field participates in the content fingerprint.");
        Require(Build(Pack(binding: new(CardKind.Xuanjian, Skill, "test:other-grant"))).ContentHash != baseline.ContentHash &&
            Build(Pack(binding: new(CardKind.Xuanjian, "test:other-equipment-skill", "test:equipment-grant"))).ContentHash != baseline.ContentHash &&
            Build(Pack(policy: new("test:other-origin", DestroyOnEquipmentExit: true, RequiredEquipmentKind: CardKind.Xuanjian))).ContentHash != baseline.ContentHash &&
            Build(Pack(policy: new("test:equipment-origin", RequiredEquipmentKind: CardKind.Xuanjian))).ContentHash != baseline.ContentHash &&
            Build(Pack(policy: new("test:equipment-origin", DestroyOnEquipmentExit: true,
                RestrictInitialPlacementToOwnerEquipment: true, RequiredEquipmentKind: CardKind.Xuanjian))).ContentHash != baseline.ContentHash &&
            Build(Pack(policy: new("test:equipment-origin", DestroyOnEquipmentExit: true, RequiredEquipmentKind: CardKind.LionBelt))).ContentHash != baseline.ContentHash &&
            Build(Pack(dependency: new(SkillProgramEffectOp.EquipSampledGenerals, CardKind.Xuanjian))).ContentHash != baseline.ContentHash &&
            Build(Pack(dependency: new(SkillProgramEffectOp.PlaceNamedWeapon, CardKind.LionBelt))).ContentHash != baseline.ContentHash,
            "Grant identity, exact lifetime source and flags, required kind and native generation dependencies all affect recovery identity.");

        var legacy = Build(null);
        var computeLegacy = typeof(ContentRegistry).GetMethod("ComputeContentHash", BindingFlags.Static | BindingFlags.NonPublic)!;
        Require(legacy.EquipmentPackages.Count == 0 && legacy.ContentHash == (string)computeLegacy.Invoke(null,
            [legacy.Packages, legacy.Cards, legacy.Skills, legacy.Generals, legacy.Decks, legacy.Modes])! &&
            legacy.ContentHash != baseline.ContentHash,
            "An absent equipment extension preserves the exact existing hash path; installing explicit content changes it.");
    }

    public static void PackageDependenciesAndInvalidReferences()
    {
        var skillOwner = new Package("test:skill-owner", builder => builder.AddSkill(new(Skill, "装备技能", "测试。")));
        var dependency = new PackageDependency(skillOwner.Manifest.Id, skillOwner.Manifest.Version);
        var equipmentOwner = new Package("test:equipment-owner", builder => builder.AddEquipmentContent(Pack()), dependency);
        var ordered = ContentRegistry.Build(equipmentOwner, skillOwner);
        var reverse = ContentRegistry.Build(skillOwner, equipmentOwner);
        Require(ordered.ContentHash == reverse.ContentHash && ordered.EquipmentPackages.Count == 1,
            "Equipment bindings resolve skills through the owning content package's declared dependency.");
        var intermediary = new Package("test:intermediary", _ => { }, dependency);
        Require(ContentRegistry.Build(new Package("test:equipment-owner", builder => builder.AddEquipmentContent(Pack()),
                new PackageDependency(intermediary.Manifest.Id, intermediary.Manifest.Version)), intermediary, skillOwner)
            .EquipmentPackages.Count == 1,
            "Transitive content dependencies can supply equipment skills without copying their definitions.");
        Reject(() => ContentRegistry.Build(new Package("test:equipment-owner", builder => builder.AddEquipmentContent(Pack())), skillOwner),
            "An unrelated installed package cannot silently supply an undeclared equipment skill dependency.");
        Reject(() => Build(Pack(binding: new(CardKind.Xuanjian, "test:missing-skill", "test:grant"))),
            "A binding cannot reference an unknown skill.");
        Reject(() => Build(new("test:duplicate-weapons", [Weapon, Weapon])),
            "Repeating a definition within one equipment package is rejected.");
        Reject(() => Build(new("test:duplicate-bindings", [Weapon],
            [new(CardKind.Xuanjian, Skill, "test:grant"), new(CardKind.Xuanjian, Skill, "test:grant")])),
            "Repeating an equipment skill binding is rejected.");
        Reject(() => Build(new("test:duplicate-policies", [Weapon], destinationPolicies:
            [new("test:same-source"), new("test:same-source")])),
            "Repeating an equipment source policy is rejected.");
        Reject(() => ContentRegistry.Build(new Package("test:duplicates", builder =>
            { builder.AddEquipmentContent(new("test:duplicate-id", [Weapon])); builder.AddEquipmentContent(new("test:duplicate-id", [Weapon])); })),
            "Equipment content package ids remain unique across registrations.");
        Reject(() => ContentRegistry.Build(new Package("test:conflicts", builder =>
            { builder.AddEquipmentContent(new("test:first", [Weapon])); builder.AddEquipmentContent(new("test:second", [Weapon with { WeaponAttackRange = 2 }])); })),
            "Conflicting definitions for the same compatibility card kind are rejected.");
        Reject(() => Build(new("test:missing-generated-kind", [Weapon],
            operationDependencies: [new(SkillProgramEffectOp.PlaceNamedWeapon, CardKind.LionBelt)])),
            "Native generation metadata requires its registered equipment definition.");
        Reject(() => Build(new("test:duplicate-operation", [Weapon], operationDependencies:
            [new(SkillProgramEffectOp.PlaceNamedWeapon, CardKind.Xuanjian), new(SkillProgramEffectOp.PlaceNamedWeapon, CardKind.Xuanjian)])),
            "Duplicate generation dependency references are rejected.");

        var definitionOwner = new Package("test:definition-owner", builder => builder.AddEquipmentContent(new("test:base-weapon", [Weapon])));
        var sourceOnly = new EquipmentContentPackage("test:source-only", [],
            destinationPolicies: [new("test:source", DestroyOnEquipmentExit: true, RequiredEquipmentKind: CardKind.Xuanjian)]);
        Require(ContentRegistry.Build(definitionOwner, new Package("test:policy-owner", builder => builder.AddEquipmentContent(sourceOnly),
            new PackageDependency(definitionOwner.Manifest.Id, definitionOwner.Manifest.Version))).EquipmentPackages.Count == 2,
            "A lifetime policy can reuse an equipment definition supplied by a declared dependency.");
        Reject(() => ContentRegistry.Build(definitionOwner, new Package("test:policy-owner", builder => builder.AddEquipmentContent(sourceOnly))),
            "Equipment definition references obey package ownership as well as skill references.");
    }

    private static EquipmentContentPackage Pack(EquipmentDefinition? weapon = null, EquipmentSkillBinding? binding = null,
        EquipmentDestinationPolicy? policy = null, EquipmentOperationDependency? dependency = null, EquipmentDefinition? armor = null) =>
        new("test:registered-weapons", [weapon ?? Weapon, armor ?? Armor],
            [binding ?? new(CardKind.Xuanjian, Skill, "test:equipment-grant")],
            [policy ?? new("test:equipment-origin", DestroyOnEquipmentExit: true, RequiredEquipmentKind: CardKind.Xuanjian)],
            [dependency ?? new(SkillProgramEffectOp.PlaceNamedWeapon, CardKind.Xuanjian)]);

    private static ContentRegistry Build(EquipmentContentPackage? equipment) => ContentRegistry.Build(
        new Package("test:registered-equipment", builder =>
        {
            builder.AddSkill(new(Skill, "装备技能", "测试。"));
            builder.AddSkill(new("test:other-equipment-skill", "第二装备技能", "测试。"));
            if (equipment is not null) builder.AddEquipmentContent(equipment);
        }));

    private sealed class Package(string id, Action<IContentRegistryBuilder> register, params PackageDependency[] dependencies) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new(id, new Version(1, 0, 0), Array.AsReadOnly(dependencies));
        public void Register(IContentRegistryBuilder builder) => register(builder);
    }

    private static void Reject(Action action, string message)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException(message);
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
