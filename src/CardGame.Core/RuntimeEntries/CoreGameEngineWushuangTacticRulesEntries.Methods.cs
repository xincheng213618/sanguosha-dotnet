#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangTacticRulesEntries
{
    internal RuleQueryEvaluation ApplyWushuangEnemyQuota(CharacterState player, RuleQueryEvaluation ordinary, SkillRuleQuery query) => _resolve().ApplyWushuangEnemyQuota(player, ordinary, query);
    internal void ChangeWushuangFreeRerolls(int seat, int delta, string reason) => _resolve().ChangeWushuangFreeRerolls(seat, delta, reason);
    internal bool HasWushuangTactic(int seat, int id) => _resolve().HasWushuangTactic(seat, id);
    internal bool IsWushuangHandLimitExempt(CharacterState owner, Card card) => _resolve().IsWushuangHandLimitExempt(owner, card);
    internal IEnumerable<RuleQueryContribution> WushuangNumericRuleContributions(CharacterState owner, SkillRuleQuery query) => _resolve().WushuangNumericRuleContributions(owner, query);
}
