#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangTacticRulesRuntime
{
    internal void ChangeWushuangFreeRerolls(int seat, int delta, string reason) => _coreWushuangOperations.RequireWushuangFeature.ChangeWushuangFreeRerolls(seat, delta, reason);
    internal bool HasWushuangTactic(int seat, int id) => _coreWushuangOperations.WushuangFeature?.HasWushuangTactic(seat, id) ?? false;
    internal bool IsWushuangHandLimitExempt(CharacterState owner, Card card) => _coreWushuangOperations.WushuangFeature?.IsWushuangHandLimitExempt(owner.Seat, card.Kind) ?? false;
    internal IEnumerable<RuleQueryContribution> WushuangNumericRuleContributions(CharacterState owner, SkillRuleQuery query) => _coreWushuangOperations.WushuangFeature?.WushuangNumericRuleContributions(owner.Seat, query) ?? [];
    internal RuleQueryEvaluation ApplyWushuangEnemyQuota(CharacterState player, RuleQueryEvaluation ordinary, SkillRuleQuery query) => _coreWushuangOperations.WushuangFeature?.ApplyWushuangEnemyQuota(player.Seat, ordinary, query) ?? ordinary;
}
