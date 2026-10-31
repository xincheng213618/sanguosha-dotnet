#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanDangMouMobileEntries
{
    internal bool AdvanceMouHanDangChildren() => _resolve().AdvanceMouHanDangChildren();
    internal PromptChoice AiMouHanDang(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouHanDang(p, f);
    internal void AnswerMouHanDang(PromptChoice selected) => _resolve().AnswerMouHanDang(selected);
    internal bool CanOfferMouHanDangGongqi(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouHanDangGongqi(gate);
    internal bool CanStartMouHanDangJiefan(CharacterState owner) => _resolve().CanStartMouHanDangJiefan(owner);
    internal bool ContinueAcceptedMouHanDang(ProgramSkillFrame f) => _resolve().ContinueAcceptedMouHanDang(f);
    internal void ContinueMouHanDang(long id) => _resolve().ContinueMouHanDang(id);
    internal CardActionContext? CurrentMouHanDangRespondedAction() => _resolve().CurrentMouHanDangRespondedAction();
    internal void FinishMouHanDang(ProgramSkillFrame f) => _resolve().FinishMouHanDang(f);
    internal bool IsMouHanDangDying() => _resolve().IsMouHanDangDying();
    internal bool IsMouHanDangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsMouHanDangMovement(f, e, p);
    internal bool IsMouHanDangNativeResponseAllowed(CharacterState provider, Card card, bool dyingUse = false) => _resolve().IsMouHanDangNativeResponseAllowed(provider, card, dyingUse);
    internal bool IsMouHanDangPublishedResponseAllowed(CharacterState provider, Card card, CardConversionSource? source, bool forResponse, bool dyingUse = false) => _resolve().IsMouHanDangPublishedResponseAllowed(provider, card, source, forResponse, dyingUse);
    internal bool IsMouHanDangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouHanDangRecoveryParent(f, recovery);
    internal bool IsMouHanDangResponseAvailable(CharacterState provider, CardKind kind, IReadOnlyList<int> ids, bool dyingUse = false) => _resolve().IsMouHanDangResponseAvailable(provider, kind, ids, dyingUse);
    internal bool IsMouHanDangSilverLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().IsMouHanDangSilverLionChild(f, child);
    internal IEnumerable<MouHanDangGongqiGrantedEvent> MouHanDangActionRestrictions(long action) => _resolve().MouHanDangActionRestrictions(action);
    internal PromptChoice MouHanDangChoice(ProgramSkillFrame f, string branch, string label, int? card = default) => _resolve().MouHanDangChoice(f, branch, label, card);
    internal IReadOnlyList<PromptChoice> MouHanDangChoices(ProgramSkillFrame f) => _resolve().MouHanDangChoices(f);
    internal Card[] MouHanDangDiscardCards(int seat) => _resolve().MouHanDangDiscardCards(seat);
    internal bool MouHanDangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHanDangFirstChild(f, child);
    internal bool MouHanDangJiefanDisabled(int owner) => _resolve().MouHanDangJiefanDisabled(owner);
    internal bool MouHanDangObserverEdge(int n) => _resolve().MouHanDangObserverEdge(n);
    internal ProgramSkillFrame? MouHanDangObserverRoot() => _resolve().MouHanDangObserverRoot();
    internal bool MouHanDangObserverSuffix(int index) => _resolve().MouHanDangObserverSuffix(index);
    internal int MouHanDangPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouHanDangPhaseUses(owner, phase);
    internal bool MouHanDangPlayStartParent(ProgramSkillFrame f) => _resolve().MouHanDangPlayStartParent(f);
    internal int MouHanDangProjectedUses(int owner, string skill, int fallback) => _resolve().MouHanDangProjectedUses(owner, skill, fallback);
    internal IReadOnlyList<int> MouHanDangRangeSeats(int target, int first) => _resolve().MouHanDangRangeSeats(target, first);
    internal void ObserveMouHanDang(IGameEvent payload) => _resolve().ObserveMouHanDang(payload);
    internal void PublishMouHanDang(ProgramSkillFrame f) => _resolve().PublishMouHanDang(f);
    internal bool ResumeMouHanDang(long id) => _resolve().ResumeMouHanDang(id);
    internal void ReturnMouHanDangDraw(ProgramSkillFrame f, NativeDrawOutcome d) => _resolve().ReturnMouHanDangDraw(f, d);
    internal bool ReturnMouHanDangMovement(ProgramSkillFrame f) => _resolve().ReturnMouHanDangMovement(f);
    internal void SealMouHanDangMovements(long id, long before) => _resolve().SealMouHanDangMovements(id, before);
    internal void SignMouHanDangUseRestrictions(CardActionContext a) => _resolve().SignMouHanDangUseRestrictions(a);
    internal SkillProgramStepOutcome StartMouHanDang(ProgramSkillFrame supplied, bool gongqi) => _resolve().StartMouHanDang(supplied, gongqi);
    internal bool ValidMouHanDang(ProgramSkillFrame f) => _resolve().ValidMouHanDang(f);
    internal bool HasMouHanDangCapability { get => _resolve().HasMouHanDangCapability; }
}
