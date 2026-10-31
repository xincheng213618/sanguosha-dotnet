#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHealthQueriesEntries
{
    internal global::System.Collections.Generic.IEnumerable<int> ExpressionHpSubjects(global::CardGame.Core.ProgramExpression expression, global::CardGame.Core.ProgramSkillFrame frame, int? iterator = default) => _resolve().ExpressionHpSubjects(expression, frame, iterator);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> HappyQiaoActionQueryUnion(global::System.Func<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction>> query) => _resolve().HappyQiaoActionQueryUnion(query);
    internal bool TryBeginHappyQiaoAttackHpLossChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().TryBeginHappyQiaoAttackHpLossChoice(attack);
    internal bool TryBeginHappyQiaoInstructionQuery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TryBeginHappyQiaoInstructionQuery(frame, effect);
    internal int? UnambiguousHp(global::CardGame.Core.CharacterState player) => _resolve().UnambiguousHp(player);
}
