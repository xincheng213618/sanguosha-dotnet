#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramLostHpChainedPaymentsEntries
{
    internal void AssertLostHpChainedPayment(ProgramSkillFrame frame) => _resolve().AssertLostHpChainedPayment(frame);
    internal bool CanContinuePaidLostHpChain(ProgramSkillFrame frame) => _resolve().CanContinuePaidLostHpChain(frame);
    internal void CommitLostHpChainedPayment(long frameId) => _resolve().CommitLostHpChainedPayment(frameId);
    internal ProgramSkillFrame FreezeLostHpChainedPayment(ProgramSkillFrame frame, string sourceBind, string? exceptBind, SkillProgramCardDestination destination) => _resolve().FreezeLostHpChainedPayment(frame, sourceBind, exceptBind, destination);
    internal bool IsLostHpChainedComposition(ProgramSkillFrame frame) => _resolve().IsLostHpChainedComposition(frame);
    internal bool ReturnLostHpChainedMovement(ProgramSkillFrame frame) => _resolve().ReturnLostHpChainedMovement(frame);
    internal bool ValidLostHpChainedPayment(ProgramSkillFrame frame) => _resolve().ValidLostHpChainedPayment(frame);
}
