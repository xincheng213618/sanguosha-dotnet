#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXinPiOlEntries
{
    internal SkillProgramStepOutcome BeginXinPi(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().BeginXinPi(supplied, e);
    internal bool CanContinueXinPi(ProgramSkillFrame f) => _resolve().CanContinueXinPi(f);
    internal bool CanOfferXinPi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferXinPi(c, t, w);
    internal void CaptureXinPiUseMovementBatch(CardMovementBatchContext batch) => _resolve().CaptureXinPiUseMovementBatch(batch);
    internal IReadOnlyList<int> ChijieClaimable(CardUseFrame use) => _resolve().ChijieClaimable(use);
    internal CardUseFrame? ChijieDamageUse(int owner, ProgramSkillWindowContext w, scoped out CompletedUndamagedUseDamageRecordedEvent damage) => _resolve().ChijieDamageUse(owner, w, out damage);
    internal bool ChijieUndamagedIncoming(int owner, ProgramSkillWindowContext w, scoped out CardUseFrame use) => _resolve().ChijieUndamagedIncoming(owner, w, out use);
    internal void CollectIssuedYinjuCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedYinjuCandidates(action, window, result);
    internal IEnumerable<BeforeDamageProgramCandidate> CollectIssuedYinjuCandidates2(int sourceSeat, int targetSeat, bool rangeChainOnly) => _resolve().CollectIssuedYinjuCandidates(sourceSeat, targetSeat, rangeChainOnly);
    internal YinjuTurnRelation? CurrentYinju(int owner) => _resolve().CurrentYinju(owner);
    internal bool DrainXinPi(ProgramSkillFrame f) => _resolve().DrainXinPi(f);
    internal CardUseFrame? ExactXinPiNativeWindow(ProgramSkillWindowContext w, bool completed) => _resolve().ExactXinPiNativeWindow(w, completed);
    internal bool ExactYinjuBeforeCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context, scoped out YinjuTurnRelation relation, scoped out BeforeDamageProgramWindowFrame window) => _resolve().ExactYinjuBeforeCandidate(c, context, out relation, out window);
    internal bool ExactYinjuUseCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context, scoped out YinjuTurnRelation relation, scoped out CardUseFrame use) => _resolve().ExactYinjuUseCandidate(c, context, out relation, out use);
    internal void FinishXinPi(ProgramSkillFrame f) => _resolve().FinishXinPi(f);
    internal bool HasIssuedYinjuCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().HasIssuedYinjuCandidate(c, w);
    internal bool IsXinPiPhysicalClaim(long useId, int cardId) => _resolve().IsXinPiPhysicalClaim(useId, cardId);
    internal bool IsXinPiRecoveryCompletion(ProgramSkillFrame f, RecoveryAttempt attempt) => _resolve().IsXinPiRecoveryCompletion(f, attempt);
    internal bool IsXinPiRemainingCardDamageIneffective(GameEngine.IDamageAttempt attack) => _resolve().IsXinPiRemainingCardDamageIneffective(attack);
    internal bool IsXinPiUseIneffective(CardUseFrame use) => _resolve().IsXinPiUseIneffective(use);
    internal void ObserveXinPiEvent(IGameEvent payload) => _resolve().ObserveXinPiEvent(payload);
    internal void PayYinjuDraw(ProgramSkillFrame f, int seat) => _resolve().PayYinjuDraw(f, seat);
    internal bool ResumeXinPi(long id) => _resolve().ResumeXinPi(id);
    internal bool ReturnXinPiMovement(ProgramSkillFrame f) => _resolve().ReturnXinPiMovement(f);
    internal bool TryAdvanceXinPiSubtree() => _resolve().TryAdvanceXinPiSubtree();
    internal CardMovementRecord? XinPiExactTunanPayment(CardUseFrame use, int id) => _resolve().XinPiExactTunanPayment(use, id);
    internal bool XinPiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().XinPiFinalizedAfterCategoryTargetWindow(query);
    internal bool XinPiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().XinPiFirstChild(root, child);
    internal bool XinPiMaterialStillOriginal(CardUseFrame use, XinPiMaterialInvoice invoice) => _resolve().XinPiMaterialStillOriginal(use, invoice);
    internal ProgramSkillFrame? XinPiObserverRoot() => _resolve().XinPiObserverRoot();
    internal bool XinPiSourceCurrent(ProgramSkillFrame f) => _resolve().XinPiSourceCurrent(f);
    internal bool YinjuRelationValid(YinjuTurnRelation r) => _resolve().YinjuRelationValid(r);
    internal bool HasXinPiCapability { get => _resolve().HasXinPiCapability; }
}
