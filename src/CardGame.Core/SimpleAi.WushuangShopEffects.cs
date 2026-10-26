namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private enum ShopReviewParty { Unknown, Owner, Other }
    private readonly record struct ShopReviewContext(ShopReviewParty Source, ShopReviewParty Target,
        ShopReviewParty Actor, ShopReviewParty Selected = ShopReviewParty.Unknown);
    private readonly record struct ShopReviewCards(ShopReviewParty Owner, int Count);

    private static bool ShopReviewHasPricedEffects(SkillProgram program) => program.Triggers.SelectMany(t => t.Effects)
        .Concat(program.Activations.SelectMany(a => a.Effects)).Any(e => e.Op is
            SkillProgramEffectOp.Draw or SkillProgramEffectOp.AdjustNormalDraw or SkillProgramEffectOp.ObtainWushuangDeckCards or
            SkillProgramEffectOp.Recover or SkillProgramEffectOp.RecoverToMaximum or SkillProgramEffectOp.ChangeMaximumHp or
            SkillProgramEffectOp.Damage or SkillProgramEffectOp.DamageParticipants or SkillProgramEffectOp.LoseHp or
            SkillProgramEffectOp.DiscardSelected or SkillProgramEffectOp.GiveSelected or SkillProgramEffectOp.TurnOver or
            SkillProgramEffectOp.ChooseOwnCardDiscard or
            SkillProgramEffectOp.OfferZhuijiActualPlayChoice);

    private static double ShopReviewedActivationValue(SkillProgram program, SkillProgramActivation activation,
        PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned, double horizon = 1, bool refinement = false)
    {
        var targets = ShopReviewedTargetParties(activation.TargetKind, ShopReviewParty.Other, self, opponent);
        if (targets.Length == 0) return 0;
        if (refinement && ShopRefinedDeferredPhaseActivation(activation, self, opponent, owned, horizon) is { } deferred)
            return deferred;
        return targets.Max(target => ShopReviewedEffectsValue(program, activation.Effects, self, opponent, owned,
            new(ShopReviewParty.Owner, ShopReviewParty.Other, ShopReviewParty.Owner, target),
            selectedCards: activation.MinCards) - activation.MinCards * 3);
    }

    private static double ShopReviewedTriggerValue(SkillProgram program, SkillProgramTrigger trigger,
        PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned)
    {
        // A judgment issued by this same program is valued at its issuing
        // instruction, including its payment and the original participants.
        // Its four possible suits are not four recurring income events.
        if (trigger.Window == SkillProgramTriggerWindow.JudgmentFinalized && trigger.JudgmentReasons.Count > 0 &&
            trigger.JudgmentReasons.All(reason => program.Triggers.SelectMany(t => t.Effects)
                .Concat(program.Activations.SelectMany(a => a.Effects)).Any(e =>
                    e.Op == SkillProgramEffectOp.StartJudgment && e.JudgmentReason == reason))) return 0;

        var contexts = ShopReviewedTriggerContexts(trigger);
        var value = contexts.Average(context =>
        {
            var net = ShopReviewedEffectsValue(program, trigger.Effects, self, opponent, owned, context);
            return trigger.Optional ? Math.Max(0, net) : net;
        });
        return trigger.Window == SkillProgramTriggerWindow.JudgmentFinalized ? value * ShopReviewedJudgmentProbability(trigger) : value;
    }

    private static ShopReviewContext[] ShopReviewedTriggerContexts(SkillProgramTrigger trigger)
    {
        var incoming = new ShopReviewContext(ShopReviewParty.Other, ShopReviewParty.Owner, ShopReviewParty.Other);
        var outgoing = new ShopReviewContext(ShopReviewParty.Owner, ShopReviewParty.Other, ShopReviewParty.Owner);
        if (trigger.Window is SkillProgramTriggerWindow.BeforeDamageApplied or SkillProgramTriggerWindow.AfterDamageApplied or
            SkillProgramTriggerWindow.DamageAppliedBeforeDying)
            return trigger.Subject switch
            {
                SkillProgramTriggerSubject.Owner or SkillProgramTriggerSubject.DamageTarget => [incoming],
                SkillProgramTriggerSubject.Source or SkillProgramTriggerSubject.DamageSource => [outgoing],
                SkillProgramTriggerSubject.Any => [incoming, outgoing],
                _ => [new()]
            };
        if (trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor) return [outgoing];
        if (trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Target) return [incoming];
        if (trigger.Window == SkillProgramTriggerWindow.JudgmentFinalized)
            return [new(trigger.JudgmentSource is null or SkillProgramTriggerSubject.Any ? ShopReviewParty.Unknown : ShopReviewParty.Owner,
                trigger.Subject == SkillProgramTriggerSubject.Any ? ShopReviewParty.Unknown : ShopReviewParty.Owner, ShopReviewParty.Unknown)];
        return trigger.Subject == SkillProgramTriggerSubject.Owner
            ? [new(ShopReviewParty.Owner, ShopReviewParty.Owner, ShopReviewParty.Owner)] : [new()];
    }

    private static double ShopReviewedJudgmentProbability(SkillProgramTrigger trigger) =>
        (trigger.Suits.Count == 0 ? 1 : trigger.Suits.Count(suit => suit != Suit.None) / 4d) *
        Math.Max(0, Math.Min(13, trigger.MaximumRank) - Math.Max(1, trigger.MinimumRank) + 1) / 13;

    private static ShopReviewParty ShopReviewedParticipant(ProgramParticipantReference? reference, ShopReviewContext context,
        ShopReviewParty fallback = ShopReviewParty.Owner) => reference?.Kind switch
    {
        null => fallback,
        ProgramParticipantRef.Owner => ShopReviewParty.Owner,
        ProgramParticipantRef.EventSource => context.Source,
        ProgramParticipantRef.EventTarget => context.Target,
        ProgramParticipantRef.Actor => context.Actor,
        ProgramParticipantRef.SelectedTarget or ProgramParticipantRef.SelectedFirst => context.Selected,
        _ => ShopReviewParty.Unknown
    };

    private static ShopReviewParty ShopReviewedEffectTarget(SkillProgramEffect effect, ShopReviewContext context) =>
        ShopReviewedParticipant(effect.TargetReference, context, effect.Target switch
        {
            SkillProgramEffectTarget.Owner => ShopReviewParty.Owner,
            SkillProgramEffectTarget.Actor => context.Actor,
            SkillProgramEffectTarget.SelectedTarget or SkillProgramEffectTarget.SelectedTargets => context.Selected,
            _ => ShopReviewParty.Unknown
        });

    private static ShopReviewParty[] ShopReviewedTargetParties(SkillProgramTargetKind? kind, ShopReviewParty eventTarget,
        PlayerSnapshot self, PlayerSnapshot? opponent) => kind switch
    {
        SkillProgramTargetKind.EventTarget => [eventTarget],
        SkillProgramTargetKind.AnyLiving => Parties(true, opponent is { IsAlive: true }),
        SkillProgramTargetKind.AnyWounded => Parties(!self.HpUndefined && self.Hp < self.MaxHp,
            opponent is { HpUndefined: false } && opponent.Hp < opponent.MaxHp),
        SkillProgramTargetKind.AnyLivingWithHand => Parties(self.HandCount > 0, opponent is { HandCount: > 0 }),
        SkillProgramTargetKind.AnyLivingHandBelowMaxHp => Parties(self.HandCount < self.MaxHp,
            opponent is not null && opponent.HandCount < opponent.MaxHp),
        SkillProgramTargetKind.OtherLiving or SkillProgramTargetKind.OtherLivingAtDistanceOne => Parties(false, opponent is { IsAlive: true }),
        SkillProgramTargetKind.OtherLivingWithHand => Parties(false, opponent is { HandCount: > 0 }),
        SkillProgramTargetKind.OtherWounded => Parties(false, opponent is { HpUndefined: false } && opponent.Hp < opponent.MaxHp),
        SkillProgramTargetKind.OtherLivingWithFewerHandCards => Parties(false, opponent is not null && opponent.HandCount < self.HandCount),
        SkillProgramTargetKind.OtherLivingHandAtLeastOwner => Parties(false, opponent is not null && opponent.HandCount >= self.HandCount),
        _ => [ShopReviewParty.Unknown]
    };

    private static ShopReviewParty[] Parties(bool owner, bool other) => owner
        ? other ? [ShopReviewParty.Owner, ShopReviewParty.Other] : [ShopReviewParty.Owner]
        : other ? [ShopReviewParty.Other] : [];

    private static double ShopReviewedChoiceValue(double[] alternatives, ShopReviewParty chooser) => alternatives.Length == 0 ? 0 :
        chooser == ShopReviewParty.Owner ? alternatives.Max() : chooser == ShopReviewParty.Other ? alternatives.Min() : alternatives.Average();

    private static bool? ShopReviewedCondition(SkillProgramCondition condition, PlayerSnapshot self,
        IReadOnlyDictionary<string, string> choices) => condition.Kind switch
    {
        SkillProgramConditionKind.ChoiceIs => condition.SourceBind is { } bind && choices.TryGetValue(bind, out var chosen)
            ? chosen == condition.OptionId : null,
        SkillProgramConditionKind.All => condition.Children.Any(c => ShopReviewedCondition(c, self, choices) == false) ? false :
            condition.Children.All(c => ShopReviewedCondition(c, self, choices) == true) ? true : null,
        SkillProgramConditionKind.Any => condition.Children.Any(c => ShopReviewedCondition(c, self, choices) == true) ? true :
            condition.Children.All(c => ShopReviewedCondition(c, self, choices) == false) ? false : null,
        SkillProgramConditionKind.Not => ShopReviewedCondition(condition.Children[0], self, choices) is { } child ? !child : null,
        _ => ShopDrawConditionApplies(condition, self)
    };

    private static double ShopReviewedEffectsValue(SkillProgram program, IReadOnlyList<SkillProgramEffect> effects,
        PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned, ShopReviewContext context,
        int selectedCards = 0, int depth = 0)
    {
        if (depth > 4) return 0;
        return Read(0, context, new(StringComparer.Ordinal), new(StringComparer.Ordinal));

        double Read(int start, ShopReviewContext participants, Dictionary<string, string> choices,
            Dictionary<string, ShopReviewCards> bindings)
        {
            var result = 0d;
            var maximumGain = effects.Where(e => e.Op == SkillProgramEffectOp.ChangeMaximumHp &&
                ShopReviewedEffectTarget(e, participants) == ShopReviewParty.Owner &&
                ShopReviewedCondition(e.Condition, self, choices) != false).Sum(e => Math.Max(0, e.Amount));
            for (var index = start; index < effects.Count; index++)
            {
                var effect = effects[index];
                var applies = ShopReviewedCondition(effect.Condition, self, choices);
                if (applies == false) continue;
                var weight = applies == true ? 1d : .5;
                if (effect.Op == SkillProgramEffectOp.ChooseOption && effect.ResultBind is { } choiceBind)
                {
                    var alternatives = effect.Options.Where(option => ShopReviewedCondition(option.Condition, self, choices) != false &&
                            (option.RequiredTargetKind is null || ShopReviewedTargetParties(option.RequiredTargetKind, participants.Target, self, opponent)
                                .Any(target => target != ShopReviewParty.Unknown)))
                        .Select(option =>
                        {
                            var chosen = new Dictionary<string, string>(choices, StringComparer.Ordinal) { [choiceBind] = option.Id };
                            return Read(index + 1, participants, chosen, new(bindings, StringComparer.Ordinal));
                        }).ToArray();
                    var chosenValue = ShopReviewedChoiceValue(alternatives, ShopReviewedParticipant(effect.ChooserRef, participants));
                    if (applies == true) return result + chosenValue;
                    var skipped = new Dictionary<string, string>(choices, StringComparer.Ordinal) { [choiceBind] = "" };
                    return result + weight * chosenValue + (1 - weight) * Read(index + 1, participants, skipped, new(bindings, StringComparer.Ordinal));
                }
                var party = ShopReviewedEffectTarget(effect, participants);
                var target = party == ShopReviewParty.Owner ? self : party == ShopReviewParty.Other ? opponent : null;
                var sign = party == ShopReviewParty.Owner ? 1 : party == ShopReviewParty.Other ? -1 : 0;
                var amount = effect.Amount > 0 ? effect.Amount : effect.NumberExpression is not null ? 1 : 0;
                double benefit = 0;
                switch (effect.Op)
                {
                    case SkillProgramEffectOp.SelectTarget:
                        var targets = ShopReviewedTargetParties(effect.TargetKind, participants.Target, self, opponent);
                        var targetValues = targets.Select(selected => Read(index + 1, participants with { Selected = selected },
                            new(choices, StringComparer.Ordinal), new(bindings, StringComparer.Ordinal))).ToArray();
                        var selectedValue = ShopReviewedChoiceValue(targetValues, ShopReviewParty.Owner);
                        return result + (applies == true ? selectedValue : weight * selectedValue + (1 - weight) *
                            Read(index + 1, participants with { Selected = ShopReviewParty.Unknown },
                                new(choices, StringComparer.Ordinal), new(bindings, StringComparer.Ordinal)));
                    case SkillProgramEffectOp.Draw:
                    case SkillProgramEffectOp.AdjustNormalDraw:
                        benefit = sign * amount * 7;
                        break;
                    case SkillProgramEffectOp.ObtainWushuangDeckCards:
                        benefit = sign * amount * (effect.OutputKind is { } card && target is not null
                            ? ShopCardValue(card, target, party == ShopReviewParty.Owner ? opponent : self,
                                party == ShopReviewParty.Owner ? owned : []) : 6);
                        break;
                    case SkillProgramEffectOp.Recover:
                        if (target is not null)
                            benefit = sign * Math.Min(amount, Math.Max(0, target.MaxHp + (sign == 1 ? maximumGain : 0) - target.Hp)) *
                                (ShopHealthValue(target) + (sign == 1 ? ShopBuildRecoveryBonus(owned) : 0));
                        break;
                    case SkillProgramEffectOp.RecoverToMaximum:
                        if (target is not null) benefit = sign * Math.Max(0, target.MaxHp - target.Hp) *
                            (ShopHealthValue(target) + (sign == 1 ? ShopBuildRecoveryBonus(owned) : 0));
                        break;
                    case SkillProgramEffectOp.ChangeMaximumHp:
                        benefit = sign * effect.Amount * 4;
                        break;
                    case SkillProgramEffectOp.Damage:
                    case SkillProgramEffectOp.DamageParticipants:
                        benefit = party == ShopReviewParty.Owner ? -amount * ShopHealthValue(self) : party == ShopReviewParty.Other ? amount * 7 : 0;
                        break;
                    case SkillProgramEffectOp.LoseHp:
                        benefit = party == ShopReviewParty.Owner ? -amount * ShopHealthValue(self) : party == ShopReviewParty.Other ? amount * 6 : 0;
                        break;
                    case SkillProgramEffectOp.DiscardSelected:
                        benefit = -Math.Max(0, Math.Max(amount, Math.Max(effect.MinimumCards, selectedCards)) - selectedCards) * 3;
                        break;
                    case SkillProgramEffectOp.GiveSelected:
                        var giftCount = Math.Max(amount, Math.Max(effect.MinimumCards, selectedCards));
                        if (party == ShopReviewParty.Other)
                            benefit = -giftCount * 7 - Math.Max(0, giftCount - selectedCards) * 3;
                        break;
                    case SkillProgramEffectOp.SelectOwnedCards when effect.ResultBind is { } bind:
                        var cardOwner = party;
                        bindings[bind] = new(cardOwner, Math.Max(1, Math.Max(amount, effect.MinimumCards)));
                        break;
                    case SkillProgramEffectOp.MoveBoundCards when effect.SourceBind is { } source && bindings.TryGetValue(source, out var cards):
                        if (effect.Destination == SkillProgramCardDestination.DiscardPile)
                            benefit = cards.Count * (cards.Owner == ShopReviewParty.Owner ? -3 : cards.Owner == ShopReviewParty.Other ? 3 : 0);
                        else if (effect.Destination == SkillProgramCardDestination.SelectedTargetHand && cards.Owner != participants.Selected)
                            benefit = cards.Count * ((cards.Owner == ShopReviewParty.Owner ? -3 : cards.Owner == ShopReviewParty.Other ? 3 : 0) +
                                (participants.Selected == ShopReviewParty.Owner ? 7 : participants.Selected == ShopReviewParty.Other ? -7 : 0));
                        bindings.Remove(source);
                        break;
                    case SkillProgramEffectOp.SelectAndMoveOwnedCard:
                        var owner = ShopReviewedParticipant(effect.CardOwnerRef, participants);
                        if (effect.Destination == SkillProgramCardDestination.DiscardPile)
                            benefit = owner == ShopReviewParty.Owner ? -3 : owner == ShopReviewParty.Other ? 3 : 0;
                        break;
                    case SkillProgramEffectOp.ChooseOwnCardDiscard:
                        var chooser = ShopReviewedParticipant(effect.ChooserRef, participants);
                        var available = chooser == ShopReviewParty.Owner ? self.HandCount + self.Equipment.Count :
                            chooser == ShopReviewParty.Other && opponent is not null ? opponent.HandCount + opponent.Equipment.Count : 0;
                        if (available > 0) benefit = chooser == ShopReviewParty.Owner ? -3 : 3;
                        break;
                    case SkillProgramEffectOp.TurnOver:
                        if (target is not null) benefit = sign * (target.IsFaceDown ? 6 : -6);
                        break;
                    case SkillProgramEffectOp.StartJudgment when effect.JudgmentReason is { } reason:
                        var judgment = new ShopReviewContext(ShopReviewedParticipant(effect.SourceRef, participants), party, ShopReviewParty.Unknown);
                        benefit = program.Triggers.Where(t => t.Window == SkillProgramTriggerWindow.JudgmentFinalized &&
                                (t.JudgmentReasons.Count == 0 || t.JudgmentReasons.Contains(reason, StringComparer.Ordinal)) &&
                                !t.ExcludedReasons.Contains(reason, StringComparer.Ordinal) &&
                                (t.Subject == SkillProgramTriggerSubject.Any || party == ShopReviewParty.Owner) &&
                                (t.JudgmentSource is null or SkillProgramTriggerSubject.Any || judgment.Source == ShopReviewParty.Owner))
                            .Sum(t => ShopReviewedJudgmentProbability(t) * JudgmentNet(t, judgment));
                        // A revealed deck card is not a card paid from the hand.
                        break;
                    case SkillProgramEffectOp.OfferZhuijiActualPlayChoice:
                        // This native operation arms an actual-play-ending debt.
                        // Price the debt on the selected branch, once, when its
                        // benefit is received; the later settle op is not income.
                        if (effects.Any(e => e.Op == SkillProgramEffectOp.Recover && ShopReviewedCondition(e.Condition, self, choices) == true))
                            benefit = -6;
                        else if (effects.Any(e => e.Op == SkillProgramEffectOp.Draw && ShopReviewedCondition(e.Condition, self, choices) == true))
                            benefit = -ShopHealthValue(self);
                        break;
                }
                result += benefit * weight;
            }
            return result;
        }

        double JudgmentNet(SkillProgramTrigger trigger, ShopReviewContext participants)
        {
            var value = ShopReviewedEffectsValue(program, trigger.Effects, self, opponent, owned, participants, depth: depth + 1);
            return trigger.Optional ? Math.Max(0, value) : value;
        }
    }
}
