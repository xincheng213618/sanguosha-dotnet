#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaHappyGodEntries
{
    internal bool AdvanceHappyGjSubtree() => _resolve().AdvanceHappyGjSubtree();
    internal void AnswerHappyGuoJia(PromptChoice c) => _resolve().AnswerHappyGuoJia(c);
    internal SkillProgramStepOutcome BeginHappyGuoJia(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHappyGuoJia(supplied, op);
    internal bool CanAwakenHappyGj(ProgramTriggerLegalityContext g) => _resolve().CanAwakenHappyGj(g);
    internal bool CanHappyGjGift(CharacterState owner) => _resolve().CanHappyGjGift(owner);
    internal bool CanHappyGjHuishi(CharacterState owner) => _resolve().CanHappyGjHuishi(owner);
    internal bool CanHappyGjZuoxing(CharacterState owner) => _resolve().CanHappyGjZuoxing(owner);
    internal void ContinueHappyGuoJia(long id) => _resolve().ContinueHappyGuoJia(id);
    internal bool ContinuePaidHappyGj(ProgramSkillFrame f) => _resolve().ContinuePaidHappyGj(f);
    internal void FinishHappyGj(ProgramSkillFrame f) => _resolve().FinishHappyGj(f);
    internal PromptChoice HappyGjAi(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyGjAi(p, f);
    internal IEnumerable<int> HappyGjBenefactors(CharacterState owner, string instance) => _resolve().HappyGjBenefactors(owner, instance);
    internal ProgramSkillFrame HappyGjChildren(ProgramSkillFrame f, HappyGuoJiaStage stage) => _resolve().HappyGjChildren(f, stage);
    internal PromptChoice HappyGjChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? targets = default, string? value = default) => _resolve().HappyGjChoice(f, branch, label, targets, value);
    internal IReadOnlyList<PromptChoice> HappyGjChoices(ProgramSkillFrame f) => _resolve().HappyGjChoices(f);
    internal bool HappyGjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HappyGjFirstChild(f, child);
    internal bool HappyGjGiftTarget(CharacterState owner, int seat) => _resolve().HappyGjGiftTarget(owner, seat);
    internal int HappyGjMaximumAfterPayment(int seat, int before, int amount) => _resolve().HappyGjMaximumAfterPayment(seat, before, amount);
    internal ProgramSkillFrame? HappyGjObserverRoot() => _resolve().HappyGjObserverRoot();
    internal IReadOnlyList<ShenGuoJiaPlan> HappyGjPlans(int owner) => _resolve().HappyGjPlans(owner);
    internal CardConversionSource HappyGjSource(ProgramSkillFrame f) => _resolve().HappyGjSource(f);
    internal bool IsHappyGuoJiaVirtualUse(CardUseFrame use) => _resolve().IsHappyGuoJiaVirtualUse(use);
    internal void IssueHappyGuoJiaUse(ProgramSkillFrame frame) => _resolve().IssueHappyGuoJiaUse(frame);
    internal void MoveHappyGjRevealed(ProgramSkillFrame f, int target) => _resolve().MoveHappyGjRevealed(f, target);
    internal void PayHappyGjMaximum(ProgramSkillFrame f, int seat, int amount, string purpose) => _resolve().PayHappyGjMaximum(f, seat, amount, purpose);
    internal void PublishHappyGj(ProgramSkillFrame f) => _resolve().PublishHappyGj(f);
    internal bool ResumeHappyGuoJia(long id) => _resolve().ResumeHappyGuoJia(id);
    internal void ReturnHappyGjDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnHappyGjDraw(supplied, outcome);
    internal bool ReturnHappyGjMovement(ProgramSkillFrame f) => _resolve().ReturnHappyGjMovement(f);
    internal bool ReturnHappyGuoJiaAttack() => _resolve().ReturnHappyGuoJiaAttack();
    internal bool TryReturnHappyGuoJiaVirtualUse(CardUseFrame use) => _resolve().TryReturnHappyGuoJiaVirtualUse(use);
    internal bool ValidHappyGj(ProgramSkillFrame f) => _resolve().ValidHappyGj(f);
}
