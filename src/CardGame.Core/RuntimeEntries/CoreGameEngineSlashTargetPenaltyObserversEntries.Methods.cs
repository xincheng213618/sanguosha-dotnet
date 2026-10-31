#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyObserversEntries
{
    internal bool IsTargetPenaltyProgramDying() => _resolve().IsTargetPenaltyProgramDying();
    internal global::CardGame.Core.ProgramSkillFrame? TargetPenaltyPaidObserverRoot(long? exactWindow = default) => _resolve().TargetPenaltyPaidObserverRoot(exactWindow);
    internal bool ValidTargetPenaltyReward(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidTargetPenaltyReward(f);
}
