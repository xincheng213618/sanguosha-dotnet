#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitObserversEntries
{
    internal bool FirstSlashDuelFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().FirstSlashDuelFirstChild(root, child);
    internal ProgramSkillFrame? FirstSlashDuelObserverRoot() => _resolve().FirstSlashDuelObserverRoot();
    internal void RequireFirstSlashDuelBenefit(ProgramSkillFrame f) => _resolve().RequireFirstSlashDuelBenefit(f);
    internal bool TryAdvanceFirstSlashDuelSubtree() => _resolve().TryAdvanceFirstSlashDuelSubtree();
    internal bool ValidFirstSlashDuelBenefit(ProgramSkillFrame f) => _resolve().ValidFirstSlashDuelBenefit(f);
}
