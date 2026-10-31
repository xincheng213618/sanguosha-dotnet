#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEnginePrivateGeneralLibrariesState : Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess, Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess
{
    private (long Lord, long Library) _combinedProjectionDependencies = default !;
    internal ref (long Lord, long Library) ReferenceToCombinedProjectionDependencies => ref _combinedProjectionDependencies;

    private long _combinedProjectionStamp = default !;
    internal ref long ReferenceToCombinedProjectionStamp => ref _combinedProjectionStamp;

    private Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> _privateGeneralLibraries = [];
    internal ref readonly Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> ReferenceToPrivateGeneralLibraries => ref _privateGeneralLibraries;

    private long _privateGeneralLibraryRevision = default !;
    internal ref long ReferenceToPrivateGeneralLibraryRevision => ref _privateGeneralLibraryRevision;

    private GameEngine.ProjectionDependency[] _privateLibraryOwnerDependencies = [];
    internal ref GameEngine.ProjectionDependency[] ReferenceToPrivateLibraryOwnerDependencies => ref _privateLibraryOwnerDependencies;

    private bool _synchronizingPrivateGeneralLibraries = default !;
    internal ref bool ReferenceToSynchronizingPrivateGeneralLibraries => ref _synchronizingPrivateGeneralLibraries;

    ref readonly Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries => ref _privateGeneralLibraries;

    ref bool Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._synchronizingPrivateGeneralLibraries => ref _synchronizingPrivateGeneralLibraries;

    ref long Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraryRevision => ref _privateGeneralLibraryRevision;

    ref GameEngine.ProjectionDependency[] Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._privateLibraryOwnerDependencies => ref _privateLibraryOwnerDependencies;

    ref (long Lord, long Library) Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionDependencies => ref _combinedProjectionDependencies;

    ref long Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionStamp => ref _combinedProjectionStamp;

    ref readonly Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess._privateGeneralLibraries => ref _privateGeneralLibraries;

    ref long Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess._privateGeneralLibraryRevision => ref _privateGeneralLibraryRevision;
}
