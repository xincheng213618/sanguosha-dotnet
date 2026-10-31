#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeYanAnniversaryEntries
{
    internal bool AdvanceHeYanChildren() => _resolve().AdvanceHeYanChildren();
    internal void AnswerHeYan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHeYan(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHeYan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHeYan(supplied, op);
    internal bool CanOfferHeYan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferHeYan(c, t, context);
    internal global::CardGame.Core.PromptChoice ChooseHeYan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseHeYan(p, f);
    internal bool ContinueHeYan(long id) => _resolve().ContinueHeYan(id);
    internal bool DrainHeYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHeYan(f);
    internal void FinishHeYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHeYan(f);
    internal bool HeYanBeforeDamageObserver(long id) => _resolve().HeYanBeforeDamageObserver(id);
    internal global::CardGame.Core.PromptChoice HeYanChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default) => _resolve().HeYanChoice(f, branch, label, cards);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HeYanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HeYanChoices(f);
    internal int HeYanChooser(global::CardGame.Core.AnniversaryHeYanReceipt r, int owner) => _resolve().HeYanChooser(r, owner);
    internal bool HeYanDamageObserver(long id) => _resolve().HeYanDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HeYanDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HeYanDiscardChoices(f);
    internal bool HeYanDyingObserver() => _resolve().HeYanDyingObserver();
    internal bool HeYanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HeYanFirstChild(f, child);
    internal global::CardGame.Core.HeYanHandPlan HeYanFreezeHand(int seat) => _resolve().HeYanFreezeHand(seat);
    internal bool HeYanLegalUseBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().HeYanLegalUseBan(q);
    internal bool HeYanMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().HeYanMaterialBan(q);
    internal bool HeYanMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HeYanMovementProof(f, effect, p);
    internal bool HeYanNativeResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().HeYanNativeResponseBan(q);
    internal global::CardGame.Core.ProgramSkillFrame? HeYanObserverRoot() => _resolve().HeYanObserverRoot();
    internal bool HeYanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().HeYanRecoveryParent(f, recovery);
    internal bool HeYanResponseIsUse(int actor, global::CardGame.Core.CardKind kind, bool dying = false) => _resolve().HeYanResponseIsUse(actor, kind, dying);
    internal void HeYanSubmittedUseBan(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().HeYanSubmittedUseBan(q);
    internal bool HeYanViewAsBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().HeYanViewAsBan(q);
    internal bool IsAnniversaryHeYanHandUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().IsAnniversaryHeYanHandUseForbidden(seat, ids);
    internal void PayHeYan(global::CardGame.Core.ProgramSkillFrame supplied, string kind, int executor, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.AnniversaryHeYanStage following, bool discard) => _resolve().PayHeYan(supplied, kind, executor, ids, destination, following, discard);
    internal void PublishHeYan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PublishHeYan(supplied);
    internal bool RetainIssuedHeYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainIssuedHeYan(f);
    internal void ReturnHeYanDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnHeYanDraw(f, draw);
    internal bool ReturnHeYanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHeYanMovement(f);
    internal void ReturnHeYanReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHeYanReveal(f);
    internal void RevealHeYan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RevealHeYan(supplied);
}
