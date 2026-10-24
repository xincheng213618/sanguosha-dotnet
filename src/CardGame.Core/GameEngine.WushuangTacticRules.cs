namespace CardGame.Core;

public sealed partial class GameEngine
{
    private void ChangeWushuangFreeRerolls(int seat, int delta, string reason) => RequireWushuangFeature.ChangeWushuangFreeRerolls(seat, delta, reason);
    private bool HasWushuangTactic(int seat, int id) => WushuangFeature?.HasWushuangTactic(seat, id) ?? false;
    private bool IsWushuangHandLimitExempt(CharacterState owner, Card card) => WushuangFeature?.IsWushuangHandLimitExempt(owner.Seat, card.Kind) ?? false;
    private IEnumerable<RuleQueryContribution> WushuangNumericRuleContributions(CharacterState owner, SkillRuleQuery query) =>
        WushuangFeature?.WushuangNumericRuleContributions(owner.Seat, query) ?? [];
    private RuleQueryEvaluation ApplyWushuangEnemyQuota(CharacterState player, RuleQueryEvaluation ordinary, SkillRuleQuery query) =>
        WushuangFeature?.ApplyWushuangEnemyQuota(player.Seat, ordinary, query) ?? ordinary;
}
