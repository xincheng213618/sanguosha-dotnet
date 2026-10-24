namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    // Reset by ChoosePlay before scoring. This is query-local memoization, not
    // a plan or evidence that can survive changed actions between calls.
    private bool? _identityLordAttackAvailable;

    private void ResetIdentityDecisionCache() => _identityLordAttackAvailable = null;

    /// <summary>
    /// Prices the identity camp's objective after ordinary card/material scoring.
    /// It never changes legality or turns a rejected attack back into a use.
    /// </summary>
    private (double Score, string Reason) ScoreIdentityAction(
        GameSnapshot view, PlayerSnapshot self, Role role, LegalAction action,
        IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction = null)
    {
        if (!UsesIdentityObjectives(view, self) || baseline.Score <= 0) return baseline;
        var lord = view.Players.FirstOrDefault(player => player.IsAlive && player.Role == Role.Lord);
        if (lord is null) return baseline;
        var adjustment = 0d;
        var reason = string.Empty;

        if (action.Kind == LegalActionKind.Peach)
        {
            if (role == Role.Lord && !self.HpUndefined && self.Hp <= 2)
            {
                adjustment = self.Hp <= 1 ? 100d : 80d;
                reason = "主公先恢复危险体力，降低阵营立即失败的风险。";
            }
            else if (role == Role.Loyalist && !lord.HpUndefined && lord.Hp <= 1 &&
                     !self.HpUndefined && self.Hp >= 2 &&
                     self.Hand.Count(card => card.Kind == CardKind.Peach) == 1)
            {
                return (-5d, baseline.Reason +
                    " 身份目标：主公仅剩一血，自己尚能存活，保留最后一张实体桃用于救主。" );
            }
            return AddIdentityObjective(baseline, adjustment, reason);
        }

        if (action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage)
        {
            if (role is Role.Loyalist or Role.Renegade && lord.Seat != Seat &&
                !lord.HpUndefined && lord.Hp <= 2)
            {
                var harmsLord = damagePrediction is null
                    ? lord.Armor == 0
                    : damagePrediction(action, lord.Seat).HpLossAmount > 0;
                if (harmsLord)
                    return AddIdentityObjective(baseline, -70d,
                        "主公体力危险，群体伤害会削弱必须保住的主公。" );
            }
            return baseline;
        }

        if (action.TargetSeats is not [var targetSeat] ||
            view.Players.FirstOrDefault(player => player.Seat == targetSeat && player.IsAlive) is not { } target)
            return baseline;
        var damaging = IsIdentityDamageAction(action.Kind);
        var offensive = damaging || action.Kind is LegalActionKind.Dismantlement or LegalActionKind.Snatch or
            LegalActionKind.Indulgence or LegalActionKind.SupplyShortage;
        if (!offensive) return baseline;
        // Removing a harmful delayed card is support, rather than pressure.
        if (action.Kind is LegalActionKind.Dismantlement or LegalActionKind.Snatch &&
            action.TargetCardId is { } removed && target.Judgment.Any(card => card.Id == removed))
            return baseline;
        var lethal = damaging && IdentityActionMayFinish(self, target, action, damagePrediction);
        var rebel = GetIdentityRoleProbability(view, target, Role.Rebel);
        var loyalist = GetIdentityRoleProbability(view, target, Role.Loyalist);

        if (role == Role.Rebel)
        {
            if (target.Seat == lord.Seat)
            {
                adjustment = 20d + Math.Max(0, 4 - Math.Min(lord.HandCount, 4)) * 3d;
                if (!lord.HpUndefined && lord.Hp <= 2) adjustment += 22d;
                if (lethal) adjustment += 64d;
                reason = "反贼以击败主公为胜利目标，优先形成对主公的有效压力。";
            }
            else if (!lethal && HasProfitableIdentityLordAttack(view, self, role, lord, legalActions, damagePrediction))
            {
                adjustment = -18d;
                reason = "已有可用的对主公攻击，减少无关边路消耗。";
            }
        }
        else if (role is Role.Lord or Role.Loyalist)
        {
            var isLordThreat = target.Seat != lord.Seat &&
                _rebelSuspicion.GetValueOrDefault(target.Seat) >= 1.5d;
            if (isLordThreat)
            {
                adjustment += (!lord.HpUndefined && lord.Hp <= 2 ? 36d : 16d) * rebel;
                reason = "优先压制公开行动显示正在威胁主公的角色。";
            }
            if (lethal)
            {
                adjustment += 18d * rebel;
                if (role == Role.Lord)
                    adjustment -= loyalist * Math.Min(90d, self.HandCount * 10d + self.Equipment.Count * 15d);
                reason += " 击杀收益按身份概率计算，主公误杀忠臣的弃牌损失计入风险。";
            }
        }
        else if (role == Role.Renegade && damaging && lethal && target.Seat != lord.Seat &&
                 !lord.HpUndefined && lord.Hp >= 3 && view.Players.Count(player => player.IsAlive) > 3)
        {
            if (ShouldPreserveIdentityLastRebel(view, role, target))
                return (-5d, baseline.Reason +
                    " 身份目标：公开人数与强烈身份线索显示这是最后的反贼，主公健康且主忠明显占优，暂缓致命攻击以保留牵制。" );
            var balance = IdentityCampBalance(view);
            if (balance.RebelCount <= 1.25d && balance.LoyalistCount >= .7d && rebel >= .65d &&
                balance.LoyalStrength > balance.RebelStrength + 2d)
            {
                adjustment -= 70d;
                reason = "内奸避免在主忠占优时过早消灭最后的反贼，保留双方牵制。";
            }
        }
        return AddIdentityObjective(baseline, Math.Clamp(adjustment, -110d, 110d), reason);
    }

    private double GetIdentityRenegadeHostility(
        GameSnapshot view, PlayerSnapshot target, double rebelProbability, double loyalistProbability)
    {
        if (target.Seat == Seat) return -1000d;
        var alive = view.Players.Count(player => player.IsAlive);
        if (target.Role == Role.Lord)
            return alive <= 2 ? 120d : -80d;
        if (alive <= 2) return 100d;
        var balance = IdentityCampBalance(view);
        if (balance.Lord is { HpUndefined: false, Hp: <= 2 } && balance.RebelCount > .1d)
            return rebelProbability * 95d - loyalistProbability * 35d;
        var tilt = Math.Clamp((balance.LoyalStrength - balance.RebelStrength) /
            Math.Max(6d, (balance.LoyalStrength + balance.RebelStrength) * .4d), -1d, 1d);
        return rebelProbability * (55d - 30d * tilt) + loyalistProbability * (55d + 30d * tilt);
    }

    private double ScoreIdentityDyingResponse(
        GameSnapshot view, Role role, PlayerSnapshot victim, double baseline)
    {
        var self = view.Players.Single(player => player.Seat == Seat);
        if (!UsesIdentityObjectives(view, self) || victim.Seat == Seat) return baseline;
        var alive = view.Players.Count(player => player.IsAlive);
        if (victim.Role == Role.Lord)
            return role is Role.Lord or Role.Loyalist || role == Role.Renegade && alive > 2
                ? Math.Max(170d, baseline) : Math.Min(-100d, baseline);
        var rebel = GetIdentityRoleProbability(view, victim, Role.Rebel);
        var loyalist = GetIdentityRoleProbability(view, victim, Role.Loyalist);
        if (role is Role.Lord or Role.Loyalist)
            return loyalist >= .6d && loyalist > rebel + .2d &&
                   GetIdentityHostility(view, role, victim) < -15d ? Math.Max(60d, baseline) : -40d;
        if (role == Role.Rebel)
            return rebel >= .6d && rebel > loyalist + .2d ? Math.Max(60d, baseline) : -40d;
        if (role != Role.Renegade || alive <= 3 || self.HpUndefined || self.Hp < 3 || self.HandCount < 2)
            return baseline;
        var balance = IdentityCampBalance(view);
        if (balance.Lord is { HpUndefined: false, Hp: <= 2 }) return -40d;
        var gap = balance.LoyalStrength - balance.RebelStrength;
        if (gap > 4d && rebel >= .7d || gap < -4d && loyalist >= .7d)
            return 24d;
        return baseline;
    }

    private double ScoreIdentityNullificationFavor(GameSnapshot view, Role role,
        PlayerSnapshot? target, CardKind effectKind, double baseline)
    {
        var self = view.Players.Single(player => player.Seat == Seat);
        if (target is null || !UsesIdentityObjectives(view, self) ||
            !ShouldPreserveIdentityLastRebel(view, role, target) || target.HpUndefined)
            return baseline;
        if (effectKind == CardKind.PeachGarden)
            return target.Hp >= target.MaxHp ? 0d : 80d;
        // This is the actual public counterspell window. At one HP these cards
        // pose a potential lethal risk; no hypothetical attack is predicted.
        if (target.Hp <= 1 && effectKind is CardKind.Duel or CardKind.FireAttack or
            CardKind.BarbarianAssault or CardKind.ArrowBarrage)
            return -80d;
        return baseline;
    }

    private bool ShouldPreserveIdentityLastRebel(GameSnapshot view, Role role, PlayerSnapshot target)
    {
        if (role != Role.Renegade || !target.IsAlive || target.Role == Role.Lord ||
            view.Players.Count(player => player.IsAlive) <= 3)
            return false;
        var balance = IdentityCampBalance(view);
        return balance.Lord is { HpUndefined: false, Hp: >= 3 } &&
            balance.RebelCount <= 1.05d && balance.LoyalistCount >= .75d &&
            GetIdentityRoleProbability(view, target, Role.Rebel) >= .85d &&
            balance.LoyalStrength >= balance.RebelStrength + 6d;
    }

    private IdentityBalance IdentityCampBalance(GameSnapshot view)
    {
        var loyalStrength = 0d;
        var rebelStrength = 0d;
        var loyalists = 0d;
        var rebels = 0d;
        PlayerSnapshot? lord = null;
        foreach (var player in view.Players.Where(player => player.IsAlive && player.Seat != Seat))
        {
            var isLord = player.Role == Role.Lord;
            if (isLord) lord = player;
            var loyal = GetIdentityRoleProbability(view, player, Role.Loyalist);
            var rebel = GetIdentityRoleProbability(view, player, Role.Rebel);
            var health = player.HpUndefined ? 3d : Math.Max(0, player.Hp);
            var strength = 1d + health * 1.3d + Math.Max(0, player.Armor) * .6d +
                Math.Clamp(player.HandCount, 0, 6) * .35d + Math.Min(player.Equipment.Count, 4) * .6d;
            loyalStrength += ((isLord ? 1d : 0d) + loyal) * strength;
            rebelStrength += rebel * strength;
            loyalists += loyal;
            rebels += rebel;
        }
        return new(loyalStrength, rebelStrength, loyalists, rebels, lord);
    }

    private bool HasProfitableIdentityLordAttack(
        GameSnapshot view, PlayerSnapshot self, Role role, PlayerSnapshot lord,
        IReadOnlyList<LegalAction> actions, Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (_identityLordAttackAvailable is { } available) return available;
        var considered = 0;
        foreach (var candidate in actions)
        {
            if (candidate.TargetSeats is not [var seat] || seat != lord.Seat || !IsIdentityDamageAction(candidate.Kind))
                continue;
            if (++considered > 12) return (_identityLordAttackAvailable = false).Value;
            var score = ScoreShortPlanAction(view, self, role, candidate, actions,
                ScoreDuelReadiness(view, self, candidate, ScoreSkillAwareAction(view, self, candidate,
                    ScoreTacticalAction(view, self, role, candidate, actions, damagePrediction))), damagePrediction).Score;
            if (score > 0) return (_identityLordAttackAvailable = true).Value;
        }
        return (_identityLordAttackAvailable = false).Value;
    }

    private static bool IdentityActionMayFinish(PlayerSnapshot self, PlayerSnapshot target, LegalAction action,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (target.HpUndefined) return false;
        if (damagePrediction is not null) return damagePrediction(action, target.Seat).IsLethal;
        var amount = action.Kind == LegalActionKind.Slash && self.HasAlcoholEffect
            ? 1L + Math.Max(1, self.AlcoholDamageBonus ?? 1) : 1L;
        if (target.IncomingDamageCap is { } cap) amount = Math.Min(amount, cap);
        return target.Hp <= Math.Max(0L, amount - target.Armor);
    }

    private static bool IsIdentityDamageAction(LegalActionKind kind) =>
        kind is LegalActionKind.Slash or LegalActionKind.Duel or LegalActionKind.FireAttack;

    private static bool UsesIdentityObjectives(GameSnapshot view, PlayerSnapshot self) =>
        view.ModeKind == ContentModeKind.Identity && self.TeamId is null &&
        !view.Players.Any(player => player.TeamId is not null);

    private static (double Score, string Reason) AddIdentityObjective(
        (double Score, string Reason) baseline, double adjustment, string reason) =>
        adjustment == 0d ? baseline : (baseline.Score + adjustment, baseline.Reason + " 身份目标：" + reason);

    private sealed record IdentityBalance(double LoyalStrength, double RebelStrength,
        double LoyalistCount, double RebelCount, PlayerSnapshot? Lord);
}
