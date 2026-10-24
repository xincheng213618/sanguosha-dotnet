namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private (double Score, string Reason) ScoreRebelCoordinationAction(
        GameSnapshot view, PlayerSnapshot self, Role role, LegalAction action,
        IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        var scored = ScoreIdentityAction(view, self, role, action, legalActions, baseline, damagePrediction);
        if (role != Role.Rebel || !UsesIdentityObjectives(view, self) || scored.Score <= 0 ||
            !IsIdentityDamageAction(action.Kind) || action.TargetSeats is not [var seat] ||
            view.Players.FirstOrDefault(player => player.IsAlive && player.Seat == seat) is not { Role: null } target)
            return scored;
        var rebel = GetIdentityRoleProbability(view, target, Role.Rebel);
        if (rebel < .4d || rebel <= GetIdentityRoleProbability(view, target, Role.Loyalist) ||
            Math.Abs(_rebelSuspicion.GetValueOrDefault(seat)) >= .5d)
            return scored;
        return (-5d, scored.Reason +
            " 反贼协同：公开人数使此未知角色更可能是同伴，且尚无足够定向行动线索，暂缓盲目消耗伤害牌。");
    }

    private (double Score, string Reason) ScoreRebelPressureAction(
        GameSnapshot view, PlayerSnapshot self, Role role, LegalAction action,
        IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (!UsesIdentityObjectives(view, self) || baseline.Score <= 0) return baseline;
        var lord = view.Players.FirstOrDefault(player => player.IsAlive && player.Role == Role.Lord);
        if (lord is null) return baseline;
        if (action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage)
        {
            var payment = RebelPublicPayment(self, lord, action, damagePrediction);
            if (!payment.IsLethal && view.Players.Any(player => player.IsAlive && player.Seat != Seat &&
                player.Seat != lord.Seat && !player.HpUndefined && player.HandCount == 0 &&
                GetIdentityRoleProbability(view, player, Role.Rebel) >= .6d &&
                RebelPublicPayment(self, player, action, damagePrediction).IsLethal))
                return (-5d, baseline.Reason + " 反贼节奏：不让无手牌且高置信的同伴为无终结机会的群伤濒死。");
            return !lord.HpUndefined && payment.HpLossAmount > 0
                ? AddIdentityObjective(baseline, (payment.IsLethal ? 85d : 25d) /
                    (1d + Math.Min(lord.HandCount, 8) * .18d), "群伤计入公开攻主收益，手牌只折扣新增收益。")
                : baseline;
        }
        if (action.TargetSeats is not [var seat] ||
            view.Players.FirstOrDefault(player => player.IsAlive && player.Seat == seat) is not { } target)
            return baseline;
        var damaging = IsIdentityDamageAction(action.Kind);
        var removal = action.Kind is LegalActionKind.Dismantlement or LegalActionKind.Snatch;
        if (!damaging && !removal && action.Kind is not (LegalActionKind.Indulgence or LegalActionKind.SupplyShortage))
            return baseline;
        if (removal && target.Judgment.Any(card => card.Id == action.TargetCardId)) return baseline;
        var paymentToTarget = damaging && !target.HpUndefined
            ? RebelPublicPayment(self, target, action, damagePrediction) : null;
        if (paymentToTarget is { HpLossAmount: <= 0, ArmorAbsorbed: <= 0, MaximumHpLossAmount: <= 0 })
            return (-5d, baseline.Reason + " 反贼节奏：不消耗当前公开预测完全无效的攻击。");
        if (target.Seat == lord.Seat)
        {
            if (paymentToTarget is { HpLossAmount: <= 0, MaximumHpLossAmount: <= 0 })
                return (Math.Min(baseline.Score, 18d + paymentToTarget.ArmorAbsorbed * 4d),
                    baseline.Reason + " 反贼节奏：仅剥甲时保留有限收益，不计作直接损血。");
            var objective = ScoreIdentityAction(view, self, role, action, legalActions, baseline, damagePrediction);
            if (paymentToTarget is not { HpLossAmount: > 0 }) return objective;
            // Keep the already priced material and response risk. Public hand
            // count discounts only this additional objective, never those costs.
            var exposure = action.Kind == LegalActionKind.Slash
                ? 1d / (1d + Math.Min(lord.HandCount, 20) * .18d) : 1d;
            return AddIdentityObjective(objective, 40d * exposure +
                Math.Max(0, paymentToTarget.HpLossAmount - 1) * 12d,
                "在原有资源与生存估值上兑现攻主节奏，不因未知闪牌削弱整段用牌价值。");
        }
        var canFinish = paymentToTarget?.IsLethal == true;
        if (!canFinish && HasEffectiveRebelLordAttack(view, self, lord, legalActions, damagePrediction))
            return AddIdentityObjective(baseline, -30d, "已有有效攻主动作，减少不能结束侧路战斗的资源分散。");
        var rebel = GetIdentityRoleProbability(view, target, Role.Rebel);
        var loyalist = GetIdentityRoleProbability(view, target, Role.Loyalist);
        var pressure = damaging ? Math.Min(18, Math.Max(0, target.MaxHp - target.Hp) * 4) +
            Math.Max(0, 3 - Math.Min(target.HandCount, 3)) * 4d : 0d;
        return AddIdentityObjective(baseline, pressure + 16d * loyalist - 16d * rebel,
            "按公开薄弱点及阵营概率集中侧路，保留结束局部战斗的机会。");
    }

    // Called only after the ordinary material, Duel and armor costs. It replaces
    // the policy-seven Rebel objective rather than stacking another bonus on it.
    private (double Score, string Reason) ScoreRebelAction(
        GameSnapshot view, PlayerSnapshot self, Role role, LegalAction action,
        IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction = null)
    {
        if (role != Role.Rebel || !UsesIdentityObjectives(view, self) || baseline.Score <= 0)
            return baseline;
        var lord = view.Players.FirstOrDefault(player => player.IsAlive && player.Role == Role.Lord);
        if (lord is null) return baseline;

        if (action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage)
        {
            var endsLord = !lord.HpUndefined && RebelPublicPayment(self, lord, action, damagePrediction).IsLethal;
            var losesAlly = view.Players.Where(player => player.IsAlive && player.Seat != Seat && player.Seat != lord.Seat)
                .Any(player => !player.HpUndefined && GetIdentityRoleProbability(view, player, Role.Rebel) >= .6d &&
                    RebelPublicPayment(self, player, action, damagePrediction).IsLethal);
            if (losesAlly && !endsLord)
                return (-5d, baseline.Reason + " 反贼协同：群伤会令高置信同伴濒死，且不能同时终结主公，保留此牌。");
            return endsLord ? AddIdentityObjective(baseline, 70d,
                "反贼群伤有公开可见的终结主公机会，优先争取阵营胜利。") : baseline;
        }
        if (action.TargetSeats is not [var seat] ||
            view.Players.FirstOrDefault(player => player.IsAlive && player.Seat == seat) is not { } target)
            return baseline;
        var damaging = IsIdentityDamageAction(action.Kind);
        var removal = action.Kind is LegalActionKind.Dismantlement or LegalActionKind.Snatch;
        if (!damaging && !removal && action.Kind is not (LegalActionKind.Indulgence or LegalActionKind.SupplyShortage))
            return baseline;
        if (removal && target.Judgment.Any(card => card.Id == action.TargetCardId)) return baseline;

        if (damaging)
        {
            if (target.HpUndefined) return baseline;
            var payment = RebelPublicPayment(self, target, action, damagePrediction);
            if (payment.HpLossAmount <= 0 && payment.ArmorAbsorbed <= 0 && payment.MaximumHpLossAmount <= 0)
                return (-5d, baseline.Reason + " 反贼协同：当前公开伤害预测既不能损血也不能消耗护甲，保留攻击资源。");
            if (target.Seat == lord.Seat)
            {
                if (payment.HpLossAmount <= 0 && payment.MaximumHpLossAmount <= 0)
                    return (Math.Min(baseline.Score, 18d + payment.ArmorAbsorbed * 4d),
                        baseline.Reason + " 反贼协同：本次只消耗主公护甲，按有限剥甲收益投入，不计作直接终结压力。");
                // Hand count is a conservative exposure prior, not knowledge of
                // a Dodge. Duel already paid its own response-risk estimate.
                var exposure = action.Kind == LegalActionKind.Slash
                    ? 1d / (1d + Math.Clamp(lord.HandCount, 0, 20) * .18d) : 1d;
                var score = baseline.Score * exposure + 12d * Math.Min(2, payment.HpLossAmount) +
                    (payment.IsLethal ? 80d : 8d);
                return (score, baseline.Reason +
                    " 反贼协同：按公开主公手牌数量与实际损血潜力集中火力，不把潜在命中当作必然伤害。");
            }
        }
        else if (target.Seat == lord.Seat)
        {
            // Short planning already compares the actual defense and competing
            // fire Slashes. Only retain its prepared, independently paid attack.
            var preparesSlash = removal && action.TargetCardId is { } removed &&
                target.Equipment.Any(card => card.Id == removed && EquipmentCatalog.IsEquipment(card.Kind) &&
                    EquipmentCatalog.Get(card.Kind).Slot == EquipmentSlot.Armor) &&
                legalActions.Any(candidate => candidate.Kind == LegalActionKind.Slash &&
                    candidate.TargetSeats is [var attackSeat] && attackSeat == lord.Seat &&
                    candidate.CardId is > 0 && candidate.CardId != action.CardId &&
                    self.Hand.Any(card => card.Id == candidate.CardId));
            return AddIdentityObjective(baseline, preparesSlash ? 12d : 4d,
                preparesSlash ? "反贼优先兑现已有独立费用的攻主拆防顺序，结算后重新判断。" :
                    "反贼对主公的控制只计有限目标价值，不取代有效伤害。");
        }

        if (HasEffectiveRebelLordAttack(view, self, lord, legalActions, damagePrediction))
            return AddIdentityObjective(baseline, -36d,
                "已有能损血或消耗护甲的合法攻主动作，减少边路分散火力。");
        var rebel = GetIdentityRoleProbability(view, target, Role.Rebel);
        var loyalist = GetIdentityRoleProbability(view, target, Role.Loyalist);
        var weakPoint = damaging ? Math.Min(12, Math.Max(0, target.MaxHp - target.Hp) * 3) +
            Math.Max(0, 3 - Math.Min(target.HandCount, 3)) * 3d : 0d;
        return AddIdentityObjective(baseline, 12d * loyalist - 12d * rebel + weakPoint,
            "当前没有有效攻主动作，按公开薄弱点和阵营概率集中侧路；已判友军的拒绝仍保留。");
    }

    private bool HasEffectiveRebelLordAttack(GameSnapshot view, PlayerSnapshot self, PlayerSnapshot lord,
        IReadOnlyList<LegalAction> actions, Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (lord.HpUndefined) return false;
        if (_identityLordAttackAvailable is { } cached) return cached;
        var considered = 0;
        foreach (var candidate in actions)
        {
            if (candidate.TargetSeats is not [var seat] || seat != lord.Seat || !IsIdentityDamageAction(candidate.Kind))
                continue;
            if (++considered > 12) break;
            var payment = RebelPublicPayment(self, lord, candidate, damagePrediction);
            if (payment.HpLossAmount <= 0 && payment.ArmorAbsorbed <= 0 && payment.MaximumHpLossAmount <= 0) continue;
            var score = ScoreShortPlanAction(view, self, Role.Rebel, candidate, actions,
                ScoreDuelReadiness(view, self, candidate, ScoreSkillAwareAction(view, self, candidate,
                    ScoreTacticalAction(view, self, Role.Rebel, candidate, actions, damagePrediction))), damagePrediction).Score;
            if (score > 0) return (_identityLordAttackAvailable = true).Value;
        }
        return (_identityLordAttackAvailable = false).Value;
    }

    private static PublicDamagePrediction RebelPublicPayment(PlayerSnapshot self, PlayerSnapshot target,
        LegalAction action, Func<LegalAction, int, PublicDamagePrediction>? prediction)
    {
        if (prediction is not null) return prediction(action, target.Seat);
        var amount = action.Kind == LegalActionKind.Slash && self.HasAlcoholEffect
            ? 1 + Math.Max(1, self.AlcoholDamageBonus ?? 1) : 1;
        if (target.IncomingDamageCap is { } cap) amount = Math.Min(amount, cap);
        return PublicDamagePrediction.Calculate(target.Hp, target.Armor, amount, maxHp: target.MaxHp);
    }

    private double ScoreRebelDyingResponse(GameSnapshot view, Role role, PlayerSnapshot victim, double baseline)
    {
        var self = view.Players.Single(player => player.Seat == Seat);
        if (role != Role.Rebel || !UsesIdentityObjectives(view, self) || victim.Seat == Seat || victim.Role == Role.Lord)
            return baseline;
        var rebel = GetIdentityRoleProbability(view, victim, Role.Rebel);
        if (!self.HpUndefined && self.Hp <= 1 && self.Hand.Count(card => card.Kind == CardKind.Peach) == 1 && rebel < .85d)
            return Math.Min(-40d, baseline);
        return baseline;
    }

    private double ScoreRebelNullificationFavor(GameSnapshot view, Role role, PlayerSnapshot? target,
        CardKind effectKind, double baseline)
    {
        var self = view.Players.Single(player => player.Seat == Seat);
        if (role != Role.Rebel || target is null || !UsesIdentityObjectives(view, self) || target.Seat == Seat ||
            GetIdentityRoleProbability(view, target, Role.Rebel) < .6d) return baseline;
        if (effectKind is CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage &&
            !target.HpUndefined && target.Hp <= 1)
            return Math.Min(-90d, baseline);
        return baseline;
    }
}
