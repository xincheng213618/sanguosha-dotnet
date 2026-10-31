#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _inheritedWeaponAbilities { get; }
}
