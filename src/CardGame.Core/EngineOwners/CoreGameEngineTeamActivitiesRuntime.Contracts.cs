#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreTeamActivitiesCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
