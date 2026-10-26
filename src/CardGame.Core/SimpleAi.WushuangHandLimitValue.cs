namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private static double? ShopHandLimitValue(SkillProgram offered, PlayerSnapshot self, SkillProgram[] owned, int draws)
    {
        var modifiers = offered.Modifiers.Where(modifier => modifier.Query == SkillRuleQuery.HandLimit &&
            modifier.RecipientScope == SkillRuleRecipientScope.Owner).ToArray();
        if (modifiers.Length == 0) return 0;
        if (self.HpUndefined) return null;
        var background = owned.SelectMany(program => program.Modifiers).Where(modifier =>
                modifier.Query == SkillRuleQuery.HandLimit && modifier.RecipientScope == SkillRuleRecipientScope.Owner)
            .Select(modifier => (Modifier: modifier, Amount: ShopKnownModifierAmount(modifier, self),
                Active: ShopDrawConditionApplies(modifier.Condition, self))).ToArray();
        var additions = modifiers.Select(modifier => (Modifier: modifier, Amount: ShopKnownModifierAmount(modifier, self),
            Active: ShopDrawConditionApplies(modifier.Condition, self))).ToArray();
        if (background.Concat(additions).Any(item => item.Active is null || item.Active == true && item.Amount is null)) return null;
        var before = background.Where(item => item.Active == true).Select((item, index) =>
            (RuleQueryContribution)new FiniteRuleQueryContribution("shop-owned-hand-limit:" + index,
                item.Modifier.Operation, item.Amount!.Value, item.Modifier.Priority)).ToArray();
        var added = additions.Where(item => item.Active == true).Select((item, index) =>
            (item.Modifier, Contribution: (RuleQueryContribution)new FiniteRuleQueryContribution("shop-added-hand-limit:" + index,
                item.Modifier.Operation, item.Amount!.Value, item.Modifier.Priority))).ToArray();
        // A Set replaces the HP base; it never adds its printed amount. Price
        // the complete group against the same remaining build for replacements.
        if (ShopReducedFiniteCount(Math.Max(0, self.Hp), before) is not { } limit ||
            ShopReducedFiniteCount(Math.Max(0, self.Hp), before.Concat(added.Where(item =>
                item.Modifier.Condition.Kind == SkillProgramConditionKind.Always).Select(item => item.Contribution)).ToArray()) is not { } guaranteed ||
            ShopReducedFiniteCount(Math.Max(0, self.Hp), before.Concat(added.Select(item => item.Contribution)).ToArray()) is not { } possible)
            return null;
        var marginal = guaranteed - limit + (possible - guaranteed) * .5;
        var unit = (long)self.HandCount + Math.Max(0, draws) > limit ? 2.5 : .7;
        return marginal * unit;
    }
}
