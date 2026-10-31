#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandSeizureEntries
{
    internal SkillProgramStepOutcome BeginPhaseHandDebtReturn(ProgramSkillFrame supplied) => _resolve().BeginPhaseHandDebtReturn(supplied);
    internal SkillProgramStepOutcome BeginPhaseHandSeizure(ProgramSkillFrame supplied, string continuation) => _resolve().BeginPhaseHandSeizure(supplied, continuation);
    internal bool CanActivatePhaseHandSeizure(CharacterState owner, SkillProgram program) => _resolve().CanActivatePhaseHandSeizure(owner, program);
    internal bool CanRunIssuedPhaseHandDebt(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunIssuedPhaseHandDebt(candidate, context);
    internal PhaseHandSeizureIssuedEvent? CurrentPhaseHandSeizureDebt(int owner, string skill, string instance, string hash, string binding) => _resolve().CurrentPhaseHandSeizureDebt(owner, skill, instance, hash, binding);
    internal bool? DecideIssuedTrigger690(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger690(candidate, context);
    internal void DrainPhaseHandSeizureMovement(long frameId) => _resolve().DrainPhaseHandSeizureMovement(frameId);
    internal bool HasCurrentIssuedPhaseHandDebt() => _resolve().HasCurrentIssuedPhaseHandDebt();
    internal bool IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate) => _resolve().IsPhaseHandDebtResolver(candidate);
    internal bool IsPhaseHandSeizureCost(int owner, string skill, Card card) => _resolve().IsPhaseHandSeizureCost(owner, skill, card);
    internal IEnumerable<ProgramTriggerCandidate> IssuedPhaseHandDebtCandidates(CharacterState owner, SkillProgramTriggerWindow window, int occurrence) => _resolve().IssuedPhaseHandDebtCandidates(owner, window, occurrence);
    internal void ObservePhaseHandSeizureDeath(IGameEvent payload) => _resolve().ObservePhaseHandSeizureDeath(payload);
    internal Card[] PhaseHandDebtCards(ProgramSkillFrame frame) => _resolve().PhaseHandDebtCards(frame);
    internal IReadOnlyList<PromptChoice> PhaseHandDebtChoices(ProgramSkillFrame frame) => _resolve().PhaseHandDebtChoices(frame);
    internal CardConversionSource PhaseHandSeizureSource(ProgramSkillFrame frame) => _resolve().PhaseHandSeizureSource(frame);
    internal void PublishPhaseHandDebtChoice(ProgramSkillFrame frame) => _resolve().PublishPhaseHandDebtChoice(frame);
    internal PhaseHandSeizureIssuedEvent RequirePhaseHandDebtParent(ProgramSkillFrame frame) => _resolve().RequirePhaseHandDebtParent(frame);
    internal void ResolvePhaseHandDebtChoice(PromptChoice choice) => _resolve().ResolvePhaseHandDebtChoice(choice);
    internal bool ResumePhaseHandSeizure(long frameId) => _resolve().ResumePhaseHandSeizure(frameId);
    internal bool ReturnPhaseHandSeizureMovement(ProgramSkillFrame frame) => _resolve().ReturnPhaseHandSeizureMovement(frame);
    internal PromptChoice SelectAiPhaseHandDebt(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiPhaseHandDebt(decision, frame);
    internal void SettlePhaseHandDebt(PhaseHandSeizureIssuedEvent due, long returnFrame, string reason, int required, int actual, long before, long after) => _resolve().SettlePhaseHandDebt(due, returnFrame, reason, required, actual, before, after);
    internal bool TryBeginForcedPhaseHandDebtReturn() => _resolve().TryBeginForcedPhaseHandDebtReturn();
    internal bool TryDrainPhaseHandSeizureMovement(ProgramSkillFrame frame) => _resolve().TryDrainPhaseHandSeizureMovement(frame);
    internal IEnumerable<PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures() => _resolve().UnsettledPhaseHandSeizures();
    internal long PhaseHandSeizureSequence { get => _resolve().PhaseHandSeizureSequence; }
    internal bool PhaseHandSeizureTerminalState { get => _resolve().PhaseHandSeizureTerminalState; }
}
