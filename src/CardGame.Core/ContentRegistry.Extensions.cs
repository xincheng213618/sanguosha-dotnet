using System.Collections.ObjectModel;

namespace CardGame.Core;

public sealed partial class ContentRegistry
{
    private IReadOnlyDictionary<string, string> _extensionSkillPackageOwners = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
    private IReadOnlyList<RegisteredEquipmentContent> _extensionEquipmentRegistrations = [];

    private static ContentRegistry CaptureExtensionProvenance(ContentRegistry registry,
        IReadOnlyDictionary<string, string> skillOwners, IEnumerable<RegisteredEquipmentContent> equipment)
    {
        registry._extensionSkillPackageOwners = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(skillOwners, StringComparer.Ordinal));
        registry._extensionEquipmentRegistrations = Array.AsReadOnly(equipment.ToArray());
        return registry;
    }

    /// <summary>Copies this immutable snapshot, appends one package and validates the entire result.</summary>
    public ContentRegistry WithExtension(IGameContentPackage extension,
        Func<ContentModeDefinition, ContentModeDefinition>? mapExistingMode = null)
    {
        ArgumentNullException.ThrowIfNull(extension);
        var manifest = ValidateManifest(extension.Manifest);
        if (Packages.Any(p => p.Id == manifest.Id))
            throw new InvalidOperationException($"Content package '{manifest.Id}' is already registered.");
        var all = Packages.Append(manifest).ToDictionary(p => p.Id, StringComparer.Ordinal);
        var ordered = TopologicallyOrderPackages(all);
        var builder = new RegistryBuilder();
        builder.ImportSnapshot(this, mapExistingMode);
        extension.Register(builder.ForPackage(manifest.Id));
        builder.ValidateReferences(ordered);
        var result = builder.Freeze(ordered);
        var programs = result.Skills.Values.Select(s => s.Program).OfType<SkillProgram>().ToArray();
        SkillProgramRules.ValidateSetModifierConflicts(programs);
        ValidateExpandedProgramCardDomain(result, programs);
        return result;
    }

    private sealed partial class RegistryBuilder
    {
        internal void ImportSnapshot(ContentRegistry source, Func<ContentModeDefinition, ContentModeDefinition>? mapMode)
        {
            foreach (var card in source.Cards.Values) AddCard(card);
            foreach (var skill in source.Skills.Values)
            {
                AddSkill(skill);
                if (!source._extensionSkillPackageOwners.TryGetValue(skill.Id, out var owner))
                    throw new InvalidOperationException($"Skill '{skill.Id}' lost its original package owner.");
                _skillPackageOwners.Add(skill.Id, owner);
            }
            foreach (var general in source.Generals.Values) AddGeneral(general);
            foreach (var deck in source.Decks.Values) AddDeck(deck);
            foreach (var mode in source.Modes.Values)
            {
                var mapped = mapMode?.Invoke(mode) ?? mode;
                if (mapped.Id != mode.Id) throw new InvalidOperationException("An extension cannot rename an existing mode.");
                AddMode(mapped);
            }
            foreach (var registration in source._extensionEquipmentRegistrations)
            {
                AddEquipmentContent(registration.Package);
                _equipmentPackageOwners.Add(registration.Package.Id, registration.OwnerPackageId);
            }
            foreach (var factory in source.ModeFeatureFactories) AddModeFeatureFactory(factory);
        }
    }
}
