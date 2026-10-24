namespace CardGame.Core;

/// <summary>Native prompt ownership and atomic reveal execution, without exposing a character or mutable resolution state.</summary>
public interface IModeNationalRevealHost
{
    DecisionKind? PendingKind { get; }
    CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode);
    IReadOnlyList<GeneralSelectionSlot> LegalRevealSlots(int actorSeat);
    CommandResult Accept(Action action);
    CommandResult Reject(CommandErrorCode code, string message);
    void RevealHumanGeneral(GeneralSelectionSlot slot, bool advanceToHumanBoundary);
}
