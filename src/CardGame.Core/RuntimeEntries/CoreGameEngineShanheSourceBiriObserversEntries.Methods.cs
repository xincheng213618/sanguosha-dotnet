#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceBiriObserversEntries
{
    internal bool AdvanceShanheSourceBiriChildren() => _resolve().AdvanceShanheSourceBiriChildren();
    internal bool HasShanheSourceBiriBeforeDamageObserver(long id) => _resolve().HasShanheSourceBiriBeforeDamageObserver(id);
    internal bool HasShanheSourceBiriDamageObserver(long id) => _resolve().HasShanheSourceBiriDamageObserver(id);
    internal bool IsShanheSourceBiriObserverDying() => _resolve().IsShanheSourceBiriObserverDying();
    internal bool IsShanheSourceBiriRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsShanheSourceBiriRecoveryParent(frame, recovery);
    internal bool ShanheSourceBiriFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().ShanheSourceBiriFirstChild(root, child);
    internal bool ShanheSourceBiriObserverEdge(int index) => _resolve().ShanheSourceBiriObserverEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? ShanheSourceBiriObserverRoot() => _resolve().ShanheSourceBiriObserverRoot();
}
