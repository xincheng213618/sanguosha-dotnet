#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnHandExchangeEntries
{
    internal void AssertCurrentTurnHandExchange(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertCurrentTurnHandExchange(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCurrentTurnHandExchange(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginCurrentTurnHandExchange(supplied);
    internal void CompleteCurrentTurnHandExchange(global::CardGame.Core.ProgramSkillFrame frame, string outcome) => _resolve().CompleteCurrentTurnHandExchange(frame, outcome);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CurrentTurnHandExchangeChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CurrentTurnHandExchangeChoices(frame);
    internal bool CurrentTurnHandExchangeFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().CurrentTurnHandExchangeFirstChild(frame, child);
    internal global::CardGame.Core.Card[] CurrentTurnHandExchangeReturnCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CurrentTurnHandExchangeReturnCards(frame);
    internal void DrainCurrentTurnHandExchangeMovement(long frameId) => _resolve().DrainCurrentTurnHandExchangeMovement(frameId);
    internal bool IsCurrentTurnHandExchangeMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsCurrentTurnHandExchangeMovement(frame, effect, pending);
    internal void IssueCurrentTurnHandExchangeReturn(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IssueCurrentTurnHandExchangeReturn(frame);
    internal void PublishCurrentTurnHandExchangeChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishCurrentTurnHandExchangeChoice(frame);
    internal global::CardGame.Core.TurnEndingBoundaryFrame RequireCurrentTurnHandExchangeParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireCurrentTurnHandExchangeParent(frame);
    internal void ResolveCurrentTurnHandExchangeChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice supplied) => _resolve().ResolveCurrentTurnHandExchangeChoice(frame, supplied);
    internal bool ResumeCurrentTurnHandExchange(long frameId) => _resolve().ResumeCurrentTurnHandExchange(frameId);
    internal bool ReturnCurrentTurnHandExchangeMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnCurrentTurnHandExchangeMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiCurrentTurnHandExchange(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiCurrentTurnHandExchange(decision, frame);
    internal bool TryDrainCurrentTurnHandExchangeMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().TryDrainCurrentTurnHandExchangeMovement(frame);
    internal bool ValidCurrentTurnHandExchangeReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidCurrentTurnHandExchangeReceipt(frame);
    internal int CurrentTurnHandExchangeSequence { get => _resolve().CurrentTurnHandExchangeSequence; }
}
