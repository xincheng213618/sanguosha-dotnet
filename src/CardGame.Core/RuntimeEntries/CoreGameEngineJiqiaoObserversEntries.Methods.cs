#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJiqiaoObserversEntries
{
    internal bool CanContinueIssuedJiqiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedJiqiao(f);
    internal bool HasJiqiaoDamageObserver(long id) => _resolve().HasJiqiaoDamageObserver(id);
    internal bool HasPaidJiqiaoSilverLion(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasPaidJiqiaoSilverLion(f);
    internal bool IsExactJiqiaoStageMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.JiqiaoPaidPending r, global::CardGame.Core.CardMovementRecord m) => _resolve().IsExactJiqiaoStageMovement(f, r, m);
    internal bool IsJiqiaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsJiqiaoMovement(f, e, p);
    internal bool IsJiqiaoPaidCostDestination(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord movement) => _resolve().IsJiqiaoPaidCostDestination(frame, movement);
    internal bool JiqiaoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().JiqiaoFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? JiqiaoObserverRoot() => _resolve().JiqiaoObserverRoot();
    internal bool TryAdvanceJiqiaoSubtree() => _resolve().TryAdvanceJiqiaoSubtree();
}
