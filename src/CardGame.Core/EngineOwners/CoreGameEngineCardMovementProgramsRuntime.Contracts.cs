#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardMovementProgramsCardsMovedWindowOperations : IProgramRuntimeOperations
{
    CardsMovedWindowRuntime Get();
}

internal interface ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations : IProgramRuntimeOperations
{
    CardMovementBatchCoordinator Get();
}

internal interface ICoreCardMovementProgramsProgramWindowCursorOperations : IProgramRuntimeOperations
{
    ProgramWindowCursorRuntime Get();
}
