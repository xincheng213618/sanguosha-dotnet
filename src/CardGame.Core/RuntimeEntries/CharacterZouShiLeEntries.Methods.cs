#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZouShiLeEntries
{
    internal bool AdvanceHuoxinChildren() => _resolve().AdvanceHuoxinChildren();
    internal void AnswerHuoxin(PromptChoice choice) => _resolve().AnswerHuoxin(choice);
    internal SkillProgramStepOutcome BeginHuoxin(ProgramSkillFrame supplied) => _resolve().BeginHuoxin(supplied);
    internal bool CanOfferHuoxin(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferHuoxin(candidate, trigger, context);
    internal void ContinueHuoxin(long id) => _resolve().ContinueHuoxin(id);
    internal void FinishHuoxin(ProgramSkillFrame f) => _resolve().FinishHuoxin(f);
    internal bool HasYunzhengEntityMark(int cardId) => _resolve().HasYunzhengEntityMark(cardId);
    internal bool HasYunzhengHandEntityMark(int ownerSeat, int cardId) => _resolve().HasYunzhengHandEntityMark(ownerSeat, cardId);
    internal PromptChoice HuoxinAi(PendingDecision p, ProgramSkillFrame f) => _resolve().HuoxinAi(p, f);
    internal IReadOnlyList<PromptChoice> HuoxinChoices(ProgramSkillFrame f) => _resolve().HuoxinChoices(f);
    internal bool HuoxinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HuoxinFirstChild(f, child);
    internal ProgramSkillFrame? HuoxinObserverRoot() => _resolve().HuoxinObserverRoot();
    internal bool HuoxinObserverSuffix(int index) => _resolve().HuoxinObserverSuffix(index);
    internal bool IsHuoxinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().IsHuoxinMovement(f, effect, p);
    internal bool IsHuoxinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsHuoxinRecoveryParent(f, recovery);
    internal bool IsYunzhengHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsYunzhengHandLimitExempt(owner, cardId);
    internal bool PermitPaidHuoxin(ProgramSkillFrame f) => _resolve().PermitPaidHuoxin(f);
    internal void PublishHuoxin(ProgramSkillFrame f) => _resolve().PublishHuoxin(f);
    internal bool ResumeHuoxin(long id) => _resolve().ResumeHuoxin(id);
    internal bool ReturnHuoxinMovement(ProgramSkillFrame f) => _resolve().ReturnHuoxinMovement(f);
    internal bool TryGetHuoxinUse(int owner, ProgramSkillWindowContext context, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetHuoxinUse(owner, context, out window);
}
