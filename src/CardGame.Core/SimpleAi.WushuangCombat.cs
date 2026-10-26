namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private (double Score, string Reason) ScoreWushuangCombatAction(GameSnapshot view, PlayerSnapshot self,
        Role role, LegalAction action, IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction = null)
    {
        if (view.Wushuang is null || baseline.Score <= 0) return baseline;
        if (_policyVersion >= AiDecisionPolicy.WushuangRefinementVersion && action.Kind == LegalActionKind.DrawTwo)
            return ScoreWushuangDrawPreparation(view, self, role, action, legalActions, baseline, damagePrediction);
        if (_policyVersion >= AiDecisionPolicy.WushuangLossReviewVersion &&
            action.Kind is LegalActionKind.IronChain or LegalActionKind.Equip)
            return ScoreWushuangLossReviewPreparation(view, self, role, action, legalActions, baseline, damagePrediction);
        if (action.Kind == LegalActionKind.Alcohol && self.WushuangTacticIds?.Contains(2210) == true &&
            self.WushuangTacticIds?.Contains(2001) != true)
            return (baseline.Score - 16d, baseline.Reason + " 无双战法：稳定士气将伤害固定为二，饮酒不能增加这次伤害，先保留酒。");
        if (action.Kind == LegalActionKind.Alcohol && self.WushuangTacticIds?.Contains(2001) != true &&
            self.WushuangBuild is { } wineBuild &&
            (wineBuild.SlashBurstBonus > 0 || wineBuild.SlashSealsTargetCards) && damagePrediction is not null &&
            legalActions.Any(slash => slash.Kind == LegalActionKind.Slash && slash.CardId != action.CardId &&
                IsWushuangOwnHandSlash(self, slash) && slash.TargetSeats is [var targetSeat] &&
                damagePrediction(slash, targetSeat).Wushuang is { TargetPayment.IsLethal: true }))
            return (baseline.Score - 18d, baseline.Reason + " 当前构筑的合法杀已可终结，直接爆发并保留独立的酒，不再多付一次准备。");
        if (damagePrediction is null || action.Kind is not (LegalActionKind.Slash or LegalActionKind.Duel or
            LegalActionKind.FireAttack or LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage)) return baseline;
        var target = view.Players.SingleOrDefault(player => player.IsAlive && player.Seat != Seat);
        if (target is null) return baseline;
        var legacy = damagePrediction(action, target.Seat);
        if (legacy.Wushuang is not { } combat) return baseline;
        var payment = combat.TargetPayment;
        if (payment.HpLossAmount <= 0 && payment.ArmorAbsorbed <= 0 && payment.MaximumHpLossAmount <= 0)
        {
            var needed = combat.IncomingReductionSegmentsRemaining;
            var independentSlashes = legalActions.Where(candidate => candidate.Kind == LegalActionKind.Slash &&
                candidate.TargetSeats is [var seat] && seat == target.Seat && candidate.CardId is > 0 &&
                self.Hand.Any(card => card.Id == candidate.CardId && IsSlashCard(card.Kind)))
                .Select(candidate => candidate.CardId).Distinct().Count();
            if (action.Kind == LegalActionKind.Slash && needed > 0 && independentSlashes > needed &&
                combat.RemainingOrdinarySlashUses > needed &&
                !(self.Hp <= 1 && target.WushuangTacticIds?.Contains(2116) == true))
                return (baseline.Score - Math.Min(60d, needed * 18d), baseline.Reason +
                    " 无双战法：已有足够独立实体杀与公开出杀额度，可先消耗有限减伤；每次结算后重新判断。");
            return (-5d, baseline.Reason + " 无双战法：公开战法会使当前伤害完全无效，保留实体攻击资源。");
        }
        if (combat.RetaliationIfTargetSurvives is { IsLethal: true } && !payment.IsLethal)
            return (-180d, baseline.Reason + " 无双战法：对手存活后的公开反伤会使自己濒死，先恢复或保留资源。");
        var adjustment = (payment.HpLossAmount - legacy.HpLossAmount) * 18d +
            (payment.IsLethal == legacy.IsLethal ? 0d : payment.IsLethal ? 45d : -45d) -
            (combat.RetaliationIfTargetSurvives?.HpLossAmount ?? 0) * 18d;
        if (combat.DrunkenSlashUnrespondable) adjustment += Math.Min(target.HandCount, 6) * 3d;
        if (action.Kind == LegalActionKind.Slash && IsWushuangOwnHandSlash(self, action) && self.WushuangBuild is { } build &&
            payment.DamageAmount > 0 && payment.DirectHpLossAmount == 0 && payment.MaximumHpLossAmount == 0 &&
            (payment.IsLethal || combat.RetaliationIfTargetSurvives is null or { HpAfter: > 2 }))
        {
            // The profile describes potential conditions. Credit extra damage
            // only when this actual public prediction supports it; sealing has
            // bounded value from visible hand count, never hidden card faces.
            var supportedBurst = combat.FixedDamage ? 0 : Math.Min(Math.Max(0, build.SlashBurstBonus),
                Math.Max(0, payment.DamageAmount - 1 - (self.HasAlcoholEffect ? Math.Max(1, self.AlcoholDamageBonus ?? 1) : 0)));
            var buildValue = Math.Min(3, supportedBurst) * 12 + (build.SlashSealsTargetCards ? Math.Min(2, target.HandCount) * 8 : 0);
            if (buildValue > 0)
            { adjustment += buildValue; baseline = (baseline.Score, baseline.Reason + " 当前构筑的实际加伤或封牌支持及时使用这张合法杀。"); }
        }
        return adjustment == 0 ? baseline : (baseline.Score + Math.Clamp(adjustment, -100d, 100d),
            baseline.Reason + " 无双战法：按公开战法的实际单段损血、终结机会及存活后的反伤调整资源收益。");
    }

    private static bool IsWushuangOwnHandSlash(PlayerSnapshot self, LegalAction action) => action.Kind == LegalActionKind.Slash &&
        action.CardId is > 0 && self.Hand.Any(card => card.Id == action.CardId) && action.TargetSeats.Count == 1;

    private double WushuangSafeInjuryValue(GameSnapshot view, PlayerSnapshot self, int attackerSeat,
        PublicDamagePrediction? incoming, bool legalDodge)
    {
        if (_policyVersion < AiDecisionPolicy.WushuangStrategyVersion || view.Wushuang is not { RemainingTurns: > 4, RemainingMilliseconds: > 60_000 } ||
            self.WushuangBuild is not { } build || self.HpUndefined || self.Hp <= 2 || self.IsChained || incoming is null ||
            !double.IsFinite(build.InjuryDrawPerDamagePoint) || !double.IsFinite(build.InjuryRecoverPerDamagePoint)) return 0;
        var payment = incoming.Wushuang?.TargetPayment ?? incoming;
        if (payment.IsLethal || payment.HpBefore != self.Hp || payment.DamageAmount != 1 || payment.HpLossAmount != 1 ||
            payment.HpAfter < 2 || payment.DirectHpLossAmount != 0 || payment.MaximumHpLossAmount != 0 || payment.MaxHpAfter < self.MaxHp) return 0;
        var attacker = view.Players.SingleOrDefault(p => p.Seat == attackerSeat && p.IsAlive && p.Seat != self.Seat);
        if (attacker is null) return 0;
        if (payment.HpAfter == 2 && (!legalDodge || !self.Hand.Any(card => card.Kind == CardKind.Dodge) || attacker.HandCount > 1 ||
            attacker.Equipment.Any(card => card.Kind == CardKind.Crossbow) || incoming.Wushuang?.RemainingOrdinarySlashUses > 1 ||
            attacker.WushuangTacticIds?.Any(id => id is 2051 or 2052 or 2156 or 2157 or 2158 or 2226 or 2282) == true ||
            attacker.WushuangBuild is { SlashBurstBonus: > 0 } or { SlashSealsTargetCards: true })) return 0;
        var benefit = Math.Min(4, Math.Max(0, build.InjuryDrawPerDamagePoint)) * 38 +
            Math.Min(1, Math.Max(0, build.InjuryRecoverPerDamagePoint)) * 76;
        return benefit <= 0 ? 0 : Math.Max(0, benefit - 20 + (legalDodge ? 16 : 0));
    }
}
