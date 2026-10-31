#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandSeizureEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPhaseHandDebtReturn(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginPhaseHandDebtReturn(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPhaseHandSeizure(global::CardGame.Core.ProgramSkillFrame supplied, string continuation) => _resolve().BeginPhaseHandSeizure(supplied, continuation);
    internal bool CanActivatePhaseHandSeizure(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgram program) => _resolve().CanActivatePhaseHandSeizure(owner, program);
    internal bool CanRunIssuedPhaseHandDebt(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunIssuedPhaseHandDebt(candidate, context);
    internal global::CardGame.Core.PhaseHandSeizureIssuedEvent? CurrentPhaseHandSeizureDebt(int owner, string skill, string instance, string hash, string binding) => _resolve().CurrentPhaseHandSeizureDebt(owner, skill, instance, hash, binding);
    internal bool? DecideIssuedTrigger690(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger690(candidate, context);
    internal void DrainPhaseHandSeizureMovement(long frameId) => _resolve().DrainPhaseHandSeizureMovement(frameId);
    internal bool HasCurrentIssuedPhaseHandDebt() => _resolve().HasCurrentIssuedPhaseHandDebt();
    internal bool IsPhaseHandDebtResolver(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().IsPhaseHandDebtResolver(candidate);
    internal bool IsPhaseHandSeizureCost(int owner, string skill, global::CardGame.Core.Card card) => _resolve().IsPhaseHandSeizureCost(owner, skill, card);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedPhaseHandDebtCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrence) => _resolve().IssuedPhaseHandDebtCandidates(owner, window, occurrence);
    internal void ObservePhaseHandSeizureDeath(global::CardGame.Core.IGameEvent payload) => _resolve().ObservePhaseHandSeizureDeath(payload);
    internal global::CardGame.Core.Card[] PhaseHandDebtCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PhaseHandDebtCards(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PhaseHandDebtChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PhaseHandDebtChoices(frame);
    internal global::CardGame.Core.CardConversionSource PhaseHandSeizureSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PhaseHandSeizureSource(frame);
    internal void PublishPhaseHandDebtChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishPhaseHandDebtChoice(frame);
    internal global::CardGame.Core.PhaseHandSeizureIssuedEvent RequirePhaseHandDebtParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequirePhaseHandDebtParent(frame);
    internal void ResolvePhaseHandDebtChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePhaseHandDebtChoice(choice);
    internal bool ResumePhaseHandSeizure(long frameId) => _resolve().ResumePhaseHandSeizure(frameId);
    internal bool ReturnPhaseHandSeizureMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPhaseHandSeizureMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiPhaseHandDebt(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiPhaseHandDebt(decision, frame);
    internal void SettlePhaseHandDebt(global::CardGame.Core.PhaseHandSeizureIssuedEvent due, long returnFrame, string reason, int required, int actual, long before, long after) => _resolve().SettlePhaseHandDebt(due, returnFrame, reason, required, actual, before, after);
    internal bool TryBeginForcedPhaseHandDebtReturn() => _resolve().TryBeginForcedPhaseHandDebtReturn();
    internal bool TryDrainPhaseHandSeizureMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().TryDrainPhaseHandSeizureMovement(frame);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures() => _resolve().UnsettledPhaseHandSeizures();
    internal long PhaseHandSeizureSequence { get => _resolve().PhaseHandSeizureSequence; }
    internal bool PhaseHandSeizureTerminalState { get => _resolve().PhaseHandSeizureTerminalState; }
}
