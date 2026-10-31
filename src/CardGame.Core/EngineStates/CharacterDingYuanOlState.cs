#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterDingYuanOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterDingYuanOlSynchronizingAdoptedChildGrantsAccess
{
    private bool _synchronizingAdoptedChildGrants = default !;
    internal ref bool ReferenceToSynchronizingAdoptedChildGrants => ref _synchronizingAdoptedChildGrants;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICharacterDingYuanOlSynchronizingAdoptedChildGrantsAccess._synchronizingAdoptedChildGrants => ref _synchronizingAdoptedChildGrants;
}
