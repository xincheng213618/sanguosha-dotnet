#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMouEntries
{
    internal void CaptureMouHqAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CaptureMouHqAttack(attack);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectMouHqGiveCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectMouHqGiveCandidates(batch);
    internal void CollectWindow_CollectMouHqGiveCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouHqGiveCandidates(batch, result);
    internal void CompleteMouHqRound(long id) => _resolve().CompleteMouHqRound(id);
    internal bool ContinueMouHqFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueMouHqFangtian(attack);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMouHqGiveContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateMouHqGiveContext(frame, c);
    internal bool ExcludeOrdinaryMovement_IsMouHqGiveTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouHqGiveTrigger(candidate, trigger);
    internal void ExpireMouHqGivenEntities(global::CardGame.Core.TurnEndedEvent ended) => _resolve().ExpireMouHqGivenEntities(ended);
    internal bool HasUnstartedMouHqRound(long id) => _resolve().HasUnstartedMouHqRound(id);
    internal bool IsMouHqGiveTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().IsMouHqGiveTrigger(trigger);
    internal bool IsMouHqRound(long id) => _resolve().IsMouHqRound(id);
    internal bool IsMouHqUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsMouHqUseIneffective(use);
    internal void IssueMouHqOriginalEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouHqOriginalEffect(f);
    internal bool MouHqFinalizedWindow(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().MouHqFinalizedWindow(q);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouHqGift> MouHqGiftsForBatch(global::CardGame.Core.CardMovementBatchContext batch, int giver) => _resolve().MouHqGiftsForBatch(batch, giver);
    internal bool MouHqParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHqParent(f);
    internal global::System.Action<global::CardGame.Core.CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.CardMovementRecord>? original = default) => _resolve().NativeGiveCompletion(producer, giver, recipient, ids, before, original);
    internal global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>> NativeGiveCompletionRecords(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? original = default) => _resolve().NativeGiveCompletionRecords(producer, giver, recipient, ids, before, original);
    internal global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement> NativeGiveRequestCompletion(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? original = default) => _resolve().NativeGiveRequestCompletion(producer, giver, recipient, ids, before, original);
    internal void RecordNativeGivenCards(long producerId, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> intended, long before, global::CardGame.Core.HappyWeiYanGainRequestSettlement? requestSettlement = default) => _resolve().RecordNativeGivenCards(producerId, giver, recipient, intended, before, requestSettlement);
    internal bool TryBeginMouHqRound(long id, bool afterAttack) => _resolve().TryBeginMouHqRound(id, afterAttack);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateMouHqGiveContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateMouHqGiveContext(frame, candidate);
    internal bool TryMouHqOriginalUse(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryMouHqOriginalUse(owner, context, out use, out window);
    internal bool ValidMouHq(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouHq(f);
    internal bool ValidMouHqEffectGrant(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MouHqEffectGrant g, bool repeats) => _resolve().ValidMouHqEffectGrant(use, g, repeats);
    internal bool HasMouHuangQuan { get => _resolve().HasMouHuangQuan; }
    internal global::CardGame.Core.MouHuangQuanRuntime MouHuangQuan { get => _resolve().MouHuangQuan; }
}
