namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.PrivateGeneralLibrarySource, global::CardGame.Core.PrivateGeneralLibrary> _privateGeneralLibraries => ref _privateGeneralLibraries2.ReferenceToPrivateGeneralLibraries;
    private ref long _privateGeneralLibraryRevision => ref _privateGeneralLibraries2.ReferenceToPrivateGeneralLibraryRevision;
    private ref bool _synchronizingPrivateGeneralLibraries => ref _privateGeneralLibraries2.ReferenceToSynchronizingPrivateGeneralLibraries;
    private ref long _combinedProjectionStamp => ref _privateGeneralLibraries2.ReferenceToCombinedProjectionStamp;
    private ref (long Lord, long Library) _combinedProjectionDependencies => ref _privateGeneralLibraries2.ReferenceToCombinedProjectionDependencies;
    private ref global::CardGame.Core.GameEngine.ProjectionDependency[] _privateLibraryOwnerDependencies => ref _privateGeneralLibraries2.ReferenceToPrivateLibraryOwnerDependencies;
}
