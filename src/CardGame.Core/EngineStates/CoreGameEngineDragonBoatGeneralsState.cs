#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDragonBoatGeneralsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess
{
    private bool _synchronizingDragonBoatGrants = default !;
    internal ref bool ReferenceToSynchronizingDragonBoatGrants => ref _synchronizingDragonBoatGrants;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess._synchronizingDragonBoatGrants => ref _synchronizingDragonBoatGrants;
}
