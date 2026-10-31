#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangMobileEntries
{
    internal bool AdvanceMobileDuanfaObserver() => _resolve().AdvanceMobileDuanfaObserver();
    internal void AnswerMobileDuanfa(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileDuanfa(selected);
    internal void AssertMobileDuanfa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileDuanfa(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileDuanfa(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMobileDuanfa(supplied);
    internal bool CanActivateMobileDuanfa(global::CardGame.Core.ProgramActivationLegalityContext g) => _resolve().CanActivateMobileDuanfa(g);
    internal void ContinueMobileDuanfa(long id) => _resolve().ContinueMobileDuanfa(id);
    internal void FinishMobileDuanfa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileDuanfa(f);
    internal bool KeepPaidMobileDuanfa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMobileDuanfa(f);
    internal bool KeepPaidMobileYoudi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMobileYoudi(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileDuanfaChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileDuanfaChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileDuanfaCosts(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileDuanfaCosts(f);
    internal bool MobileDuanfaDyingProof() => _resolve().MobileDuanfaDyingProof();
    internal bool MobileDuanfaHasBlackCost(global::CardGame.Core.CharacterState owner) => _resolve().MobileDuanfaHasBlackCost(owner);
    internal bool MobileDuanfaObserverChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileDuanfaObserverChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MobileDuanfaObserverRoot() => _resolve().MobileDuanfaObserverRoot();
    internal global::CardGame.Core.SkillGrantPhaseExpiry? MobileDuanfaPhase(int owner) => _resolve().MobileDuanfaPhase(owner);
    internal int MobileDuanfaRemaining(int owner) => _resolve().MobileDuanfaRemaining(owner);
    internal int MobileDuanfaSpent(int owner, global::CardGame.Core.SkillGrantPhaseExpiry phase) => _resolve().MobileDuanfaSpent(owner, phase);
    internal bool MobileYoudiPaidBait(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramSkillCardSetBinding? bait, scoped out global::CardGame.Core.NativeLogicalDiscardInvocationRecordedEvent? invoice) => _resolve().MobileYoudiPaidBait(f, out bait, out invoice);
    internal void PublishMobileYoudiBait(global::CardGame.Core.ProgramSkillFrame supplied, string sourceBind) => _resolve().PublishMobileYoudiBait(supplied, sourceBind);
    internal bool ResumeMobileDuanfa(long id) => _resolve().ResumeMobileDuanfa(id);
    internal void ReturnMobileDuanfaDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileDuanfaDraw(supplied, outcome);
    internal global::CardGame.Core.PromptChoice SelectMobileDuanfaAi(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectMobileDuanfaAi(d, f);
}
