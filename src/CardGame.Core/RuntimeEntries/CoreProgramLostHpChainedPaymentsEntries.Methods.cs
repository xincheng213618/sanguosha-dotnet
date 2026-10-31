#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramLostHpChainedPaymentsEntries
{
    internal void AssertLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertLostHpChainedPayment(frame);
    internal bool CanContinuePaidLostHpChain(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePaidLostHpChain(frame);
    internal void CommitLostHpChainedPayment(long frameId) => _resolve().CommitLostHpChainedPayment(frameId);
    internal global::CardGame.Core.ProgramSkillFrame FreezeLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, string? exceptBind, global::CardGame.Core.SkillProgramCardDestination destination) => _resolve().FreezeLostHpChainedPayment(frame, sourceBind, exceptBind, destination);
    internal bool IsLostHpChainedComposition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsLostHpChainedComposition(frame);
    internal bool ReturnLostHpChainedMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnLostHpChainedMovement(frame);
    internal bool ValidLostHpChainedPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidLostHpChainedPayment(frame);
}
