#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashReturnsEntries
{
    internal global::CardGame.Core.RuleQueryEvaluation ApplyInspectedHandDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.RuleQueryEvaluation prior) => _resolve().ApplyInspectedHandDistance(actor, target, prior);
    internal bool CanIssueInspectedHandSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanIssueInspectedHandSlash(f);
    internal void CompleteInspectedHandSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteInspectedHandSlash(completion);
    internal void IssueInspectedHandSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueInspectedHandSlash(f);
    internal bool ReturnRegisteredCompleteInspectedHandSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteInspectedHandSlash(completion);
    internal bool ValidInspectedHandSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidInspectedHandSlash(f);
}
