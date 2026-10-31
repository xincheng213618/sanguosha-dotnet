#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreLordSkillProjectionsBoundaryYuanShuOperations : IProgramRuntimeOperations
{
    BoundaryYuanShuRuntime Get();
}

internal interface ICoreLordSkillProjectionsCoreModeAllegianceOperations : IProgramRuntimeOperations
{
    IMatchModeAllegiancePolicy ModeAllegiancePolicy { get; }
}
