#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAdvancedLifecycleState : Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess, Contracts.ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess
{
    private Dictionary<int, IReadOnlyList<CardKind>> _copiedWeaponAbilities = [];
    internal ref readonly Dictionary<int, IReadOnlyList<CardKind>> ReferenceToCopiedWeaponAbilities => ref _copiedWeaponAbilities;

    private Dictionary<int, IReadOnlyList<CardKind>> _inheritedWeaponAbilities = [];
    internal ref readonly Dictionary<int, IReadOnlyList<CardKind>> ReferenceToInheritedWeaponAbilities => ref _inheritedWeaponAbilities;

    private HashSet<int> _weaponCopyConsumed = [];
    internal ref readonly HashSet<int> ReferenceToWeaponCopyConsumed => ref _weaponCopyConsumed;

    ref readonly HashSet<int> Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._weaponCopyConsumed => ref _weaponCopyConsumed;

    ref readonly Dictionary<int, IReadOnlyList<CardKind>> Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._copiedWeaponAbilities => ref _copiedWeaponAbilities;

    ref readonly Dictionary<int, IReadOnlyList<CardKind>> Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._inheritedWeaponAbilities => ref _inheritedWeaponAbilities;

    ref readonly Dictionary<int, IReadOnlyList<CardKind>> Contracts.ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess._inheritedWeaponAbilities => ref _inheritedWeaponAbilities;
}
