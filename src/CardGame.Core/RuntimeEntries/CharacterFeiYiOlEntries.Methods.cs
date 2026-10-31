#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFeiYiOlEntries
{
    internal bool AdvanceFeiYiSubtree() => _resolve().AdvanceFeiYiSubtree();
    internal PromptChoice AiFeiYi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiFeiYi(p, f);
    internal bool AllowsFeiYiYanru(CharacterState actor) => _resolve().AllowsFeiYiYanru(actor);
    internal void AnswerFeiYi(PromptChoice choice) => _resolve().AnswerFeiYi(choice);
    internal SkillProgramStepOutcome BeginFeiYi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginFeiYi(supplied, op);
    internal void BeginFeiYiDiscard(ProgramSkillFrame f) => _resolve().BeginFeiYiDiscard(f);
    internal void BeginFeiYiDraw(ProgramSkillFrame f, bool hezhong) => _resolve().BeginFeiYiDraw(f, hezhong);
    internal void BindFeiYiRankEffects(long id, ProgramTrickContinuation trick) => _resolve().BindFeiYiRankEffects(id, trick);
    internal bool CanOfferFeiYiSingleton(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferFeiYiSingleton(c, t, context);
    internal CardMovementBatchContext CaptureFeiYiSingletonBatch(CardMovementBatchContext batch) => _resolve().CaptureFeiYiSingletonBatch(batch);
    internal void CaptureFeiYiTrickPaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureFeiYiTrickPaymentBatch(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectFeiYiSingletonCandidates(CardMovementBatchContext batch) => _resolve().CollectFeiYiSingletonCandidates(batch);
    internal void CollectWindow_CollectFeiYiSingletonCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectFeiYiSingletonCandidates(batch, result);
    internal void CompleteFeiYi(ProgramSkillFrame f, bool applied) => _resolve().CompleteFeiYi(f, applied);
    internal CardMovementBatchContext CompleteMovementBatchCaptureFeiYiSingletonBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureFeiYiSingletonBatch(batch, completed);
    internal CardMovementBatchContext CompleteMovementBatchCaptureFeiYiTrickPaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureFeiYiTrickPaymentBatch(batch, completed);
    internal void ContinueFeiYi(long id) => _resolve().ContinueFeiYi(id);
    internal bool ContinuePaidFeiYi(ProgramSkillFrame f) => _resolve().ContinuePaidFeiYi(f);
    internal ProgramSkillWindowContext CreateFeiYiSingletonContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => _resolve().CreateFeiYiSingletonContext(frame, c);
    internal FeiYiSingletonTransition? CurrentFeiYiSingleton(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentFeiYiSingleton(c, context);
    internal bool DrainFeiYiChildren(ProgramSkillFrame f) => _resolve().DrainFeiYiChildren(f);
    internal bool ExcludeOrdinaryMovement_IsFeiYiSingletonTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsFeiYiSingletonTrigger(candidate, trigger);
    internal int? FeiYiAcceptedActor(CardUseFrame use) => _resolve().FeiYiAcceptedActor(use);
    internal bool FeiYiCanDiscard(int owner, Card card) => _resolve().FeiYiCanDiscard(owner, card);
    internal IReadOnlyList<PromptChoice> FeiYiChoices(ProgramSkillFrame f) => _resolve().FeiYiChoices(f);
    internal bool FeiYiDirectionAvailable(int owner, bool greater) => _resolve().FeiYiDirectionAvailable(owner, greater);
    internal bool FeiYiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().FeiYiFirstChild(root, child);
    internal ProgramSkillFrame? FeiYiObserverRoot() => _resolve().FeiYiObserverRoot();
    internal bool FeiYiSingletonIsWholeOperation(CardMovementBatchContext batch, FeiYiSingletonTransition transition) => _resolve().FeiYiSingletonIsWholeOperation(batch, transition);
    internal CardConversionSource FeiYiSource(ProgramSkillFrame f) => _resolve().FeiYiSource(f);
    internal bool FeiYiSourceCurrent(ProgramSkillFrame f) => _resolve().FeiYiSourceCurrent(f);
    internal bool FeiYiTrickPaymentMaterialsMatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().FeiYiTrickPaymentMaterialsMatch(use, batch);
    internal bool FeiYiYanruAvailable(CharacterState actor) => _resolve().FeiYiYanruAvailable(actor);
    internal bool HasFeiYiTrickPaymentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _resolve().HasFeiYiTrickPaymentTail(use, continuation);
    internal bool HasUnstartedFeiYiExtraEffect(long id) => _resolve().HasUnstartedFeiYiExtraEffect(id);
    internal bool IsFeiYiExtraEffectRound(long id) => _resolve().IsFeiYiExtraEffectRound(id);
    internal bool IsFeiYiSingletonTrigger(SkillProgramTrigger t) => _resolve().IsFeiYiSingletonTrigger(t);
    internal bool IsFeiYiTrickPaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsFeiYiTrickPaymentBatch(use, batch);
    internal int NativeExtraEffectRoundOrdinal(long id) => _resolve().NativeExtraEffectRoundOrdinal(id);
    internal ExtraCardEffectState? NativeExtraEffectState(CardUseFrame use) => _resolve().NativeExtraEffectState(use);
    internal void PayFeiYiDiscard(ProgramSkillFrame f) => _resolve().PayFeiYiDiscard(f);
    internal void PayFeiYiYanru(ProgramSkillFrame f) => _resolve().PayFeiYiYanru(f);
    internal bool? ProveFeiYiMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveFeiYiMovement(trigger, candidate, context);
    internal void PublishFeiYiChoice(ProgramSkillFrame f) => _resolve().PublishFeiYiChoice(f);
    internal void RecordFeiYiMovement(long id, long before, FeiYiStage stage, int minimum) => _resolve().RecordFeiYiMovement(id, before, stage, minimum);
    internal bool ResumeFeiYi(long id) => _resolve().ResumeFeiYi(id);
    internal bool ReturnFeiYiMovement(ProgramSkillFrame f) => _resolve().ReturnFeiYiMovement(f);
    internal bool TryBeginFeiYiExtraEffectRound(long id, bool afterAttack) => _resolve().TryBeginFeiYiExtraEffectRound(id, afterAttack);
    internal ProgramSkillWindowContext? TryCreateFeiYiSingletonContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateFeiYiSingletonContext(frame, candidate);
    internal void UpdateNativeExtraEffectState(long id, Func<ExtraCardEffectState, ExtraCardEffectState> update) => _resolve().UpdateNativeExtraEffectState(id, update);
    internal bool HasFeiYiHezhong { get => _resolve().HasFeiYiHezhong; }
}
