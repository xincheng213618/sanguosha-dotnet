#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetPenaltyObserversEntries
{
    internal bool IsTargetPenaltyProgramDying() => _resolve().IsTargetPenaltyProgramDying();
    internal ProgramSkillFrame? TargetPenaltyPaidObserverRoot(long? exactWindow = default) => _resolve().TargetPenaltyPaidObserverRoot(exactWindow);
    internal bool ValidTargetPenaltyReward(ProgramSkillFrame f) => _resolve().ValidTargetPenaltyReward(f);
}
