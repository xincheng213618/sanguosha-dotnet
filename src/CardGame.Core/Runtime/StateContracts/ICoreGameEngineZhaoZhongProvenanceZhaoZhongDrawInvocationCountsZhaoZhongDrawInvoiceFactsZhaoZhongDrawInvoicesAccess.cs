#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts { get; }

    ref global::System.Collections.Generic.Dictionary<(int Turn, int Recipient), global::CardGame.Core.NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices { get; }

    ref global::System.Collections.Generic.Dictionary<long, int> _zhaoZhongDrawInvocationCounts { get; }
}
