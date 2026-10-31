#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitObserversEntries
{
    internal void AssertRegisteredFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredFirstSlashDuelBenefit(frame);
    internal bool FirstSlashDuelFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().FirstSlashDuelFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? FirstSlashDuelObserverRoot() => _resolve().FirstSlashDuelObserverRoot();
    internal bool HasFirstSlashDuelDamageObserver(long id) => _resolve().HasFirstSlashDuelDamageObserver(id);
    internal bool IsFirstSlashDuelDying() => _resolve().IsFirstSlashDuelDying();
    internal void RequireFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireFirstSlashDuelBenefit(f);
    internal bool TryAdvanceFirstSlashDuelSubtree() => _resolve().TryAdvanceFirstSlashDuelSubtree();
    internal bool ValidFirstSlashDuelBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidFirstSlashDuelBenefit(f);
}
