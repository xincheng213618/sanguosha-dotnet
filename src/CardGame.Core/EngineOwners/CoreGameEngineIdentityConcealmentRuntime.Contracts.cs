#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreIdentityConcealmentCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
