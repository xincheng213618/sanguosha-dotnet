#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePrivateGeneralLibrariesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess
{
    private (long Lord, long Library) _combinedProjectionDependencies = default !;
    internal ref (long Lord, long Library) ReferenceToCombinedProjectionDependencies => ref _combinedProjectionDependencies;

    private long _combinedProjectionStamp = default !;
    internal ref long ReferenceToCombinedProjectionStamp => ref _combinedProjectionStamp;

    private global::System.Collections.Generic.Dictionary<global::CardGame.Core.PrivateGeneralLibrarySource, global::CardGame.Core.PrivateGeneralLibrary> _privateGeneralLibraries = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.PrivateGeneralLibrarySource, global::CardGame.Core.PrivateGeneralLibrary> ReferenceToPrivateGeneralLibraries => ref _privateGeneralLibraries;

    private long _privateGeneralLibraryRevision = default !;
    internal ref long ReferenceToPrivateGeneralLibraryRevision => ref _privateGeneralLibraryRevision;

    private global::CardGame.Core.GameEngine.ProjectionDependency[] _privateLibraryOwnerDependencies = [];
    internal ref global::CardGame.Core.GameEngine.ProjectionDependency[] ReferenceToPrivateLibraryOwnerDependencies => ref _privateLibraryOwnerDependencies;

    private bool _synchronizingPrivateGeneralLibraries = default !;
    internal ref bool ReferenceToSynchronizingPrivateGeneralLibraries => ref _synchronizingPrivateGeneralLibraries;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.PrivateGeneralLibrarySource, global::CardGame.Core.PrivateGeneralLibrary> global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries => ref _privateGeneralLibraries;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._synchronizingPrivateGeneralLibraries => ref _synchronizingPrivateGeneralLibraries;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraryRevision => ref _privateGeneralLibraryRevision;

    ref global::CardGame.Core.GameEngine.ProjectionDependency[] global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._privateLibraryOwnerDependencies => ref _privateLibraryOwnerDependencies;

    ref (long Lord, long Library) global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionDependencies => ref _combinedProjectionDependencies;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionStamp => ref _combinedProjectionStamp;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.PrivateGeneralLibrarySource, global::CardGame.Core.PrivateGeneralLibrary> global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess._privateGeneralLibraries => ref _privateGeneralLibraries;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess._privateGeneralLibraryRevision => ref _privateGeneralLibraryRevision;
}
