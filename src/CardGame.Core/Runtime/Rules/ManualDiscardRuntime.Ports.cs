namespace CardGame.Core;

// Read the existing rule owners at their original stages. Character references
// cross this boundary unchanged, including after a native seat replacement.
internal interface IManualDiscardFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    EngineStatus Status { get; }
    int RequiredDiscard(CharacterState owner);
    int QuotaMultiplier(CharacterState owner);
    IReadOnlyList<Card> DiscardableHand(CharacterState owner);
    int HandLimit(CharacterState owner);
}

// The manual discard owns its prompts and payment until the native phase finishes.
internal interface IManualDiscardTransitions : IProgramRuntimeOperations
{
    void SetPrompt(PendingDecision prompt);
    void PublishState();
    void FinishDiscardPhase(CharacterState owner);
    void Log(string type, string message, int actorSeat);
}

// Acceptance still enters the original command session. The supplied action is
// the module's mutation sequence; this adapter contains no discard algorithm.
internal interface IManualDiscardCommands : IProgramRuntimeOperations
{
    CommandError? ValidatePrompt(int actorSeat, PromptId? promptId);
    CommandResult Accept(Action operation);
    CommandResult Reject(CommandErrorCode code, string message);
}
