#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongAnniversaryGodEntries
{
    internal void AddShenPtChainActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> cards, int? selectedPhysicalCardId = default) => _resolve().AddShenPtChainActions(actions, actor, cards, selectedPhysicalCardId);
    internal bool AdvanceShenPtChildren() => _resolve().AdvanceShenPtChildren();
    internal SkillProgramStepOutcome BeginShenPt(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenPt(supplied, op);
    internal void CompleteShenPangTongKunyuDeath(DeathFrame supplied) => _resolve().CompleteShenPangTongKunyuDeath(supplied);
    internal void ContinueShenPt(long id) => _resolve().ContinueShenPt(id);
    internal bool DrainShenPt(long id) => _resolve().DrainShenPt(id);
    internal void FinishShenPt(ProgramSkillFrame f) => _resolve().FinishShenPt(f);
    internal bool IsShenPangTongHandDiscardForbidden(int actor, Card card, CardLocation from, OwnedCardMoveIntent intent) => _resolve().IsShenPangTongHandDiscardForbidden(actor, card, from, intent);
    internal bool KeepPaidShenPt(ProgramSkillFrame f) => _resolve().KeepPaidShenPt(f);
    internal void RequireShenPtRevival(DeathFrame death, ShenPangTongRevivalReceipt r) => _resolve().RequireShenPtRevival(death, r);
    internal bool ResumeShenPt(long id) => _resolve().ResumeShenPt(id);
    internal void ReturnShenPtDraw(ProgramSkillFrame supplied, NativeDrawOutcome o) => _resolve().ReturnShenPtDraw(supplied, o);
    internal bool ReturnShenPtMovement(ProgramSkillFrame f) => _resolve().ReturnShenPtMovement(f);
    internal GameEngine.ProgramCardIdentityMatch? ShenPangTongLuansuoIdentity(CharacterState holder, Card card) => _resolve().ShenPangTongLuansuoIdentity(holder, card);
    internal bool ShenPtActualDamageTarget(ProgramSkillFrame f, ShenPangTongReceipt r, AttackAttemptState a) => _resolve().ShenPtActualDamageTarget(f, r, a);
    internal bool ShenPtDying() => _resolve().ShenPtDying();
    internal bool ShenPtEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().ShenPtEligible(c, t, w);
    internal bool ShenPtFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShenPtFirstChild(f, child);
    internal bool ShenPtMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().ShenPtMovement(f, effect, p);
    internal ProgramSkillFrame? ShenPtObserverRoot() => _resolve().ShenPtObserverRoot();
    internal bool ShenPtPaidRemoval(long frame, long death, int card, long before, long after, long batch) => _resolve().ShenPtPaidRemoval(frame, death, card, before, after, batch);
    internal ProgramCardTriggerWindowFrame? ShenPtUse(ProgramSkillWindowContext w) => _resolve().ShenPtUse(w);
    internal bool TryBeginShenPangTongKunyuDeath(DeathFrame supplied) => _resolve().TryBeginShenPangTongKunyuDeath(supplied);
    internal CharacterState? ShenPtLuanOwner { get => _resolve().ShenPtLuanOwner; }
}
