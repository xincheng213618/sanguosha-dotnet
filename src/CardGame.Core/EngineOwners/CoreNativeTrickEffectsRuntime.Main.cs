#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeTrickEffectsRuntime
{
    internal void ResolveDrawTwoEffect(NullificationWindowFrame pending)
    {
        if (_coreNativeCardEffectDrawsOperations.TryBeginNativeDrawTwo(pending))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? pending.SourceSeat];
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.ResolvingEffect);
        var canDraw = source.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
        if (canDraw)
            _corePhysicalDrawFacadesOperations.DrawCards(source, _coreProgramDrawTwoBonusOperations.NativeDrawTwoCount(pending.SourceSeat), log: true);
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending));
        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
        _coreMatchLogOperations.AddLog("CardEffect", canDraw ? $"{(source.Name)} 使用【无中生有】，摸两张牌。" : $"{(source.Name)} 的【无中生有】结算完毕。", source.Seat);
    }

    internal void ResolveGroupCardEffect(NullificationWindowFrame pending)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var requiredCardKind = pending.RequiredCardKind ?? throw new InvalidOperationException("A group card must retain its response card kind.");
        var physicalCards = _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.ParentFrameId);
        var effectiveCard = pending.EffectCardKind == _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending).Kind ? _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending) : _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending)with
        {
            Kind = pending.EffectCardKind
        };
        var group = _kernelCoreMainOperations.CreateGroupCardHandle(pending.ParentFrameId, source.Seat, effectiveCard, pending.TargetSeats, GroupCardEffect.ResponseAttack, requiredCardKind, physicalCards);
        if (!_coreExtraCardEffectRoundsOperations.RestoreExtraGroupDamageSource(group) && !_characterMengHuoMouMobileOperations.Value.RestoreMouMengHuoDamageSource(group) && effectiveCard.Kind == CardKind.BarbarianAssault)
        {
            var mengHuo = _coreMatchParticipantsPlayersAccess._players.FirstOrDefault(player => player.IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.AttributeGlobalDamage, CardKind.BarbarianAssault));
            if (mengHuo is not null)
            {
                group.DamageSourceSeat = mengHuo.Seat;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoshouAttributedEvent(group.ResolutionId, source.Seat, mengHuo.Seat));
            }
        }

        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = group;
        _coreExtraEffectMaterialClaimsOperations.RestoreExtraEffectGroupMaterialClaims(group);
        _coreExtraCardEffectRoundsOperations.CaptureExtraGroupDamageSource(group);
        _coreMatchLogOperations.AddLog("CardUsed", $"{(source.Name)} 使用【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】，依次攻击 {(pending.TargetSeats.Count)} 名角色。", source.Seat);
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupCardUsedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, source.Seat, pending.TargetSeats));
        _coreNativeAiDecisionsOperations.NotifyAiOfGroupAttack(source, pending.TargetSeats);
        _coreNativeGroupAttackOperations.Value.BeginGroupAttackResponse(group);
    }

    internal void ResolvePeachGardenEffect(NullificationWindowFrame pending)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var effectiveCard = pending.EffectCardKind == _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending).Kind ? _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending) : _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending)with
        {
            Kind = pending.EffectCardKind
        };
        var group = _kernelCoreMainOperations.CreateGroupCardHandle(pending.ParentFrameId, source.Seat, effectiveCard, pending.TargetSeats, GroupCardEffect.Recovery, requiredCardKind: null, _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.ParentFrameId));
        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = group;
        _coreMatchLogOperations.AddLog("CardUsed", $"{(source.Name)} 使用【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】，使 {(pending.TargetSeats.Count)} 名存活角色依次回复体力。", source.Seat);
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupCardUsedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, source.Seat, pending.TargetSeats));
    }

    internal void ResolveFiveGrainsEffect(NullificationWindowFrame pending)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var effectiveCard = pending.EffectCardKind == _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending).Kind ? _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending) : _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending)with
        {
            Kind = pending.EffectCardKind
        };
        var group = _kernelCoreMainOperations.CreateGroupCardHandle(pending.ParentFrameId, source.Seat, effectiveCard, pending.TargetSeats, GroupCardEffect.PublicDraft, requiredCardKind: null, _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.ParentFrameId));
        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = group;
        foreach (var _ in pending.TargetSeats)
        {
            var revealed = _corePhysicalDrawFacadesOperations.DrawOneToProcessing(CardMoveReasons.Reveal);
            if (revealed is null)
            {
                if (_characterZhugeGuoLeOperations.IsLeChengyanEffectUse(pending.ParentFrameId) || _characterTianChouOlOperations.IsTianChouShandaoUse(pending.ParentFrameId) || _characterQinLangOlOperations.IsQinLangXianyingUse(pending.ParentFrameId) || _characterYangBiaoAnniversaryOperations.IsAnniversaryYangBiaoFiveGrainsUse(pending.ParentFrameId))
                    break;
                throw new InvalidOperationException("FiveGrains could not reveal the promised card count.");
            }

            group.AddRevealedCard(revealed.Id);
        }

        _coreMatchLogOperations.AddLog("CardUsed", $"{(source.Name)} 使用【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】，公开展示 {(group.RevealedCardIds.Count)} 张牌并依次选取。", source.Seat);
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupCardUsedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, source.Seat, pending.TargetSeats));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(pending.ParentFrameId, group.RevealedCardIds.Select(cardId => _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == cardId)).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray()));
        if (group.TargetSeats.Count == 0)
        {
            _coreNativeHarvestOperations.Value.FinishFiveGrains(group);
        }
        else
        {
            _coreNativeHarvestOperations.Value.BeginHarvestSelection(group);
        }
    }

    internal void ResolveTargetCardEffect(NullificationWindowFrame pending)
    {
        var targetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("A target-card effect must retain one target.");
        _coreWushuangTargetCardsOperations.BindWushuangTargetCards(pending, targetSeat);
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        Card targetCard;
        CardLocation targetCardFromZone;
        if (_coreExtraEffectForeignSelectionOperations.TryBeginExtraForeignSelection(pending, target))
            return;
        if (pending.TargetCardId is { } requestedTargetCardId)
        {
            var publicTarget = _corePhysicalOwnedCardQueriesOperations.FindPublicTargetCard(target, requestedTargetCardId);
            if (publicTarget is null)
            {
                SkipUnavailableTargetCardEffect(pending, target, CardEffectSkipReason.PublicTargetMissing);
                return;
            }

            targetCard = publicTarget.Value.Card;
            targetCardFromZone = publicTarget.Value.Location;
        }
        else
        {
            var targetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
            if (targetHand.Count == 0)
            {
                SkipUnavailableTargetCardEffect(pending, target, CardEffectSkipReason.TargetHandEmpty);
                return;
            }

            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId);
            var selected = use?.TargetHandPreselection;
            if (selected is not null)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use!.Id, frame => frame with { TargetHandPreselection = null });
            if (selected is not null && selected.TargetSeat == target.Seat && selected.HandCount == targetHand.Count && selected.HandMovementSequence == _coreTargetCardPreselectionOperations.TargetHandMovementSequence(target.Seat) && selected.SlotIndex >= 0 && selected.SlotIndex < targetHand.Count)
            {
                targetCard = targetHand[selected.SlotIndex];
                targetCardFromZone = CardLocation.Hand(target.Seat);
            }
            else
            {
                BeginTargetCardSelection(pending, target, targetHand.Count);
                return;
            }
        }

        ApplyTargetCardEffect(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind, pending.SourceSeat, target.Seat, pending.ActionKind, targetCard, targetCardFromZone);
    }

    internal void BeginTargetCardSelection(NullificationWindowFrame pending, CharacterState target, int candidateCount)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not null)
        {
            throw new InvalidOperationException("The engine cannot open two hidden target-card selections at once.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var candidateSlots = Enumerable.Range(0, candidateCount).ToArray();
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.AwaitingResponse);
        _coreRuntimeOperations.PushRuntimeFrame(new TargetCardSelectionFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, pending.ParentFrameId, source.Seat, target.Seat, pending.EffectCardId, pending.EffectCardKind, pending.ActionKind, Array.AsReadOnly(candidateSlots)));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.SelectTargetCard, source.Seat, $"{(source.Name)} 使用【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】，请选择 {(target.Name)} 的一张{((_coreWushuangCombatTacticsOperations.WushuangHasVisibleTargetHand(source.Seat, pending.EffectCardKind) ? "手牌" : "暗牌"))}。", [], [target.Seat], source.Seat, pending.EffectCardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = Array.AsReadOnly(candidateSlots.Select(slot => new PromptChoice(new ChoiceId($"target-card.slot-{(slot)}.resolution-{(pending.ParentFrameId)}"), _coreWushuangCombatTacticsOperations.WushuangHasVisibleTargetHand(source.Seat, pending.EffectCardKind) ? $"选择【{(_corePhysicalOwnedCardQueriesOperations.GetHand(target)[slot].DisplayName)}】（{(_corePhysicalOwnedCardQueriesOperations.GetHand(target)[slot].RankText)}）" : $"选择 {(target.Name)} 的一张暗牌（牌位 {(slot + 1)}）", [], [target.Seat], new Dictionary<string, string> { ["action"] = "target-card-slot", ["slot-index"] = slot.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray()),
            IsPrivate = true,
            TargetSeat = target.Seat
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetCardSelectionRequestedEvent(pending.ParentFrameId, source.Seat, target.Seat, pending.ActionKind, candidateCount));
        _coreMatchLogOperations.AddLog("TargetCardSelectionRequested", $"{(source.Name)} 对 {(target.Name)} 的手牌进行不透明牌位选择。", source.Seat, target.Seat);
        _coreMatchOutcomeStatusWinnerAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanCardSelection : EngineStatus.Running;
    }

    internal void ResolveTargetCardSelection(TargetCardSelectionFrame pending, int slot)
    {
        if (_coreExtraEffectForeignSelectionOperations.TryResolveExtraForeignSelection(pending, slot))
            return;
        if (!ReferenceEquals(_resolutionStackOperations._resolutionStack.LastOrDefault(), pending))
        {
            throw new InvalidOperationException("The hidden target-card selection is not the current resolution frame.");
        }

        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat];
        var targetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
        if (targetHand.Count != pending.CandidateSlots.Count || slot < 0 || slot >= pending.CandidateSlots.Count)
        {
            throw new InvalidOperationException("The target hand changed while the hidden target-card selection was pending.");
        }

        var targetCard = targetHand[slot];
        var effectCard = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(pending.ParentFrameId, pending.EffectCardId, requireProcessing: true);
        _coreRuntimeOperations.ReplaceRuntimeTop(pending with { Step = ResolutionFrameStep.Completed });
        _coreRuntimeOperations.PopResolutionFrame(pending.Id, ResolutionFrameKind.TargetCardSelection);
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.ResolvingEffect);
        ApplyTargetCardEffect(pending.ParentFrameId, effectCard, pending.EffectCardKind, pending.SourceSeat, pending.TargetSeat, pending.ActionKind, targetCard, CardLocation.Hand(target.Seat));
    }

    internal void ApplyTargetCardEffect(long resolutionId, Card effectCard, CardKind effectiveCardKind, int sourceSeat, int targetSeat, LegalActionKind actionKind, Card targetCard, CardLocation targetCardFromZone)
    {
        if (_coreUnexpectedAssaultOperations.TryApplyUnexpectedAssaultTargetCardEffect(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind, targetCard, targetCardFromZone))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[sourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var targetMoveReason = CoreNativeOwnedCardQueriesRuntime.GetTargetCardMoveReason(actionKind, targetCardFromZone.Zone);
        if (actionKind == LegalActionKind.Dismantlement && _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(sourceSeat, targetCard, targetCardFromZone, OwnedCardMoveIntent.Discard))
        {
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(resolutionId, effectCard);
            _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, effectCard, effectiveCardKind);
            return;
        }

        var publicTargetKind = targetCardFromZone.Zone == CardZoneKind.Judgment ? _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(targetCard) : targetCard.Kind;
        var logicalDiscard = actionKind == LegalActionKind.Dismantlement ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(sourceSeat, sourceSeat, [targetCard.Id], targetMoveReason, resolutionId) : null;
        _corePhysicalMovementFacadesOperations.MoveCard(targetCard, targetCardFromZone, CardLocation.Processing, targetMoveReason, disclosure: actionKind == LegalActionKind.Dismantlement ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.FaceDown);
        if (actionKind == LegalActionKind.Dismantlement)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetCardDiscardedEvent(resolutionId, source.Seat, target.Seat, targetCardFromZone.Zone, CoreNativeCardClassificationsRuntime.IsPublicTargetZone(targetCardFromZone.Zone) ? targetCard.Id : null, CoreNativeCardClassificationsRuntime.IsPublicTargetZone(targetCardFromZone.Zone) ? publicTargetKind : null));
            _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(targetCard, CardLocation.DiscardPile, CoreNativeOwnedCardQueriesRuntime.GetTargetCardFinishedMoveReason(actionKind, targetCardFromZone.Zone));
            _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        }
        else
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetCardTakenEvent(resolutionId, source.Seat, target.Seat, targetCardFromZone.Zone, CoreNativeCardClassificationsRuntime.IsPublicTargetZone(targetCardFromZone.Zone) ? targetCard.Id : null, CoreNativeCardClassificationsRuntime.IsPublicTargetZone(targetCardFromZone.Zone) ? publicTargetKind : null));
            _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(targetCard, CardLocation.Hand(source.Seat), CoreNativeOwnedCardQueriesRuntime.GetTargetCardFinishedMoveReason(actionKind, targetCardFromZone.Zone));
        }

        if (_coreWushuangTargetCardsOperations.TryContinueWushuangTargetCards(resolutionId, effectCard, effectiveCardKind, sourceSeat, targetSeat, actionKind))
            return;
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(resolutionId, effectCard);
        _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, effectCard, effectiveCardKind);
        var displayName = CardCatalog.Get(effectiveCardKind).DisplayName;
        var targetDescription = targetCardFromZone.Zone switch
        {
            CardZoneKind.Equipment => $"装备【{(targetCard.DisplayName)}】",
            CardZoneKind.Judgment => $"判定区【{(CardCatalog.Get(publicTargetKind).DisplayName)}】",
            _ => "一张手牌"
        };
        var outcome = actionKind == LegalActionKind.Dismantlement ? $"弃置 {(target.Name)} 的{(targetDescription)}" : $"获得 {(target.Name)} 的{(targetDescription)}";
        _coreMatchLogOperations.AddLog("CardEffect", CoreNativeCardClassificationsRuntime.IsPublicTargetZone(targetCardFromZone.Zone) ? $"{(source.Name)} 使用【{(displayName)}】，{(outcome)}。" : $"{(source.Name)} 使用【{(displayName)}】，{(outcome)}（牌面不公开）。", source.Seat, target.Seat);
    }

    internal void ResolveFireAttackEffect(NullificationWindowFrame pending)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var targetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("FireAttack must retain one target.");
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Count == 0)
        {
            SkipUnavailableTargetCardEffect(pending, _coreMatchParticipantsPlayersAccess._players[targetSeat], CardEffectSkipReason.TargetHandEmpty);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame cardUse || cardUse.Id != pending.ParentFrameId || cardUse.CardId != pending.EffectCardId || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(cardUse) != pending.EffectCardKind || cardUse.SourceSeat != source.Seat || cardUse.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(cardUse).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(cardUse)[cardUse.TargetIndex] != targetSeat)
        {
            throw new InvalidOperationException("FireAttack lost its current CardUse target.");
        }

        _coreNativeFireAttackOperations.SetFireAttackSelection(cardUse.Id, new FireAttackSelectionState(null));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(source.Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 使用【火攻】，等待目标展示一张手牌。", source.Seat, targetSeat);
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(pending.EffectCardId, pending.EffectCardKind, source.Seat, targetSeat));
        _coreNativeAiDecisionsOperations.NotifyAiOfFireAttack(source, _coreMatchParticipantsPlayersAccess._players[targetSeat]);
        _coreNativeFireAttackOperations.BeginFireAttackReveal(_coreNativeCardContinuationCursorsOperations.ActiveFireAttack ?? throw new InvalidOperationException("FireAttack lost its reveal selection."));
    }

    internal void ResolveBorrowedSwordEffect(NullificationWindowFrame pending)
    {
        if (pending.TargetSeats.Count != 2)
        {
            throw new InvalidOperationException("Borrowed Sword must retain its ordered two targets.");
        }

        // Sheyan invalidates one primary Borrowed Sword effect. A primary can
        // also be another pair's Slash victim; that Slash owns its defenses.
        var ineffectiveTarget = _coreTargetedOrdinaryTrickOperations.HasTargetedOrdinaryTrickAdjustment ? _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(pending.ParentFrameId, pending.TargetSeats[0]) ? pending.TargetSeats[0] : -1 : pending.TargetSeats.FirstOrDefault(targetSeat => _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(pending.ParentFrameId, targetSeat), -1);
        if (ineffectiveTarget >= 0)
        {
            _coreProgramCardTargetEffectsOperations.CompleteIneffectiveTrickTarget(pending, ineffectiveTarget);
            return;
        }

        var resolution = _kernelCoreMainOperations.CreateBorrowedSwordHandle(pending.ParentFrameId, pending.SourceSeat, pending.TargetSeats[0], pending.TargetSeats[1], _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending));
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = resolution;
        _coreMatchLogOperations.AddLog("CardUsed", $"{(_coreMatchParticipantsPlayersAccess._players[resolution.SourceSeat].Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[resolution.WeaponOwnerSeat].Name)} 使用【借刀杀人】，指定其攻击 {(_coreMatchParticipantsPlayersAccess._players[resolution.SlashTargetSeat].Name)}。", resolution.SourceSeat, resolution.WeaponOwnerSeat);
        _coreNativeBorrowedSwordOperations.BeginBorrowedSwordSlashChoice(resolution, includeFactionSlash: true);
    }

    internal void SkipUnavailableTargetCardEffect(NullificationWindowFrame pending, CharacterState target, CardEffectSkipReason reason)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(pending.ParentFrameId, pending.SourceSeat, target.Seat, pending.EffectCardKind, reason));
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending));
        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
        var explanation = reason == CardEffectSkipReason.TargetHandEmpty ? "已没有手牌" : "所选的公开牌已不在原处";
        _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】结算时，{(target.Name)}{(explanation)}，本次效果跳过。", pending.SourceSeat, target.Seat);
    }

    internal void ResolveDuelEffect(NullificationWindowFrame pending)
    {
        if (_coreNativeSlashTargetDuelOperations.TryResolveNativeSlashTargetDuel(pending))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        var targetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("Duel must retain one target.");
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var virtualOrigin = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.SelectedActorDuelOrigin;
        if (virtualOrigin is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { SelectedActorDuelOrigin = virtualOrigin with { AttackStarted = true } });
        var obtainOrigin = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.DamageTargetDuelOrigin;
        if (obtainOrigin is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { DamageTargetDuelOrigin = obtainOrigin with { AttackStarted = true } });
        var dualColorOrigin = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.DualColorDuelOrigin;
        if (dualColorOrigin is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { DualColorDuelOrigin = dualColorOrigin with { AttackStarted = true } });
        var conditionalOrigin = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.ConditionalDiscardDuelOrigin;
        if (conditionalOrigin is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { ConditionalDiscardDuelOrigin = conditionalOrigin with { AttackStarted = true } });
        var shiQuanOrigin = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.ShiQuanDuelOrigin;
        if (shiQuanOrigin is not null)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { ShiQuanDuelOrigin = shiQuanOrigin with { AttackStarted = true } });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.FriendRecastDuel is { } friendOrigin)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { FriendRecastDuel = friendOrigin with { AttackStarted = true } });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.ZhenyingDuel is { } zhenyingOrigin)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { ZhenyingDuel = zhenyingOrigin with { AttackStarted = true } });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.HeJinMouzhu is { } heJinOrigin)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { HeJinMouzhu = heJinOrigin with { AttackStarted = true } });
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.WuAnGuoLiyong is { } liyongOrigin)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(pending.ParentFrameId, use => use with { WuAnGuoLiyong = liyongOrigin with { AttackStarted = true } });
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(pending.ParentFrameId, source.Seat, target.Seat, virtualOrigin is not null || obtainOrigin is not null || dualColorOrigin is not null || conditionalOrigin is not null || shiQuanOrigin is not null || _characterHeJinOlOperations.IsHeJinMouzhuDuelUse(pending.ParentFrameId) || _characterWuAnGuoOlOperations.IsWuAnGuoLiyongDuelUse(pending.ParentFrameId) || _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(pending.ParentFrameId) || _characterHaoPuOlOperations.IsZhenyingDuelUse(pending.ParentFrameId) || _coreFriendEventsOperations.IsFriendRecastDuelUse(pending.ParentFrameId) || (_characterZhugeLiangMouOperations.IsZeroEntityUse(pending.ParentFrameId) || _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(pending.ParentFrameId)) ? null : _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), playedCardKind: pending.EffectCardKind, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.ParentFrameId), damageAmount: _coreDamageTargetRegionObtainOperations.PrepareDamageTargetDuelDamage(pending.ParentFrameId, targetSeat), programSkillCardUseFrameId: virtualOrigin?.ParentProgramFrameId ?? obtainOrigin?.ParentProgramFrameId ?? dualColorOrigin?.ParentProgramFrameId ?? conditionalOrigin?.ParentProgramFrameId ?? shiQuanOrigin?.ParentProgramFrameId ?? _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.FriendRecastDuel?.ProgramFrameId ?? _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.ZhenyingDuel?.ProgramFrameId ?? _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.HeJinMouzhu?.ProgramFrameId ?? _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)?.WuAnGuoLiyong?.ProgramFrameId ?? _characterWuJingOlOperations.WuJingHejiProgramParent(pending.ParentFrameId) ?? _characterFanJiangZhangDaOlOperations.JueshengDuelProgramParent(pending.ParentFrameId) ?? _characterXuHuangMouMobileOperations.MouXuHuangDuelProgramParent(pending.ParentFrameId) ?? _characterWenChouMouOlOperations.MouWenChouDuelProgramParent(pending.ParentFrameId));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        var duel = _kernelCoreMainOperations.CreateDuelHandle(attack);
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = duel;
        _coreMatchLogOperations.AddLog("CardUsed", $"{(source.Name)} 对 {(target.Name)} 使用【决斗】。", source.Seat, target.Seat);
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(pending.ParentFrameId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(pending.EffectCardId, pending.EffectCardKind, source.Seat, target.Seat));
        _coreNativeAiDecisionsOperations.NotifyAiOfDuel(source, target);
        _coreNativeDuelOperations.BeginDuelResponse(duel);
    }

    internal void ResolveIronChainEffect(NullificationWindowFrame pending)
    {
        if (_characterLuXunMouMobileOperations.Value.ResolveMouLuXunMultiIronChain(pending))
            return;
        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.ResolvingEffect);
        var changes = new List<string>(pending.TargetSeats.Count);
        foreach (var targetSeat in pending.TargetSeats)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
            if (!target.IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(pending.ParentFrameId, targetSeat))
            {
                continue;
            }

            var wasChained = target.IsChained;
            target.IsChained = _coreEquipmentSlotGroupProgramsOperations.ResolveEnteringChain(target, _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ForceChained) || !target.IsChained);
            if (!wasChained && target.IsChained)
                _coreCharacterStateProgramsOperations.RecordCharacterStateChange(target.Seat, SkillProgramTriggerWindow.CharacterEnteredChain, pending.ParentFrameId);
            changes.Add($"{(target.Name)}{((target.IsChained ? "横置" : "重置"))}");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(pending.ParentFrameId, pending.SourceSeat, target.Seat, target.IsChained));
        }

        CompleteIronChainEffect(pending, changes);
    }

    internal void CompleteIronChainEffect(NullificationWindowFrame pending, List<string> changes)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainResolvedEvent(pending.ParentFrameId, pending.SourceSeat, pending.TargetSeats));
        var source = _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat];
        _coreMatchLogOperations.AddLog("CardEffect", changes.Count == 0 ? $"{(source.Name)} 使用【铁索连环】，但目标已无存活角色。" : $"{(source.Name)} 使用【铁索连环】，" + string.Join("、", changes) + "。", source.Seat);
        var finishedIronChain = _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending);
        if (!_coreExtraCardEffectRoundsOperations.HasUnstartedExtraCardEffect(pending.ParentFrameId) && !_coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(pending.ParentFrameId) && !_coreTieredRoundZeroReturnsOperations.SkipTieredRoundZeroFinishedMovement(pending.ParentFrameId, finishedIronChain) && !_characterDiaoChanLeOperations.SkipDiouFinishedMovement(pending.ParentFrameId, finishedIronChain) && !_characterXiaoQiaoMouOlOperations.SkipMouXqJueyanFinishedMovement(pending.ParentFrameId, finishedIronChain) && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(pending.ParentFrameId, finishedIronChain.Id) && !_coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(pending.ParentFrameId) && !(_characterMiHengLeOperations.IsSiruiOutsideGeneralWeaponMaterial(pending.ParentFrameId, finishedIronChain.Id) || _characterHuangYueyingOlMouOperations.IsMouLixianRemovedMaterial(pending.ParentFrameId, finishedIronChain.Id)))
            _corePhysicalMovementFacadesOperations.MoveCard(finishedIronChain, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.IronChainFinished);
        _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
    }
}
