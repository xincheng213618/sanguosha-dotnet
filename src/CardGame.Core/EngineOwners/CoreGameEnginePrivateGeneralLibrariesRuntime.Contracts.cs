#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePrivateGeneralLibrariesCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
