#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceBiriObserversEntries
{
    internal bool AdvanceShanheSourceBiriChildren() => _resolve().AdvanceShanheSourceBiriChildren();
    internal bool IsShanheSourceBiriObserverDying() => _resolve().IsShanheSourceBiriObserverDying();
    internal bool IsShanheSourceBiriRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsShanheSourceBiriRecoveryParent(frame, recovery);
    internal bool ShanheSourceBiriFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().ShanheSourceBiriFirstChild(root, child);
    internal bool ShanheSourceBiriObserverEdge(int index) => _resolve().ShanheSourceBiriObserverEdge(index);
    internal ProgramSkillFrame? ShanheSourceBiriObserverRoot() => _resolve().ShanheSourceBiriObserverRoot();
}
