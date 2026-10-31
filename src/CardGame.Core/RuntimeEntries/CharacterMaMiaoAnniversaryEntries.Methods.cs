#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaMiaoAnniversaryEntries
{
    internal bool AdvanceMaMiaoChildren() => _resolve().AdvanceMaMiaoChildren();
    internal void AnswerMaMiao(PromptChoice choice) => _resolve().AnswerMaMiao(choice);
    internal SkillProgramStepOutcome BeginMaMiao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMaMiao(supplied, op);
    internal bool CanOfferMaMiao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferMaMiao(c, t, w);
    internal void CaptureAnniversaryMaMiaoGainBatch(CardMovementBatchContext batch) => _resolve().CaptureAnniversaryMaMiaoGainBatch(batch);
    internal void CaptureMaMiaoTransferProducer(CardMovementBatchContext batch) => _resolve().CaptureMaMiaoTransferProducer(batch);
    internal PromptChoice ChooseMaMiao(PendingDecision p) => _resolve().ChooseMaMiao(p);
    internal IEnumerable<ProgramTriggerCandidate> CollectAnniversaryMaMiaoGainCandidates(CardMovementBatchContext batch) => _resolve().CollectAnniversaryMaMiaoGainCandidates(batch);
    internal void CollectWindow_CollectAnniversaryMaMiaoGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectAnniversaryMaMiaoGainCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureAnniversaryMaMiaoGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureAnniversaryMaMiaoGainBatch(batch, completed);
    internal void ContinueMaMiao(long id) => _resolve().ContinueMaMiao(id);
    internal ProgramSkillWindowContext CreateAnniversaryMaMiaoGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c) => _resolve().CreateAnniversaryMaMiaoGainContext(parent, c);
    internal bool DrainMaMiao(ProgramSkillFrame f) => _resolve().DrainMaMiao(f);
    internal bool ExcludeOrdinaryMovement_IsAnniversaryMaMiaoCongfengTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsAnniversaryMaMiaoCongfengTrigger(candidate, trigger);
    internal void FinishMaMiao(ProgramSkillFrame f) => _resolve().FinishMaMiao(f);
    internal IReadOnlyList<PromptChoice> MaMiaoChoices(ProgramSkillFrame f) => _resolve().MaMiaoChoices(f);
    internal int MaMiaoChooser(ProgramSkillFrame f) => _resolve().MaMiaoChooser(f);
    internal bool MaMiaoDying() => _resolve().MaMiaoDying();
    internal bool MaMiaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MaMiaoFirstChild(f, child);
    internal IEnumerable<(int Index, CardMovementRecord Move, int Recipient, int Holder)> MaMiaoForeignGains(CardMovementBatchContext batch, int owner) => _resolve().MaMiaoForeignGains(batch, owner);
    internal CardLocation MaMiaoGainOriginalSource(CardMovementRecord movement, CardMovementBatchContext batch) => _resolve().MaMiaoGainOriginalSource(movement, batch);
    internal AnniversaryMaMiaoGainQualifiedEvent? MaMiaoGainQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().MaMiaoGainQualification(c, w);
    internal bool MaMiaoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().MaMiaoMovement(f, e, p);
    internal bool MaMiaoObserverEdge(int i) => _resolve().MaMiaoObserverEdge(i);
    internal ProgramSkillFrame? MaMiaoObserverRoot() => _resolve().MaMiaoObserverRoot();
    internal AnniversaryMaMiaoEntity[] MaMiaoObtainPool(int recipient, int holder) => _resolve().MaMiaoObtainPool(recipient, holder);
    internal bool MaMiaoOriginalParent(ProgramSkillFrame f) => _resolve().MaMiaoOriginalParent(f);
    internal bool MaMiaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MaMiaoRecoveryParent(f, recovery);
    internal void PayMaMiaoGain(ProgramSkillFrame f, AnniversaryMaMiaoEntity card) => _resolve().PayMaMiaoGain(f, card);
    internal bool? ProveMaMiaoMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveMaMiaoMovement(trigger, candidate, context);
    internal void PublishMaMiao(ProgramSkillFrame f) => _resolve().PublishMaMiao(f);
    internal void RecordMaMiaoMovement(long id, string mode, int count, long before) => _resolve().RecordMaMiaoMovement(id, mode, count, before);
    internal bool ResumeMaMiao(long id) => _resolve().ResumeMaMiao(id);
    internal bool RetainMaMiao(ProgramSkillFrame f) => _resolve().RetainMaMiao(f);
    internal void ReturnMaMiaoDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMaMiaoDraw(f, outcome);
    internal bool ReturnMaMiaoMovement(ProgramSkillFrame f) => _resolve().ReturnMaMiaoMovement(f);
    internal ProgramSkillWindowContext? TryCreateAnniversaryMaMiaoGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateAnniversaryMaMiaoGainContext(frame, candidate);
    internal bool ValidMaMiao(ProgramSkillFrame f) => _resolve().ValidMaMiao(f);
    internal bool HasAnniversaryMaMiaoGains { get => _resolve().HasAnniversaryMaMiaoGains; }
}
