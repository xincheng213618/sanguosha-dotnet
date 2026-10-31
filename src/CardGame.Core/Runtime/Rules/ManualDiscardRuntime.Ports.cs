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

// The module owns the lifecycle. These primitive transitions retain the current
// prompt setter and its distinct discard status, native ended window and logs.
internal interface IManualDiscardTransitions : IProgramRuntimeOperations
{
    void SetPrompt(PendingDecision prompt);
    void PublishState();
    bool BeginDiscardPhaseEnded(CharacterState owner);
    void EndTurn();
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

public sealed partial class GameEngine
{
}
