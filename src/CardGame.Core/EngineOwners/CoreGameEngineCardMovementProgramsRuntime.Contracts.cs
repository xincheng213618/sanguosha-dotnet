#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardMovementProgramsCardsMovedWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardsMovedWindowRuntime Get();
}

internal interface ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementBatchCoordinator Get();
}

internal interface ICoreCardMovementProgramsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}
