#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineNativeFactionChangesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess
{
    private global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> _nativeFactionObservations = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> ReferenceToNativeFactionObservations => ref _nativeFactionObservations;

    private (long Base, long Revision) _nativeFactionQualificationDependencies = default !;
    internal ref (long Base, long Revision) ReferenceToNativeFactionQualificationDependencies => ref _nativeFactionQualificationDependencies;

    private long _nativeFactionQualificationStamp = default !;
    internal ref long ReferenceToNativeFactionQualificationStamp => ref _nativeFactionQualificationStamp;

    private long _nativeFactionRevision = default !;
    internal ref long ReferenceToNativeFactionRevision => ref _nativeFactionRevision;

    private bool _synchronizingNativeFactions = default !;
    internal ref bool ReferenceToSynchronizingNativeFactions => ref _synchronizingNativeFactions;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionObservations => ref _nativeFactionObservations;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionRevision => ref _nativeFactionRevision;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations => ref _nativeFactionObservations;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision => ref _nativeFactionRevision;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._synchronizingNativeFactions => ref _synchronizingNativeFactions;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationStamp => ref _nativeFactionQualificationStamp;

    ref (long Base, long Revision) global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationDependencies => ref _nativeFactionQualificationDependencies;
}
