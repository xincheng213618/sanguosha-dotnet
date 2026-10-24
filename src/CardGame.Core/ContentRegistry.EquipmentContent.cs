using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardGame.Core;

public sealed partial class ContentRegistry
{
    /// <summary>Explicit equipment extensions, frozen and ordered by equipment package id.</summary>
    public IReadOnlyList<EquipmentContentPackage> EquipmentPackages { get; }

    private sealed record RegisteredEquipmentContent(string OwnerPackageId, EquipmentContentPackage Package);

    private static string ExtendEquipmentContentHash(string legacyContentHash,
        IReadOnlyList<RegisteredEquipmentContent> registrations)
    {
        // Existing registries deliberately keep their exact historical hash.
        // Only an explicit equipment registration adds this fingerprint layer.
        if (registrations.Count == 0) return legacyContentHash;
        var canonical = JsonSerializer.Serialize(new
        {
            LegacyContentHash = legacyContentHash,
            EquipmentPackages = registrations.OrderBy(item => item.Package.Id, StringComparer.Ordinal).Select(item => new
            {
                item.OwnerPackageId,
                item.Package.Id,
                Definitions = item.Package.Definitions.OrderBy(definition => definition.Kind).Select(definition => new
                {
                    Kind = definition.Kind.ToString(), definition.DisplayName, Slot = definition.Slot.ToString(),
                    definition.Description, definition.WeaponAttackRange, definition.AttackRangeBonus,
                    definition.SlashLimitBonus, definition.OutgoingDistanceModifier, definition.IncomingDistanceModifier,
                    definition.DrawCountBonus, definition.IgnoresArmor, definition.BlocksBlackSlash
                }).ToArray(),
                SkillBindings = item.Package.SkillBindings.OrderBy(binding => binding.Kind).Select(binding => new
                {
                    Kind = binding.Kind.ToString(), binding.SkillId, binding.GrantPrefix
                }).ToArray(),
                DestinationPolicies = item.Package.DestinationPolicies.OrderBy(policy => policy.SourceId, StringComparer.Ordinal).Select(policy => new
                {
                    policy.SourceId, policy.DestroyOnEquipmentExit, policy.RestrictInitialPlacementToOwnerEquipment,
                    RequiredEquipmentKind = policy.RequiredEquipmentKind?.ToString()
                }).ToArray(),
                OperationDependencies = item.Package.OperationDependencies.OrderBy(dependency => dependency.Operation)
                    .ThenBy(dependency => dependency.Kind).Select(dependency => new
                    {
                        Operation = dependency.Operation.ToString(), Kind = dependency.Kind.ToString()
                    }).ToArray()
            }).ToArray()
        });
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical)));
    }

    private sealed partial class RegistryBuilder
    {
        private readonly Dictionary<string, EquipmentContentPackage> _equipmentPackages = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _equipmentPackageOwners = new(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _skillPackageOwners = new(StringComparer.Ordinal);
        private IEnumerable<RegisteredEquipmentContent> EquipmentRegistrations => _equipmentPackages.Values
            .Select(package => new RegisteredEquipmentContent(_equipmentPackageOwners[package.Id], package));

        public void AddEquipmentContent(EquipmentContentPackage package)
        {
            ArgumentNullException.ThrowIfNull(package);
            var normalized = NormalizeEquipmentContent(package);
            Add(_equipmentPackages, normalized, normalized.Id, "equipment content");
        }

        private static EquipmentContentPackage NormalizeEquipmentContent(EquipmentContentPackage package)
        {
            if (package.Definitions.Any(item => item is null) || package.SkillBindings.Any(item => item is null) ||
                package.DestinationPolicies.Any(item => item is null) || package.OperationDependencies.Any(item => item is null))
                throw new InvalidOperationException($"Equipment package '{package.Id}' contains a null registration.");
            if (package.Definitions.Select(item => item.Kind).Distinct().Count() != package.Definitions.Count ||
                package.SkillBindings.Select(item => item.Kind).Distinct().Count() != package.SkillBindings.Count ||
                package.DestinationPolicies.Select(item => item.SourceId).Distinct(StringComparer.Ordinal).Count() != package.DestinationPolicies.Count ||
                package.OperationDependencies.Distinct().Count() != package.OperationDependencies.Count)
                throw new InvalidOperationException($"Equipment package '{package.Id}' repeats a definition, binding, policy or operation dependency.");
            if (package.Definitions.Any(item => !Enum.IsDefined(item.Kind)) ||
                package.SkillBindings.Any(item => !Enum.IsDefined(item.Kind)) ||
                package.DestinationPolicies.Any(item => item.RequiredEquipmentKind is { } kind && !Enum.IsDefined(kind)) ||
                package.OperationDependencies.Any(item => !Enum.IsDefined(item.Kind) || !Enum.IsDefined(item.Operation)))
                throw new InvalidOperationException($"Equipment package '{package.Id}' references an unknown card kind or operation.");
            foreach (var dependency in package.OperationDependencies) _ = ProgramOperationCatalog.Default.Resolve(dependency.Operation);
            return new(package.Id,
                package.Definitions.OrderBy(item => item.Kind),
                package.SkillBindings.OrderBy(item => item.Kind),
                package.DestinationPolicies.OrderBy(item => item.SourceId, StringComparer.Ordinal),
                package.OperationDependencies.OrderBy(item => item.Operation).ThenBy(item => item.Kind));
        }

        private void ValidateEquipmentReferences(IReadOnlyList<PackageManifest> manifests)
        {
            if (_equipmentPackages.Count == 0) return;
            var registrations = EquipmentRegistrations.ToArray();
            // Reuse the equipment registry's definition, binding and source-policy
            // conflict validation; identical shared definitions remain composable.
            _ = EquipmentContentRegistry.Create(registrations.Select(item => item.Package));
            var packages = manifests.ToDictionary(item => item.Id, StringComparer.Ordinal);
            var reachable = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            HashSet<string> Dependencies(string packageId)
            {
                if (reachable.TryGetValue(packageId, out var known)) return known;
                var allowed = new HashSet<string>(StringComparer.Ordinal) { packageId };
                foreach (var dependency in packages[packageId].Dependencies)
                    allowed.UnionWith(Dependencies(dependency.Id));
                reachable.Add(packageId, allowed);
                return allowed;
            }
            foreach (var registration in registrations)
            {
                var allowed = Dependencies(registration.OwnerPackageId);
                var equipment = registration.Package;
                foreach (var binding in equipment.SkillBindings)
                {
                    if (!_skills.ContainsKey(binding.SkillId))
                        throw new InvalidOperationException($"Equipment package '{equipment.Id}' references unknown skill '{binding.SkillId}'.");
                    if (!_skillPackageOwners.TryGetValue(binding.SkillId, out var owner) || !allowed.Contains(owner))
                        throw new InvalidOperationException($"Equipment package '{equipment.Id}' must declare a content-package dependency for skill '{binding.SkillId}'.");
                }
                foreach (var kind in equipment.SkillBindings.Select(item => item.Kind)
                    .Concat(equipment.DestinationPolicies.Where(item => item.RequiredEquipmentKind is not null).Select(item => item.RequiredEquipmentKind!.Value))
                    .Concat(equipment.OperationDependencies.Select(item => item.Kind)).Distinct())
                {
                    if (!registrations.Any(candidate => allowed.Contains(candidate.OwnerPackageId) &&
                            candidate.Package.Definitions.Any(definition => definition.Kind == kind)))
                        throw new InvalidOperationException($"Equipment package '{equipment.Id}' requires '{kind}' from its own content package or a declared dependency.");
                }
            }
        }
    }
}
