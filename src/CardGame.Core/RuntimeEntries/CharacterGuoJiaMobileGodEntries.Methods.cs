#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaMobileGodEntries
{
    internal bool AdvanceShenGuoJiaSubtree() => _resolve().AdvanceShenGuoJiaSubtree();
    internal void AnswerShenGuoJia(PromptChoice choice) => _resolve().AnswerShenGuoJia(choice);
    internal SkillProgramStepOutcome BeginShenGuoJia(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginShenGuoJia(supplied, operation);
    internal void BeginShenGuoJiaJudgment(ProgramSkillFrame frame) => _resolve().BeginShenGuoJiaJudgment(frame);
    internal bool CanActivateShenGuoJiaHuishi(CharacterState owner) => _resolve().CanActivateShenGuoJiaHuishi(owner);
    internal bool CanActivateShenGuoJiaZuoxing(CharacterState owner) => _resolve().CanActivateShenGuoJiaZuoxing(owner);
    internal bool CanAwakenShenGuoJia(ProgramTriggerLegalityContext gate) => _resolve().CanAwakenShenGuoJia(gate);
    internal bool CanBeginShenGuoJiaJudgment(ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, SkillProgramCardSetVisibility visibility) => _resolve().CanBeginShenGuoJiaJudgment(frame, targetSeat, reason, resultBind, visibility);
    internal PromptChoice ChooseShenGuoJiaAi(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().ChooseShenGuoJiaAi(prompt, frame);
    internal void ChooseShenGuoJiaRecipient(ProgramSkillFrame frame) => _resolve().ChooseShenGuoJiaRecipient(frame);
    internal bool ContinuePaidShenGuoJia(ProgramSkillFrame frame) => _resolve().ContinuePaidShenGuoJia(frame);
    internal void ContinueShenGuoJia(long id) => _resolve().ContinueShenGuoJia(id);
    internal bool DrainShenGuoJia(ProgramSkillFrame frame) => _resolve().DrainShenGuoJia(frame);
    internal void FinishShenGuoJia(ProgramSkillFrame frame) => _resolve().FinishShenGuoJia(frame);
    internal void FinishShenGuoJiaIssue(long id) => _resolve().FinishShenGuoJiaIssue(id);
    internal bool HasShenGuoJiaAwakeningCondition(int seat, string skill, string? binding = default) => _resolve().HasShenGuoJiaAwakeningCondition(seat, skill, binding);
    internal bool HasShenGuoJiaAwakeningReceipt(ProgramSkillFrame frame) => _resolve().HasShenGuoJiaAwakeningReceipt(frame);
    internal bool IsShenGuoJiaJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _resolve().IsShenGuoJiaJudgment(judgment, frame);
    internal bool IsShenGuoJiaVirtualUse(CardUseFrame use) => _resolve().IsShenGuoJiaVirtualUse(use);
    internal void IssueShenGuoJiaVirtualUse(ProgramSkillFrame frame) => _resolve().IssueShenGuoJiaVirtualUse(frame);
    internal void MoveShenGuoJiaJudgments(ProgramSkillFrame frame, int target) => _resolve().MoveShenGuoJiaJudgments(frame, target);
    internal void PayShenGuoJiaFinalCost(ProgramSkillFrame frame, int amount, string purpose) => _resolve().PayShenGuoJiaFinalCost(frame, amount, purpose);
    internal void PayShenGuoJiaMaximum(ProgramSkillFrame frame, int seat, int amount, string purpose) => _resolve().PayShenGuoJiaMaximum(frame, seat, amount, purpose);
    internal void PublishShenGuoJiaChoice(ProgramSkillFrame frame) => _resolve().PublishShenGuoJiaChoice(frame);
    internal bool ResumeShenGuoJia(long id) => _resolve().ResumeShenGuoJia(id);
    internal bool ReturnShenGuoJiaAttack() => _resolve().ReturnShenGuoJiaAttack();
    internal void ReturnShenGuoJiaFourDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShenGuoJiaFourDraw(supplied, outcome);
    internal bool ReturnShenGuoJiaMovement(ProgramSkillFrame frame) => _resolve().ReturnShenGuoJiaMovement(frame);
    internal ProgramSkillFrame SetShenGuoJiaChildren(ProgramSkillFrame frame, ShenGuoJiaStage stage) => _resolve().SetShenGuoJiaChildren(frame, stage);
    internal bool ShenGuoJiaAlreadyAwakened(int seat, string skill) => _resolve().ShenGuoJiaAlreadyAwakened(seat, skill);
    internal IEnumerable<string> ShenGuoJiaAwakeningBindings(string skill) => _resolve().ShenGuoJiaAwakeningBindings(skill);
    internal bool ShenGuoJiaAwakeningOverride(ProgramTriggerLegalityContext gate) => _resolve().ShenGuoJiaAwakeningOverride(gate);
    internal IReadOnlyList<(int Seat, string Instance)> ShenGuoJiaBenefactors(CharacterState owner, string instance) => _resolve().ShenGuoJiaBenefactors(owner, instance);
    internal PromptChoice ShenGuoJiaChoice(ProgramSkillFrame frame, string branch, string label, IReadOnlyList<int>? targets = default, string? value = default) => _resolve().ShenGuoJiaChoice(frame, branch, label, targets, value);
    internal IReadOnlyList<PromptChoice> ShenGuoJiaChoices(ProgramSkillFrame frame) => _resolve().ShenGuoJiaChoices(frame);
    internal bool ShenGuoJiaFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().ShenGuoJiaFirstChild(frame, child);
    internal ProgramSkillFrame? ShenGuoJiaObserverRoot() => _resolve().ShenGuoJiaObserverRoot();
    internal IReadOnlyList<ShenGuoJiaPlan> ShenGuoJiaPlans(int owner) => _resolve().ShenGuoJiaPlans(owner);
    internal CardConversionSource ShenGuoJiaSource(ProgramSkillFrame frame) => _resolve().ShenGuoJiaSource(frame);
    internal IReadOnlyList<IndexedSkillProgramInstance> ShenGuoJiaUnawakened(int seat) => _resolve().ShenGuoJiaUnawakened(seat);
    internal bool TryReturnShenGuoJiaJudgment(JudgmentFrame judgment) => _resolve().TryReturnShenGuoJiaJudgment(judgment);
    internal bool TryReturnShenGuoJiaVirtualUse(CardUseFrame use) => _resolve().TryReturnShenGuoJiaVirtualUse(use);
    internal bool ValidShenGuoJia(ProgramSkillFrame frame) => _resolve().ValidShenGuoJia(frame);
}
