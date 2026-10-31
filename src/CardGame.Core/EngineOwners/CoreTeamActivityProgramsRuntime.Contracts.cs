#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreTeamActivityProgramsCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
