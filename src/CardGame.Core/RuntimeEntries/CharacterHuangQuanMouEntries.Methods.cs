#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanMouEntries
{
    internal void CaptureMouHqAttack(NativeOwners.CardAttackHandle attack) => _resolve().CaptureMouHqAttack(attack);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectMouHqGiveCandidates(CardMovementBatchContext batch) => _resolve().CollectMouHqGiveCandidates(batch);
    internal void CollectWindow_CollectMouHqGiveCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouHqGiveCandidates(batch, result);
    internal void CompleteMouHqRound(long id) => _resolve().CompleteMouHqRound(id);
    internal bool ContinueMouHqFangtian(NativeOwners.CardAttackHandle attack) => _resolve().ContinueMouHqFangtian(attack);
    internal ProgramSkillWindowContext CreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => _resolve().CreateMouHqGiveContext(frame, c);
    internal bool ExcludeOrdinaryMovement_IsMouHqGiveTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouHqGiveTrigger(candidate, trigger);
    internal void ExpireMouHqGivenEntities(TurnEndedEvent ended) => _resolve().ExpireMouHqGivenEntities(ended);
    internal bool HasUnstartedMouHqRound(long id) => _resolve().HasUnstartedMouHqRound(id);
    internal bool IsMouHqGiveTrigger(SkillProgramTrigger trigger) => _resolve().IsMouHqGiveTrigger(trigger);
    internal bool IsMouHqRound(long id) => _resolve().IsMouHqRound(id);
    internal bool IsMouHqUseIneffective(CardUseFrame use) => _resolve().IsMouHqUseIneffective(use);
    internal void IssueMouHqOriginalEffect(ProgramSkillFrame f) => _resolve().IssueMouHqOriginalEffect(f);
    internal bool MouHqFinalizedWindow(ProgramTargetCardWindowQuery q) => _resolve().MouHqFinalizedWindow(q);
    internal IReadOnlyList<MouHqGift> MouHqGiftsForBatch(CardMovementBatchContext batch, int giver) => _resolve().MouHqGiftsForBatch(batch, giver);
    internal bool MouHqParent(ProgramSkillFrame f) => _resolve().MouHqParent(f);
    internal Action<CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, IReadOnlyList<int> ids, long before, Action<CardMovementRecord>? original = default) => _resolve().NativeGiveCompletion(producer, giver, recipient, ids, before, original);
    internal Action<long, IReadOnlyList<CardMovementRecord>> NativeGiveCompletionRecords(long producer, int giver, int recipient, IReadOnlyList<int> ids, long before, Action<long, IReadOnlyList<CardMovementRecord>>? original = default) => _resolve().NativeGiveCompletionRecords(producer, giver, recipient, ids, before, original);
    internal Action<HappyWeiYanGainRequestSettlement> NativeGiveRequestCompletion(long producer, int giver, int recipient, IReadOnlyList<int> ids, long before, Action<HappyWeiYanGainRequestSettlement>? original = default) => _resolve().NativeGiveRequestCompletion(producer, giver, recipient, ids, before, original);
    internal void RecordNativeGivenCards(long producerId, int giver, int recipient, IReadOnlyList<int> intended, long before, HappyWeiYanGainRequestSettlement? requestSettlement = default) => _resolve().RecordNativeGivenCards(producerId, giver, recipient, intended, before, requestSettlement);
    internal bool TryBeginMouHqRound(long id, bool afterAttack) => _resolve().TryBeginMouHqRound(id, afterAttack);
    internal ProgramSkillWindowContext? TryCreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouHqGiveContext(frame, candidate);
    internal bool TryMouHqOriginalUse(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryMouHqOriginalUse(owner, context, out use, out window);
    internal bool ValidMouHq(ProgramSkillFrame f) => _resolve().ValidMouHq(f);
    internal bool ValidMouHqEffectGrant(CardUseFrame use, MouHqEffectGrant g, bool repeats) => _resolve().ValidMouHqEffectGrant(use, g, repeats);
    internal bool HasMouHuangQuan { get => _resolve().HasMouHuangQuan; }
    internal MouHuangQuanRuntime MouHuangQuan { get => _resolve().MouHuangQuan; }
}
