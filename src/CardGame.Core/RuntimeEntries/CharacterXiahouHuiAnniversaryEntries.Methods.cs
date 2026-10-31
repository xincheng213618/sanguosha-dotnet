#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouHuiAnniversaryEntries
{
    internal bool AdvanceXiahouHuiSubtree() => _resolve().AdvanceXiahouHuiSubtree();
    internal bool AnniversaryXiahouHuiCannotRespond(int sourceSeat, int responderSeat) => _resolve().AnniversaryXiahouHuiCannotRespond(sourceSeat, responderSeat);
    internal bool AnniversaryXiahouHuiResponseForbidden(int actorSeat, CardKind requiredKind) => _resolve().AnniversaryXiahouHuiResponseForbidden(actorSeat, requiredKind);
    internal void AnswerXiahouHui(PromptChoice answer) => _resolve().AnswerXiahouHui(answer);
    internal SkillProgramStepOutcome BeginXiahouHui(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginXiahouHui(supplied, operation);
    internal bool CanOfferXiahouHui(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferXiahouHui(candidate, t, c);
    internal void CaptureXiahouHuiFacts(IGameEvent fact) => _resolve().CaptureXiahouHuiFacts(fact);
    internal bool ContinueXiahouHui(long id) => _resolve().ContinueXiahouHui(id);
    internal void FinishXiahouHui(ProgramSkillFrame f) => _resolve().FinishXiahouHui(f);
    internal bool KeepXiahouHuiPaid(ProgramSkillFrame f) => _resolve().KeepXiahouHuiPaid(f);
    internal void PublishXiahouHui(ProgramSkillFrame f) => _resolve().PublishXiahouHui(f);
    internal bool ReturnXiahouHui(ProgramSkillFrame f) => _resolve().ReturnXiahouHui(f);
    internal PromptChoice SelectAiXiahouHuiTransfer(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiXiahouHuiTransfer(decision, frame);
    internal bool ValidXiahouHui(ProgramSkillFrame f) => _resolve().ValidXiahouHui(f);
    internal IReadOnlyList<PromptChoice> XiahouHuiChoices(ProgramSkillFrame f) => _resolve().XiahouHuiChoices(f);
    internal bool XiahouHuiCounterspellBan(ProgramCardUseQuery q) => _resolve().XiahouHuiCounterspellBan(q);
    internal bool XiahouHuiDamageCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c, bool issued = false) => _resolve().XiahouHuiDamageCandidate(candidate, c, issued);
    internal bool XiahouHuiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().XiahouHuiFirstChild(f, child);
    internal XiahouHuiLossRecordedEvent? XiahouHuiLossCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().XiahouHuiLossCandidate(candidate, c);
    internal bool XiahouHuiNativeResponseBan(ProgramNativeResponseQuery q) => _resolve().XiahouHuiNativeResponseBan(q);
    internal ProgramSkillFrame? XiahouHuiObserverRoot() => _resolve().XiahouHuiObserverRoot();
    internal IReadOnlyList<XiahouHuiCandidateCard> XiahouHuiObtainable(ProgramSkillFrame f, int donor, int? recipient = default) => _resolve().XiahouHuiObtainable(f, donor, recipient);
    internal XiahouHuiBoundEvent? XiahouHuiPartner(int owner, string? instance = default, bool requireCurrent = true) => _resolve().XiahouHuiPartner(owner, instance, requireCurrent);
    internal bool XiahouHuiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().XiahouHuiRecoveryParent(f, recovery);
    internal bool HasXiahouHuiDujun { get => _resolve().HasXiahouHuiDujun; }
    internal bool HasXiahouHuiJikun { get => _resolve().HasXiahouHuiJikun; }
}
