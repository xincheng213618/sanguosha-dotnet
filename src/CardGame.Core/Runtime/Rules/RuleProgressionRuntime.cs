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
    internal void AdvanceFact(IGameEvent payload)
    {
        observers.BeforeEvent(payload);
        operations.QueueFact(payload);
        observers.AfterEvent(payload);
    }

    internal void AdvanceState()
    {
        observers.BeforeStatePublish();
        if (!operations.BeginCharacterStateWindow() && !operations.BeginHpChangedWindow())
            operations.BeginCardsMovedWindow();
        operations.RequestStatePublication();
    }
}
