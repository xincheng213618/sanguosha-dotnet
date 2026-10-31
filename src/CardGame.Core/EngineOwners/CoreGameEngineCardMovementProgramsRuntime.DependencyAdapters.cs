#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CardsMovedWindowRuntime))]
internal sealed class CoreCardMovementProgramsCardsMovedWindowAdapter(Func<CardsMovedWindowRuntime> targetFactory) : ICoreCardMovementProgramsCardsMovedWindowOperations
{
    private readonly Func<CardsMovedWindowRuntime> _targetFactory = targetFactory;
    public CardsMovedWindowRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CardMovementBatchCoordinator))]
internal sealed class CoreCardMovementProgramsCardMovementBatchCoordinatorAdapter(Func<CardMovementBatchCoordinator> targetFactory) : ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations
{
    private readonly Func<CardMovementBatchCoordinator> _targetFactory = targetFactory;
    public CardMovementBatchCoordinator Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ProgramWindowCursorRuntime))]
internal sealed class CoreCardMovementProgramsProgramWindowCursorAdapter(Func<ProgramWindowCursorRuntime> targetFactory) : ICoreCardMovementProgramsProgramWindowCursorOperations
{
    private readonly Func<ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public ProgramWindowCursorRuntime Get() => this._targetFactory();
}
