#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeFactionChangesCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
