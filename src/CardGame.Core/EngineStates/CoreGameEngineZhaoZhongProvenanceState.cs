#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineZhaoZhongProvenanceState : Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess
{
    private Dictionary<long, int> _zhaoZhongDrawInvocationCounts = [];
    internal ref Dictionary<long, int> ReferenceToZhaoZhongDrawInvocationCounts => ref _zhaoZhongDrawInvocationCounts;

    private IReadOnlyList<NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts = default !;
    internal ref IReadOnlyList<NativeDrawInvocationRecordedEvent>? ReferenceToZhaoZhongDrawInvoiceFacts => ref _zhaoZhongDrawInvoiceFacts;

    private Dictionary<(int Turn, int Recipient), NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices = [];
    internal ref Dictionary<(int Turn, int Recipient), NativeDrawInvocationRecordedEvent[]> ReferenceToZhaoZhongDrawInvoices => ref _zhaoZhongDrawInvoices;

    ref IReadOnlyList<NativeDrawInvocationRecordedEvent>? Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess._zhaoZhongDrawInvoiceFacts => ref _zhaoZhongDrawInvoiceFacts;

    ref Dictionary<(int Turn, int Recipient), NativeDrawInvocationRecordedEvent[]> Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess._zhaoZhongDrawInvoices => ref _zhaoZhongDrawInvoices;

    ref Dictionary<long, int> Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess._zhaoZhongDrawInvocationCounts => ref _zhaoZhongDrawInvocationCounts;
}
