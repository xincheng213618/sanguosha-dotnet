#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations : IProgramRuntimeOperations
{
    ProgramPaidCardRuntime Get();
}

internal interface ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
