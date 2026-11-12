#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengYuMouEntries
{
    internal void ApplyMouCyInvalidation(ProgramSkillFrame f) => _resolve().ApplyMouCyInvalidation(f);
    internal bool HasMouCyMatchingName(int actor, CardKind kind) => _resolve().HasMouCyMatchingName(actor, kind);
    internal bool IsMouCyForeignHealthCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsMouCyForeignHealthCandidate(candidate, context);
    internal bool MouCyAdvanceChildren() => _resolve().MouCyAdvanceChildren();
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
    internal bool HasMouChengYuAnniversary { get => _resolve().HasMouChengYuAnniversary; }
    internal MouChengYuAnniversaryRuntime MouChengYuAnniversary { get => _resolve().MouChengYuAnniversary; }
}
