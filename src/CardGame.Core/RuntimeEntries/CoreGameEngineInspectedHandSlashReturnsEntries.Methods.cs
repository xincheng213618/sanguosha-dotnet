#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashReturnsEntries
{
    internal RuleQueryEvaluation ApplyInspectedHandDistance(CharacterState actor, CharacterState target, RuleQueryEvaluation prior) => _resolve().ApplyInspectedHandDistance(actor, target, prior);
    internal bool CanIssueInspectedHandSlash(ProgramSkillFrame f) => _resolve().CanIssueInspectedHandSlash(f);
    internal void CompleteInspectedHandSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteInspectedHandSlash(completion);
    internal void IssueInspectedHandSlash(ProgramSkillFrame f) => _resolve().IssueInspectedHandSlash(f);
    internal bool ReturnRegisteredCompleteInspectedHandSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteInspectedHandSlash(completion);
    internal bool ValidInspectedHandSlash(ProgramSkillFrame f) => _resolve().ValidInspectedHandSlash(f);
}
