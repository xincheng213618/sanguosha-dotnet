#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaMiaoAnniversaryEntries
{
    internal bool AdvanceMaMiaoChildren() => _resolve().AdvanceMaMiaoChildren();
    internal void AnswerMaMiao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMaMiao(choice);
    internal void AssertMaMiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMaMiao(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMaMiao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMaMiao(supplied, op);
    internal bool CanOfferMaMiao(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferMaMiao(c, t, w);
    internal void CaptureAnniversaryMaMiaoGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureAnniversaryMaMiaoGainBatch(batch);
    internal void CaptureMaMiaoTransferProducer(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMaMiaoTransferProducer(batch);
    internal global::CardGame.Core.PromptChoice ChooseMaMiao(global::CardGame.Core.PendingDecision p) => _resolve().ChooseMaMiao(p);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectAnniversaryMaMiaoGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectAnniversaryMaMiaoGainCandidates(batch);
    internal void CollectWindow_CollectAnniversaryMaMiaoGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectAnniversaryMaMiaoGainCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureAnniversaryMaMiaoGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureAnniversaryMaMiaoGainBatch(batch, completed);
    internal void ContinueMaMiao(long id) => _resolve().ContinueMaMiao(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateAnniversaryMaMiaoGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateAnniversaryMaMiaoGainContext(parent, c);
    internal bool DrainMaMiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMaMiao(f);
    internal bool ExcludeOrdinaryMovement_IsAnniversaryMaMiaoCongfengTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsAnniversaryMaMiaoCongfengTrigger(candidate, trigger);
    internal void FinishMaMiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMaMiao(f);
    internal bool HasAnniversaryMaMiaoPrompt() => _resolve().HasAnniversaryMaMiaoPrompt();
    internal bool MaMiaoBeforeDamageObserver(long id) => _resolve().MaMiaoBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MaMiaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaMiaoChoices(f);
    internal int MaMiaoChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaMiaoChooser(f);
    internal bool MaMiaoDamageObserver(long id) => _resolve().MaMiaoDamageObserver(id);
    internal bool MaMiaoDying() => _resolve().MaMiaoDying();
    internal bool MaMiaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MaMiaoFirstChild(f, child);
    internal global::System.Collections.Generic.IEnumerable<(int Index, global::CardGame.Core.CardMovementRecord Move, int Recipient, int Holder)> MaMiaoForeignGains(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().MaMiaoForeignGains(batch, owner);
    internal global::CardGame.Core.CardLocation MaMiaoGainOriginalSource(global::CardGame.Core.CardMovementRecord movement, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().MaMiaoGainOriginalSource(movement, batch);
    internal global::CardGame.Core.AnniversaryMaMiaoGainQualifiedEvent? MaMiaoGainQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().MaMiaoGainQualification(c, w);
    internal bool MaMiaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MaMiaoMovement(f, e, p);
    internal bool MaMiaoObserverEdge(int i) => _resolve().MaMiaoObserverEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? MaMiaoObserverRoot() => _resolve().MaMiaoObserverRoot();
    internal global::CardGame.Core.AnniversaryMaMiaoEntity[] MaMiaoObtainPool(int recipient, int holder) => _resolve().MaMiaoObtainPool(recipient, holder);
    internal bool MaMiaoOriginalParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaMiaoOriginalParent(f);
    internal bool MaMiaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MaMiaoRecoveryParent(f, recovery);
    internal void PayMaMiaoGain(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.AnniversaryMaMiaoEntity card) => _resolve().PayMaMiaoGain(f, card);
    internal bool? ProveMaMiaoMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveMaMiaoMovement(trigger, candidate, context);
    internal void PublishMaMiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMaMiao(f);
    internal void RecordMaMiaoMovement(long id, string mode, int count, long before) => _resolve().RecordMaMiaoMovement(id, mode, count, before);
    internal bool ResumeMaMiao(long id) => _resolve().ResumeMaMiao(id);
    internal bool RetainMaMiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainMaMiao(f);
    internal void ReturnMaMiaoDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMaMiaoDraw(f, outcome);
    internal bool ReturnMaMiaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMaMiaoMovement(f);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateAnniversaryMaMiaoGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateAnniversaryMaMiaoGainContext(frame, candidate);
    internal bool ValidMaMiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMaMiao(f);
    internal bool HasAnniversaryMaMiaoGains { get => _resolve().HasAnniversaryMaMiaoGains; }
}
