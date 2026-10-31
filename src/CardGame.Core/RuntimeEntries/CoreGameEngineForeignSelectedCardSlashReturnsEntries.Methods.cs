#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignSelectedCardSlashReturnsEntries
{
    internal bool CanContinueSelectedForeignCardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueSelectedForeignCardSlash(frame);
    internal bool DistancePhysicalSlash120HasIssuedSelectedForeignCardSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash120HasIssuedSelectedForeignCardSlashDistance(q);
    internal bool ForeignSelectedCardSlashStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().ForeignSelectedCardSlashStructuralEdge(parent, child);
    internal bool HasIssuedSelectedForeignCardSlashDistance(long? useId, int actor) => _resolve().HasIssuedSelectedForeignCardSlashDistance(useId, actor);
    internal bool IsSelectedForeignCardSlashHpChange(long resumeFrameId, global::CardGame.Core.HpChangeContext change) => _resolve().IsSelectedForeignCardSlashHpChange(resumeFrameId, change);
    internal bool IsSelectedForeignCardSlashMovementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsSelectedForeignCardSlashMovementBatch(frame, batch);
    internal bool IsSelectedForeignCardSlashProgramDying() => _resolve().IsSelectedForeignCardSlashProgramDying();
    internal bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => _resolve().IsSelectedForeignCardSlashRemovedMaterial(useId, cardId);
    internal bool IsSelectedForeignCardSlashUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsSelectedForeignCardSlashUse(use);
    internal void ObserveSelectedForeignCardSlashAppliedDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveSelectedForeignCardSlashAppliedDamage(payload);
    internal bool SelectedForeignCardSlashFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().SelectedForeignCardSlashFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? SelectedForeignCardSlashObserverRoot() => _resolve().SelectedForeignCardSlashObserverRoot();
    internal bool TryAdvanceSelectedForeignCardSlashSubtree() => _resolve().TryAdvanceSelectedForeignCardSlashSubtree();
    internal bool ValidSelectedForeignCardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidSelectedForeignCardSlash(frame);
    internal bool WindowSettlement_IsSelectedForeignCardSlashMovementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsSelectedForeignCardSlashMovementBatch(frame, batch);
}
