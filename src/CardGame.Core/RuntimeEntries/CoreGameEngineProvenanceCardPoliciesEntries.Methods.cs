#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceCardPoliciesEntries
{
    internal void ClearProvenanceUseOnActorChange(long useId, int actor) => _resolve().ClearProvenanceUseOnActorChange(useId, actor);
    internal bool DistanceBorrowedPhysicalSlash50HasProvenanceUseDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash50HasProvenanceUseDistance(q);
    internal bool DistanceBorrowedSlash90HasPotentialProvenanceSlash(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash90HasPotentialProvenanceSlash(q);
    internal bool DistancePhysicalSlash150HasIssuedProvenanceUseDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash150HasIssuedProvenanceUseDistance(q);
    internal bool DistancePhysicalSlash160HasProvenanceUseDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash160HasProvenanceUseDistance(q);
    internal bool DistanceVirtualSlash60HasProvenanceUseDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash60HasProvenanceUseDistance(q);
    internal global::CardGame.Core.CardConversionSource? FreezeProvenanceCardSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().FreezeProvenanceCardSource(owner, ids);
    internal global::CardGame.Core.CardConversionSource? FreezeProvenanceCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().FreezeProvenanceCounterspellSource(owner, ids);
    internal bool HasIssuedProvenanceUseDistance(long? useId, int owner) => _resolve().HasIssuedProvenanceUseDistance(useId, owner);
    internal bool HasPotentialProvenanceSlash(global::CardGame.Core.CharacterState owner) => _resolve().HasPotentialProvenanceSlash(owner);
    internal bool HasProvenanceCounterspellMaterials(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> validIds) => _resolve().HasProvenanceCounterspellMaterials(owner, validIds);
    internal bool HasProvenanceGrainCounterspellMaterials(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> validIds) => _resolve().HasProvenanceGrainCounterspellMaterials(owner, validIds);
    internal bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().HasProvenanceUseDistance(owner, card);
    internal bool HasProvenanceUseDistance2(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds) => _resolve().HasProvenanceUseDistance(owner, physicalIds);
    internal bool IsProvenanceEntityCurrentlyHeld(global::CardGame.Core.CharacterState owner, int id, string tag) => _resolve().IsProvenanceEntityCurrentlyHeld(owner, id, tag);
    internal void IssueProvenanceUsePolicy(long useId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueProvenanceUsePolicy(useId, action);
    internal void TryFreezeProvenanceCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds, ref global::CardGame.Core.CardConversionSource? source) => _resolve().TryFreezeProvenanceCounterspellSource(owner, physicalCardIds, ref source);
}
