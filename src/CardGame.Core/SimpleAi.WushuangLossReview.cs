namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private (double Score, string Reason) ScoreWushuangLossReviewPreparation(GameSnapshot view, PlayerSnapshot self,
        Role role, LegalAction action, IReadOnlyList<LegalAction> legalActions, (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (view.Players.Count(player => player.IsAlive) != 2) return baseline;
        if (action.Kind == LegalActionKind.IronChain && !self.IsChained &&
            self.WushuangBuild is { HasChainSynergy: false } &&
            action.TargetSeats.All(seat => view.Players.SingleOrDefault(player => player.Seat == seat && player.IsAlive) is { IsChained: false }) &&
            action.TargetSeats.Any(seat => seat != self.Seat &&
                GetHostility(view, role, view.Players.Single(player => player.Seat == seat)) > 15))
            return (-5d, baseline.Reason + " 单骑无双：当前双方均未连环，也没有自身技能的连环或用牌联动，不为横置敌人而附带横置自己，优先重铸补牌。");

        // This small plan handles a published, independent native Slash and one
        // specific weapon bonus. It does not generally promote equipment, infer
        // hidden responses, or create an attack after its quota has been spent.
        if (action.Kind != LegalActionKind.Equip || damagePrediction is null || view.CurrentSeat != self.Seat ||
            action.CardId is not > 0 || !self.Hand.Any(card => card.Id == action.CardId && card.Kind == CardKind.GudingBlade) ||
            self.Equipment.Any(card => (card.OccupiedEquipmentSlot ?? EquipmentCatalog.Get(card.Kind).Slot) == EquipmentSlot.Weapon)) return baseline;

        var best = baseline.Score;
        foreach (var slash in legalActions)
        {
            if (slash.Kind != LegalActionKind.Slash || slash.CardId is not > 0 || slash.CardId == action.CardId ||
                slash.ConversionSource is not null || slash.AdditionalConversionSources is { Count: > 0 } ||
                !self.Hand.Any(card => card.Id == slash.CardId && IsSlashCard(card.Kind)) ||
                slash.TargetSeats is not [var targetSeat]) continue;
            var target = view.Players.SingleOrDefault(player => player.Seat == targetSeat && player.IsAlive);
            if (target is null || target.HandCount != 0 || GetHostility(view, role, target) <= 15 ||
                target.Equipment.Any(card => (card.OccupiedEquipmentSlot ?? EquipmentCatalog.Get(card.Kind).Slot) == EquipmentSlot.Armor)) continue;
            var prediction = damagePrediction(slash, targetSeat);
            if (prediction.Wushuang is not { FixedDamage: false, RemainingOrdinarySlashUses: > 0,
                IncomingReductionSegmentsRemaining: 0 } combat) continue;
            var payment = combat.TargetPayment;
            if (payment.IsLethal || payment.DamageAmount <= 0 || payment.DirectHpLossAmount != 0 || payment.MaximumHpLossAmount != 0) continue;
            var amount = (int)Math.Min(int.MaxValue, (long)payment.DamageAmount + 1);
            if (target.IncomingDamageCap is { } cap) amount = Math.Min(amount, cap);
            var prepared = PublicDamagePrediction.Calculate(target.Hp, target.Armor, amount, maxHp: target.MaxHp);
            if (!prepared.IsLethal) continue;
            var currentScore = ScorePlanningAction(view, self, role, slash, legalActions,
                ScoreTacticalAction(view, self, role, slash, legalActions, damagePrediction), damagePrediction).Score;
            if (currentScore > 0) best = Math.Max(best, currentScore + 45d);
        }
        return best <= baseline.Score ? baseline : (best, baseline.Reason +
            " 单骑无双：已有独立实体杀和剩余额度，先装备古锭刀可使对公开空手目标的下一杀达到斩杀伤害，结算后重新判断。");
    }
}
