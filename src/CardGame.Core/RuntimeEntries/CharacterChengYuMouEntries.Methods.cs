#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengYuMouEntries
{
    internal void ApplyMouCyInvalidation(ProgramSkillFrame f, MouCyUseInvalidatedEvent proof) => _resolve().ApplyMouCyInvalidation(f, proof);
    internal bool ContinueMouCyIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().ContinueMouCyIneffectiveDodge(attack, action, continuation);
    internal bool ContinueMouCyIneffectiveSimple(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueMouCyIneffectiveSimple(id, continuation);
    internal bool HasMouCyMatchingName(int actor, CardKind kind) => _resolve().HasMouCyMatchingName(actor, kind);
    internal bool IsMouCyForeignHealthCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsMouCyForeignHealthCandidate(candidate, context);
    internal bool IsMouCyUseIneffective(CardUseFrame use) => _resolve().IsMouCyUseIneffective(use);
    internal bool MouCyAdvanceChildren() => _resolve().MouCyAdvanceChildren();
    internal bool MouCyCounterspellInvalidated(NullificationWindowFrame pending, CardActionContext action) => _resolve().MouCyCounterspellInvalidated(pending, action);
    internal bool MouCyDying() => _resolve().MouCyDying();
    internal bool MouCyEdge(int i) => _resolve().MouCyEdge(i);
    internal bool MouCyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouCyFirstChild(f, child);
    internal bool MouCyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => _resolve().MouCyMovement(f, effect, movement);
    internal IReadOnlyList<NamedAmbushPileSnapshot>? MouCyNamesView(IReadOnlyList<NamedAmbushPileSnapshot>? existing, int holder, int viewer, bool revealAll) => _resolve().MouCyNamesView(existing, holder, viewer, revealAll);
    internal ProgramSkillFrame? MouCyObserverRoot() => _resolve().MouCyObserverRoot();
    internal CardSnapshot[] MouCyPrivateTop(int viewer) => _resolve().MouCyPrivateTop(viewer);
    internal bool MouCyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouCyRecoveryParent(f, recovery);
    internal bool MouCySuffix(int n) => _resolve().MouCySuffix(n);
    internal Core.MouCyUse? MouCyUse(ProgramSkillWindowContext c) => _resolve().MouCyUse(c);
    internal bool TryMouCyEquipmentCommitted(long id, Card equipment) => _resolve().TryMouCyEquipmentCommitted(id, equipment);
    internal bool ValidMouCyInvalidation(MouCyUseInvalidatedEvent proof, CardActionContext action, long parent) => _resolve().ValidMouCyInvalidation(proof, action, parent);
    internal bool HasMouChengYuAnniversary { get => _resolve().HasMouChengYuAnniversary; }
    internal MouChengYuAnniversaryRuntime MouChengYuAnniversary { get => _resolve().MouChengYuAnniversary; }
}
