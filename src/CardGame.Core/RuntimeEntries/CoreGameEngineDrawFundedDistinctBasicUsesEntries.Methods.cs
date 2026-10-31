#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicUsesEntries
{
    internal void BeginDrawFundedDistinctBasicSlash(CharacterState actor, IReadOnlyList<int> targets, DrawFundedDistinctBasicPayment p, NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit, long? programParent) => _resolve().BeginDrawFundedDistinctBasicSlash(actor, targets, p, borrowed, countsTowardLimit, programParent);
    internal bool ContinueDrawFundedDistinctBasicSimpleUse(long frameId, ProgramSimpleCardContinuation continuation) => _resolve().ContinueDrawFundedDistinctBasicSimpleUse(frameId, continuation);
    internal Card DrawFundedDistinctBasicRepresentation(long frameId) => _resolve().DrawFundedDistinctBasicRepresentation(frameId);
    internal void EmitDrawFundedDistinctBasicIssued(DrawFundedDistinctBasicUseReceipt receipt) => _resolve().EmitDrawFundedDistinctBasicIssued(receipt);
    internal void ExecuteDrawFundedDistinctBasicPaid(DrawFundedDistinctBasicFrame frame) => _resolve().ExecuteDrawFundedDistinctBasicPaid(frame);
    internal bool FinishDrawFundedDistinctBasicAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishDrawFundedDistinctBasicAttack(attack, out paused);
    internal bool IsDrawFundedDistinctBasicIssued(DrawFundedDistinctBasicUseReceipt receipt) => _resolve().IsDrawFundedDistinctBasicIssued(receipt);
    internal bool IsDrawFundedDistinctBasicUse(long frameId) => _resolve().IsDrawFundedDistinctBasicUse(frameId);
    internal bool IsIssuedDrawFundedDistinctBasicUse(CardUseFrame use) => _resolve().IsIssuedDrawFundedDistinctBasicUse(use);
    internal void IssueDrawFundedDistinctBasicUse(CardUseFrame use, DrawFundedDistinctBasicPayment? payment) => _resolve().IssueDrawFundedDistinctBasicUse(use, payment);
    internal CardActionContext PrepareDrawFundedDistinctBasicAction(CardActionContext action, DrawFundedDistinctBasicPayment? payment) => _resolve().PrepareDrawFundedDistinctBasicAction(action, payment);
}
