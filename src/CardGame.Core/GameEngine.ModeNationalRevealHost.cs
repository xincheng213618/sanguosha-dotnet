namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeNationalRevealHost CreateModeNationalRevealHost() => new ModeNationalRevealHost(this);

    private sealed class ModeNationalRevealHost(GameEngine engine) : IModeNationalRevealHost
    {
        public DecisionKind? PendingKind => engine._pendingDecision?.Kind;
        public CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId,
            CommandErrorCode inactiveCode) => engine.ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
        public IReadOnlyList<GeneralSelectionSlot> LegalRevealSlots(int actorSeat) => Array.AsReadOnly(
            engine.BuildNationalRevealActions(engine._players[actorSeat]).Where(action => action.Kind == LegalActionKind.RevealGeneral &&
                action.GeneralSlot is not null).Select(action => action.GeneralSlot!.Value).ToArray());
        public CommandResult Accept(Action action) => engine.Accept(action);
        public CommandResult Reject(CommandErrorCode code, string message) => engine.Reject(code, message);
        public void RevealHumanGeneral(GeneralSelectionSlot slot, bool advanceToHumanBoundary) =>
            engine.HumanRevealGeneralCore(slot, advanceToHumanBoundary);
    }
}
