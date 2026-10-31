#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangMouEntries
{
    internal bool CanStartMouZhuge(CharacterState owner, ProgramExecutionPlan plan) => _resolve().CanStartMouZhuge(owner, plan);
    internal void CaptureMouWujingAllowance(long batchId, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMouWujingAllowance(batchId, movements);
    internal void CaptureMovementBatchFactsCaptureMouWujingAllowance(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureMouWujingAllowance(batch, movements);
    internal bool CompleteMouZhijueNativeAttackReturn() => _resolve().CompleteMouZhijueNativeAttackReturn();
    internal bool ContinueMouZhijueSimpleUse(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueMouZhijueSimpleUse(id, continuation);
    internal void ContinueMouZhitianFirePayment(long id) => _resolve().ContinueMouZhitianFirePayment(id);
    internal SkillProgramStepOutcome ExecuteMouZhuge(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ExecuteMouZhuge(frame, effect);
    internal bool FinishMouZhijueZeroAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouZhijueZeroAttack(attack, out paused);
    internal bool HasMouOperation(CharacterState owner, SkillProgramEffectOp op) => _resolve().HasMouOperation(owner, op);
    internal bool HasMouWujingAllowance(CharacterState owner) => _resolve().HasMouWujingAllowance(owner);
    internal bool IsMouZhijueUse(long id) => _resolve().IsMouZhijueUse(id);
    internal bool IsMouZhijueZeroUse(long id) => _resolve().IsMouZhijueZeroUse(id);
    internal bool IsZeroEntityUse(long id) => _resolve().IsZeroEntityUse(id);
    internal void IssueMouZhijueUse(ProgramSkillFrame frame, LegalAction action, GameEngine.ProgramOrdinaryTrickUseOption? trick, Card card, IReadOnlyList<int> physicalIds) => _resolve().IssueMouZhijueUse(frame, action, trick, card, physicalIds);
    internal bool MatchesMouZhijueUseAction(long id, CardActionContext action, int cardId) => _resolve().MatchesMouZhijueUseAction(id, action, cardId);
    internal int[] MatchingMouWujingIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingMouWujingIndexes(batch, candidate);
    internal bool MouDiscardNotFromUse(CardMovementRecord move) => _resolve().MouDiscardNotFromUse(move);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> MouYangOptions(CharacterState owner) => _resolve().MouYangOptions(owner);
    internal IReadOnlyList<LegalAction> MouYinActions(ProgramSkillFrame frame) => _resolve().MouYinActions(frame);
    internal IReadOnlyList<PromptChoice> MouZhijueChoices(ProgramSkillFrame frame) => _resolve().MouZhijueChoices(frame);
    internal bool MouZhijueYang(int seat) => _resolve().MouZhijueYang(seat);
    internal int MouZhitianCount(int seat) => _resolve().MouZhitianCount(seat);
    internal IEnumerable<CardSnapshot> MouZhitianPrivateCards(int viewerSeat) => _resolve().MouZhitianPrivateCards(viewerSeat);
    internal IReadOnlyList<Card> MouZhitianTop(int seat) => _resolve().MouZhitianTop(seat);
    internal void ObserveMouZhugeEvent(IGameEvent payload) => _resolve().ObserveMouZhugeEvent(payload);
    internal void PublishMouPrompt(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string text) => _resolve().PublishMouPrompt(frame, choices, text);
    internal void ResolveMouZhugeChoice(PromptChoice choice) => _resolve().ResolveMouZhugeChoice(choice);
    internal bool ResumeMouZhijue(ProgramSkillFrame frame) => _resolve().ResumeMouZhijue(frame);
    internal void ReturnMouZhijueUse(CardUseFrame use) => _resolve().ReturnMouZhijueUse(use);
    internal int[]? SelectWindowIndexes_MatchingMouWujingIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingMouWujingIndexes(batch, candidate, trigger);
    internal bool TryResolveMouZhitianFireDiscard(CardUseFrame pending, int? selectedCardId) => _resolve().TryResolveMouZhitianFireDiscard(pending, selectedCardId);
    internal bool ValidMouZhijueUse(CardUseFrame use, MouZhijueUseReturn ret) => _resolve().ValidMouZhijueUse(use, ret);
}
