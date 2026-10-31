#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAdvancedLifecycleState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess
{
    private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _copiedWeaponAbilities = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> ReferenceToCopiedWeaponAbilities => ref _copiedWeaponAbilities;

    private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _inheritedWeaponAbilities = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> ReferenceToInheritedWeaponAbilities => ref _inheritedWeaponAbilities;

    private global::System.Collections.Generic.HashSet<int> _weaponCopyConsumed = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToWeaponCopyConsumed => ref _weaponCopyConsumed;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._weaponCopyConsumed => ref _weaponCopyConsumed;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._copiedWeaponAbilities => ref _copiedWeaponAbilities;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess._inheritedWeaponAbilities => ref _inheritedWeaponAbilities;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess._inheritedWeaponAbilities => ref _inheritedWeaponAbilities;
}
