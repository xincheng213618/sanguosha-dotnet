#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<int> _nextUnlimitedCardOwners { get; }
}
