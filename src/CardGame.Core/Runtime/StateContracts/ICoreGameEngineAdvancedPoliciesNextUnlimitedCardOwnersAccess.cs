#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.HashSet<int> _nextUnlimitedCardOwners { get; }
}
