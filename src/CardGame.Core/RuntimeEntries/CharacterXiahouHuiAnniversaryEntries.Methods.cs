#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouHuiAnniversaryEntries
{
    internal bool AdvanceXiahouHuiSubtree() => _resolve().AdvanceXiahouHuiSubtree();
    internal bool AnniversaryXiahouHuiCannotRespond(int sourceSeat, int responderSeat) => _resolve().AnniversaryXiahouHuiCannotRespond(sourceSeat, responderSeat);
    internal bool AnniversaryXiahouHuiResponseForbidden(int actorSeat, global::CardGame.Core.CardKind requiredKind) => _resolve().AnniversaryXiahouHuiResponseForbidden(actorSeat, requiredKind);
    internal void AnswerXiahouHui(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerXiahouHui(answer);
    internal void AssertXiahouHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertXiahouHui(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXiahouHui(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginXiahouHui(supplied, operation);
    internal bool CanOfferXiahouHui(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferXiahouHui(candidate, t, c);
    internal void CaptureXiahouHuiFacts(global::CardGame.Core.IGameEvent fact) => _resolve().CaptureXiahouHuiFacts(fact);
    internal bool ContinueXiahouHui(long id) => _resolve().ContinueXiahouHui(id);
    internal void FinishXiahouHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXiahouHui(f);
    internal bool HasAnniversaryXiahouHuiPrompt() => _resolve().HasAnniversaryXiahouHuiPrompt();
    internal bool KeepXiahouHuiPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepXiahouHuiPaid(f);
    internal void PublishXiahouHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXiahouHui(f);
    internal bool ReturnXiahouHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXiahouHui(f);
    internal global::CardGame.Core.PromptChoice SelectAiXiahouHuiTransfer(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiXiahouHuiTransfer(decision, frame);
    internal bool ValidXiahouHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidXiahouHui(f);
    internal bool XiahouHuiBeforeDamageObserver(long id) => _resolve().XiahouHuiBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XiahouHuiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XiahouHuiChoices(f);
    internal bool XiahouHuiCounterspellBan(global::CardGame.Core.ProgramCardUseQuery q) => _resolve().XiahouHuiCounterspellBan(q);
    internal bool XiahouHuiDamageCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c, bool issued = false) => _resolve().XiahouHuiDamageCandidate(candidate, c, issued);
    internal bool XiahouHuiDamageObserver(long id) => _resolve().XiahouHuiDamageObserver(id);
    internal bool XiahouHuiDyingObserver() => _resolve().XiahouHuiDyingObserver();
    internal bool XiahouHuiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().XiahouHuiFirstChild(f, child);
    internal global::CardGame.Core.XiahouHuiLossRecordedEvent? XiahouHuiLossCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().XiahouHuiLossCandidate(candidate, c);
    internal bool XiahouHuiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().XiahouHuiMovement(f, e, p);
    internal bool XiahouHuiNativeResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().XiahouHuiNativeResponseBan(q);
    internal global::CardGame.Core.ProgramSkillFrame? XiahouHuiObserverRoot() => _resolve().XiahouHuiObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.XiahouHuiCandidateCard> XiahouHuiObtainable(global::CardGame.Core.ProgramSkillFrame f, int donor, int? recipient = default) => _resolve().XiahouHuiObtainable(f, donor, recipient);
    internal global::CardGame.Core.XiahouHuiBoundEvent? XiahouHuiPartner(int owner, string? instance = default, bool requireCurrent = true) => _resolve().XiahouHuiPartner(owner, instance, requireCurrent);
    internal bool XiahouHuiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().XiahouHuiRecoveryParent(f, recovery);
    internal bool HasXiahouHuiDujun { get => _resolve().HasXiahouHuiDujun; }
    internal bool HasXiahouHuiJikun { get => _resolve().HasXiahouHuiJikun; }
}
