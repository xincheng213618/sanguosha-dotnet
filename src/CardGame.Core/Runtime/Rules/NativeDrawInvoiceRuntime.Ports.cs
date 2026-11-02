namespace CardGame.Core;

// Match state used by the shared native draw admission boundary.
internal interface INativeDrawInvoiceFacts : IProgramSelectionQuery
{
    bool SetupComplete { get; }
    Winner Winner { get; }
    EngineStatus Status { get; }
    bool IsValidSeat(int seat);
}

internal interface INativeDrawInvoiceCompletions : IProgramRuntimeOperations
{
    void CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof);
    void CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof);
    void CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof);
}
