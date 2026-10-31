#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangCanMobileEntries
{
    internal bool AdmitMobileWangCanRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().AdmitMobileWangCanRecovery(c, w);
    internal bool AdvanceMobileWangCanXiHealth() => _resolve().AdvanceMobileWangCanXiHealth();
    internal void AnswerMobileWangCanXi(PromptChoice selected) => _resolve().AnswerMobileWangCanXi(selected);
    internal SkillProgramStepOutcome BeginMobileWangCanXi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileWangCanXi(supplied, op);
    internal bool CanOfferMobileWangCanMark(ProgramTriggerLegalityContext g) => _resolve().CanOfferMobileWangCanMark(g);
    internal void CaptureMobileWangCanRecovery(HpChangeContext change) => _resolve().CaptureMobileWangCanRecovery(change);
    internal PromptChoice ChooseMobileWangCanXi(PendingDecision d, ProgramSkillFrame f) => _resolve().ChooseMobileWangCanXi(d, f);
    internal void ContinueMobileWangCanXi(long id) => _resolve().ContinueMobileWangCanXi(id);
    internal bool ExactMobileWangCanRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().ExactMobileWangCanRecovery(c, w);
    internal void FinishMobileWangCanXi(ProgramSkillFrame f) => _resolve().FinishMobileWangCanXi(f);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMobileWangCanRecovery(HpChangeContext change, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeMobileWangCanRecovery(change, ordinary);
    internal bool KeepMobileWangCanXi(ProgramSkillFrame f) => _resolve().KeepMobileWangCanXi(f);
    internal IReadOnlyList<PromptChoice> MobileWangCanXiChoices(ProgramSkillFrame f) => _resolve().MobileWangCanXiChoices(f);
    internal IReadOnlyList<PromptChoice> MobileWangCanXiCosts(ProgramSkillFrame f) => _resolve().MobileWangCanXiCosts(f);
    internal bool MobileWangCanXiDyingProof() => _resolve().MobileWangCanXiDyingProof();
    internal bool MobileWangCanXiHealthSubject(int target) => _resolve().MobileWangCanXiHealthSubject(target);
    internal bool MobileWangCanXiObserverChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MobileWangCanXiObserverChild(f, child);
    internal ProgramSkillFrame? MobileWangCanXiObserverRoot() => _resolve().MobileWangCanXiObserverRoot();
    internal IEnumerable<int> MobileWangCanXiTargets(int owner) => _resolve().MobileWangCanXiTargets(owner);
    internal bool ResumeMobileWangCanXi(long id) => _resolve().ResumeMobileWangCanXi(id);
}
