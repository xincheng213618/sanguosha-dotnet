#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess : IProgramRuntimeOperations
{
    ref bool _synchronizingDragonBoatGrants { get; }
}
