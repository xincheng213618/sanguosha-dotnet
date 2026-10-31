namespace CardGame.Core;

// Native adjacency queries shared by custom operation runtimes. The operation
// proves its first child; this port reuses existing native observer/rescue
// proofs for the suffix and the common frame-owned recovery invoices.
internal interface IProgramHealthChildQueries : IProgramSelectionQuery
{
    DyingFrame? ActiveDying { get; }
    HpChangeContext? LastPendingLoss(long parentFrameId, int targetSeat);
    bool ProgramRecoveryChild(ProgramSkillFrame parent, ResolutionFrame child);
    bool PaidObserverSuffix(long programFrameId);
}
internal interface IProgramHealthChildOperations : IProgramRuntimeOperations
{
    bool AdvanceObserverSubtree();
}

public sealed partial class GameEngine
{
}
