#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangJiBoundaryMobileEntries
{
    internal bool AdvanceBoundaryWjChild() => _resolve().AdvanceBoundaryWjChild();
    internal PromptChoice AiBoundaryWj(PendingDecision decision, ProgramSkillFrame frame) => _resolve().AiBoundaryWj(decision, frame);
    internal void AnswerBoundaryWj(PromptChoice choice) => _resolve().AnswerBoundaryWj(choice);
    internal SkillProgramStepOutcome BeginBoundaryWj(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginBoundaryWj(supplied, operation);
    internal bool BoundaryWjCard(CardActionContext action) => _resolve().BoundaryWjCard(action);
    internal void BoundaryWjChangeMarkerCharge(long frameId, int owner, PlayerMarkerKind marker, int delta, int capacity, string reason) => _resolve().BoundaryWjChangeMarkerCharge(frameId, owner, marker, delta, capacity, reason);
    internal int BoundaryWjCharge(int owner) => _resolve().BoundaryWjCharge(owner);
    internal IEnumerable<string> BoundaryWjChargeSources(int seat) => _resolve().BoundaryWjChargeSources(seat);
    internal bool BoundaryWjChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().BoundaryWjChild(frame, child);
    internal PromptChoice BoundaryWjChoice(ProgramSkillFrame frame, string branch, int value, string text, bool card = false) => _resolve().BoundaryWjChoice(frame, branch, value, text, card);
    internal IReadOnlyList<PromptChoice> BoundaryWjChoices(ProgramSkillFrame frame) => _resolve().BoundaryWjChoices(frame);
    internal bool BoundaryWjCompletedWindow(ProgramSkillWindowContext context, scoped out ProgramCardTriggerWindowFrame window) => _resolve().BoundaryWjCompletedWindow(context, out window);
    internal bool BoundaryWjDying() => _resolve().BoundaryWjDying();
    internal bool BoundaryWjObservesResponse(CardActionContext action) => _resolve().BoundaryWjObservesResponse(action);
    internal int BoundaryWjPeak(int owner) => _resolve().BoundaryWjPeak(owner);
    internal CardSnapshot[] BoundaryWjPrivateHand(int viewer) => _resolve().BoundaryWjPrivateHand(viewer);
    internal IReadOnlyList<int> BoundaryWjRecipients(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().BoundaryWjRecipients(owner, phase);
    internal bool BoundaryWjRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child) => _resolve().BoundaryWjRecovery(frame, child);
    internal ProgramSkillFrame? BoundaryWjRoot() => _resolve().BoundaryWjRoot();
    internal bool BoundaryWjSuffix(int index) => _resolve().BoundaryWjSuffix(index);
    internal bool BoundaryWjTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().BoundaryWjTargetWindow(query);
    internal BoundaryWangJiTurnEvent? BoundaryWjTurn() => _resolve().BoundaryWjTurn();
    internal bool CanSpendBoundaryWj(int owner, int target) => _resolve().CanSpendBoundaryWj(owner, target);
    internal void ContinueBoundaryWj(long id) => _resolve().ContinueBoundaryWj(id);
    internal bool DrainBoundaryWj(ProgramSkillFrame frame) => _resolve().DrainBoundaryWj(frame);
    internal void FinishBoundaryWj(ProgramSkillFrame frame) => _resolve().FinishBoundaryWj(frame);
    internal void GrantBoundaryWjCharge(ProgramSkillFrame frame, int target, string skill) => _resolve().GrantBoundaryWjCharge(frame, target, skill);
    internal bool KeepBoundaryWj(ProgramSkillFrame frame) => _resolve().KeepBoundaryWj(frame);
    internal void ObserveBoundaryWjTurn(TurnStartedEvent entry) => _resolve().ObserveBoundaryWjTurn(entry);
    internal bool OfferBoundaryWj(ProgramTriggerLegalityContext gate) => _resolve().OfferBoundaryWj(gate);
    internal void PublishBoundaryWj(ProgramSkillFrame frame) => _resolve().PublishBoundaryWj(frame);
    internal bool ResumeBoundaryWj(long id) => _resolve().ResumeBoundaryWj(id);
    internal void ReturnBoundaryWjDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnBoundaryWjDraw(frame, outcome);
    internal bool ReturnBoundaryWjMovement(ProgramSkillFrame frame) => _resolve().ReturnBoundaryWjMovement(frame);
    internal bool ValidBoundaryWj(ProgramSkillFrame frame) => _resolve().ValidBoundaryWj(frame);
}
