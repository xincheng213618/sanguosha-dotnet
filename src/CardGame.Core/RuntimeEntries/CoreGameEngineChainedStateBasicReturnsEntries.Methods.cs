#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineChainedStateBasicReturnsEntries
{
    internal void AssertChainedStateBasicUseCompletion(CardUseFrame use) => _resolve().AssertChainedStateBasicUseCompletion(use);
    internal void BeginChainedStateBasicSlash(CharacterState actor, IReadOnlyList<int> targets, ChainedStateBasicPayment payment, NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit, long? programParent) => _resolve().BeginChainedStateBasicSlash(actor, targets, payment, borrowed, countsTowardLimit, programParent);
    internal bool ChainedStateBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().ChainedStateBasicFrameRidesOn(child, parent);
    internal bool ChainedStateBasicLegacyAlcoholRide(ChainedStateBasicFrame paid, int dyingIndex, DyingFrame dying) => _resolve().ChainedStateBasicLegacyAlcoholRide(paid, dyingIndex, dying);
    internal ChainedStateBasicFrame? ChainedStateBasicObserverRoot(long? requestedId = default) => _resolve().ChainedStateBasicObserverRoot(requestedId);
    internal bool ChainedStateBasicPaidSuffix(int rootIndex) => _resolve().ChainedStateBasicPaidSuffix(rootIndex);
    internal bool ChainedStateBasicProgramActor(ProgramSkillFrame parent, ChainedStateBasicPayment p) => _resolve().ChainedStateBasicProgramActor(parent, p);
    internal Card ChainedStateBasicRepresentation(long id) => _resolve().ChainedStateBasicRepresentation(id);
    internal bool ChainedStateBasicStateChild(ChainedStateBasicFrame paid, ResolutionFrame child) => _resolve().ChainedStateBasicStateChild(paid, child);
    internal bool ChainedStateBasicStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().ChainedStateBasicStructuralEdge(parent, child);
    internal void ClearChainedStateBasicDodgeResponse(NativeOwners.CardAttackHandle attack) => _resolve().ClearChainedStateBasicDodgeResponse(attack);
    internal void EmitChainedStateBasicIssued(ChainedStateBasicUseReceipt receipt) => _resolve().EmitChainedStateBasicIssued(receipt);
    internal void EmitChainedStateBasicReturned(ChainedStateBasicUseReceipt r) => _resolve().EmitChainedStateBasicReturned(r);
    internal void ExecuteChainedStateBasicPaid(ChainedStateBasicFrame frame) => _resolve().ExecuteChainedStateBasicPaid(frame);
    internal bool FinishChainedStateBasicAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishChainedStateBasicAttack(attack, out paused);
    internal bool IsChainedStateBasicIssued(ChainedStateBasicUseReceipt receipt) => _resolve().IsChainedStateBasicIssued(receipt);
    internal bool IsChainedStateBasicProgramDying() => _resolve().IsChainedStateBasicProgramDying();
    internal bool IsChainedStateBasicProgramSelection(ProgramSkillFrame parent, SkillProgramEffect effect, ChainedStateBasicFrame paid) => _resolve().IsChainedStateBasicProgramSelection(parent, effect, paid);
    internal bool IsChainedStateBasicUse(long id) => _resolve().IsChainedStateBasicUse(id);
    internal bool IsIssuedChainedStateBasicUse(CardUseFrame use) => _resolve().IsIssuedChainedStateBasicUse(use);
    internal void IssueChainedStateBasicUse(CardUseFrame use, ChainedStateBasicPayment? payment) => _resolve().IssueChainedStateBasicUse(use, payment);
    internal CardActionContext PrepareChainedStateBasicAction(CardActionContext action, ChainedStateBasicPayment? payment) => _resolve().PrepareChainedStateBasicAction(action, payment);
    internal Card ReadChainedStateBasicUseAppearance(long id, CardAppearanceReference appearance) => _resolve().ReadChainedStateBasicUseAppearance(id, appearance);
    internal CardActionContext RequireChainedStateBasicResponseAction(CardUseFrame use) => _resolve().RequireChainedStateBasicResponseAction(use);
    internal void ResolveChainedStateBasicDodge(ChainedStateBasicPayment p) => _resolve().ResolveChainedStateBasicDodge(p);
    internal void ReturnChainedStateBasicUse(CardUseFrame completed) => _resolve().ReturnChainedStateBasicUse(completed);
    internal bool SkipChainedStateBasicFinishedMovement(long id, Card card) => _resolve().SkipChainedStateBasicFinishedMovement(id, card);
}
