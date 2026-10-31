#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5YimieObserversEntries
{
    internal bool AdvanceShanheS5YimieChildren() => _resolve().AdvanceShanheS5YimieChildren();
    internal bool HasShanheS5YimieDyingObserver() => _resolve().HasShanheS5YimieDyingObserver();
    internal bool ShanheS5YimieCostFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().ShanheS5YimieCostFirstChild(root, child);
    internal ProgramSkillFrame? ShanheS5YimieCostObserverRoot() => _resolve().ShanheS5YimieCostObserverRoot();
    internal bool ShanheS5YimieRecoveryFirstChild(ShanheS5YimieRecoveryFrame root, ResolutionFrame child) => _resolve().ShanheS5YimieRecoveryFirstChild(root, child);
    internal ShanheS5YimieRecoveryFrame? ShanheS5YimieRecoveryObserverRoot() => _resolve().ShanheS5YimieRecoveryObserverRoot();
}
