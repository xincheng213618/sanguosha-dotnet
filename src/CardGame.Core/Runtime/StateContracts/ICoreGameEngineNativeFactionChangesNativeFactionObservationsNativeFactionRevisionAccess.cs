#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, GameEngine.NativeFactionObservation> _nativeFactionObservations { get; }

    ref long _nativeFactionRevision { get; }
}
