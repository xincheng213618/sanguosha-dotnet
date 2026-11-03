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
    internal bool HasUnstartedMouHqRound(long id) => _resolve().HasUnstartedMouHqRound(id);
    internal bool IsMouHqGiveTrigger(SkillProgramTrigger trigger) => _resolve().IsMouHqGiveTrigger(trigger);
    internal bool IsMouHqRound(long id) => _resolve().IsMouHqRound(id);
    internal bool IsMouHqUseIneffective(CardUseFrame use) => _resolve().IsMouHqUseIneffective(use);
    internal void IssueMouHqOriginalEffect(ProgramSkillFrame f) => _resolve().IssueMouHqOriginalEffect(f);
    internal bool MouHqFinalizedWindow(ProgramTargetCardWindowQuery q) => _resolve().MouHqFinalizedWindow(q);
    internal bool MouHqParent(ProgramSkillFrame f) => _resolve().MouHqParent(f);
    internal bool TryBeginMouHqRound(long id, bool afterAttack) => _resolve().TryBeginMouHqRound(id, afterAttack);
    internal ProgramSkillWindowContext? TryCreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouHqGiveContext(frame, candidate);
    internal bool TryMouHqOriginalUse(int owner, ProgramSkillWindowContext? context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryMouHqOriginalUse(owner, context, out use, out window);
    internal bool ValidMouHq(ProgramSkillFrame f) => _resolve().ValidMouHq(f);
    internal bool ValidMouHqEffectGrant(CardUseFrame use, MouHqEffectGrant g, bool repeats) => _resolve().ValidMouHqEffectGrant(use, g, repeats);
    internal bool HasMouHuangQuan { get => _resolve().HasMouHuangQuan; }
    internal MouHuangQuanRuntime MouHuangQuan { get => _resolve().MouHuangQuan; }
}
