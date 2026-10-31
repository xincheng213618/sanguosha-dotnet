#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, IReadOnlyList<CardKind>> _inheritedWeaponAbilities { get; }
}
