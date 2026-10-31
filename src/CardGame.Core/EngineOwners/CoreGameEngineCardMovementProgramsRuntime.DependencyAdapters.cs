#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardsMovedWindowRuntime))]
internal sealed class CoreCardMovementProgramsCardsMovedWindowAdapter(global::System.Func<global::CardGame.Core.CardsMovedWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCardsMovedWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardsMovedWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardsMovedWindowRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardMovementBatchCoordinator))]
internal sealed class CoreCardMovementProgramsCardMovementBatchCoordinatorAdapter(global::System.Func<global::CardGame.Core.CardMovementBatchCoordinator> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardMovementBatchCoordinator> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementBatchCoordinator Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreCardMovementProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}
