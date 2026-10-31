#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuZhiOlEntries
{
    internal bool BuZhiActivationTargetAllowed(CharacterState target, SkillProgramActivation a) => _resolve().BuZhiActivationTargetAllowed(target, a);
    internal IReadOnlyList<PromptChoice> BuZhiChoices(ProgramSkillFrame f) => _resolve().BuZhiChoices(f);
    internal int BuZhiChooser(ProgramSkillFrame f) => _resolve().BuZhiChooser(f);
    internal bool BuZhiCurrent(ProgramSkillFrame f) => _resolve().BuZhiCurrent(f);
    internal bool BuZhiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BuZhiFirstChild(f, child);
    internal bool BuZhiInitialParentMatches(ProgramSkillFrame f, bool gainLoss) => _resolve().BuZhiInitialParentMatches(f, gainLoss);
    internal bool BuZhiMovementQualifies(CardMovementBatchContext batch, int owner) => _resolve().BuZhiMovementQualifies(batch, owner);
    internal ProgramSkillFrame? BuZhiObserverRoot() => _resolve().BuZhiObserverRoot();
    internal bool BuZhiPaidEquipmentEntitlement(ProgramSkillFrame f) => _resolve().BuZhiPaidEquipmentEntitlement(f);
    internal SkillProgramEffect? BuZhiPausedEffect(ProgramSkillFrame f) => _resolve().BuZhiPausedEffect(f);
    internal int? BuZhiPhaseLimit(CharacterState owner, SkillProgramActivation a) => _resolve().BuZhiPhaseLimit(owner, a);
    internal bool CanContinueBuZhi(ProgramSkillFrame f) => _resolve().CanContinueBuZhi(f);
    internal GameEngine.NativeBuZhiDiscardInvocationDraft? CaptureNativeBuZhiDiscardInvocation(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _resolve().CaptureNativeBuZhiDiscardInvocation(frame, zones, reason);
    internal void CompleteNativeBuZhiDiscardInvocation(GameEngine.NativeBuZhiDiscardInvocationDraft? draft, IReadOnlyList<int> actualIds) => _resolve().CompleteNativeBuZhiDiscardInvocation(draft, actualIds);
    internal void FinishBuZhi(ProgramSkillFrame f) => _resolve().FinishBuZhi(f);
    internal bool IsBuZhiActivation(SkillProgramActivation a) => _resolve().IsBuZhiActivation(a);
    internal bool IsBuZhiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsBuZhiMovement(f, e, p);
    internal void IssueBuZhiDamage(ProgramSkillFrame f) => _resolve().IssueBuZhiDamage(f);
    internal void PayBuZhiDraw(ProgramSkillFrame f, int target, string next) => _resolve().PayBuZhiDraw(f, target, next);
    internal void PayBuZhiEquipment(ProgramSkillFrame f, IReadOnlyList<int> ids, CardLocation destination, string next) => _resolve().PayBuZhiEquipment(f, ids, destination, next);
    internal bool? ProveBuZhiMovement(SkillProgramTrigger trigger, CardMovementBatchContext batch, CharacterState owner) => _resolve().ProveBuZhiMovement(trigger, batch, owner);
    internal void PublishBuZhi(ProgramSkillFrame f) => _resolve().PublishBuZhi(f);
    internal SkillProgramStepOutcome ResolveBuZhi(ProgramSkillFrame supplied, bool gainLoss) => _resolve().ResolveBuZhi(supplied, gainLoss);
    internal void ResolveBuZhiChoice(PromptChoice selected) => _resolve().ResolveBuZhiChoice(selected);
    internal bool ResumeBuZhi(long id) => _resolve().ResumeBuZhi(id);
    internal bool ReturnBuZhiMovement(ProgramSkillFrame f) => _resolve().ReturnBuZhiMovement(f);
    internal PromptChoice SelectAiBuZhi(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiBuZhi(p, f);
    internal bool TryAdvanceBuZhiSubtree() => _resolve().TryAdvanceBuZhiSubtree();
    internal bool ValidBuZhi(ProgramSkillFrame f) => _resolve().ValidBuZhi(f);
    internal bool ValidNativeBuZhiDiscardInvocation(NativeBuZhiDiscardInvocationProof p) => _resolve().ValidNativeBuZhiDiscardInvocation(p);
}
