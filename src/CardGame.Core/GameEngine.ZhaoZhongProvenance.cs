namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts => ref _zhaoZhongProvenance.ReferenceToZhaoZhongDrawInvoiceFacts;
    private ref global::System.Collections.Generic.Dictionary<(int Turn, int Recipient), global::CardGame.Core.NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices => ref _zhaoZhongProvenance.ReferenceToZhaoZhongDrawInvoices;
    private ref global::System.Collections.Generic.Dictionary<long, int> _zhaoZhongDrawInvocationCounts => ref _zhaoZhongProvenance.ReferenceToZhaoZhongDrawInvocationCounts;
}
