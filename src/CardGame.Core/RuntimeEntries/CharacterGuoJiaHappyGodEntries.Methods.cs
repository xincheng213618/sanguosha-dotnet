#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaHappyGodEntries
{
    internal bool AdvanceHappyGjSubtree() => _resolve().AdvanceHappyGjSubtree();
    internal void AnswerHappyGuoJia(global::CardGame.Core.PromptChoice c) => _resolve().AnswerHappyGuoJia(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyGuoJia(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHappyGuoJia(supplied, op);
    internal bool CanAwakenHappyGj(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanAwakenHappyGj(g);
    internal bool CanHappyGjGift(global::CardGame.Core.CharacterState owner) => _resolve().CanHappyGjGift(owner);
    internal bool CanHappyGjHuishi(global::CardGame.Core.CharacterState owner) => _resolve().CanHappyGjHuishi(owner);
    internal bool CanHappyGjZuoxing(global::CardGame.Core.CharacterState owner) => _resolve().CanHappyGjZuoxing(owner);
    internal void ContinueHappyGuoJia(long id) => _resolve().ContinueHappyGuoJia(id);
    internal bool ContinuePaidHappyGj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHappyGj(f);
    internal void FinishHappyGj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHappyGj(f);
    internal global::CardGame.Core.PromptChoice HappyGjAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGjAi(p, f);
    internal bool HappyGjBeforeDamageObserver(long id) => _resolve().HappyGjBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IEnumerable<int> HappyGjBenefactors(global::CardGame.Core.CharacterState owner, string instance) => _resolve().HappyGjBenefactors(owner, instance);
    internal global::CardGame.Core.ProgramSkillFrame HappyGjChildren(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HappyGuoJiaStage stage) => _resolve().HappyGjChildren(f, stage);
    internal global::CardGame.Core.PromptChoice HappyGjChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, string? value = default) => _resolve().HappyGjChoice(f, branch, label, targets, value);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyGjChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGjChoices(f);
    internal bool HappyGjDamageObserver(long id) => _resolve().HappyGjDamageObserver(id);
    internal bool HappyGjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HappyGjFirstChild(f, child);
    internal bool HappyGjGiftTarget(global::CardGame.Core.CharacterState owner, int seat) => _resolve().HappyGjGiftTarget(owner, seat);
    internal int HappyGjMaximumAfterPayment(int seat, int before, int amount) => _resolve().HappyGjMaximumAfterPayment(seat, before, amount);
    internal bool HappyGjMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HappyGjMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? HappyGjObserverRoot() => _resolve().HappyGjObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ShenGuoJiaPlan> HappyGjPlans(int owner) => _resolve().HappyGjPlans(owner);
    internal global::CardGame.Core.CardConversionSource HappyGjSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGjSource(f);
    internal bool IsHappyGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsHappyGuoJiaVirtualUse(use);
    internal void IssueHappyGuoJiaUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IssueHappyGuoJiaUse(frame);
    internal void MoveHappyGjRevealed(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().MoveHappyGjRevealed(f, target);
    internal void PayHappyGjMaximum(global::CardGame.Core.ProgramSkillFrame f, int seat, int amount, string purpose) => _resolve().PayHappyGjMaximum(f, seat, amount, purpose);
    internal void PublishHappyGj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyGj(f);
    internal bool ResumeHappyGuoJia(long id) => _resolve().ResumeHappyGuoJia(id);
    internal void ReturnHappyGjDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyGjDraw(supplied, outcome);
    internal bool ReturnHappyGjMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyGjMovement(f);
    internal bool ReturnHappyGuoJiaAttack() => _resolve().ReturnHappyGuoJiaAttack();
    internal bool TryReturnHappyGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().TryReturnHappyGuoJiaVirtualUse(use);
    internal bool ValidHappyGj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyGj(f);
}
