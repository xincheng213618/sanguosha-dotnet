#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangMobileEntries
{
    internal bool AdvanceMobileDuanfaObserver() => _resolve().AdvanceMobileDuanfaObserver();
    internal void AnswerMobileDuanfa(PromptChoice selected) => _resolve().AnswerMobileDuanfa(selected);
    internal SkillProgramStepOutcome BeginMobileDuanfa(ProgramSkillFrame supplied) => _resolve().BeginMobileDuanfa(supplied);
    internal bool CanActivateMobileDuanfa(ProgramActivationLegalityContext g) => _resolve().CanActivateMobileDuanfa(g);
    internal void ContinueMobileDuanfa(long id) => _resolve().ContinueMobileDuanfa(id);
    internal void FinishMobileDuanfa(ProgramSkillFrame f) => _resolve().FinishMobileDuanfa(f);
    internal bool KeepPaidMobileDuanfa(ProgramSkillFrame f) => _resolve().KeepPaidMobileDuanfa(f);
    internal bool KeepPaidMobileYoudi(ProgramSkillFrame f) => _resolve().KeepPaidMobileYoudi(f);
    internal IReadOnlyList<PromptChoice> MobileDuanfaChoices(ProgramSkillFrame f) => _resolve().MobileDuanfaChoices(f);
    internal IReadOnlyList<PromptChoice> MobileDuanfaCosts(ProgramSkillFrame f) => _resolve().MobileDuanfaCosts(f);
    internal bool MobileDuanfaDyingProof() => _resolve().MobileDuanfaDyingProof();
    internal bool MobileDuanfaHasBlackCost(CharacterState owner) => _resolve().MobileDuanfaHasBlackCost(owner);
    internal bool MobileDuanfaObserverChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MobileDuanfaObserverChild(f, child);
    internal ProgramSkillFrame? MobileDuanfaObserverRoot() => _resolve().MobileDuanfaObserverRoot();
    internal SkillGrantPhaseExpiry? MobileDuanfaPhase(int owner) => _resolve().MobileDuanfaPhase(owner);
    internal int MobileDuanfaRemaining(int owner) => _resolve().MobileDuanfaRemaining(owner);
    internal int MobileDuanfaSpent(int owner, SkillGrantPhaseExpiry phase) => _resolve().MobileDuanfaSpent(owner, phase);
    internal bool MobileYoudiPaidBait(ProgramSkillFrame f, scoped out ProgramSkillCardSetBinding? bait, scoped out NativeLogicalDiscardInvocationRecordedEvent? invoice) => _resolve().MobileYoudiPaidBait(f, out bait, out invoice);
    internal void PublishMobileYoudiBait(ProgramSkillFrame supplied, string sourceBind) => _resolve().PublishMobileYoudiBait(supplied, sourceBind);
    internal bool ResumeMobileDuanfa(long id) => _resolve().ResumeMobileDuanfa(id);
    internal void ReturnMobileDuanfaDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnMobileDuanfaDraw(supplied, outcome);
    internal PromptChoice SelectMobileDuanfaAi(PendingDecision d, ProgramSkillFrame f) => _resolve().SelectMobileDuanfaAi(d, f);
}
