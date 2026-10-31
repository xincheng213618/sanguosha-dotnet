#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePrivateGeneralLibrariesFieldsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> _privateGeneralLibraries { get; }

    ref bool _synchronizingPrivateGeneralLibraries { get; }

    ref long _privateGeneralLibraryRevision { get; }

    ref GameEngine.ProjectionDependency[] _privateLibraryOwnerDependencies { get; }

    ref (long Lord, long Library) _combinedProjectionDependencies { get; }

    ref long _combinedProjectionStamp { get; }
}
