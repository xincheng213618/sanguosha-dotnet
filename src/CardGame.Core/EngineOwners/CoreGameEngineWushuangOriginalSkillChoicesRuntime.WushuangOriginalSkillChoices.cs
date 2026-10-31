#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangOriginalSkillChoicesRuntime
{
    [ProgramAnswer("wushuang-original")]
    internal void ResolveWushuangOriginalChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WushuangOriginalSkill: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != f.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !prompt.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("An official skill answer lost its exact privately published prompt.");
        _coreWushuangOriginalSkillsOperations.AssertWushuangOriginalSkill(f);
        var option = choice.Parameters["option"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (option == "finish")
        {
            _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
            return;
        }

        if (option == "yiji-give")
        {
            var id = choice.Cards.Single();
            var target = choice.Targets.Single();
            if (!r.Cards.Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("Yiji lost its original viewed entity or chosen recipient.");
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Cards = r.Cards.Where(c => c != id).ToArray() });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.Hand(target), new("skill-program.wushuang.yiji.give"), disclosure: CardMovementDisclosure.FaceDown);
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option == "jieming-target")
        {
            var target = choice.Targets.Single();
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f, false);
                return;
            }

            _coreWushuangOriginalSkillsOperations.DrawWushuangOriginal(f, target, Math.Max(0, Math.Min(5, _coreMatchParticipantsPlayersAccess._players[target].MaxHp) - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count), "finish");
            return;
        }

        if (option == "yashi-suppress")
        {
            var target = choice.Targets.Single();
            if (target != f.WindowContext?.SourceSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("Yashi's non-locked suppression lost its actual damage source.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangYashiSuppressedEvent(f.Id, f.OwnerSeat, target, _coreActualTurnProgressTurnNumberAccess._turnNumber));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
            _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
            return;
        }

        if (option == "guanxu-target")
        {
            var target = choice.Targets.Single();
            if (target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0)
                throw new InvalidOperationException("Guanxu lost its living other target with hand cards.");
            _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "guanxu-view", Target = target, Cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray(), Top = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Take(5).Select(c => c.Id).ToArray() });
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option == "guanxu-exchange")
        {
            if (choice.Cards is not [var hand, var deck] || !r.Cards.Contains(hand) || !r.Top.Contains(deck) || _cardZonesOperations._cardZones.GetLocation(hand) != CardLocation.Hand(r.Target) || _cardZonesOperations._cardZones.GetLocation(deck) != CardLocation.DrawPile)
                throw new InvalidOperationException("Guanxu exchange changed its private hand or original top-five entity.");
            var finalOrder = r.Top.Select(id => id == deck ? hand : id).ToArray();
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "guanxu-discard", Cards = r.Cards.Select(id => id == hand ? deck : id).ToArray(), Top = [] });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(deck), CardLocation.DrawPile, CardLocation.Hand(r.Target), new("skill-program.wushuang.guanxu.exchange"), disclosure: CardMovementDisclosure.FaceDown);
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(hand), CardLocation.Hand(r.Target), CardLocation.DrawPile, new("skill-program.wushuang.guanxu.exchange"), disclosure: CardMovementDisclosure.FaceDown);
            _cardZonesOperations._cardZones.ReorderDrawPileSubsetToTop(finalOrder, finalOrder);
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option == "guanxu-discard")
        {
            if (choice.Cards.Count != 3 || choice.Cards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.Target)) || choice.Cards.Select(id => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.Target], _coreAdvancedLifecycleOperations.GetAdvancedCard(id))).Distinct().Count() != 1)
                throw new InvalidOperationException("Guanxu requires three current target hand entities of one suit.");
            f = _coreWushuangOriginalSkillsOperations.WushuangOriginalStage(f, "finish");
            _coreWushuangOriginalSkillFlowsOperations.DiscardWushuangOriginal(f, r.Target, choice.Cards);
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option == "choufa-target")
        {
            var target = choice.Targets.Single();
            if (target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0)
                throw new InvalidOperationException("Choufa requires a living other hand owner.");
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "choufa-reveal", Target = target, Cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray() });
            // The blind selection does not grant a hand inspection.
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option == "choufa-reveal")
        {
            var index = int.Parse(choice.Parameters["index"], CultureInfo.InvariantCulture);
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).ToArray();
            if (index < 0 || index >= hand.Length)
                throw new InvalidOperationException("Choufa's blind hand position changed.");
            var card = hand[index];
            var category = CardCatalog.Get(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.Target], card)).CategoryName;
            var rewritten = hand.Where(c => CardCatalog.Get(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.Target], c)).CategoryName != category).Select(c => c.Id).ToArray();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), r.Target, "choufa-revealed", Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangChoufaRewrittenEvent(f.Id, f.OwnerSeat, r.Target, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreCardMovementJournalMovementSequenceAccess._movementSequence, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), rewritten));
            _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
            return;
        }

        if (option == "shenfu-damage")
        {
            var target = choice.Targets.Single();
            if (target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                throw new InvalidOperationException("Shenfu requires a living other damage target.");
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "shenfu-odd-after", Target = target, Count = r.Count + 1 });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1, nature: DamageNature.Thunder);
            return;
        }

        if (option == "shenfu-draw")
        {
            var target = choice.Targets.Single();
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Target = target, Count = r.Count + 1 });
            _coreWushuangOriginalSkillsOperations.DrawWushuangOriginal(f, target, 1, "shenfu-even-after");
            return;
        }

        if (option == "shenfu-discard-target")
        {
            var target = choice.Targets.Single();
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).ToArray();
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "shenfu-discard", Target = target, Count = r.Count + 1 });
            _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, hand.Select((card, index) => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "shenfu-discard-card-" + index, target == f.OwnerSeat ? $"弃置【{(card.DisplayName)}】" : $"弃置 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 第 {(index + 1)} 张手牌", targets: [target])).ToArray());
            return;
        }

        if (option.StartsWith("shenfu-discard-card-", StringComparison.Ordinal))
        {
            var index = int.Parse(option[20..], CultureInfo.InvariantCulture);
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).ToArray();
            if (index < 0 || index >= hand.Length)
                throw new InvalidOperationException("Shenfu lost its selected current hand position.");
            f = _coreWushuangOriginalSkillsOperations.WushuangOriginalStage(f, "shenfu-even-after");
            _coreWushuangOriginalSkillFlowsOperations.DiscardWushuangOriginal(f, r.Target, [hand[index].Id]);
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option == "lingren-target")
        {
            var target = choice.Targets.Single();
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]);
            var mask = (hand.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[target], c), SkillProgramCardCategory.Basic)) ? 1 : 0) | (hand.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[target], c), SkillProgramCardCategory.Trick)) ? 2 : 0) | (hand.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[target], c), SkillProgramCardCategory.Equipment)) ? 4 : 0);
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "lingren-guess", Target = target, Mask = mask });
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (option.StartsWith("lingren-guess-", StringComparison.Ordinal))
        {
            var guess = int.Parse(option[14..], CultureInfo.InvariantCulture);
            var correct = 3 - System.Numerics.BitOperations.PopCount((uint)(guess ^ r.Mask));
            var context = f.WindowContext!.CardUse!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangLingrenGuessedEvent(f.Id, f.OwnerSeat, r.Target, _coreActualTurnProgressTurnNumberAccess._turnNumber, context.ParentCardUseFrameId, context.CardActionId, correct));
            if (correct >= 3)
                _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], $"wushuang-lingren:{(_coreActualTurnProgressTurnNumberAccess._turnNumber)}:{(f.OwnerSeat)}:{(f.Id)}", ["boundary:jianxiong", "classic:xingshang"]);
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "finish", Correct = correct });
            if (correct >= 2)
                _coreWushuangOriginalSkillsOperations.DrawWushuangOriginal(f, f.OwnerSeat, 2, "finish");
            else
                _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        throw new InvalidOperationException("Unknown official skill choice.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWushuangOriginalYiji, SkillProgramEffectOp.ResolveWushuangYashi, SkillProgramEffectOp.ResolveWushuangGuanxu, SkillProgramEffectOp.ResolveWushuangChoufa, SkillProgramEffectOp.ResolveWushuangShenfu, SkillProgramEffectOp.ResolveWushuangLingren, SkillProgramEffectOp.ResolveWushuangOriginalJieming)]
    internal PromptChoice SelectWushuangOriginalAi(PendingDecision decision, ProgramSkillFrame f)
    {
        double Score(PromptChoice choice)
        {
            var option = choice.Parameters.GetValueOrDefault("option") ?? "";
            var target = choice.Targets.FirstOrDefault(-1);
            var ally = target >= 0 && _coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, target);
            if (option == "finish")
                return 0;
            if (option is "yiji-give" or "jieming-target" or "shenfu-draw")
                return ally ? 30 - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count : -20;
            if (option is "shenfu-damage" or "shenfu-discard-target" or "yashi-suppress" or "choufa-target" or "guanxu-target")
                return ally ? -20 : 30;
            if (option == "guanxu-exchange")
            {
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.WushuangOriginalSkill!.Target]).Select(c => c.Id == choice.Cards[0] ? _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards[1]) : c).ToArray();
                return hand.GroupBy(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.WushuangOriginalSkill.Target], c)).Any(g => g.Count() >= 3) ? 20 : -5;
            }

            if (option.StartsWith("lingren-guess-", StringComparison.Ordinal))
            {
                // Guess from the public hand count, never the frozen private mask.
                var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.WushuangOriginalSkill!.Target]).Count;
                return option == (count == 0 ? "lingren-guess-0" : count == 1 ? "lingren-guess-1" : "lingren-guess-3") ? 20 : 0;
            }

            return 10;
        }

        return decision.Choices.OrderByDescending(Score).First();
    }
}
