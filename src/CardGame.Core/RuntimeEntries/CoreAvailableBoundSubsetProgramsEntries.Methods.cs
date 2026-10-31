#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAvailableBoundSubsetProgramsEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? GetAvailableBoundDamageObserverRoot(long damageWindowId) => _resolve().GetAvailableBoundDamageObserverRoot(damageWindowId);
    internal bool HasAvailableBoundDamageObserver(long damageWindowId) => _resolve().HasAvailableBoundDamageObserver(damageWindowId);
}
