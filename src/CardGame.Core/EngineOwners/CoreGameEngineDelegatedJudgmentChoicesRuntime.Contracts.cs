#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDelegatedJudgmentChoicesCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
