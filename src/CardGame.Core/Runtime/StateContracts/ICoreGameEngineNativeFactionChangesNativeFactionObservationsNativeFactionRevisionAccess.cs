#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> _nativeFactionObservations { get; }

    ref long _nativeFactionRevision { get; }
}
