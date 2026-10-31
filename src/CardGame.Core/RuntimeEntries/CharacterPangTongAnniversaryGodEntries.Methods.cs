#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongAnniversaryGodEntries
{
    internal void AddShenPtChainActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int? selectedPhysicalCardId = default) => _resolve().AddShenPtChainActions(actions, actor, cards, selectedPhysicalCardId);
    internal bool AdvanceShenPtChildren() => _resolve().AdvanceShenPtChildren();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenPt(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenPt(supplied, op);
    internal void CompleteShenPangTongKunyuDeath(global::CardGame.Core.DeathFrame supplied) => _resolve().CompleteShenPangTongKunyuDeath(supplied);
    internal void ContinueShenPt(long id) => _resolve().ContinueShenPt(id);
    internal bool DrainShenPt(long id) => _resolve().DrainShenPt(id);
    internal void FinishShenPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenPt(f);
    internal bool IsShenPangTongHandDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => _resolve().IsShenPangTongHandDiscardForbidden(actor, card, from, intent);
    internal bool KeepPaidShenPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidShenPt(f);
    internal void RequireShenPtRevival(global::CardGame.Core.DeathFrame death, global::CardGame.Core.ShenPangTongRevivalReceipt r) => _resolve().RequireShenPtRevival(death, r);
    internal bool ResumeShenPt(long id) => _resolve().ResumeShenPt(id);
    internal void ReturnShenPtDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnShenPtDraw(supplied, o);
    internal bool ReturnShenPtMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenPtMovement(f);
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShenPangTongLuansuoIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card) => _resolve().ShenPangTongLuansuoIdentity(holder, card);
    internal bool ShenPtActualDamageTarget(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShenPangTongReceipt r, global::CardGame.Core.AttackAttemptState a) => _resolve().ShenPtActualDamageTarget(f, r, a);
    internal bool ShenPtDying() => _resolve().ShenPtDying();
    internal bool ShenPtEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ShenPtEligible(c, t, w);
    internal bool ShenPtFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShenPtFirstChild(f, child);
    internal bool ShenPtMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShenPtMovement(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShenPtObserverRoot() => _resolve().ShenPtObserverRoot();
    internal bool ShenPtPaidRemoval(long frame, long death, int card, long before, long after, long batch) => _resolve().ShenPtPaidRemoval(frame, death, card, before, after, batch);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? ShenPtUse(global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ShenPtUse(w);
    internal bool TryBeginShenPangTongKunyuDeath(global::CardGame.Core.DeathFrame supplied) => _resolve().TryBeginShenPangTongKunyuDeath(supplied);
    internal global::CardGame.Core.CharacterState? ShenPtLuanOwner { get => _resolve().ShenPtLuanOwner; }
}
