#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeMiHengJiguCapability { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhaoZhongCapability { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPangJiCapability { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTaoQianCapability { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFengweiCapability { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCoreHellKingProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHellZhiwangHolder { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFeiYiHezhong { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterTangTangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTangTangZhiyi { get; }

    void CompleteTangTangNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof);
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterYuanJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasYuanJiCapability { get; }
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeZhouFeiLingkongCapability { get; }

    void CompleteLingkongNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof);
}

internal interface ICoreNativeDrawInvoiceRuntimePortsCharacterWangCanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSanwenCapability { get; }

    void CompleteSanwenNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof);
}

internal interface ICoreNativeDrawInvoiceRuntimePortsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}
