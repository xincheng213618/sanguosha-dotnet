#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterDingYuanOlState : Contracts.ICharacterDingYuanOlSynchronizingAdoptedChildGrantsAccess
{
    private bool _synchronizingAdoptedChildGrants = default !;
    internal ref bool ReferenceToSynchronizingAdoptedChildGrants => ref _synchronizingAdoptedChildGrants;

    ref bool Contracts.ICharacterDingYuanOlSynchronizingAdoptedChildGrantsAccess._synchronizingAdoptedChildGrants => ref _synchronizingAdoptedChildGrants;
}
