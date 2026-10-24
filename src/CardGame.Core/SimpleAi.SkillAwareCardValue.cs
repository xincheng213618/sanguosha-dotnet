namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    /// <summary>
    /// Prices a physical payment using only the owner's published cards and
    /// response capabilities. These are reserve estimates, not assertions that
    /// a future response will be legal: the issued response prompt remains the
    /// authority for actual conversions and their current limits.
    /// </summary>
    private static double GetSkillAwareCardKeepValue(
        GameSnapshot view, PlayerSnapshot self, CardSnapshot card)
    {
        var printedValue = CardCatalog.Get(card.Kind).HandKeepValue;
        var usable = self.Hand.Concat(self.WoodenOxGrain ?? []).DistinctBy(item => item.Id).ToArray();
        if (usable.All(item => item.Id != card.Id)) return printedValue;

        var skills = (self.Skills ?? []).Concat(self.SecondarySkills ?? [])
            .Where(skill => !skill.Tags.HasFlag(SkillTag.Lord) || self.Role == Role.Lord)
            .ToArray();
        bool CanRespond(CardSnapshot material, CardKind output) => material.Kind == output ||
            skills.Any(skill => CanViewAs(skill, material, output, forPlay: false));
        var value = (double)printedValue;

        if (CanRespond(card, CardKind.Dodge))
        {
            var alternatives = usable.Count(material => CanRespond(material, CardKind.Dodge));
            var dodgeValue = alternatives == 1
                ? 70d + (self.Hp <= 2 ? 65d : 25d)
                : alternatives == 2 ? self.Hp <= 2 ? 65d : 60d : 55d;
            // A view-as material retains its printed utility too. Several
            // capabilities on one entity never count as several spare cards.
            value = card.Kind == CardKind.Dodge ? dodgeValue : Math.Max(value, dodgeValue);
        }

        if (CanRespond(card, CardKind.Peach))
        {
            var alternatives = usable.Count(material => CanRespond(material, CardKind.Peach));
            var peachValue = alternatives == 1
                ? self.Hp <= 2 ? 105d : self.Hp < self.MaxHp ? 80d : 70d
                : self.Hp <= 2 ? 80d : 60d;
            value = Math.Max(value, peachValue);
        }

        if (CanRespond(card, CardKind.Alcohol))
        {
            var rescueCount = usable.Count(material => CanRespond(material, CardKind.Alcohol) ||
                CanRespond(material, CardKind.Peach));
            if (self.Hp <= 1 && rescueCount == 1)
                value = Math.Max(value, 135d);
            else if (card.Kind == CardKind.Alcohol && self.Hp > 2 &&
                     usable.Count(material => material.Kind == CardKind.Alcohol) > 1)
                value = Math.Max(printedValue - 8d, value - 8d);
        }

        // Ordinary excess attack cards lose some reserve value when the owner
        // would otherwise discard. Never discount a card's defensive capability.
        var handLimit = self.HandLimit ?? Math.Max(0, self.Hp);
        if (view.CurrentSeat == self.Seat && value <= printedValue && IsSlashCard(card.Kind) && self.HandCount > handLimit &&
            usable.Count(material => IsSlashCard(material.Kind)) > 1 &&
            !CanRespond(card, CardKind.Dodge) && !CanRespond(card, CardKind.Peach) &&
            !CanRespond(card, CardKind.Alcohol))
            value -= Math.Min(10d, (self.HandCount - handLimit) * 3d);

        return Math.Max(0d, value);
    }

    /// <summary>
    /// Adds the opportunity cost missing from ordinary one-card play scoring.
    /// Active-skill payments use the same keep value in their existing selection
    /// and cost calculation. Historical Slash reserve charges are retained once.
    /// </summary>
    private static (double Score, string Reason) ScoreSkillAwareAction(
        GameSnapshot view, PlayerSnapshot self, LegalAction action,
        (double Score, string Reason) baseline)
    {
        if (baseline.Score <= 0 || action.CardId is not > 0 ||
            action.Kind is LegalActionKind.EndPlay or LegalActionKind.UseProgramSkill or
                LegalActionKind.UseEquipmentEffect or LegalActionKind.Peach or LegalActionKind.Alcohol)
            return baseline;
        var card = self.Hand.Concat(self.WoodenOxGrain ?? []).FirstOrDefault(item => item.Id == action.CardId);
        if (card is null) return baseline;

        var adjustment = (GetSkillAwareCardKeepValue(view, self, card) -
                          CardCatalog.Get(card.Kind).HandKeepValue) * .35d;
        var previousReserve = action.Kind == LegalActionKind.Slash && action.TargetSeats.Count <= 1 &&
                              !IsSlashCard(card.Kind)
            ? card.Kind switch
            {
                CardKind.Peach => self.Hp < self.MaxHp ? 105d : 65d,
                CardKind.Dodge when self.Hand.Count(item => item.Kind == CardKind.Dodge) == 1 => self.Hp <= 2 ? 40d : 15d,
                CardKind.Alcohol when self.Hp <= 1 => 75d,
                _ => 0d
            }
            : 0d;
        // A high legacy reserve is not refunded, and does not get charged twice.
        if (previousReserve > 0) adjustment = Math.Max(0d, adjustment - previousReserve);
        if (Math.Abs(adjustment) < .001d) return baseline;
        return (baseline.Score - adjustment,
            baseline.Reason + $" 按自身手牌、体力和技能响应用途调整实体牌机会成本 {adjustment:0.#} 分。");
    }

    /// <summary>
    /// A native Duel asks its target for Slash first. Estimate whether that
    /// target can answer more times than the source's remaining physical
    /// response materials. Public hand count is a prior, never a read of the
    /// target's actual cards, and visible conversions do not promise availability.
    /// </summary>
    private static (double Score, string Reason) ScoreDuelReadiness(
        GameSnapshot view, PlayerSnapshot self, LegalAction action,
        (double Score, string Reason) baseline)
    {
        if (baseline.Score <= 0 || action.Kind != LegalActionKind.Duel || action.CardId is not > 0 ||
            action.TargetSeats is not [var targetSeat] || action.ConversionSource is not null ||
            action.AdditionalConversionSources is { Count: > 0 } ||
            action.PlayedCardKind is { } played && played != CardKind.Duel ||
            !self.Hand.Any(card => card.Id == action.CardId && card.Kind == CardKind.Duel))
            return baseline;
        var target = view.Players.FirstOrDefault(player => player.Seat == targetSeat && player.IsAlive);
        if (target is null || target.HandCount <= 0) return baseline;

        var ownSkills = ResponseSkills(self);
        var responses = self.Hand.Concat(self.WoodenOxGrain ?? [])
            .Where(card => card.Id != action.CardId).DistinctBy(card => card.Id)
            .Count(card => IsSlashCard(card.Kind) || ownSkills.Any(skill =>
                skill.ViewAsOpportunities?.Any(rule => rule.ForResponse && IsSlashCard(rule.OutputKind) &&
                    CanViewAs(skill, card, rule.OutputKind, forPlay: false)) == true));
        var trials = Math.Min(target.HandCount, 32);
        if (responses >= trials) return baseline;

        var hasResponseConversion = ResponseSkills(target).Any(skill =>
            skill.ViewAsOpportunities?.Any(rule => rule.ForResponse && IsSlashCard(rule.OutputKind) &&
                (rule.InputKinds.Count == 0 || rule.InputKinds.Any(kind => !IsSlashCard(kind)))) == true);
        var prior = hasResponseConversion ? .45d : .25d;
        // Exact binomial CDF on a deliberately bounded public-hand model.
        // Target wins only if it has at least responses + 1 Slash materials.
        var term = Math.Pow(1d - prior, trials);
        var cumulative = term;
        for (var count = 0; count < responses; count++)
        {
            term *= (trials - count) / (double)(count + 1) * prior / (1d - prior);
            cumulative += term;
        }
        var failure = Math.Clamp(1d - cumulative, 0d, 1d);
        var survivalReserve = (long)self.Hp + Math.Max(0, self.Armor);
        var criticalRisk = self.HpUndefined ? 0d : survivalReserve <= 1 ? 160d : survivalReserve == 2 ? 65d : 0d;
        var risk = failure * (85d + criticalRisk);
        if (risk < .001d) return baseline;
        return (baseline.Score - risk,
            baseline.Reason + $" 决斗响应储备 {responses} 张，对手公开手牌 {target.HandCount} 张；" +
            $"按 {prior:P0} 响应先验估计自身先耗尽概率 {failure:P0}，扣除风险 {risk:0.#} 分。");

        static GeneralSkillDefinition[] ResponseSkills(PlayerSnapshot player) =>
            (player.Skills ?? []).Concat(player.SecondarySkills ?? [])
                .Where(skill => !skill.Tags.HasFlag(SkillTag.Lord) || player.Role == Role.Lord).ToArray();
    }
}
