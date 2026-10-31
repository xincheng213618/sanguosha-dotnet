#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineChainedStateBasicReturnsEntries
{
    internal void AssertChainedStateBasicUseCompletion(global::CardGame.Core.CardUseFrame use) => _resolve().AssertChainedStateBasicUseCompletion(use);
    internal void BeginChainedStateBasicSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.ChainedStateBasicPayment payment, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit, long? programParent) => _resolve().BeginChainedStateBasicSlash(actor, targets, payment, borrowed, countsTowardLimit, programParent);
    internal bool ChainedStateBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().ChainedStateBasicFrameRidesOn(child, parent);
    internal bool ChainedStateBasicLegacyAlcoholRide(global::CardGame.Core.ChainedStateBasicFrame paid, int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().ChainedStateBasicLegacyAlcoholRide(paid, dyingIndex, dying);
    internal global::CardGame.Core.ChainedStateBasicFrame? ChainedStateBasicObserverRoot(long? requestedId = default) => _resolve().ChainedStateBasicObserverRoot(requestedId);
    internal bool ChainedStateBasicPaidSuffix(int rootIndex) => _resolve().ChainedStateBasicPaidSuffix(rootIndex);
    internal bool ChainedStateBasicProgramActor(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.ChainedStateBasicPayment p) => _resolve().ChainedStateBasicProgramActor(parent, p);
    internal global::CardGame.Core.Card ChainedStateBasicRepresentation(long id) => _resolve().ChainedStateBasicRepresentation(id);
    internal bool ChainedStateBasicStateChild(global::CardGame.Core.ChainedStateBasicFrame paid, global::CardGame.Core.ResolutionFrame child) => _resolve().ChainedStateBasicStateChild(paid, child);
    internal bool ChainedStateBasicStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().ChainedStateBasicStructuralEdge(parent, child);
    internal void ClearChainedStateBasicDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ClearChainedStateBasicDodgeResponse(attack);
    internal void EmitChainedStateBasicIssued(global::CardGame.Core.ChainedStateBasicUseReceipt receipt) => _resolve().EmitChainedStateBasicIssued(receipt);
    internal void EmitChainedStateBasicReturned(global::CardGame.Core.ChainedStateBasicUseReceipt r) => _resolve().EmitChainedStateBasicReturned(r);
    internal void ExecuteChainedStateBasicPaid(global::CardGame.Core.ChainedStateBasicFrame frame) => _resolve().ExecuteChainedStateBasicPaid(frame);
    internal bool FinishChainedStateBasicAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishChainedStateBasicAttack(attack, out paused);
    internal bool IsChainedStateBasicIssued(global::CardGame.Core.ChainedStateBasicUseReceipt receipt) => _resolve().IsChainedStateBasicIssued(receipt);
    internal bool IsChainedStateBasicProgramDying() => _resolve().IsChainedStateBasicProgramDying();
    internal bool IsChainedStateBasicProgramSelection(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ChainedStateBasicFrame paid) => _resolve().IsChainedStateBasicProgramSelection(parent, effect, paid);
    internal bool IsChainedStateBasicUse(long id) => _resolve().IsChainedStateBasicUse(id);
    internal bool IsIssuedChainedStateBasicUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsIssuedChainedStateBasicUse(use);
    internal void IssueChainedStateBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChainedStateBasicPayment? payment) => _resolve().IssueChainedStateBasicUse(use, payment);
    internal global::CardGame.Core.CardActionContext PrepareChainedStateBasicAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ChainedStateBasicPayment? payment) => _resolve().PrepareChainedStateBasicAction(action, payment);
    internal global::CardGame.Core.Card ReadChainedStateBasicUseAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadChainedStateBasicUseAppearance(id, appearance);
    internal global::CardGame.Core.CardActionContext RequireChainedStateBasicResponseAction(global::CardGame.Core.CardUseFrame use) => _resolve().RequireChainedStateBasicResponseAction(use);
    internal void ResolveChainedStateBasicDodge(global::CardGame.Core.ChainedStateBasicPayment p) => _resolve().ResolveChainedStateBasicDodge(p);
    internal void ReturnChainedStateBasicUse(global::CardGame.Core.CardUseFrame completed) => _resolve().ReturnChainedStateBasicUse(completed);
    internal bool SkipChainedStateBasicFinishedMovement(long id, global::CardGame.Core.Card card) => _resolve().SkipChainedStateBasicFinishedMovement(id, card);
}
