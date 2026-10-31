#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDragonBoatGeneralsState : Contracts.ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess
{
    private bool _synchronizingDragonBoatGrants = default !;
    internal ref bool ReferenceToSynchronizingDragonBoatGrants => ref _synchronizingDragonBoatGrants;

    ref bool Contracts.ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess._synchronizingDragonBoatGrants => ref _synchronizingDragonBoatGrants;
}
