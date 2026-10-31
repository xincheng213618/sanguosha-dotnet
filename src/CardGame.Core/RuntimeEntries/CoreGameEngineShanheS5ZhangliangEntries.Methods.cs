#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhangliangEntries
{
    internal void BindOwnTurnEnemyHandGain(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BindOwnTurnEnemyHandGain(frame);
    internal bool CanRunOwnTurnEnemyHandGain(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunOwnTurnEnemyHandGain(candidate, context);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectOwnTurnEnemyHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectOwnTurnEnemyHandGainCandidates(batch);
    internal void CollectWindow_CollectOwnTurnEnemyHandGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectOwnTurnEnemyHandGainCandidates(batch, result);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateOwnTurnEnemyHandGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateOwnTurnEnemyHandGainContext(parent, candidate);
    internal bool ExcludeOrdinaryMovement_IsOwnTurnEnemyHandGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsOwnTurnEnemyHandGainTrigger(candidate, trigger);
    internal bool HasExactOwnTurnEnemyHandGainSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactOwnTurnEnemyHandGainSelection(frame);
    internal int[] OwnTurnEnemyHandGainIndexes(global::CardGame.Core.CardMovementBatchContext batch, int target) => _resolve().OwnTurnEnemyHandGainIndexes(batch, target);
    internal int? OwnTurnEnemyHandGainTarget(global::CardGame.Core.CardMovementBatchContext batch, int owner, int threshold) => _resolve().OwnTurnEnemyHandGainTarget(batch, owner, threshold);
    internal bool? ProveOwnTurnEnemyGain(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveOwnTurnEnemyGain(trigger, candidate, context);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateOwnTurnEnemyHandGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateOwnTurnEnemyHandGainContext(frame, candidate);
}
