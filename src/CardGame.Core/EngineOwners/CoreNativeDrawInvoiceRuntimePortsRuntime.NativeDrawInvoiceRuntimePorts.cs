#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawInvoiceRuntimePortsRuntime
{
    bool INativeDrawInvoiceFacts.ModeTracksInvocations => _modeRulesOperations._modeRules.RuntimeRequirements.TracksNativeDrawInvocations;

    bool INativeDrawInvoiceFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    Winner INativeDrawInvoiceFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus INativeDrawInvoiceFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    bool INativeDrawInvoiceFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool INativeDrawInvoiceFacts.HasPangJiCapability => _characterPangJiOlOperations.HasPangJiCapability;

    bool INativeDrawInvoiceFacts.HasLeMiHengJiguCapability => _characterMiHengLeOperations.HasLeMiHengJiguCapability;

    bool INativeDrawInvoiceFacts.HasLeZhouFeiLingkongCapability => _characterZhouFeiLeOperations.HasLeZhouFeiLingkongCapability;

    bool INativeDrawInvoiceFacts.HasTaoQianCapability => _characterTaoQianOlOperations.HasTaoQianCapability;

    bool INativeDrawInvoiceFacts.HasZhaoZhongCapability => _characterZhaoZhongOlOperations.HasZhaoZhongCapability;

    bool INativeDrawInvoiceFacts.HasHellZhiwangHolder => _coreHellKingProgramsOperations.Value.HasHellZhiwangHolder;

    bool INativeDrawInvoiceFacts.HasFengweiCapability => _characterLiuZhangOlOperations.HasFengweiCapability;

    bool INativeDrawInvoiceFacts.HasFeiYiHezhong => _characterFeiYiOlOperations.HasFeiYiHezhong;

    bool INativeDrawInvoiceFacts.HasTangTangZhiyi => _characterTangTangOlOperations.Value.HasTangTangZhiyi;

    bool INativeDrawInvoiceFacts.HasYuanJiCapability => _characterYuanJiOlOperations.Value.HasYuanJiCapability;

    bool INativeDrawInvoiceFacts.HasSanwenCapability => _characterWangCanOlOperations.HasSanwenCapability;

    void INativeDrawInvoiceCompletions.CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof) => _characterWangCanOlOperations.CompleteSanwenNativeDrawQualification(proof);
    void INativeDrawInvoiceCompletions.CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof) => _characterTangTangOlOperations.Value.CompleteTangTangNativeDrawQualification(proof);
    void INativeDrawInvoiceCompletions.CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof) => _characterZhouFeiLeOperations.CompleteLingkongNativeDrawQualification(proof);
}
