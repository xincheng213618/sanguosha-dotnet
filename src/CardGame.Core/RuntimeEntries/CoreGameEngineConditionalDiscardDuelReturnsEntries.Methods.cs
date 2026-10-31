#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelReturnsEntries
{
    internal void CompleteConditionalDiscardDuelAttackReturn(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteConditionalDiscardDuelAttackReturn(completion);
    internal bool IsConditionalDiscardDuelUse(long id) => _resolve().IsConditionalDiscardDuelUse(id);
    internal bool MatchesConditionalDuelOrigin(ProgramSkillFrame f, ConditionalDiscardDuelOrigin o) => _resolve().MatchesConditionalDuelOrigin(f, o);
    internal void ReturnConditionalDiscardDuel(CardUseFrame use) => _resolve().ReturnConditionalDiscardDuel(use);
    internal bool ReturnRegisteredCompleteConditionalDiscardDuelAttackReturn(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteConditionalDiscardDuelAttackReturn(completion);
    internal bool ValidConditionalDuelPayments(ProgramSkillFrame f) => _resolve().ValidConditionalDuelPayments(f);
}
