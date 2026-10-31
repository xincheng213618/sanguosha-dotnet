#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawInvoiceRuntimePortsRuntime
{
    bool global::CardGame.Core.INativeDrawInvoiceFacts.ModeTracksInvocations => _modeRulesOperations._modeRules.RuntimeRequirements.TracksNativeDrawInvocations;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    Winner global::CardGame.Core.INativeDrawInvoiceFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus global::CardGame.Core.INativeDrawInvoiceFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasPangJiCapability => _characterPangJiOlOperations.HasPangJiCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasLeMiHengJiguCapability => _characterMiHengLeOperations.HasLeMiHengJiguCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasLeZhouFeiLingkongCapability => _characterZhouFeiLeOperations.HasLeZhouFeiLingkongCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasTaoQianCapability => _characterTaoQianOlOperations.HasTaoQianCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasZhaoZhongCapability => _characterZhaoZhongOlOperations.HasZhaoZhongCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasHellZhiwangHolder => _coreHellKingProgramsOperations.HasHellZhiwangHolder;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasFengweiCapability => _characterLiuZhangOlOperations.HasFengweiCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasFeiYiHezhong => _characterFeiYiOlOperations.HasFeiYiHezhong;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasTangTangZhiyi => _characterTangTangOlOperations.HasTangTangZhiyi;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasYuanJiCapability => _characterYuanJiOlOperations.HasYuanJiCapability;

    bool global::CardGame.Core.INativeDrawInvoiceFacts.HasSanwenCapability => _characterWangCanOlOperations.HasSanwenCapability;

    void global::CardGame.Core.INativeDrawInvoiceCompletions.CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof) => _characterWangCanOlOperations.CompleteSanwenNativeDrawQualification(proof);
    void global::CardGame.Core.INativeDrawInvoiceCompletions.CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof) => _characterTangTangOlOperations.CompleteTangTangNativeDrawQualification(proof);
    void global::CardGame.Core.INativeDrawInvoiceCompletions.CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof) => _characterZhouFeiLeOperations.CompleteLingkongNativeDrawQualification(proof);
}
