#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXinPiOlEntries
{
    internal void AssertXinPiFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertXinPiFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXinPi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginXinPi(supplied, e);
    internal bool CanContinueXinPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueXinPi(f);
    internal bool CanOfferXinPi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferXinPi(c, t, w);
    internal void CaptureXinPiUseMovementBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureXinPiUseMovementBatch(batch);
    internal global::System.Collections.Generic.IReadOnlyList<int> ChijieClaimable(global::CardGame.Core.CardUseFrame use) => _resolve().ChijieClaimable(use);
    internal global::CardGame.Core.CardUseFrame? ChijieDamageUse(int owner, global::CardGame.Core.ProgramSkillWindowContext w, scoped out global::CardGame.Core.CompletedUndamagedUseDamageRecordedEvent damage) => _resolve().ChijieDamageUse(owner, w, out damage);
    internal bool ChijieUndamagedIncoming(int owner, global::CardGame.Core.ProgramSkillWindowContext w, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().ChijieUndamagedIncoming(owner, w, out use);
    internal void CollectIssuedYinjuCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedYinjuCandidates(action, window, result);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectIssuedYinjuCandidates2(int sourceSeat, int targetSeat, bool rangeChainOnly) => _resolve().CollectIssuedYinjuCandidates(sourceSeat, targetSeat, rangeChainOnly);
    internal global::CardGame.Core.YinjuTurnRelation? CurrentYinju(int owner) => _resolve().CurrentYinju(owner);
    internal bool DrainXinPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainXinPi(f);
    internal global::CardGame.Core.CardUseFrame? ExactXinPiNativeWindow(global::CardGame.Core.ProgramSkillWindowContext w, bool completed) => _resolve().ExactXinPiNativeWindow(w, completed);
    internal bool ExactYinjuBeforeCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.YinjuTurnRelation relation, scoped out global::CardGame.Core.BeforeDamageProgramWindowFrame window) => _resolve().ExactYinjuBeforeCandidate(c, context, out relation, out window);
    internal bool ExactYinjuUseCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.YinjuTurnRelation relation, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().ExactYinjuUseCandidate(c, context, out relation, out use);
    internal void FinishXinPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXinPi(f);
    internal bool HasIssuedYinjuCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HasIssuedYinjuCandidate(c, w);
    internal bool HasXinPiBeforeDamageObserver(long id) => _resolve().HasXinPiBeforeDamageObserver(id);
    internal bool HasXinPiCardObserver(long useId) => _resolve().HasXinPiCardObserver(useId);
    internal bool HasXinPiDamageObserver(long id) => _resolve().HasXinPiDamageObserver(id);
    internal bool IsXinPiDying() => _resolve().IsXinPiDying();
    internal bool IsXinPiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsXinPiMovement(f, e, p);
    internal bool IsXinPiPhysicalClaim(long useId, int cardId) => _resolve().IsXinPiPhysicalClaim(useId, cardId);
    internal bool IsXinPiRecoveryCompletion(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryAttempt attempt) => _resolve().IsXinPiRecoveryCompletion(f, attempt);
    internal bool IsXinPiRemainingCardDamageIneffective(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().IsXinPiRemainingCardDamageIneffective(attack);
    internal bool IsXinPiUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsXinPiUseIneffective(use);
    internal void ObserveXinPiEvent(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveXinPiEvent(payload);
    internal void PayYinjuDraw(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().PayYinjuDraw(f, seat);
    internal bool ResumeXinPi(long id) => _resolve().ResumeXinPi(id);
    internal bool ReturnXinPiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXinPiMovement(f);
    internal bool TryAdvanceXinPiSubtree() => _resolve().TryAdvanceXinPiSubtree();
    internal global::CardGame.Core.CardMovementRecord? XinPiExactTunanPayment(global::CardGame.Core.CardUseFrame use, int id) => _resolve().XinPiExactTunanPayment(use, id);
    internal bool XinPiFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().XinPiFinalizedAfterCategoryTargetWindow(query);
    internal bool XinPiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().XinPiFirstChild(root, child);
    internal bool XinPiMaterialStillOriginal(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.XinPiMaterialInvoice invoice) => _resolve().XinPiMaterialStillOriginal(use, invoice);
    internal global::CardGame.Core.ProgramSkillFrame? XinPiObserverRoot() => _resolve().XinPiObserverRoot();
    internal bool XinPiSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XinPiSourceCurrent(f);
    internal bool YinjuRelationValid(global::CardGame.Core.YinjuTurnRelation r) => _resolve().YinjuRelationValid(r);
    internal bool HasXinPiCapability { get => _resolve().HasXinPiCapability; }
}
