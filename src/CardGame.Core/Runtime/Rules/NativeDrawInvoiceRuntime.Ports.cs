namespace CardGame.Core;

// Match state used by the shared native draw admission boundary.
internal interface INativeDrawInvoiceFacts : IProgramSelectionQuery
{
    bool SetupComplete { get; }
    Winner Winner { get; }
    EngineStatus Status { get; }
    bool IsValidSeat(int seat);
}
