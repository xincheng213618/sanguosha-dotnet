#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunYuMouEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeMouXyAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeMouXyAfterTurnEndedItems(parent, ordinary);
    internal bool IsIssuedMouXyAllocationCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedMouXyAllocationCandidate(c, context);
    internal bool IsMouXyVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsMouXyVirtualUse(use);
    internal void IssueMouXyTrick(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouXyTrick(f);
    internal bool MouXyAdvanceChildren() => _resolve().MouXyAdvanceChildren();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouXyAllocationCard> MouXyAllocationCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouXyAllocationCards(f);
    internal global::CardGame.Core.MouXyBizuoGrant? MouXyAllocationLease(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouXyAllocationLease(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouXyBizuoEndedEvent> MouXyAllocationsDue() => _resolve().MouXyAllocationsDue();
    internal bool MouXyAttackReturned() => _resolve().MouXyAttackReturned();
    internal bool MouXyBeforeDamage(long id) => _resolve().MouXyBeforeDamage(id);
    internal bool MouXyCompletedLogicalDiscard(global::CardGame.Core.MouXyDiscardEntry entry) => _resolve().MouXyCompletedLogicalDiscard(entry);
    internal bool MouXyDamageWindow(long id) => _resolve().MouXyDamageWindow(id);
    internal bool MouXyDrawTwoTargets(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().MouXyDrawTwoTargets(query);
    internal bool MouXyDying() => _resolve().MouXyDying();
    internal bool MouXyEdge(int i) => _resolve().MouXyEdge(i);
    internal bool MouXyExactZhifouRemoval(global::CardGame.Core.CaoXianZhifouRemovedEvent e, global::CardGame.Core.MouXyBizuoGrant grant) => _resolve().MouXyExactZhifouRemoval(e, grant);
    internal bool MouXyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouXyFirstChild(f, child);
    internal bool MouXyMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouXyMovement(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? MouXyObserverRoot() => _resolve().MouXyObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouXyPlan> MouXyPlans(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouXyPlans(f);
    internal bool MouXyRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouXyRecoveryParent(f, recovery);
    internal bool MouXySuffix(int n) => _resolve().MouXySuffix(n);
    internal bool ReturnMouXyVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouXyVirtualUse(use);
    internal global::CardGame.Core.MouXunYuAnniversaryRuntime MouXyAnniversary { get => _resolve().MouXyAnniversary; }
}
