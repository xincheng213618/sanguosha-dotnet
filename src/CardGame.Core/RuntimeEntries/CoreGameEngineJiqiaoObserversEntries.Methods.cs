#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJiqiaoObserversEntries
{
    internal bool CanContinueIssuedJiqiao(ProgramSkillFrame f) => _resolve().CanContinueIssuedJiqiao(f);
    internal bool HasPaidJiqiaoSilverLion(ProgramSkillFrame f) => _resolve().HasPaidJiqiaoSilverLion(f);
    internal bool IsExactJiqiaoStageMovement(ProgramSkillFrame f, JiqiaoPaidPending r, CardMovementRecord m) => _resolve().IsExactJiqiaoStageMovement(f, r, m);
    internal bool IsJiqiaoPaidCostDestination(ProgramSkillFrame frame, CardMovementRecord movement) => _resolve().IsJiqiaoPaidCostDestination(frame, movement);
    internal bool JiqiaoFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().JiqiaoFirstChild(root, child);
    internal ProgramSkillFrame? JiqiaoObserverRoot() => _resolve().JiqiaoObserverRoot();
    internal bool TryAdvanceJiqiaoSubtree() => _resolve().TryAdvanceJiqiaoSubtree();
}
