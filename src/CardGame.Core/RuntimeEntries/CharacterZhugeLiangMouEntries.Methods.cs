#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangMouEntries
{
    internal bool CanStartMouZhuge(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().CanStartMouZhuge(owner, plan);
    internal void CaptureMouWujingAllowance(long batchId, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMouWujingAllowance(batchId, movements);
    internal void CaptureMovementBatchFactsCaptureMouWujingAllowance(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureMouWujingAllowance(batch, movements);
    internal bool CompleteMouZhijueNativeAttackReturn() => _resolve().CompleteMouZhijueNativeAttackReturn();
    internal bool ContinueMouZhijueSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueMouZhijueSimpleUse(id, continuation);
    internal void ContinueMouZhitianFirePayment(long id) => _resolve().ContinueMouZhitianFirePayment(id);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteMouZhuge(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteMouZhuge(frame, effect);
    internal bool FinishMouZhijueZeroAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouZhijueZeroAttack(attack, out paused);
    internal bool HasMouOperation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().HasMouOperation(owner, op);
    internal bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner) => _resolve().HasMouWujingAllowance(owner);
    internal bool IsMouZhijueUse(long id) => _resolve().IsMouZhijueUse(id);
    internal bool IsMouZhijueZeroUse(long id) => _resolve().IsMouZhijueZeroUse(id);
    internal bool IsZeroEntityUse(long id) => _resolve().IsZeroEntityUse(id);
    internal void IssueMouZhijueUse(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.LegalAction action, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption? trick, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> physicalIds) => _resolve().IssueMouZhijueUse(frame, action, trick, card, physicalIds);
    internal bool MatchesMouZhijueUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesMouZhijueUseAction(id, action, cardId);
    internal int[] MatchingMouWujingIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingMouWujingIndexes(batch, candidate);
    internal bool MouDiscardNotFromUse(global::CardGame.Core.CardMovementRecord move) => _resolve().MouDiscardNotFromUse(move);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> MouYangOptions(global::CardGame.Core.CharacterState owner) => _resolve().MouYangOptions(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> MouYinActions(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MouYinActions(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouZhijueChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MouZhijueChoices(frame);
    internal bool MouZhijueYang(int seat) => _resolve().MouZhijueYang(seat);
    internal int MouZhitianCount(int seat) => _resolve().MouZhitianCount(seat);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> MouZhitianPrivateCards(int viewerSeat) => _resolve().MouZhitianPrivateCards(viewerSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MouZhitianTop(int seat) => _resolve().MouZhitianTop(seat);
    internal void ObserveMouZhugeEvent(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouZhugeEvent(payload);
    internal void PublishMouPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text) => _resolve().PublishMouPrompt(frame, choices, text);
    internal void ResolveMouZhugeChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveMouZhugeChoice(choice);
    internal bool ResumeMouZhijue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeMouZhijue(frame);
    internal void ReturnMouZhijueUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouZhijueUse(use);
    internal int[]? SelectWindowIndexes_MatchingMouWujingIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingMouWujingIndexes(batch, candidate, trigger);
    internal bool TryResolveMouZhitianFireDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId) => _resolve().TryResolveMouZhitianFireDiscard(pending, selectedCardId);
    internal bool ValidMouZhijueUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MouZhijueUseReturn ret) => _resolve().ValidMouZhijueUse(use, ret);
}
