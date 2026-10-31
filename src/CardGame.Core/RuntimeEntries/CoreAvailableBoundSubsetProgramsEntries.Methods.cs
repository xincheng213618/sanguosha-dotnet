#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAvailableBoundSubsetProgramsEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? GetAvailableBoundDamageObserverRoot(long damageWindowId) => _resolve().GetAvailableBoundDamageObserverRoot(damageWindowId);
    internal bool HasAvailableBoundDamageObserver(long damageWindowId) => _resolve().HasAvailableBoundDamageObserver(damageWindowId);
    internal bool IsAvailableBoundDamageProgramDying() => _resolve().IsAvailableBoundDamageProgramDying();
    internal bool IsAvailableBoundPeachRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().IsAvailableBoundPeachRescueRide(dyingIndex, dying);
}
