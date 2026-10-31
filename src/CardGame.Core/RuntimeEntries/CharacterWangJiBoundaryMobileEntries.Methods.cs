#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangJiBoundaryMobileEntries
{
    internal bool AdvanceBoundaryWjChild() => _resolve().AdvanceBoundaryWjChild();
    internal global::CardGame.Core.PromptChoice AiBoundaryWj(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiBoundaryWj(decision, frame);
    internal void AnswerBoundaryWj(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryWj(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryWj(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginBoundaryWj(supplied, operation);
    internal bool BoundaryWjBeforeDamage(long id) => _resolve().BoundaryWjBeforeDamage(id);
    internal bool BoundaryWjCard(global::CardGame.Core.CardActionContext action) => _resolve().BoundaryWjCard(action);
    internal void BoundaryWjChangeMarkerCharge(long frameId, int owner, global::CardGame.Core.PlayerMarkerKind marker, int delta, int capacity, string reason) => _resolve().BoundaryWjChangeMarkerCharge(frameId, owner, marker, delta, capacity, reason);
    internal int BoundaryWjCharge(int owner) => _resolve().BoundaryWjCharge(owner);
    internal global::System.Collections.Generic.IEnumerable<string> BoundaryWjChargeSources(int seat) => _resolve().BoundaryWjChargeSources(seat);
    internal bool BoundaryWjChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryWjChild(frame, child);
    internal global::CardGame.Core.PromptChoice BoundaryWjChoice(global::CardGame.Core.ProgramSkillFrame frame, string branch, int value, string text, bool card = false) => _resolve().BoundaryWjChoice(frame, branch, value, text, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryWjChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryWjChoices(frame);
    internal bool BoundaryWjCompletedWindow(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().BoundaryWjCompletedWindow(context, out window);
    internal bool BoundaryWjDamage(long id) => _resolve().BoundaryWjDamage(id);
    internal bool BoundaryWjDying() => _resolve().BoundaryWjDying();
    internal bool BoundaryWjMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation continuation) => _resolve().BoundaryWjMovement(frame, effect, continuation);
    internal bool BoundaryWjObservesResponse(global::CardGame.Core.CardActionContext action) => _resolve().BoundaryWjObservesResponse(action);
    internal int BoundaryWjPeak(int owner) => _resolve().BoundaryWjPeak(owner);
    internal global::CardGame.Core.CardSnapshot[] BoundaryWjPrivateHand(int viewer) => _resolve().BoundaryWjPrivateHand(viewer);
    internal global::System.Collections.Generic.IReadOnlyList<int> BoundaryWjRecipients(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().BoundaryWjRecipients(owner, phase);
    internal bool BoundaryWjRecovery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().BoundaryWjRecovery(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryWjRoot() => _resolve().BoundaryWjRoot();
    internal bool BoundaryWjSuffix(int index) => _resolve().BoundaryWjSuffix(index);
    internal bool BoundaryWjTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().BoundaryWjTargetWindow(query);
    internal global::CardGame.Core.BoundaryWangJiTurnEvent? BoundaryWjTurn() => _resolve().BoundaryWjTurn();
    internal bool CanSpendBoundaryWj(int owner, int target) => _resolve().CanSpendBoundaryWj(owner, target);
    internal void ContinueBoundaryWj(long id) => _resolve().ContinueBoundaryWj(id);
    internal bool DrainBoundaryWj(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainBoundaryWj(frame);
    internal void FinishBoundaryWj(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishBoundaryWj(frame);
    internal void GrantBoundaryWjCharge(global::CardGame.Core.ProgramSkillFrame frame, int target, string skill) => _resolve().GrantBoundaryWjCharge(frame, target, skill);
    internal bool KeepBoundaryWj(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepBoundaryWj(frame);
    internal void ObserveBoundaryWjTurn(global::CardGame.Core.TurnStartedEvent entry) => _resolve().ObserveBoundaryWjTurn(entry);
    internal bool OfferBoundaryWj(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().OfferBoundaryWj(gate);
    internal void PublishBoundaryWj(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryWj(frame);
    internal bool ResumeBoundaryWj(long id) => _resolve().ResumeBoundaryWj(id);
    internal void ReturnBoundaryWjDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundaryWjDraw(frame, outcome);
    internal bool ReturnBoundaryWjMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryWjMovement(frame);
    internal bool ValidBoundaryWj(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryWj(frame);
}
