namespace CardGame.Core;

internal interface ICardSkillMarkSources : IProgramSelectionQuery
{
    void Project(int cardId, ref CardSkillMarkProjection projection);
}

// Only the current projection is retained here. Rule owners decide whether a
// label applies; this object owns the separator and null-versus-empty contract.
internal struct CardSkillMarkProjection
{
    private string? _label;
    private string? _last;
    private Dictionary<string, string>? _marks;

    internal void AppendMark(string mark, string label) => (_marks ??= new(StringComparer.Ordinal)).TryAdd(mark, label);
    internal void OverrideMarkLabel(string mark, string label) => (_marks ??= new(StringComparer.Ordinal))[mark] = label;

    internal void Append(string? label)
    {
        if (label is not null) _label = _label is null ? label : _label + " / " + label;
    }

    // Capture at the owner's original read point, then display after all
    // ordinary labels. Never retain a callback that would re-read live facts.
    internal void AppendLast(string? label)
    {
        if (label is not null) _last = _last is null ? label : _last + " / " + label;
    }

    internal string? Complete()
    {
        if (_marks is not null)
            foreach (var (_, label) in _marks.OrderBy(pair => pair.Key, StringComparer.Ordinal)) Append(label);
        return _last is null ? _label : _label is null ? _last : _label + " / " + _last;
    }
}

[ProgramRuntimeModule]
internal sealed class CardSkillMarkProjector(ICardSkillMarkSources sources, ICardEntityMarkQueries marks)
{
    internal string? Label(int cardId)
    {
        var projection = new CardSkillMarkProjection();
        foreach (var (mark, label) in marks.MarkedNames(cardId)) projection.AppendMark(mark, label);
        sources.Project(cardId, ref projection);
        return projection.Complete();
    }
}
