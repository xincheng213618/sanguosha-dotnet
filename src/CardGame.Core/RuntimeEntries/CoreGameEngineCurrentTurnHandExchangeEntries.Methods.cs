#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnHandExchangeEntries
{
    internal SkillProgramStepOutcome BeginCurrentTurnHandExchange(ProgramSkillFrame supplied) => _resolve().BeginCurrentTurnHandExchange(supplied);
    internal void CompleteCurrentTurnHandExchange(ProgramSkillFrame frame, string outcome) => _resolve().CompleteCurrentTurnHandExchange(frame, outcome);
    internal IReadOnlyList<PromptChoice> CurrentTurnHandExchangeChoices(ProgramSkillFrame frame) => _resolve().CurrentTurnHandExchangeChoices(frame);
    internal Card[] CurrentTurnHandExchangeReturnCards(ProgramSkillFrame frame) => _resolve().CurrentTurnHandExchangeReturnCards(frame);
    internal void DrainCurrentTurnHandExchangeMovement(long frameId) => _resolve().DrainCurrentTurnHandExchangeMovement(frameId);
    internal bool IsCurrentTurnHandExchangeMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsCurrentTurnHandExchangeMovement(frame, effect, pending);
    internal void IssueCurrentTurnHandExchangeReturn(ProgramSkillFrame frame) => _resolve().IssueCurrentTurnHandExchangeReturn(frame);
    internal void PublishCurrentTurnHandExchangeChoice(ProgramSkillFrame frame) => _resolve().PublishCurrentTurnHandExchangeChoice(frame);
    internal TurnEndingBoundaryFrame RequireCurrentTurnHandExchangeParent(ProgramSkillFrame frame) => _resolve().RequireCurrentTurnHandExchangeParent(frame);
    internal void ResolveCurrentTurnHandExchangeChoice(ProgramSkillFrame frame, PromptChoice supplied) => _resolve().ResolveCurrentTurnHandExchangeChoice(frame, supplied);
    internal bool ResumeCurrentTurnHandExchange(long frameId) => _resolve().ResumeCurrentTurnHandExchange(frameId);
    internal bool ReturnCurrentTurnHandExchangeMovement(ProgramSkillFrame frame) => _resolve().ReturnCurrentTurnHandExchangeMovement(frame);
    internal PromptChoice SelectAiCurrentTurnHandExchange(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiCurrentTurnHandExchange(decision, frame);
    internal bool TryDrainCurrentTurnHandExchangeMovement(ProgramSkillFrame frame) => _resolve().TryDrainCurrentTurnHandExchangeMovement(frame);
    internal bool ValidCurrentTurnHandExchangeReceipt(ProgramSkillFrame frame) => _resolve().ValidCurrentTurnHandExchangeReceipt(frame);
    internal int CurrentTurnHandExchangeSequence { get => _resolve().CurrentTurnHandExchangeSequence; }
}
