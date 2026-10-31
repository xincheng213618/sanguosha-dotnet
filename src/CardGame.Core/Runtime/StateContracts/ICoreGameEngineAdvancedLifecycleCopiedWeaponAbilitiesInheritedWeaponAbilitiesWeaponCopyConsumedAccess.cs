#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.HashSet<int> _weaponCopyConsumed { get; }

    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _copiedWeaponAbilities { get; }

    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _inheritedWeaponAbilities { get; }
}
