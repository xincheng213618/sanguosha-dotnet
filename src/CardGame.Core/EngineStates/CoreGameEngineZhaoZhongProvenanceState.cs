#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineZhaoZhongProvenanceState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess
{
    private global::System.Collections.Generic.Dictionary<long, int> _zhaoZhongDrawInvocationCounts = [];
    internal ref global::System.Collections.Generic.Dictionary<long, int> ReferenceToZhaoZhongDrawInvocationCounts => ref _zhaoZhongDrawInvocationCounts;

    private global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts = default !;
    internal ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativeDrawInvocationRecordedEvent>? ReferenceToZhaoZhongDrawInvoiceFacts => ref _zhaoZhongDrawInvoiceFacts;

    private global::System.Collections.Generic.Dictionary<(int Turn, int Recipient), global::CardGame.Core.NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices = [];
    internal ref global::System.Collections.Generic.Dictionary<(int Turn, int Recipient), global::CardGame.Core.NativeDrawInvocationRecordedEvent[]> ReferenceToZhaoZhongDrawInvoices => ref _zhaoZhongDrawInvoices;

    ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativeDrawInvocationRecordedEvent>? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess._zhaoZhongDrawInvoiceFacts => ref _zhaoZhongDrawInvoiceFacts;

    ref global::System.Collections.Generic.Dictionary<(int Turn, int Recipient), global::CardGame.Core.NativeDrawInvocationRecordedEvent[]> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess._zhaoZhongDrawInvoices => ref _zhaoZhongDrawInvoices;

    ref global::System.Collections.Generic.Dictionary<long, int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess._zhaoZhongDrawInvocationCounts => ref _zhaoZhongDrawInvocationCounts;
}
