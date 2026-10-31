#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> _privateGeneralLibraries { get; }

    ref long _privateGeneralLibraryRevision { get; }
}
