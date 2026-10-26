using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryLiuFengXiansi, SkillProgramEffectOp.ResolveBoundaryLiuFengQinling)]
    private PromptChoice SelectAiBoundaryLiuFeng(PendingDecision decision, ProgramSkillFrame frame)
    {
        var view = CreateSnapshot(decision.PlayerSeat);
        var self = view.Players.Single(p => p.Seat == decision.PlayerSeat);
        var brain = _aiBrains.GetValueOrDefault(decision.PlayerSeat) ?? new SimpleAiBrain(decision.PlayerSeat,
            unchecked(_options.Seed * 397) ^ (decision.PlayerSeat + 1), _options.AiPolicyVersion);
        var gift = new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false);
        var damage = new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false)
        { TargetDamage = 1, TargetHealthEffects = [new(PublicHealthEffectKind.Damage, 1)] };
        double CollectValue(PromptChoice choice)
        {
            var count = int.Parse(choice.Parameters["count"], CultureInfo.InvariantCulture);
            var value = 0d;
            for (var i = 0; i < count; i++)
            {
                var seat = int.Parse(choice.Parameters[$"{i}.card-owner-seat"], CultureInfo.InvariantCulture);
                var zone = Enum.Parse<CardZoneKind>(choice.Parameters[$"{i}.source-zone"]);
                var slot = int.Parse(choice.Parameters[$"{i}.slot-index"], CultureInfo.InvariantCulture);
                var target = view.Players.Single(p => p.Seat == seat);
                var hostility = -brain.ScoreProgramTarget(view, seat, gift) / 28;
                // Foreign hand slots deliberately have the same unknown value.
                var card = zone == CardZoneKind.Equipment ? target.Equipment.ElementAtOrDefault(slot) :
                    seat == self.Seat ? self.Hand.ElementAtOrDefault(slot) : null;
                var keep = card is null ? 5 : CardCatalog.Get(card.Kind).HandKeepValue;
                value += hostility * keep;
                if (card?.Kind == CardKind.SilverLion && target.Hp < target.MaxHp && hostility < 0) value += 16 * -hostility;
            }
            // Every newly completed pair is a real extra Slash resource for the
            // opponent. A fragile owner need not fill an optional second slot.
            var pairs = (self.AuthorityCount + count) / 2 - self.AuthorityCount / 2;
            return value - pairs * (self.Hp <= 2 ? 9 : 3);
        }
        double CostValue(PromptChoice choice)
        {
            var card = self.Hand.Concat(self.Equipment).Single(c => choice.Cards.Contains(c.Id));
            // Match the common active-program opportunity-cost scale before
            // comparing with public damage and the possible one-card reward.
            var keep = CardCatalog.Get(card.Kind).HandKeepValue * .35d;
            if (card.Kind == CardKind.Peach && self.Hp <= 2) keep = Math.Max(keep, self.Hp <= 1 ? 36 : 20);
            if (card.Kind == CardKind.Dodge && self.Hp <= 2 && self.Hand.Count(c => c.Kind == CardKind.Dodge) <= 1) keep = Math.Max(keep, 14);
            if (self.Equipment.Any(c => c.Id == card.Id)) keep += card.Kind == CardKind.SilverLion && self.Hp < self.MaxHp ? -14 : 4;
            return frame.BoundaryLiuFeng!.Targets.Sum(t => brain.ScoreProgramTarget(view, t, damage) / 4) + 4 - keep;
        }
        double Value(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch") switch
        {
            "cancel" => 0,
            "collect" => CollectValue(choice),
            "target" => brain.ScoreProgramTarget(view, choice.Targets.Single(), damage) / 4,
            "cost" => CostValue(choice),
            _ => double.NegativeInfinity
        };
        return decision.Choices.OrderByDescending(Value).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
