#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhangliangEntries
{
    internal void BindOwnTurnEnemyHandGain(ProgramSkillFrame frame) => _resolve().BindOwnTurnEnemyHandGain(frame);
    internal bool CanRunOwnTurnEnemyHandGain(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunOwnTurnEnemyHandGain(candidate, context);
    internal IEnumerable<ProgramTriggerCandidate> CollectOwnTurnEnemyHandGainCandidates(CardMovementBatchContext batch) => _resolve().CollectOwnTurnEnemyHandGainCandidates(batch);
    internal void CollectWindow_CollectOwnTurnEnemyHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectOwnTurnEnemyHandGainCandidates(batch, result);
    internal ProgramSkillWindowContext CreateOwnTurnEnemyHandGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate) => _resolve().CreateOwnTurnEnemyHandGainContext(parent, candidate);
    internal bool ExcludeOrdinaryMovement_IsOwnTurnEnemyHandGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsOwnTurnEnemyHandGainTrigger(candidate, trigger);
    internal int[] OwnTurnEnemyHandGainIndexes(CardMovementBatchContext batch, int target) => _resolve().OwnTurnEnemyHandGainIndexes(batch, target);
    internal int? OwnTurnEnemyHandGainTarget(CardMovementBatchContext batch, int owner, int threshold) => _resolve().OwnTurnEnemyHandGainTarget(batch, owner, threshold);
    internal bool? ProveOwnTurnEnemyGain(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveOwnTurnEnemyGain(trigger, candidate, context);
    internal ProgramSkillWindowContext? TryCreateOwnTurnEnemyHandGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateOwnTurnEnemyHandGainContext(frame, candidate);
}
