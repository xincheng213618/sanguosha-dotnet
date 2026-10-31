#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangPublicPredictionRuntime
{
    internal PublicDamagePrediction AttachWushuangPublicCombatPrediction(GameSnapshot view, int actorSeat, LegalAction action, int targetSeat, int amount, CardKind kind, int paidHand, PublicDamagePrediction legacy)
    {
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || view.Players.Count != 2 || actorSeat == targetSeat)
            return legacy;
        var actor = view.Players.Single(player => player.Seat == actorSeat);
        var target = view.Players.Single(player => player.Seat == targetSeat);
        var material = action.CardId is { } id ? actor.Hand.Concat(actor.WoodenOxGrain ?? []).Concat(actor.Equipment).Concat(actor.AuthorityCards ?? []).FirstOrDefault(card => card.Id == id) : null;
        var suit = material?.Suit ?? Suit.None;
        // Only an entity already present in the actor's own supplied view may
        // qualify its declared suit. No arbitrary/foreign ID probes a registry.
        if (material is not null)
        {
            var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreCardAttackStateOperations.GetAttackCard(material.Id), action.ConversionSource);
            suit = _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, [material.Id], card) ?? card.Suit;
        }

        var payment = _corePublicDamagePredictionOperations.PredictVisibleDamage(view, actorSeat, targetSeat, amount, kind, true, false, false, false, paidHand, _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(_coreMatchParticipantsPlayersAccess._players[actorSeat]), wushuangTactics: true, prospectiveSuit: suit);
        PublicDamageSequencePrediction? retaliation = null;
        // The victim's death closes its damage observers. A potential rescue is
        // still unknown; this describes the branch in which the target survives.
        if (payment.HpLossAmount > 0 && !payment.IsLethal && target.IsAlive)
        {
            var returned = new List<PublicHealthEffect>();
            var targetAfter = target with
            {
                Hp = payment.HpAfter,
                Armor = payment.ArmorAfter
            };
            var replyView = view with
            {
                Players = Array.AsReadOnly(view.Players.Select(player => player.Seat == targetSeat ? targetAfter : player).ToArray())
            };
            var previousReplies = 0;
            foreach (var reflected in new[]
            {
                target.WushuangTacticIds?.Contains(2116) == true ? 1 : 0,
                target.WushuangTacticIds?.Contains(2131) == true && targetSeat != view.CurrentSeat && payment.DamageAmount >= 3 ? payment.DamageAmount : 0
            }

            )
            {
                if (reflected == 0)
                    continue;
                var reply = _corePublicDamagePredictionOperations.PredictVisibleDamage(replyView, targetSeat, actorSeat, reflected, null, false, false, false, false, 0, false, wushuangTactics: true, additionalIncomingSegments: 1, priorTargetSegments: previousReplies++);
                returned.Add(reply.MaximumHpLossAmount > 0 ? new(PublicHealthEffectKind.MaximumHpLoss, reply.MaximumHpLossAmount) : reply.DirectHpLossAmount > 0 ? new(PublicHealthEffectKind.HpLoss, reply.DirectHpLossAmount) : new(PublicHealthEffectKind.Damage, reply.DamageAmount));
                replyView = replyView with
                {
                    Players = Array.AsReadOnly(replyView.Players.Select(player => player.Seat == actorSeat ? player with { Hp = reply.HpAfter, Armor = reply.ArmorAfter, MaxHp = reply.MaxHpAfter } : player).ToArray())
                };
                if (reply.IsLethal)
                    break;
            }

            if (returned.Count > 0)
                retaliation = PublicDamageSequencePrediction.Calculate(actor.Hp, actor.MaxHp, actor.Armor, returned);
        }

        var reductions = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangIncomingReductionIssuedEvent>().Count(fact => fact.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && fact.TargetSeat == targetSeat);
        var remainingReduction = Math.Max(target.WushuangTacticIds?.Contains(2196) == true ? Math.Max(0, 2 - reductions) : 0, target.WushuangTacticIds?.Contains(2197) == true ? Math.Max(0, 3 - reductions) : 0);
        return legacy with
        {
            Wushuang = new(payment, retaliation, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && actor.HasAlcoholEffect && actor.WushuangTacticIds?.Contains(2001) == true, actor.WushuangTacticIds?.Contains(2210) == true, remainingReduction, Math.Max(0, _coreRuleQueriesOperations.GetSlashUseLimit(_coreMatchParticipantsPlayersAccess._players[actorSeat]) - (actorSeat == view.CurrentSeat ? _coreCurrentTurnStatisticsSlashCountThisTurnAccess._slashCountThisTurn : 0)))
        };
    }

    internal int WushuangPublicNativeDamageBonus(GameSnapshot view, int sourceSeat, int targetSeat, CardKind? kind, Suit suit, int additionalIncomingSegments, int priorTargetSegments, long? issuedUseId = null)
    {
        var source = view.Players.Single(player => player.Seat == sourceSeat);
        var target = view.Players.Single(player => player.Seat == targetSeat);
        bool Own(int id) => source.WushuangTacticIds?.Contains(id) == true;
        bool Defends(int id) => target.WushuangTacticIds?.Contains(id) == true;
        var bonus = (Own(2162) && source.Hp <= 2 ? 1 : 0) + (Own(2163) && source.Hp <= 3 ? 1 : 0);
        if (kind is { } slash && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(slash))
        {
            bonus += (Own(2150) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 3 ? 1 : 0) + (Own(2151) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 5 ? 1 : 0) + (Own(2152) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 7 ? 1 : 0);
            var ordinal = issuedUseId is { } useId ? _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCombatUseFactEvent>().LastOrDefault(fact => fact.CardUseFrameId == useId)?.SlashOrdinal : 1 + _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCombatUseFactEvent>().Count(fact => fact.OwnerSeat == sourceSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(fact.Kind));
            if ((Own(2042) || Own(2043)) && ordinal is> 0 && ordinal % 3 == 0)
                bonus += (Own(2042) ? 1 : 0) + (Own(2043) ? 2 : 0);
            if (Own(2045) && suit == Suit.Heart)
                bonus += 2;
            if (Own(2115) && sourceSeat == view.CurrentSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().LastOrDefault(turn => turn.ActorSeat == sourceSeat && turn.TurnNumber < view.TurnNumber)is { } previous && !_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCombatUseFactEvent>().Any(fact => fact.OwnerSeat == sourceSeat && fact.Turn == previous.TurnNumber && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(fact.Kind)))
                bonus++;
        }

        if (kind is null)
            bonus += (Own(2171) ? 1 : 0) + (Own(2172) ? 2 : 0);
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCombatDamageFactEvent>();
        if (Own(2127))
        {
            var last = facts.LastOrDefault(fact => fact.SourceSeat == sourceSeat && !fact.SourceLess);
            var tail = last is null ? facts : facts.SkipWhile(fact => !ReferenceEquals(fact, last)).Skip(1);
            bonus += additionalIncomingSegments + tail.Count(fact => fact.TargetSeat == sourceSeat && fact.Amount > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().LastOrDefault(turn => turn.TurnNumber == fact.Turn)?.ActorSeat != sourceSeat);
        }

        if (_coreActualRoundProgressRoundNumberAccess._roundNumber > 1 && (Own(2188) || Own(2189)))
        {
            var previousDamage = facts.Where(fact => fact.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber - 1 && fact.SourceSeat == sourceSeat && !fact.SourceLess).Sum(fact => fact.Amount);
            bonus += (Own(2188) && previousDamage == 0 ? 1 : 0) + (Own(2189) && previousDamage < 3 ? 1 : 0);
        }

        if (sourceSeat == view.CurrentSeat && !facts.Any(fact => fact.Turn == view.TurnNumber && fact.SourceSeat == sourceSeat && !fact.SourceLess))
            bonus += (Own(2333) ? 1 : 0) + (Own(2334) ? 2 : 0);
        if (kind is null || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind.Value) == SkillProgramCardCategory.Trick)
            bonus += (Own(2357) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 2 ? 2 : 0) + (Own(2358) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 4 ? 1 : 0) + (Own(2359) && _coreActualRoundProgressRoundNumberAccess._roundNumber >= 3 ? 1 : 0);
        if (targetSeat != view.CurrentSeat && !facts.Any(fact => fact.Turn == view.TurnNumber && fact.TargetSeat == targetSeat))
            bonus += (Defends(2333) ? 1 : 0) + (Defends(2334) ? 2 : 0);
        if (Defends(2196) || Defends(2197))
        {
            var received = priorTargetSegments + _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangIncomingReductionIssuedEvent>().Count(fact => fact.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && fact.TargetSeat == targetSeat);
            bonus -= (Defends(2196) && received < 2 ? 1 : 0) + (Defends(2197) && received < 3 ? 2 : 0);
        }

        return bonus;
    }
}
