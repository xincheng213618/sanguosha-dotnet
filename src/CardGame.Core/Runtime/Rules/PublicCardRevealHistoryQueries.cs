namespace CardGame.Core;

// This immutable view is derived only from the existing typed journal index.
// Its snapshot identity also invalidates the view after pending-fact recovery.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class PublicCardRevealHistoryQueries(IProgramEventHistoryQueries history)
    : IPublicCardRevealHistoryQueries
{
    private IReadOnlyList<PublicCardRevealHistoryRecordedEvent>? _source;
    private IReadOnlyList<PublicCardRevealHistoryFact> _facts = Array.AsReadOnly(Array.Empty<PublicCardRevealHistoryFact>());

    public IReadOnlyList<PublicCardRevealHistoryFact> Read()
    {
        var source = history.Read<PublicCardRevealHistoryRecordedEvent>();
        if (ReferenceEquals(source, _source)) return _facts;
        _facts = Array.AsReadOnly(source.SelectMany(item => item.Facts).ToArray());
        _source = source;
        return _facts;
    }
}
