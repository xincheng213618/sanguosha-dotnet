#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess : IProgramRuntimeOperations
{
    ref IReadOnlyList<NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts { get; }

    ref Dictionary<(int Turn, int Recipient), NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices { get; }

    ref Dictionary<long, int> _zhaoZhongDrawInvocationCounts { get; }
}
