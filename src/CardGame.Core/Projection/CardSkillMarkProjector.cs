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

    internal string? Complete() => _last is null ? _label : _label is null ? _last : _label + " / " + _last;
}

[ProgramRuntimeModule]
internal sealed class CardSkillMarkProjector(ICardSkillMarkSources sources)
{
    internal string? Label(int cardId)
    {
        var projection = new CardSkillMarkProjection();
        sources.Project(cardId, ref projection);
        return projection.Complete();
    }
}
