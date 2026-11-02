#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawInvoiceRuntimePortsRuntime
{
    bool INativeDrawInvoiceFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    Winner INativeDrawInvoiceFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus INativeDrawInvoiceFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    bool INativeDrawInvoiceFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    void INativeDrawInvoiceCompletions.CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof) => _characterWangCanOlOperations.CompleteSanwenNativeDrawQualification(proof);
    void INativeDrawInvoiceCompletions.CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof) => _characterTangTangOlOperations.Value.CompleteTangTangNativeDrawQualification(proof);
    void INativeDrawInvoiceCompletions.CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof) => _characterZhouFeiLeOperations.CompleteLingkongNativeDrawQualification(proof);
}
