#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeAiDecisionsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreNativeAiDecisionsCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    ProgramCardLegality CardLegality { get; }
}
