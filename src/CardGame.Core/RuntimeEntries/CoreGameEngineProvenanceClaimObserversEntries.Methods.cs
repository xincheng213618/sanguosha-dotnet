#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceClaimObserversEntries
{
    internal bool HasProvenanceClaimObserver(long damageId) => _resolve().HasProvenanceClaimObserver(damageId);
    internal bool IsProvenanceClaimProgramDying() => _resolve().IsProvenanceClaimProgramDying();
    internal global::CardGame.Core.ProgramSkillFrame? ProvenanceClaimDamageObserverRoot(long damageId) => _resolve().ProvenanceClaimDamageObserverRoot(damageId);
    internal bool ProvenanceClaimTurnedOverEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().ProvenanceClaimTurnedOverEdge(child, parent);
}
