#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyObserversEntries
{
    internal void AssertTargetPenaltyWindows() => _resolve().AssertTargetPenaltyWindows();
    internal bool HasTargetPenaltyDamageObserver(long id) => _resolve().HasTargetPenaltyDamageObserver(id);
    internal bool HasTargetPenaltyUseObserver(long useId) => _resolve().HasTargetPenaltyUseObserver(useId);
    internal bool IsTargetPenaltyMovement(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsTargetPenaltyMovement(root, effect, pending);
    internal bool IsTargetPenaltyProgramDying() => _resolve().IsTargetPenaltyProgramDying();
    internal global::CardGame.Core.ProgramSkillFrame? TargetPenaltyPaidObserverRoot(long? exactWindow = default) => _resolve().TargetPenaltyPaidObserverRoot(exactWindow);
    internal bool ValidTargetPenaltyReward(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidTargetPenaltyReward(f);
}
