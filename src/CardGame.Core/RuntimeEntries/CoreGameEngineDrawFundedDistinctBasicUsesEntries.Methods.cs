#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicUsesEntries
{
    internal void BeginDrawFundedDistinctBasicSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.DrawFundedDistinctBasicPayment p, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit, long? programParent) => _resolve().BeginDrawFundedDistinctBasicSlash(actor, targets, p, borrowed, countsTowardLimit, programParent);
    internal bool ContinueDrawFundedDistinctBasicSimpleUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueDrawFundedDistinctBasicSimpleUse(frameId, continuation);
    internal global::CardGame.Core.Card DrawFundedDistinctBasicRepresentation(long frameId) => _resolve().DrawFundedDistinctBasicRepresentation(frameId);
    internal void EmitDrawFundedDistinctBasicIssued(global::CardGame.Core.DrawFundedDistinctBasicUseReceipt receipt) => _resolve().EmitDrawFundedDistinctBasicIssued(receipt);
    internal void ExecuteDrawFundedDistinctBasicPaid(global::CardGame.Core.DrawFundedDistinctBasicFrame frame) => _resolve().ExecuteDrawFundedDistinctBasicPaid(frame);
    internal bool FinishDrawFundedDistinctBasicAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishDrawFundedDistinctBasicAttack(attack, out paused);
    internal bool IsDrawFundedDistinctBasicIssued(global::CardGame.Core.DrawFundedDistinctBasicUseReceipt receipt) => _resolve().IsDrawFundedDistinctBasicIssued(receipt);
    internal bool IsDrawFundedDistinctBasicUse(long frameId) => _resolve().IsDrawFundedDistinctBasicUse(frameId);
    internal bool IsIssuedDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsIssuedDrawFundedDistinctBasicUse(use);
    internal void IssueDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DrawFundedDistinctBasicPayment? payment) => _resolve().IssueDrawFundedDistinctBasicUse(use, payment);
    internal global::CardGame.Core.CardActionContext PrepareDrawFundedDistinctBasicAction(global::CardGame.Core.CardActionContext action, global::CardGame.Core.DrawFundedDistinctBasicPayment? payment) => _resolve().PrepareDrawFundedDistinctBasicAction(action, payment);
}
