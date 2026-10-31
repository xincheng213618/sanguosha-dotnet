#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeYanAnniversaryEntries
{
    internal bool AdvanceHeYanChildren() => _resolve().AdvanceHeYanChildren();
    internal void AnswerHeYan(PromptChoice choice) => _resolve().AnswerHeYan(choice);
    internal SkillProgramStepOutcome BeginHeYan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHeYan(supplied, op);
    internal bool CanOfferHeYan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferHeYan(c, t, context);
    internal PromptChoice ChooseHeYan(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseHeYan(p, f);
    internal bool ContinueHeYan(long id) => _resolve().ContinueHeYan(id);
    internal bool DrainHeYan(ProgramSkillFrame f) => _resolve().DrainHeYan(f);
    internal void FinishHeYan(ProgramSkillFrame f) => _resolve().FinishHeYan(f);
    internal PromptChoice HeYanChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default) => _resolve().HeYanChoice(f, branch, label, cards);
    internal IReadOnlyList<PromptChoice> HeYanChoices(ProgramSkillFrame f) => _resolve().HeYanChoices(f);
    internal int HeYanChooser(AnniversaryHeYanReceipt r, int owner) => _resolve().HeYanChooser(r, owner);
    internal IReadOnlyList<PromptChoice> HeYanDiscardChoices(ProgramSkillFrame f) => _resolve().HeYanDiscardChoices(f);
    internal bool HeYanDyingObserver() => _resolve().HeYanDyingObserver();
    internal bool HeYanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HeYanFirstChild(f, child);
    internal HeYanHandPlan HeYanFreezeHand(int seat) => _resolve().HeYanFreezeHand(seat);
    internal bool HeYanLegalUseBan(ProgramLegalActionQuery q) => _resolve().HeYanLegalUseBan(q);
    internal bool HeYanMaterialBan(ProgramCardMaterialQuery q) => _resolve().HeYanMaterialBan(q);
    internal bool HeYanMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().HeYanMovementProof(f, effect, p);
    internal bool HeYanNativeResponseBan(ProgramNativeResponseQuery q) => _resolve().HeYanNativeResponseBan(q);
    internal ProgramSkillFrame? HeYanObserverRoot() => _resolve().HeYanObserverRoot();
    internal bool HeYanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().HeYanRecoveryParent(f, recovery);
    internal bool HeYanResponseIsUse(int actor, CardKind kind, bool dying = false) => _resolve().HeYanResponseIsUse(actor, kind, dying);
    internal void HeYanSubmittedUseBan(ProgramSubmittedUseQuery q) => _resolve().HeYanSubmittedUseBan(q);
    internal bool HeYanViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().HeYanViewAsBan(q);
    internal bool IsAnniversaryHeYanHandUseForbidden(int seat, IReadOnlyList<int> ids) => _resolve().IsAnniversaryHeYanHandUseForbidden(seat, ids);
    internal void PayHeYan(ProgramSkillFrame supplied, string kind, int executor, IReadOnlyList<int> ids, CardLocation destination, AnniversaryHeYanStage following, bool discard) => _resolve().PayHeYan(supplied, kind, executor, ids, destination, following, discard);
    internal void PublishHeYan(ProgramSkillFrame supplied) => _resolve().PublishHeYan(supplied);
    internal bool RetainIssuedHeYan(ProgramSkillFrame f) => _resolve().RetainIssuedHeYan(f);
    internal void ReturnHeYanDraw(ProgramSkillFrame f, NativeDrawOutcome draw) => _resolve().ReturnHeYanDraw(f, draw);
    internal bool ReturnHeYanMovement(ProgramSkillFrame f) => _resolve().ReturnHeYanMovement(f);
    internal void ReturnHeYanReveal(ProgramSkillFrame f) => _resolve().ReturnHeYanReveal(f);
    internal void RevealHeYan(ProgramSkillFrame supplied) => _resolve().RevealHeYan(supplied);
}
