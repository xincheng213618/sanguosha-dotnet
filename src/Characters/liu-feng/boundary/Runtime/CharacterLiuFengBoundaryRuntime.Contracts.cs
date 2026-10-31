#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLiuFengBoundaryCoreProgramDistanceRulesOperations : IProgramRuntimeOperations
{
    ProgramDistanceRules DistanceRules { get; }
}

internal interface ICharacterLiuFengBoundaryCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterLiuFengBoundaryCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
