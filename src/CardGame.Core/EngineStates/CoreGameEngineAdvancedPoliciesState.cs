#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAdvancedPoliciesState : Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess
{
    private HashSet<int> _nextUnlimitedCardOwners = [];
    internal ref readonly HashSet<int> ReferenceToNextUnlimitedCardOwners => ref _nextUnlimitedCardOwners;

    ref readonly HashSet<int> Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess._nextUnlimitedCardOwners => ref _nextUnlimitedCardOwners;
}
