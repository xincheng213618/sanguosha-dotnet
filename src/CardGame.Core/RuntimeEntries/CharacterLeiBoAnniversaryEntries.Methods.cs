#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLeiBoAnniversaryEntries
{
    internal bool AdvanceLeiBoChildren() => _resolve().AdvanceLeiBoChildren();
    internal global::CardGame.Core.PromptChoice AiLeiBo(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiLeiBo(p, f);
    internal void AnswerLeiBo(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerLeiBo(answer);
    internal void AssertLeiBo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLeiBo(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLeiBoShuaijie(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginLeiBoShuaijie(supplied);
    internal bool CanActivateLeiBo(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateLeiBo(c);
    internal bool CanLeiBoShuaijie(int owner) => _resolve().CanLeiBoShuaijie(owner);
    internal bool CanOfferLeiBo(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferLeiBo(candidate, trigger, c);
    internal void CompleteLeiBo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteLeiBo(f);
    internal void ContinueLeiBo(long id) => _resolve().ContinueLeiBo(id);
    internal bool DrainLeiBo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLeiBo(f);
    internal global::CardGame.Core.SkillProgramStepOutcome FinishLeiBoShuaijie(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().FinishLeiBoShuaijie(supplied);
    internal bool HasAnniversaryLeiBoPrompt() => _resolve().HasAnniversaryLeiBoPrompt();
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeLeiBo(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeLeiBo(supplied);
    internal bool IsAnniversaryLeiBoFactionChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => _resolve().IsAnniversaryLeiBoFactionChoiceResult(f, b);
    internal bool IsAnniversaryLeiBoFixedSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsAnniversaryLeiBoFixedSlash(f);
    internal bool IsAnniversaryLeiBoSelfLoot(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect e) => _resolve().IsAnniversaryLeiBoSelfLoot(f, e);
    internal void IssueLeiBoMovement(global::CardGame.Core.ProgramSkillFrame f, bool discard) => _resolve().IssueLeiBoMovement(f, discard);
    internal bool LeiBoBeforeDamageObserver(long id) => _resolve().LeiBoBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LeiBoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LeiBoChoices(f);
    internal bool LeiBoDamage(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().LeiBoDamage(c);
    internal bool LeiBoDamageObserver(long id) => _resolve().LeiBoDamageObserver(id);
    internal bool LeiBoDamageParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LeiBoDamageParent(f);
    internal bool LeiBoDyingObserver() => _resolve().LeiBoDyingObserver();
    internal bool LeiBoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LeiBoFirstChild(f, child);
    internal bool LeiBoLoot(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().LeiBoLoot(owner, c);
    internal int LeiBoMarkedSeat(int owner) => _resolve().LeiBoMarkedSeat(owner);
    internal bool LeiBoMaximumPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LeiBoMaximumPaid(f);
    internal bool LeiBoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().LeiBoMovement(f, e, p);
    internal bool LeiBoObserverEdge(int i) => _resolve().LeiBoObserverEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? LeiBoObserverRoot() => _resolve().LeiBoObserverRoot();
    internal void MoveLeiBoMark(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().MoveLeiBoMark(f, target);
    internal void ObserveLeiBoSlash(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().ObserveLeiBoSlash(fact);
    internal void PublishLeiBo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLeiBo(f);
    internal global::CardGame.Core.SkillProgramStepOutcome RecordLeiBoLoot(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RecordLeiBoLoot(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestLeiBoSlash(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RequestLeiBoSlash(supplied);
    internal bool ResumeLeiBo(long id) => _resolve().ResumeLeiBo(id);
    internal bool RetainLeiBo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainLeiBo(f);
    internal bool ReturnLeiBoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLeiBoMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleLeiBoPenalty(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SettleLeiBoPenalty(supplied);
    internal bool ValidLeiBo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLeiBo(f);
}
