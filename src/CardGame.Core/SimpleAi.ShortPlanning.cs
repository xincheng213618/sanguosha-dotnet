namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    // A deliberately bounded, one-continuation estimate. Both steps must already
    // be published by Core. We do not manufacture future legal actions, simulate
    // hidden hands, or promise that movement-triggered skills leave a combo legal.
    private (double Score, string Reason) ScoreShortPlanAction(
        GameSnapshot view,
        PlayerSnapshot self,
        Role role,
        LegalAction action,
        IReadOnlyList<LegalAction> legalActions,
        (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction = null)
    {
        if (baseline.Score <= 0) return baseline;
        if (action.Kind == LegalActionKind.Slash)
            return ScoreCurrentSlashArmor(view, self, role, action, baseline, damagePrediction);
        if (action.Kind is not (LegalActionKind.Dismantlement or LegalActionKind.Snatch) ||
            action.TargetSeat is not { } targetSeat || action.TargetCardId is not { } armorId ||
            action.CardId is not > 0 || !self.Hand.Any(card => card.Id == action.CardId))
            return baseline;

        var target = view.Players.FirstOrDefault(player => player.Seat == targetSeat && player.IsAlive);
        if (target is null || GetHostility(view, role, target) <= 15)
            return baseline;
        var armor = target.Equipment.FirstOrDefault(card => card.Id == armorId && !card.IsBone);
        if (armor is null || !EquipmentCatalog.IsEquipment(armor.Kind) ||
            (armor.OccupiedEquipmentSlot ?? EquipmentCatalog.Get(armor.Kind).Slot) != EquipmentSlot.Armor)
            return baseline;

        // Removing a wounded enemy's Silver Lion heals it even if no attack
        // follows. In particular, do not rescue a one-HP enemy before attacking.
        var removalCost = armor.Kind == CardKind.SilverLion && target.Hp < target.MaxHp
            ? target.Hp <= 1 ? 72d : 38d
            : 0d;
        var bestCurrentAttack = double.NegativeInfinity;
        var bestPreparedAttack = double.NegativeInfinity;
        var considered = 0;
        foreach (var followUp in legalActions)
        {
            if (followUp.Kind != LegalActionKind.Slash || followUp.TargetSeats.Count != 1 ||
                followUp.TargetSeat != targetSeat || followUp.CardId is not > 0 || followUp.CardId == action.CardId ||
                followUp.TieredRoundZeroUse is not null || followUp.DrawFundedDistinctBasicUse is not null ||
                followUp.ChainedStateBasicUse is not null)
                continue;
            var material = self.Hand.FirstOrDefault(card => card.Id == followUp.CardId);
            if (material is null) continue;
            // The payment must actually remain after step one. A card used for
            // Qixi cannot also be the Slash material. Public-pile uses have
            // separate authority and are intentionally outside this small plan.
            var kind = followUp.PlayedCardKind ?? material.Kind;
            if (!IsSlashCard(kind)) continue;
            var followUpScore = ScoreSkillAwareAction(view, self, followUp,
                ScoreTacticalAction(view, self, role, followUp, legalActions, damagePrediction)).Score;
            if (followUpScore <= 0) continue;
            var prediction = damagePrediction?.Invoke(followUp, targetSeat);
            var estimate = EstimatePublicArmorForSlash(self, target, armor.Kind, followUp.SlashTraits, kind, prediction);
            // Compare the best attack on either side, rather than selecting the
            // largest isolated gain. A normal Slash blocked by Vine must not
            // justify removing it when another available fire Slash benefits.
            var currentArmorAdjustment = armor.Kind == CardKind.SilverLion ? 0d : -estimate.RemovalGain;
            var currentAttack = estimate.FullyBlocks ? -1d : followUpScore + currentArmorAdjustment;
            bestCurrentAttack = Math.Max(bestCurrentAttack, currentAttack);
            bestPreparedAttack = Math.Max(bestPreparedAttack,
                armor.Kind == CardKind.SilverLion ? followUpScore + estimate.RemovalGain : followUpScore);
            if (++considered == 12) break;
        }

        var bestContinuationGain = double.IsNegativeInfinity(bestPreparedAttack)
            ? 0d : bestPreparedAttack - bestCurrentAttack;
        // Discount a prospective second action: Nullification, card movement and
        // skills may change the table. Actual settlement always causes a fresh
        // choice from a newly published action list; this stores no pending plan.
        var sequenceAdjustment = bestContinuationGain > 0 ? bestContinuationGain * .8d : bestContinuationGain;
        var adjustment = Math.Clamp(sequenceAdjustment - removalCost, -96d, 64d);
        if (adjustment == 0) return baseline;
        var reason = removalCost > 0
            ? $" 失去白银狮子会使受伤敌人回复体力，计入 {removalCost:0.#} 分代价。"
            : string.Empty;
        if (bestContinuationGain != 0)
            reason += bestContinuationGain > 0
                ? $" 已有另一张独立费用的合法杀，一步规划预计先移除【{armor.DisplayName}】可改善后续攻击；结算后重新判断。"
                : $" 当前合法杀可利用【{armor.DisplayName}】的火焰弱点，先移除会损失后续攻击收益。";
        return (baseline.Score + adjustment, baseline.Reason + reason);
    }

    private (double Score, string Reason) ScoreCurrentSlashArmor(
        GameSnapshot view,
        PlayerSnapshot self,
        Role role,
        LegalAction action,
        (double Score, string Reason) baseline,
        Func<LegalAction, int, PublicDamagePrediction>? damagePrediction)
    {
        if (action.TargetSeats.Count != 1 || action.TargetSeat is not { } targetSeat ||
            action.CardId is not > 0 || action.TieredRoundZeroUse is not null ||
            action.DrawFundedDistinctBasicUse is not null || action.ChainedStateBasicUse is not null)
            return baseline;
        var target = view.Players.FirstOrDefault(player => player.Seat == targetSeat && player.IsAlive);
        var material = self.Hand.FirstOrDefault(card => card.Id == action.CardId);
        var minimumHostility = _policyVersion >= AiDecisionPolicy.IdentityStrategyVersion &&
            view.ModeKind == ContentModeKind.Identity ? 0d : 15d;
        if (target is null || material is null || GetHostility(view, role, target) <= minimumHostility)
            return baseline;
        var armor = target.Equipment.FirstOrDefault(card => !card.IsBone && EquipmentCatalog.IsEquipment(card.Kind) &&
            (card.OccupiedEquipmentSlot ?? EquipmentCatalog.Get(card.Kind).Slot) == EquipmentSlot.Armor);
        // The existing tactical scorer already prices Silver Lion's damage cap;
        // only the preparation path refunds its disappearing penalty.
        if (armor is null || armor.Kind == CardKind.SilverLion) return baseline;
        var kind = action.PlayedCardKind ?? material.Kind;
        if (!IsSlashCard(kind)) return baseline;
        var estimate = EstimatePublicArmorForSlash(self, target, armor.Kind, action.SlashTraits, kind,
            damagePrediction?.Invoke(action, targetSeat));
        if (estimate.FullyBlocks)
            return (-1d, baseline.Reason + $" 公开【{armor.DisplayName}】会令这张杀无效，保留实体牌，先寻找移除防具或其他合法攻击。");
        var gain = estimate.RemovalGain;
        if (gain == 0) return baseline;
        return (baseline.Score - gain, baseline.Reason + (gain > 0
            ? $" 公开【{armor.DisplayName}】会阻挡或抵御这张杀，降低当前攻击收益 {gain:0.#} 分。"
            : $" 这张火杀可利用公开【{armor.DisplayName}】的火焰弱点，提高当前攻击收益 {-gain:0.#} 分。"));
    }

    private readonly record struct PublicArmorSlashEstimate(double RemovalGain = 0d, bool FullyBlocks = false);

    private static PublicArmorSlashEstimate EstimatePublicArmorForSlash(
        PlayerSnapshot self,
        PlayerSnapshot target,
        CardKind armor,
        PublicSlashTraits? traits,
        CardKind slashKind,
        PublicDamagePrediction? prediction)
    {
        // Equipment definitions are public. A known bypass weapon or a published
        // direct HP/max-HP replacement makes armor preparation unnecessary.
        var ignoresArmor = traits?.IgnoresArmor ?? self.Equipment.Any(card => !card.IsBone &&
            EquipmentCatalog.IsEquipment(card.Kind) && EquipmentCatalog.Get(card.Kind).IgnoresArmor);
        if (ignoresArmor ||
            prediction is { DirectHpLossAmount: > 0 } or { MaximumHpLossAmount: > 0 })
            return new();
        var nature = traits?.Nature ?? (slashKind == CardKind.FireSlash ||
            self.Equipment.Any(card => card.Kind == CardKind.CrimsonSoulZither && !card.IsBone)
                ? DamageNature.Fire : slashKind == CardKind.ThunderSlash ? DamageNature.Thunder :
                  slashKind == CardKind.ColdSlash ? DamageNature.Cold : DamageNature.Normal);
        var fire = nature == DamageNature.Fire;
        return armor switch
        {
            // Physical card color is insufficient: Liubing and other native
            // appearance rules may change this exact use. Only Core's published
            // declaration traits establish a certainly black Slash.
            CardKind.RenwangShield when traits?.IsRed == false => new(FullyBlocks: true),
            CardKind.QimenEightTrigrams => new(FullyBlocks: true),
            // Vine stops an ordinary Slash at the card-effect layer before
            // damage conversion. A fire/thunder damage trait does not turn an
            // ordinary Slash card into a Fire/Thunder Slash card.
            CardKind.Tengjia when slashKind == CardKind.Slash => new(FullyBlocks: true),
            // Keep the publicly known extra fire damage. The last two HP make
            // destroying this vulnerability especially costly for a follow-up.
            CardKind.Tengjia when fire && target.IncomingDamageCap is not 1 =>
                new(target.Hp <= 2 && target.Armor == 0 ? -84d : -44d),
            CardKind.BaguaFormation => new(26d),
            CardKind.SilverLion when (self.HasAlcoholEffect || target.HandCount == 0 &&
                self.Equipment.Any(card => card.Kind == CardKind.GudingBlade)) &&
                target.IncomingDamageCap is not 1 => new(36d),
            // The registered robe rule prevents fire and thunder damage, not
            // cold damage or an elemental card whose damage became normal.
            CardKind.RedCottonRobe when nature is DamageNature.Fire or DamageNature.Thunder => new(FullyBlocks: true),
            _ => new()
        };
    }
}
