#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiKangMobileEntries
{
    internal bool AdvanceMobileJiKangHealth() => _resolve().AdvanceMobileJiKangHealth();
    internal void AnswerMobileJiKang(PromptChoice selected) => _resolve().AnswerMobileJiKang(selected);
    internal SkillProgramStepOutcome BeginMobileJiKang(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileJiKang(supplied, op);
    internal bool CanActivateMobileJiKang(ProgramActivationLegalityContext gate) => _resolve().CanActivateMobileJiKang(gate);
    internal void ContinueMobileJiKang(long id) => _resolve().ContinueMobileJiKang(id);
    internal bool DrainMobileJiKang(long id) => _resolve().DrainMobileJiKang(id);
    internal void FinishMobileJiKang(ProgramSkillFrame f) => _resolve().FinishMobileJiKang(f);
    internal bool KeepMobileJiKangIssued(ProgramSkillFrame f) => _resolve().KeepMobileJiKangIssued(f);
    internal void LoseMobileJiKangHp(ProgramSkillFrame f, int target, MobileJiKangStage after) => _resolve().LoseMobileJiKangHp(f, target, after);
    internal Dictionary<string, string> MobileJiKangArgs(ProgramSkillFrame f, string branch) => _resolve().MobileJiKangArgs(f, branch);
    internal IReadOnlyList<PromptChoice> MobileJiKangChoices(ProgramSkillFrame f) => _resolve().MobileJiKangChoices(f);
    internal IEnumerable<Card> MobileJiKangClubCards(int actor) => _resolve().MobileJiKangClubCards(actor);
    internal IReadOnlyList<PromptChoice> MobileJiKangCosts(long frame, int owner) => _resolve().MobileJiKangCosts(frame, owner);
    internal void MobileJiKangDrawReturned(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().MobileJiKangDrawReturned(supplied, outcome);
    internal bool MobileJiKangDyingProof() => _resolve().MobileJiKangDyingProof();
    internal bool MobileJiKangHasCost(CharacterState owner) => _resolve().MobileJiKangHasCost(owner);
    internal bool MobileJiKangObserverChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MobileJiKangObserverChild(f, child);
    internal ProgramSkillFrame? MobileJiKangObserverRoot() => _resolve().MobileJiKangObserverRoot();
    internal bool MobileJiKangPaidInstruction(ProgramSkillFrame f) => _resolve().MobileJiKangPaidInstruction(f);
    internal bool MobileJiKangTargetAllowed(CardConversionSource source, int seat) => _resolve().MobileJiKangTargetAllowed(source, seat);
    internal void PayMobileJiKang(ProgramSkillFrame f, IReadOnlyList<int> cards, int actor, int subject, MobileJiKangStage after) => _resolve().PayMobileJiKang(f, cards, actor, subject, after);
    internal void PublishMobileJiKang(ProgramSkillFrame f) => _resolve().PublishMobileJiKang(f);
    internal bool ResumeMobileJiKang(long id) => _resolve().ResumeMobileJiKang(id);
    internal PromptChoice SelectMobileJiKangAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectMobileJiKangAi(decision, f);
    internal void StepMobileJiKang(ProgramSkillFrame f, MobileJiKangReceipt r) => _resolve().StepMobileJiKang(f, r);
}
