namespace CardGame.Core;

internal interface IRuleProgressionOperations
{
    void QueueFact(IGameEvent payload);
    bool BeginCharacterStateWindow();
    bool BeginHpChangedWindow();
    bool BeginCardsMovedWindow();
    void RequestStatePublication();
}

/// <summary>Settles synchronous rule observers before entering the output boundary.</summary>
internal sealed class RuleProgressionRuntime(ProgramRuleObservers observers, IRuleProgressionOperations operations)
{
    private readonly List<IGameEvent> _advancingFacts = [];
    internal IEnumerable<IGameEvent> AdvancingFacts
    {
        get
        {
            for (var index = _advancingFacts.Count - 1; index >= 0; index--)
                yield return _advancingFacts[index];
        }
    }

    internal void AdvanceFact(IGameEvent payload)
    {
        _advancingFacts.Add(payload);
        try
        {
            observers.BeforeEvent(payload);
            operations.QueueFact(payload);
            observers.AfterEvent(payload);
        }
        finally { _advancingFacts.RemoveAt(_advancingFacts.Count - 1); }
    }

    internal void AdvanceState()
    {
        observers.BeforeStatePublish();
        if (!operations.BeginCharacterStateWindow() && !operations.BeginHpChangedWindow())
            operations.BeginCardsMovedWindow();
        operations.RequestStatePublication();
    }
}
