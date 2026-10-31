namespace CardGame.Core;

// Transitional reads retain each existing domain's capability and current phase
// authority. Admission and the short-circuit order live in the invoice runtime.
internal interface INativeDrawInvoiceFacts : IProgramSelectionQuery
{
    bool ModeTracksInvocations { get; }
    bool SetupComplete { get; }
    Winner Winner { get; }
    EngineStatus Status { get; }
    bool IsValidSeat(int seat);
    bool HasPangJiCapability { get; }
    bool HasLeMiHengJiguCapability { get; }
    bool HasLeZhouFeiLingkongCapability { get; }
    bool HasTaoQianCapability { get; }
    bool HasZhaoZhongCapability { get; }
    bool HasHellZhiwangHolder { get; }
    bool HasFengweiCapability { get; }
    bool HasFeiYiHezhong { get; }
    bool HasTangTangZhiyi { get; }
    bool HasYuanJiCapability { get; }
    bool HasSanwenCapability { get; }
}

internal interface INativeDrawInvoiceCompletions : IProgramRuntimeOperations
{
    void CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof);
    void CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof);
    void CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof);
}

public sealed partial class GameEngine
{
}
