#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelReturnsEntries
{
    internal void AssertConditionalDiscardDuels() => _resolve().AssertConditionalDiscardDuels();
    internal void CompleteConditionalDiscardDuelAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteConditionalDiscardDuelAttackReturn(completion);
    internal bool IsConditionalDiscardDuelUse(long id) => _resolve().IsConditionalDiscardDuelUse(id);
    internal bool MatchesConditionalDuelOrigin(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ConditionalDiscardDuelOrigin o) => _resolve().MatchesConditionalDuelOrigin(f, o);
    internal void ReturnConditionalDiscardDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnConditionalDiscardDuel(use);
    internal bool ReturnRegisteredCompleteConditionalDiscardDuelAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteConditionalDiscardDuelAttackReturn(completion);
    internal bool ValidConditionalDuelPayments(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidConditionalDuelPayments(f);
}
