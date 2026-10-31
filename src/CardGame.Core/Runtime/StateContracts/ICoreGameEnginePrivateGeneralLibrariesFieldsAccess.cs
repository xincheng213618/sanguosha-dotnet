#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePrivateGeneralLibrariesFieldsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.PrivateGeneralLibrarySource, global::CardGame.Core.PrivateGeneralLibrary> _privateGeneralLibraries { get; }

    ref bool _synchronizingPrivateGeneralLibraries { get; }

    ref long _privateGeneralLibraryRevision { get; }

    ref global::CardGame.Core.GameEngine.ProjectionDependency[] _privateLibraryOwnerDependencies { get; }

    ref (long Lord, long Library) _combinedProjectionDependencies { get; }

    ref long _combinedProjectionStamp { get; }
}
