#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanDangMouMobileEntries
{
    internal bool AdvanceMouHanDangChildren() => _resolve().AdvanceMouHanDangChildren();
    internal global::CardGame.Core.PromptChoice AiMouHanDang(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouHanDang(p, f);
    internal void AnswerMouHanDang(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouHanDang(selected);
    internal bool CanOfferMouHanDangGongqi(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouHanDangGongqi(gate);
    internal bool CanStartMouHanDangJiefan(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouHanDangJiefan(owner);
    internal bool ContinueAcceptedMouHanDang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedMouHanDang(f);
    internal void ContinueMouHanDang(long id) => _resolve().ContinueMouHanDang(id);
    internal global::CardGame.Core.CardActionContext? CurrentMouHanDangRespondedAction() => _resolve().CurrentMouHanDangRespondedAction();
    internal void FinishMouHanDang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouHanDang(f);
    internal bool IsMouHanDangDying() => _resolve().IsMouHanDangDying();
    internal bool IsMouHanDangMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouHanDangMovement(f, e, p);
    internal bool IsMouHanDangNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, bool dyingUse = false) => _resolve().IsMouHanDangNativeResponseAllowed(provider, card, dyingUse);
    internal bool IsMouHanDangPublishedResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool forResponse, bool dyingUse = false) => _resolve().IsMouHanDangPublishedResponseAllowed(provider, card, source, forResponse, dyingUse);
    internal bool IsMouHanDangRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouHanDangRecoveryParent(f, recovery);
    internal bool IsMouHanDangResponseAvailable(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids, bool dyingUse = false) => _resolve().IsMouHanDangResponseAvailable(provider, kind, ids, dyingUse);
    internal bool IsMouHanDangSilverLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().IsMouHanDangSilverLionChild(f, child);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouHanDangGongqiGrantedEvent> MouHanDangActionRestrictions(long action) => _resolve().MouHanDangActionRestrictions(action);
    internal global::CardGame.Core.PromptChoice MouHanDangChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default) => _resolve().MouHanDangChoice(f, branch, label, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouHanDangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHanDangChoices(f);
    internal global::CardGame.Core.Card[] MouHanDangDiscardCards(int seat) => _resolve().MouHanDangDiscardCards(seat);
    internal bool MouHanDangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouHanDangFirstChild(f, child);
    internal bool MouHanDangJiefanDisabled(int owner) => _resolve().MouHanDangJiefanDisabled(owner);
    internal bool MouHanDangObserverEdge(int n) => _resolve().MouHanDangObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouHanDangObserverRoot() => _resolve().MouHanDangObserverRoot();
    internal bool MouHanDangObserverSuffix(int index) => _resolve().MouHanDangObserverSuffix(index);
    internal int MouHanDangPhaseUses(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouHanDangPhaseUses(owner, phase);
    internal bool MouHanDangPlayStartParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHanDangPlayStartParent(f);
    internal int MouHanDangProjectedUses(int owner, string skill, int fallback) => _resolve().MouHanDangProjectedUses(owner, skill, fallback);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouHanDangRangeSeats(int target, int first) => _resolve().MouHanDangRangeSeats(target, first);
    internal void ObserveMouHanDang(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouHanDang(payload);
    internal void PublishMouHanDang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouHanDang(f);
    internal bool ResumeMouHanDang(long id) => _resolve().ResumeMouHanDang(id);
    internal void ReturnMouHanDangDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnMouHanDangDraw(f, d);
    internal bool ReturnMouHanDangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouHanDangMovement(f);
    internal void SealMouHanDangMovements(long id, long before) => _resolve().SealMouHanDangMovements(id, before);
    internal void SignMouHanDangUseRestrictions(global::CardGame.Core.CardActionContext a) => _resolve().SignMouHanDangUseRestrictions(a);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouHanDang(global::CardGame.Core.ProgramSkillFrame supplied, bool gongqi) => _resolve().StartMouHanDang(supplied, gongqi);
    internal bool ValidMouHanDang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouHanDang(f);
    internal bool HasMouHanDangCapability { get => _resolve().HasMouHanDangCapability; }
}
