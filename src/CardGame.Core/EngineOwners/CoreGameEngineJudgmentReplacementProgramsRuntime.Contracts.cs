#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreJudgmentReplacementProgramsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
