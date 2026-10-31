#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLeiBoAnniversaryEntries
{
    internal bool AdvanceLeiBoChildren() => _resolve().AdvanceLeiBoChildren();
    internal PromptChoice AiLeiBo(PendingDecision p, ProgramSkillFrame f) => _resolve().AiLeiBo(p, f);
    internal void AnswerLeiBo(PromptChoice answer) => _resolve().AnswerLeiBo(answer);
    internal SkillProgramStepOutcome BeginLeiBoShuaijie(ProgramSkillFrame supplied) => _resolve().BeginLeiBoShuaijie(supplied);
    internal bool CanActivateLeiBo(ProgramActivationLegalityContext c) => _resolve().CanActivateLeiBo(c);
    internal bool CanLeiBoShuaijie(int owner) => _resolve().CanLeiBoShuaijie(owner);
    internal bool CanOfferLeiBo(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferLeiBo(candidate, trigger, c);
    internal void CompleteLeiBo(ProgramSkillFrame f) => _resolve().CompleteLeiBo(f);
    internal void ContinueLeiBo(long id) => _resolve().ContinueLeiBo(id);
    internal bool DrainLeiBo(ProgramSkillFrame f) => _resolve().DrainLeiBo(f);
    internal SkillProgramStepOutcome FinishLeiBoShuaijie(ProgramSkillFrame supplied) => _resolve().FinishLeiBoShuaijie(supplied);
    internal SkillProgramStepOutcome InitializeLeiBo(ProgramSkillFrame supplied) => _resolve().InitializeLeiBo(supplied);
    internal bool IsAnniversaryLeiBoFixedSlash(ProgramSkillFrame f) => _resolve().IsAnniversaryLeiBoFixedSlash(f);
    internal bool IsAnniversaryLeiBoSelfLoot(ProgramSkillFrame f, SkillProgramEffect e) => _resolve().IsAnniversaryLeiBoSelfLoot(f, e);
    internal void IssueLeiBoMovement(ProgramSkillFrame f, bool discard) => _resolve().IssueLeiBoMovement(f, discard);
    internal IReadOnlyList<PromptChoice> LeiBoChoices(ProgramSkillFrame f) => _resolve().LeiBoChoices(f);
    internal bool LeiBoDamage(ProgramSkillWindowContext c) => _resolve().LeiBoDamage(c);
    internal bool LeiBoDamageParent(ProgramSkillFrame f) => _resolve().LeiBoDamageParent(f);
    internal bool LeiBoDyingObserver() => _resolve().LeiBoDyingObserver();
    internal bool LeiBoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LeiBoFirstChild(f, child);
    internal bool LeiBoLoot(int owner, ProgramSkillWindowContext c) => _resolve().LeiBoLoot(owner, c);
    internal int LeiBoMarkedSeat(int owner) => _resolve().LeiBoMarkedSeat(owner);
    internal bool LeiBoMaximumPaid(ProgramSkillFrame f) => _resolve().LeiBoMaximumPaid(f);
    internal bool LeiBoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().LeiBoMovement(f, e, p);
    internal bool LeiBoObserverEdge(int i) => _resolve().LeiBoObserverEdge(i);
    internal ProgramSkillFrame? LeiBoObserverRoot() => _resolve().LeiBoObserverRoot();
    internal void MoveLeiBoMark(ProgramSkillFrame f, int target) => _resolve().MoveLeiBoMark(f, target);
    internal void ObserveLeiBoSlash(CardUseDeclaredEvent fact) => _resolve().ObserveLeiBoSlash(fact);
    internal void PublishLeiBo(ProgramSkillFrame f) => _resolve().PublishLeiBo(f);
    internal SkillProgramStepOutcome RecordLeiBoLoot(ProgramSkillFrame supplied) => _resolve().RecordLeiBoLoot(supplied);
    internal SkillProgramStepOutcome RequestLeiBoSlash(ProgramSkillFrame supplied) => _resolve().RequestLeiBoSlash(supplied);
    internal bool ResumeLeiBo(long id) => _resolve().ResumeLeiBo(id);
    internal bool RetainLeiBo(ProgramSkillFrame f) => _resolve().RetainLeiBo(f);
    internal bool ReturnLeiBoMovement(ProgramSkillFrame f) => _resolve().ReturnLeiBoMovement(f);
    internal SkillProgramStepOutcome SettleLeiBoPenalty(ProgramSkillFrame supplied) => _resolve().SettleLeiBoPenalty(supplied);
    internal bool ValidLeiBo(ProgramSkillFrame f) => _resolve().ValidLeiBo(f);
}
