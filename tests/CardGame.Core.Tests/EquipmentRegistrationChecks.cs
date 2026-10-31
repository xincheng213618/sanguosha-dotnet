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

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
