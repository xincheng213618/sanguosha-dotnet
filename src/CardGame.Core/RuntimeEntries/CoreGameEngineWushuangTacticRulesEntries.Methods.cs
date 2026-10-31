#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangTacticRulesEntries
{
    internal global::CardGame.Core.RuleQueryEvaluation ApplyWushuangEnemyQuota(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary, global::CardGame.Core.SkillRuleQuery query) => _resolve().ApplyWushuangEnemyQuota(player, ordinary, query);
    internal void ChangeWushuangFreeRerolls(int seat, int delta, string reason) => _resolve().ChangeWushuangFreeRerolls(seat, delta, reason);
    internal bool HasWushuangTactic(int seat, int id) => _resolve().HasWushuangTactic(seat, id);
    internal bool IsWushuangHandLimitExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsWushuangHandLimitExempt(owner, card);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> WushuangNumericRuleContributions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillRuleQuery query) => _resolve().WushuangNumericRuleContributions(owner, query);
}
