#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<int> _weaponCopyConsumed { get; }

    ref readonly Dictionary<int, IReadOnlyList<CardKind>> _copiedWeaponAbilities { get; }

    ref readonly Dictionary<int, IReadOnlyList<CardKind>> _inheritedWeaponAbilities { get; }
}
