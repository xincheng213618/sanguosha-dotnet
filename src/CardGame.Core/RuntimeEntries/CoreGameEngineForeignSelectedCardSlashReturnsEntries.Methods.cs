#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignSelectedCardSlashReturnsEntries
{
    internal bool CanContinueSelectedForeignCardSlash(ProgramSkillFrame frame) => _resolve().CanContinueSelectedForeignCardSlash(frame);
    internal bool DistancePhysicalSlash120HasIssuedSelectedForeignCardSlashDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash120HasIssuedSelectedForeignCardSlashDistance(q);
    internal bool ForeignSelectedCardSlashStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().ForeignSelectedCardSlashStructuralEdge(parent, child);
    internal bool HasIssuedSelectedForeignCardSlashDistance(long? useId, int actor) => _resolve().HasIssuedSelectedForeignCardSlashDistance(useId, actor);
    internal bool IsSelectedForeignCardSlashHpChange(long resumeFrameId, HpChangeContext change) => _resolve().IsSelectedForeignCardSlashHpChange(resumeFrameId, change);
    internal bool IsSelectedForeignCardSlashMovementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().IsSelectedForeignCardSlashMovementBatch(frame, batch);
    internal bool IsSelectedForeignCardSlashProgramDying() => _resolve().IsSelectedForeignCardSlashProgramDying();
    internal bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId) => _resolve().IsSelectedForeignCardSlashRemovedMaterial(useId, cardId);
    internal bool IsSelectedForeignCardSlashUse(CardUseFrame use) => _resolve().IsSelectedForeignCardSlashUse(use);
    internal void ObserveSelectedForeignCardSlashAppliedDamage(IGameEvent payload) => _resolve().ObserveSelectedForeignCardSlashAppliedDamage(payload);
    internal bool SelectedForeignCardSlashFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().SelectedForeignCardSlashFirstChild(frame, child);
    internal ProgramSkillFrame? SelectedForeignCardSlashObserverRoot() => _resolve().SelectedForeignCardSlashObserverRoot();
    internal bool TryAdvanceSelectedForeignCardSlashSubtree() => _resolve().TryAdvanceSelectedForeignCardSlashSubtree();
    internal bool ValidSelectedForeignCardSlash(ProgramSkillFrame frame) => _resolve().ValidSelectedForeignCardSlash(frame);
    internal bool WindowSettlement_IsSelectedForeignCardSlashMovementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsSelectedForeignCardSlashMovementBatch(frame, batch);
}
