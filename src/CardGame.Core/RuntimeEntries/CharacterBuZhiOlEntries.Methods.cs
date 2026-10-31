#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuZhiOlEntries
{
    internal void AssertBuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBuZhi(f);
    internal bool BuZhiActivationTargetAllowed(global::CardGame.Core.CharacterState target, global::CardGame.Core.SkillProgramActivation a) => _resolve().BuZhiActivationTargetAllowed(target, a);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuZhiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BuZhiChoices(f);
    internal int BuZhiChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BuZhiChooser(f);
    internal bool BuZhiCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BuZhiCurrent(f);
    internal bool BuZhiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BuZhiFirstChild(f, child);
    internal bool BuZhiInitialParentMatches(global::CardGame.Core.ProgramSkillFrame f, bool gainLoss) => _resolve().BuZhiInitialParentMatches(f, gainLoss);
    internal bool BuZhiMovementQualifies(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().BuZhiMovementQualifies(batch, owner);
    internal global::CardGame.Core.ProgramSkillFrame? BuZhiObserverRoot() => _resolve().BuZhiObserverRoot();
    internal bool BuZhiPaidEquipmentEntitlement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BuZhiPaidEquipmentEntitlement(f);
    internal global::CardGame.Core.SkillProgramEffect? BuZhiPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BuZhiPausedEffect(f);
    internal int? BuZhiPhaseLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation a) => _resolve().BuZhiPhaseLimit(owner, a);
    internal bool CanContinueBuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueBuZhi(f);
    internal global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? CaptureNativeBuZhiDiscardInvocation(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => _resolve().CaptureNativeBuZhiDiscardInvocation(frame, zones, reason);
    internal void CompleteNativeBuZhiDiscardInvocation(global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<int> actualIds) => _resolve().CompleteNativeBuZhiDiscardInvocation(draft, actualIds);
    internal void FinishBuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishBuZhi(f);
    internal bool HasBuZhiBeforeDamageObserver(long id) => _resolve().HasBuZhiBeforeDamageObserver(id);
    internal bool HasBuZhiDamageObserver(long id) => _resolve().HasBuZhiDamageObserver(id);
    internal bool IsBuZhiActivation(global::CardGame.Core.SkillProgramActivation a) => _resolve().IsBuZhiActivation(a);
    internal bool IsBuZhiDying() => _resolve().IsBuZhiDying();
    internal bool IsBuZhiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsBuZhiMovement(f, e, p);
    internal void IssueBuZhiDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueBuZhiDamage(f);
    internal void PayBuZhiDraw(global::CardGame.Core.ProgramSkillFrame f, int target, string next) => _resolve().PayBuZhiDraw(f, target, next);
    internal void PayBuZhiEquipment(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, string next) => _resolve().PayBuZhiEquipment(f, ids, destination, next);
    internal bool? ProveBuZhiMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CharacterState owner) => _resolve().ProveBuZhiMovement(trigger, batch, owner);
    internal void PublishBuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBuZhi(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveBuZhi(global::CardGame.Core.ProgramSkillFrame supplied, bool gainLoss) => _resolve().ResolveBuZhi(supplied, gainLoss);
    internal void ResolveBuZhiChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveBuZhiChoice(selected);
    internal bool ResumeBuZhi(long id) => _resolve().ResumeBuZhi(id);
    internal bool ReturnBuZhiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBuZhiMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiBuZhi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiBuZhi(p, f);
    internal bool TryAdvanceBuZhiSubtree() => _resolve().TryAdvanceBuZhiSubtree();
    internal bool ValidBuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBuZhi(f);
    internal bool ValidNativeBuZhiDiscardInvocation(global::CardGame.Core.NativeBuZhiDiscardInvocationProof p) => _resolve().ValidNativeBuZhiDiscardInvocation(p);
}
