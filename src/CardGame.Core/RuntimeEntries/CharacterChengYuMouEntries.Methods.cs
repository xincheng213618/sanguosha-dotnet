#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengYuMouEntries
{
    internal void ApplyMouCyInvalidation(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MouCyUseInvalidatedEvent proof) => _resolve().ApplyMouCyInvalidation(f, proof);
    internal bool ContinueMouCyIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().ContinueMouCyIneffectiveDodge(attack, action, continuation);
    internal bool ContinueMouCyIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueMouCyIneffectiveSimple(id, continuation);
    internal bool HasMouCyMatchingName(int actor, global::CardGame.Core.CardKind kind) => _resolve().HasMouCyMatchingName(actor, kind);
    internal bool IsMouCyForeignHealthCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsMouCyForeignHealthCandidate(candidate, context);
    internal bool IsMouCyUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsMouCyUseIneffective(use);
    internal bool MouCyAdvanceChildren() => _resolve().MouCyAdvanceChildren();
    internal bool MouCyCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action) => _resolve().MouCyCounterspellInvalidated(pending, action);
    internal bool MouCyDying() => _resolve().MouCyDying();
    internal bool MouCyEdge(int i) => _resolve().MouCyEdge(i);
    internal bool MouCyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouCyFirstChild(f, child);
    internal bool MouCyMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().MouCyMovement(f, effect, movement);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? MouCyNamesView(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? existing, int holder, int viewer, bool revealAll) => _resolve().MouCyNamesView(existing, holder, viewer, revealAll);
    internal global::CardGame.Core.ProgramSkillFrame? MouCyObserverRoot() => _resolve().MouCyObserverRoot();
    internal global::CardGame.Core.CardSnapshot[] MouCyPrivateTop(int viewer) => _resolve().MouCyPrivateTop(viewer);
    internal bool MouCyRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouCyRecoveryParent(f, recovery);
    internal bool MouCySuffix(int n) => _resolve().MouCySuffix(n);
    internal global::CardGame.Core.MouCyUse? MouCyUse(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().MouCyUse(c);
    internal bool TryMouCyEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => _resolve().TryMouCyEquipmentCommitted(id, equipment);
    internal bool ValidMouCyInvalidation(global::CardGame.Core.MouCyUseInvalidatedEvent proof, global::CardGame.Core.CardActionContext action, long parent) => _resolve().ValidMouCyInvalidation(proof, action, parent);
    internal bool HasMouChengYuAnniversary { get => _resolve().HasMouChengYuAnniversary; }
    internal global::CardGame.Core.MouChengYuAnniversaryRuntime MouChengYuAnniversary { get => _resolve().MouChengYuAnniversary; }
}
