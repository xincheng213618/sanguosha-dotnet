namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private double? ScoreWushuangDeferredPhaseTarget(GameSnapshot view, PlayerSnapshot target, SkillProgramAiHint hint)
    {
        if (_policyVersion < AiDecisionPolicy.WushuangRefinementVersion || view.Wushuang is null ||
            !HasWushuangDeferredPhase(hint)) return null;
        var self = view.Players.Single(player => player.Seat == Seat);
        var own = target.Seat == Seat;
        if (!own && hint.TargetHpLoss > 0 && target.Hp <= hint.TargetHpLoss)
            return null; // Preserve the existing immediate-lethal priority; there is no later draw for this branch.
        var reachesNextDraw = view.Wushuang.RemainingTurns is not { } turns || turns >= (own ? 2 : 1);
        var clock = view.Wushuang.RemainingMilliseconds <= 5_000 ? 0 :
            view.Wushuang.RemainingMilliseconds < 60_000 ? .25 : 1;
        var value = WushuangDeferredPhaseNet(self, target, hint.TargetHpLoss, hint.TargetDraw,
            hint.TargetNextDrawBonus, hint.TargetNextDiscardHandLimitPenalty, reachesNextDraw ? clock : 0,
            target.HandLimit ?? Math.Max(0, target.Hp));
        // Active-skill base priority is twelve. A zero-net deferred exchange
        // must not become a profitable action merely because it is legal.
        return value <= 0 ? value * 4 - 16 : value * 4;
    }

    private static bool HasWushuangDeferredPhase(SkillProgramAiHint hint) =>
        hint.TargetNextDrawBonus > 0 || hint.TargetNextDiscardHandLimitPenalty > 0;

    private static double? ShopRefinedDeferredPhaseActivation(SkillProgramActivation activation,
        PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned, double horizon)
    {
        var hint = WushuangDeferredPhaseAi.Describe(activation, new(0, 0, 0, 0, 0, 0, false, false));
        if (!HasWushuangDeferredPhase(hint)) return null;
        var effect = activation.Effects[1];
        var hpLoss = effect.Op == SkillProgramEffectOp.LoseHp ? effect.Amount : 0;
        var draw = effect.Op == SkillProgramEffectOp.Draw ? effect.Amount : 0;
        var targets = ShopReviewedTargetParties(activation.TargetKind, ShopReviewParty.Other, self, opponent);
        if (targets.Length == 0) return 0;
        return targets.Select(party => party == ShopReviewParty.Owner ? self : party == ShopReviewParty.Other ? opponent : null)
            .Where(target => target is not null).Cast<PlayerSnapshot>().Select(target =>
            {
                var own = target.Seat == self.Seat;
                var futureDraw = Math.Clamp(horizon - (own ? 1 : .5), 0, 1);
                return WushuangDeferredPhaseNet(self, target, hpLoss, draw, hint.TargetNextDrawBonus,
                    hint.TargetNextDiscardHandLimitPenalty, futureDraw,
                    own ? ShopDeferredHandLimit(self, owned) : target.HandLimit ?? Math.Max(0, target.Hp));
            }).DefaultIfEmpty(0).Max();
    }

    private static int ShopDeferredHandLimit(PlayerSnapshot self, SkillProgram[] owned)
    {
        if (self.HandLimit is { } known) return Math.Max(0, known);
        var modifiers = owned.SelectMany(program => program.Modifiers).Where(modifier =>
            modifier.Query == SkillRuleQuery.HandLimit && modifier.RecipientScope == SkillRuleRecipientScope.Owner)
            .Select(modifier => (Modifier: modifier, Active: ShopDrawConditionApplies(modifier.Condition, self),
                Amount: ShopKnownModifierAmount(modifier, self))).ToArray();
        if (modifiers.Any(item => item.Active is null || item.Active == true && item.Amount is null))
            return Math.Max(0, self.Hp);
        var contributions = modifiers.Where(item => item.Active == true).Select((item, index) =>
            (RuleQueryContribution)new FiniteRuleQueryContribution("shop-deferred-limit:" + index,
                item.Modifier.Operation, item.Amount!.Value, item.Modifier.Priority)).ToArray();
        return ShopReducedFiniteCount(Math.Max(0, self.Hp), contributions) ?? Math.Max(0, self.Hp);
    }

    private static double WushuangDeferredPhaseNet(PlayerSnapshot self, PlayerSnapshot target,
        int hpLoss, int draw, int nextDraw, int handLimitPenalty, double nextDrawChance, int handLimit)
    {
        var own = target.Seat == self.Seat;
        if (target.HpUndefined) return 0;
        if (hpLoss > 0 && target.Hp <= hpLoss)
            return own ? -250 : hpLoss * 6 + 18;
        var signed = own ? 1 : -1;
        var hpValue = own ? ShopHealthValue(self) : 6;
        var afterDraw = (long)target.HandCount + Math.Max(0, draw);
        var beforeOverflow = Math.Max(0L, afterDraw - handLimit);
        var afterOverflow = Math.Max(0L, afterDraw - Math.Max(0, handLimit - handLimitPenalty));
        var penalty = Math.Max(0, afterOverflow - beforeOverflow) * 3;
        return signed * (draw * 7 - hpLoss * hpValue + nextDraw * 7 * nextDrawChance - penalty);
    }
}
