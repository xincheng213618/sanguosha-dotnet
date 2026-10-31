#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundRankBonusObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? BoundRankBonusMovementRoot(long damageWindowId) => _resolve().BoundRankBonusMovementRoot(damageWindowId);
    internal bool HasBoundRankBonusMovementObserver(long damageWindowId) => _resolve().HasBoundRankBonusMovementObserver(damageWindowId);
}
