#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceCardPoliciesEntries
{
    internal void ClearProvenanceUseOnActorChange(long useId, int actor) => _resolve().ClearProvenanceUseOnActorChange(useId, actor);
    internal bool DistanceBorrowedPhysicalSlash50HasProvenanceUseDistance(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash50HasProvenanceUseDistance(q);
    internal bool DistanceBorrowedSlash90HasPotentialProvenanceSlash(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash90HasPotentialProvenanceSlash(q);
    internal bool DistancePhysicalSlash150HasIssuedProvenanceUseDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash150HasIssuedProvenanceUseDistance(q);
    internal bool DistancePhysicalSlash160HasProvenanceUseDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash160HasProvenanceUseDistance(q);
    internal bool DistanceVirtualSlash60HasProvenanceUseDistance(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash60HasProvenanceUseDistance(q);
    internal CardConversionSource? FreezeProvenanceCardSource(CharacterState owner, IReadOnlyList<int> ids) => _resolve().FreezeProvenanceCardSource(owner, ids);
    internal CardConversionSource? FreezeProvenanceCounterspellSource(CharacterState owner, IReadOnlyList<int> ids) => _resolve().FreezeProvenanceCounterspellSource(owner, ids);
    internal bool HasIssuedProvenanceUseDistance(long? useId, int owner) => _resolve().HasIssuedProvenanceUseDistance(useId, owner);
    internal bool HasPotentialProvenanceSlash(CharacterState owner) => _resolve().HasPotentialProvenanceSlash(owner);
    internal bool HasProvenanceCounterspellMaterials(CharacterState owner, IReadOnlyList<int> validIds) => _resolve().HasProvenanceCounterspellMaterials(owner, validIds);
    internal bool HasProvenanceGrainCounterspellMaterials(CharacterState owner, IReadOnlyList<int> validIds) => _resolve().HasProvenanceGrainCounterspellMaterials(owner, validIds);
    internal bool HasProvenanceUseDistance(CharacterState owner, Card card) => _resolve().HasProvenanceUseDistance(owner, card);
    internal bool HasProvenanceUseDistance2(CharacterState owner, IReadOnlyList<int>? physicalIds) => _resolve().HasProvenanceUseDistance(owner, physicalIds);
    internal bool IsProvenanceEntityCurrentlyHeld(CharacterState owner, int id, string tag) => _resolve().IsProvenanceEntityCurrentlyHeld(owner, id, tag);
    internal void IssueProvenanceUsePolicy(long useId, CardActionContext? action) => _resolve().IssueProvenanceUsePolicy(useId, action);
    internal void TryFreezeProvenanceCounterspellSource(CharacterState owner, IReadOnlyList<int> physicalCardIds, ref CardConversionSource? source) => _resolve().TryFreezeProvenanceCounterspellSource(owner, physicalCardIds, ref source);
}
