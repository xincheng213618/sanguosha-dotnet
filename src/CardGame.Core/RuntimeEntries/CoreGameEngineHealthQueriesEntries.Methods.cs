#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHealthQueriesEntries
{
    internal IEnumerable<int> ExpressionHpSubjects(ProgramExpression expression, ProgramSkillFrame frame, int? iterator = default) => _resolve().ExpressionHpSubjects(expression, frame, iterator);
    internal IReadOnlyList<LegalAction> HappyQiaoActionQueryUnion(Func<IReadOnlyList<LegalAction>> query) => _resolve().HappyQiaoActionQueryUnion(query);
    internal bool TryBeginHappyQiaoAttackHpLossChoice(GameEngine.IDamageAttempt attack) => _resolve().TryBeginHappyQiaoAttackHpLossChoice(attack);
    internal bool TryBeginHappyQiaoInstructionQuery(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TryBeginHappyQiaoInstructionQuery(frame, effect);
    internal int? UnambiguousHp(CharacterState player) => _resolve().UnambiguousHp(player);
}
