#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineAdvancedPoliciesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess
{
    private global::System.Collections.Generic.HashSet<int> _nextUnlimitedCardOwners = [];
    internal ref readonly global::System.Collections.Generic.HashSet<int> ReferenceToNextUnlimitedCardOwners => ref _nextUnlimitedCardOwners;

    ref readonly global::System.Collections.Generic.HashSet<int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess._nextUnlimitedCardOwners => ref _nextUnlimitedCardOwners;
}
