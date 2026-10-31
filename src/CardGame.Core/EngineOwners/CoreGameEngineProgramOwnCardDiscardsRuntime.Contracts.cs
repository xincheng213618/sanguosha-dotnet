#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramOwnCardDiscardsProgramPaidCardOperations : IProgramRuntimeOperations
{
    ProgramPaidCardRuntime Get();
}

internal interface ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
