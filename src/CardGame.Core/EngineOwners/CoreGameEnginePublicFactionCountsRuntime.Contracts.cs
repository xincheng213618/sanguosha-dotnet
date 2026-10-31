#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePublicFactionCountsCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
