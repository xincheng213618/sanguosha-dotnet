#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunYuMouEntries
{
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeMouXyAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeMouXyAfterTurnEndedItems(parent, ordinary);
    internal bool IsIssuedMouXyAllocationCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedMouXyAllocationCandidate(c, context);
    internal bool IsMouXyVirtualUse(CardUseFrame use) => _resolve().IsMouXyVirtualUse(use);
    internal void IssueMouXyTrick(ProgramSkillFrame f) => _resolve().IssueMouXyTrick(f);
    internal bool MouXyAdvanceChildren() => _resolve().MouXyAdvanceChildren();
    internal IReadOnlyList<MouXyAllocationCard> MouXyAllocationCards(ProgramSkillFrame f) => _resolve().MouXyAllocationCards(f);
    internal MouXyBizuoGrant? MouXyAllocationLease(ProgramSkillFrame f) => _resolve().MouXyAllocationLease(f);
    internal IEnumerable<MouXyBizuoEndedEvent> MouXyAllocationsDue() => _resolve().MouXyAllocationsDue();
    internal bool MouXyAttackReturned() => _resolve().MouXyAttackReturned();
    internal bool MouXyCompletedLogicalDiscard(MouXyDiscardEntry entry) => _resolve().MouXyCompletedLogicalDiscard(entry);
    internal bool MouXyDrawTwoTargets(ProgramTargetCardWindowQuery query) => _resolve().MouXyDrawTwoTargets(query);
    internal bool MouXyDying() => _resolve().MouXyDying();
    internal bool MouXyEdge(int i) => _resolve().MouXyEdge(i);
    internal bool MouXyExactZhifouRemoval(CaoXianZhifouRemovedEvent e, MouXyBizuoGrant grant) => _resolve().MouXyExactZhifouRemoval(e, grant);
    internal bool MouXyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouXyFirstChild(f, child);
    internal bool MouXyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouXyMovement(f, effect, p);
    internal ProgramSkillFrame? MouXyObserverRoot() => _resolve().MouXyObserverRoot();
    internal IReadOnlyList<MouXyPlan> MouXyPlans(ProgramSkillFrame f) => _resolve().MouXyPlans(f);
    internal bool MouXyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouXyRecoveryParent(f, recovery);
    internal bool MouXySuffix(int n) => _resolve().MouXySuffix(n);
    internal bool ReturnMouXyVirtualUse(CardUseFrame use) => _resolve().ReturnMouXyVirtualUse(use);
    internal MouXunYuAnniversaryRuntime MouXyAnniversary { get => _resolve().MouXyAnniversary; }
}
