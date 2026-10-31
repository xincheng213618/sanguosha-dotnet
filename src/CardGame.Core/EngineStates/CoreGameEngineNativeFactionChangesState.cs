#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineNativeFactionChangesState : Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess, Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess
{
    private Dictionary<int, GameEngine.NativeFactionObservation> _nativeFactionObservations = [];
    internal ref readonly Dictionary<int, GameEngine.NativeFactionObservation> ReferenceToNativeFactionObservations => ref _nativeFactionObservations;

    private (long Base, long Revision) _nativeFactionQualificationDependencies = default !;
    internal ref (long Base, long Revision) ReferenceToNativeFactionQualificationDependencies => ref _nativeFactionQualificationDependencies;

    private long _nativeFactionQualificationStamp = default !;
    internal ref long ReferenceToNativeFactionQualificationStamp => ref _nativeFactionQualificationStamp;

    private long _nativeFactionRevision = default !;
    internal ref long ReferenceToNativeFactionRevision => ref _nativeFactionRevision;

    private bool _synchronizingNativeFactions = default !;
    internal ref bool ReferenceToSynchronizingNativeFactions => ref _synchronizingNativeFactions;

    ref readonly Dictionary<int, GameEngine.NativeFactionObservation> Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionObservations => ref _nativeFactionObservations;

    ref long Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionRevision => ref _nativeFactionRevision;

    ref readonly Dictionary<int, GameEngine.NativeFactionObservation> Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionObservations => ref _nativeFactionObservations;

    ref long Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionRevision => ref _nativeFactionRevision;

    ref bool Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._synchronizingNativeFactions => ref _synchronizingNativeFactions;

    ref long Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationStamp => ref _nativeFactionQualificationStamp;

    ref (long Base, long Revision) Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess._nativeFactionQualificationDependencies => ref _nativeFactionQualificationDependencies;
}
