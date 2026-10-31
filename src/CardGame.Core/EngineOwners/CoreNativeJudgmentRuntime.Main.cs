#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeJudgmentRuntime
{
    internal bool? ResolveBaguaJudgment(NativeOwners.CardAttackHandle attack, CharacterState defender)
    {
        if (attack.IgnoresArmor)
        {
            throw new InvalidOperationException("The attacking weapon ignores armor, so Bagua cannot respond.");
        }

        if (!_coreNativeEquipmentAndFactionQueriesOperations.HasBagua(defender))
        {
            throw new InvalidOperationException("Bagua judgment requires Bagua equipment.");
        }

        if (_coreNativeEquipmentInvocationsOperations.TryBeginBaguaEquipmentInvocation(attack, defender))
            return null;
        return BeginJudgment(attack, defender.Seat, JudgmentReasons.BaguaDefense, attack.ResolutionId, attack.EffectiveCardKind, JudgmentContinuationKind.Bagua);
    }

    internal void CompleteBaguaResponse(NativeOwners.CardAttackHandle attack, bool succeeded)
    {
        succeeded &= _characterHuangZuMobileShiOperations.ShiHzResponseColorAllowed(attack.TargetSeat, effectiveIsRed: null);
        if (!succeeded)
        {
            _coreBaguaResponsesOperations.PublishFailedBaguaResponse(attack);
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } group && CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, attack))
        {
            var requiredCardKind = group.RequiredCardKind ?? throw new InvalidOperationException("A group response attack must declare a required card kind.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, requiredCardKind, attack.TargetSeat, UsedResponse: true, ResponseCardId: null, ResponseCardKind: CardKind.Dodge));
        }

        var continuation = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } responseGroup && CoreNativeDamageHandlesRuntime.SameAttackOwner(responseGroup.CurrentAttack, attack) ? ProgramCardContinuation.GroupResponse : ProgramCardContinuation.Dodge;
        if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], requesterSeat: null, attack.SourceSeat, CardKind.Dodge, [], continuation))
        {
            return;
        }

        _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }

    internal bool? BeginJudgment(NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, CardKind? sourceCard, JudgmentContinuationKind continuation, Card? delayedCard = null, int? sourceSeat = null, string? programResultBind = null, SkillProgramCardSetVisibility? programResultVisibility = null, OriginalUseLightningAttachment? originalUseLightning = null, int? originalUseLightningExcludedCardId = null)
    {
        if (_coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is { } originalJudgment && !_coreFinalizedJudgmentDamageChildrenOperations.Value.CanBeginFinalizedDamageChildJudgment(originalJudgment, parentFrameId, targetSeat, reason, continuation, programResultBind, programResultVisibility))
        {
            throw new InvalidOperationException("The engine cannot resolve two judgments at once.");
        }

        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var frame = new JudgmentFrame(frameId, parentFrameId, targetSeat, reason, CardId: null, CardKind: null, Suit: null, Succeeded: null)
        {
            SourceSeat = sourceSeat ?? attack?.SourceSeat ?? targetSeat,
            Continuation = continuation,
            ParentAttackId = attack?.ResolutionId,
            DelayedCardId = delayedCard?.Id,
            OriginalUseLightning = originalUseLightning,
            OriginalUseLightningExcludedCardId = originalUseLightningExcludedCardId,
            ProgramResultBind = programResultBind,
            ProgramResultVisibility = programResultVisibility
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentRequestedEvent(frameId, parentFrameId, targetSeat, reason, sourceCard));
        if (originalUseLightning is { } attachment)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningJudgmentEvent(attachment.Id, frameId, targetSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        var curseCard = _coreSourceCursesOperations.TakeSourceCurseForJudgment(frame);
        if (curseCard is null && !_corePhysicalDrawFacadesOperations.EnsureDrawPile())
        {
            frame = frame with
            {
                Succeeded = false,
                Step = ResolutionFrameStep.Completed
            };
            ReplaceJudgmentFrame(frame);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentResolvedEvent(frameId, parentFrameId, targetSeat, reason, CardId: null, CardKind: null, Suit: null, Rank: null, Succeeded: false));
            _coreMatchLogOperations.AddLog("Judgment", $"{(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 的判定因牌堆耗尽而失败。", targetSeat, frame.SourceSeat);
            _coreRuntimeOperations.PopResolutionFrame(frameId, ResolutionFrameKind.Judgment);
            if (CoreActualTurnsRuntime.IsDelayedJudgmentContinuation(continuation) || continuation == JudgmentContinuationKind.ProgramSkill)
            {
                ResumeCompletedJudgment(new GameEngine.JudgmentCompletionReceipt(frame, Succeeded: false));
            }

            return false;
        }

        frame = GetJudgmentFrame(frame.Id);
        var judgmentCard = curseCard ?? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
        frame = frame with
        {
            CardId = judgmentCard.Id,
            CardKind = judgmentCard.Kind,
            Suit = judgmentCard.Suit
        };
        ReplaceJudgmentFrame(frame);
        _corePhysicalMovementFacadesOperations.MoveCard(judgmentCard, curseCard is null ? CardLocation.DrawPile : CardLocation.Processing, CardLocation.Judgment(targetSeat), CardMoveReasons.JudgmentReveal);
        var candidates = CollectJudgmentReplacementCandidates(judgmentCard, targetSeat, reason);
        var orderedCandidates = candidates.ToArray();
        frame = frame with
        {
            ReplacementCandidates = Array.AsReadOnly(orderedCandidates),
            ReplacementCandidateIndex = 0,
            Step = orderedCandidates.Length == 0 ? ResolutionFrameStep.ResolvingEffect : ResolutionFrameStep.AwaitingResponse
        };
        ReplaceJudgmentFrame(frame);
        var pending = frame;
        if (orderedCandidates.Length == 0)
        {
            var succeeded = FinalizeJudgment(pending);
            if (succeeded is { } completed && (CoreActualTurnsRuntime.IsDelayedJudgmentContinuation(continuation) || continuation == JudgmentContinuationKind.ProgramSkill))
            {
                ResumeCompletedJudgment(new GameEngine.JudgmentCompletionReceipt(pending, completed));
            }

            return succeeded;
        }

        BeginJudgmentReplacementChoice(pending);
        return null;
    }

    internal IReadOnlyList<JudgmentTriggerCandidate> CollectJudgmentReplacementCandidates(Card judgmentCard, int targetSeat, string reason)
    {
        var candidates = new List<JudgmentTriggerCandidate>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
        {
            foreach (var binding in _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(owner, SkillProgramTriggerWindow.JudgmentReplacing).OrderBy(item => item.SkillId, StringComparer.Ordinal).ThenBy(item => item.Trigger.Id, StringComparer.Ordinal))
            {
                var program = binding.Program;
                var trigger = binding.Trigger;
                if (!_coreJudgmentReplacementProgramsOperations.MatchesProgramJudgmentReplacement(owner, trigger, targetSeat, reason) || (CharacterWangYiMobileRuntime.IsDeckTopJudgmentReplacement(trigger) ? !_characterWangYiMobileOperations.Value.HasDeckTopJudgmentReplacementMaterial() : _coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacementCards(owner, trigger, targetSeat, reason, ignoreCurrentColor: true).Count == 0))
                {
                    continue;
                }

                candidates.Add(new JudgmentTriggerCandidate(owner.Seat, $"program:{(program.Id)}:{(trigger.Id)}", Priority: 0, ProgramId: program.Id, ProgramTriggerId: trigger.Id, SkillInstanceId: binding.SkillInstanceId, GameplayHash: program.GameplayHash));
            }
        }

        var orderingSeat = (_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        return JudgmentTriggerOrdering.Order(candidates, orderingSeat, _coreMatchDefinitionPlayerCountAccess._playerCount, _coreTableOrderOperations.CurrentTableOrder());
    }

    internal void BeginJudgmentReplacementChoice(JudgmentFrame pending)
    {
        pending = GetJudgmentFrame(pending.Id);
        if (_coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment?.Id != pending.Id || _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)is not { } judgmentCard)
        {
            throw new InvalidOperationException("The replacement judgment is not the current resolution.");
        }

        var frame = GetJudgmentFrame(pending.Id);
        if (frame.ReplacementCandidateIndex >= frame.ReplacementCandidates.Count)
        {
            var succeeded = FinalizeJudgment(pending);
            if (succeeded is { } completed)
            {
                ResumeCompletedJudgment(new GameEngine.JudgmentCompletionReceipt(pending, completed));
            }

            return;
        }

        var candidate = CurrentJudgmentCandidate(pending) ?? throw new InvalidOperationException("The judgment replacement candidate cursor is invalid.");
        _coreJudgmentReplacementProgramsOperations.BeginProgramJudgmentReplacementChoice(pending, candidate);
    }

    internal bool IsProtectedJudgmentSourceEquipment(CharacterState owner, Card card, int judgmentSubjectSeat, string judgmentReason) => owner.Seat == judgmentSubjectSeat && string.Equals(judgmentReason, JudgmentReasons.BaguaDefense, StringComparison.Ordinal) && card.Kind == CardKind.BaguaFormation && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Equipment(owner.Seat);
    internal void AdvanceJudgmentCandidate(JudgmentFrame pending)
    {
        if (_coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment?.Id != pending.Id)
        {
            throw new InvalidOperationException("The replacement judgment is not the current resolution.");
        }

        var frame = GetJudgmentFrame(pending.Id);
        var nextIndex = checked(frame.ReplacementCandidateIndex + 1);
        if (nextIndex > frame.ReplacementCandidates.Count)
            throw new InvalidOperationException("The judgment replacement cursor advanced past its candidates.");
        ReplaceJudgmentFrame(frame with { ReplacementCandidateIndex = nextIndex, DelegatedReplacement = null, Step = nextIndex == frame.ReplacementCandidates.Count ? ResolutionFrameStep.ResolvingEffect : ResolutionFrameStep.AwaitingResponse });
        if (nextIndex == frame.ReplacementCandidates.Count)
        {
            pending = GetJudgmentFrame(pending.Id);
            var succeeded = FinalizeJudgment(pending);
            if (succeeded is { } completed)
            {
                ResumeCompletedJudgment(new GameEngine.JudgmentCompletionReceipt(pending, completed));
            }

            return;
        }

        BeginJudgmentReplacementChoice(pending);
    }

    internal bool? FinalizeJudgment(JudgmentFrame pending)
    {
        pending = GetJudgmentFrame(pending.Id);
        if (_coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)is not { } judgmentCard)
        {
            throw new InvalidOperationException("A judgment cannot resolve without a card.");
        }

        var frame = GetJudgmentFrame(pending.Id);
        var judgmentSuit = _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(pending, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], judgmentCard));
        if (_coreZhenyiJudgmentWindowsOperations.TryBeginZhenyiBeforeResultWindow(pending, judgmentCard, judgmentSuit))
            return null;
        var judgmentRank = _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentRank(pending, judgmentCard.Rank);
        _coreDiscardedProvenanceOperations.Value.CaptureProvenanceJudgmentOrigin(pending, judgmentCard, judgmentSuit);
        _coreSlashSuitDiscardOperations.Value.CaptureProgramSlashSuitJudgment(pending, judgmentSuit);
        _coreDamageJudgmentSuitPaymentsOperations.CaptureDamageJudgmentSuitResult(pending, judgmentCard, judgmentSuit);
        _coreExactRepeatedJudgmentOperations.Value.CaptureProgramRepeatedJudgmentClaimOutcome(pending, judgmentCard, judgmentSuit);
        var succeeded = _characterGuoJiaMouOperations.IsMouGjLightningJudgment(pending) || _coreSourceHuojieOperations.Value.IsSourceHuojieLightningJudgment(pending) ? judgmentSuit == Suit.Spade && judgmentRank is >= 2 and <= 9 : pending.Continuation switch
        {
            JudgmentContinuationKind.Lightning => judgmentSuit == Suit.Spade && judgmentRank is >= 2 and <= 9,
            JudgmentContinuationKind.Indulgence => judgmentSuit == Suit.Heart,
            JudgmentContinuationKind.SupplyShortage => judgmentSuit == Suit.Club,
            _ => CoreNativeCardClassificationsRuntime.IsRedSuit(judgmentSuit)};
        succeeded = _corePassiveRulePoliciesOperations.ApplyDelayedJudgmentOutcomePolicies(pending, succeeded);
        ReplaceJudgmentFrame(frame with { CardId = judgmentCard.Id, CardKind = judgmentCard.Kind, Suit = judgmentSuit, Succeeded = succeeded, Step = ResolutionFrameStep.Completed });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentResolvedEvent(pending.Id, frame.ParentFrameId, pending.TargetSeat, pending.Reason, judgmentCard.Id, judgmentCard.Kind, judgmentSuit, judgmentRank, succeeded));
        var abilityName = pending.Continuation switch
        {
            JudgmentContinuationKind.Bagua or JudgmentContinuationKind.FactionDefenseBagua => "八卦阵",
            JudgmentContinuationKind.Indulgence => "乐不思蜀",
            JudgmentContinuationKind.SupplyShortage => "兵粮寸断",
            JudgmentContinuationKind.Lightning => "闪电",
            JudgmentContinuationKind.ProgramSkill when _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(owner => owner.Id == pending.ParentFrameId)is { SlashSuitDiscard: { Stage: ProgramSlashSuitDiscardStage.Judging } draft } namedOwner && draft.JudgmentFrameId == pending.Id && draft.CardUseFrameId == pending.ParentAttackId && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(namedOwner.SkillId, out var definition) => definition.Name,
            _ => pending.Reason
        };
        var judgmentResult = pending.Continuation == JudgmentContinuationKind.Lightning ? succeeded ? "命中" : "未命中" : succeeded ? "成功" : "失败";
        _coreMatchLogOperations.AddLog("Judgment", pending.ZhenyiOverride is null ? $"{(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat].Name)} 发动【{(abilityName)}】判定：最终为【{(judgmentCard.DisplayName)}】（{(judgmentCard.Suit)} {(judgmentCard.RankText)}），判定{(judgmentResult)}。" : $"{(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat].Name)} 发动【{(abilityName)}】判定：最终为【{(judgmentCard.DisplayName)}】（{(judgmentSuit)} {(judgmentRank)}），判定{(judgmentResult)}。", pending.TargetSeat, pending.SourceSeat);
        pending = GetJudgmentFrame(pending.Id);
        if (_coreJudgmentProgramsOperations.Value.TryBeginProgramJudgmentWindow(pending, judgmentCard, judgmentSuit, succeeded))
        {
            return null;
        }

        return CompleteFinalizedJudgment(pending, judgmentCard, succeeded);
    }

    internal bool? CompleteFinalizedJudgment(JudgmentFrame pending, Card judgmentCard, bool succeeded)
    {
        if (pending.Continuation == JudgmentContinuationKind.ProgramSkill)
        {
            FinishProgramSkillJudgment(pending, judgmentCard);
            return succeeded;
        }

        FinishResolvedJudgment(pending);
        return succeeded;
    }

    internal void FinishResolvedJudgment(JudgmentFrame pending)
    {
        var judgmentCard = _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending) ?? throw new InvalidOperationException("A resolved judgment must retain its card until disposition.");
        if (_cardZonesOperations._cardZones.GetLocation(judgmentCard.Id) == CardLocation.Judgment(pending.TargetSeat))
        {
            _corePhysicalMovementFacadesOperations.MoveCard(judgmentCard, CardLocation.Judgment(pending.TargetSeat), CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        }

        SetJudgmentFrameStep(pending.Id, ResolutionFrameStep.Completed);
        if (pending.Continuation != JudgmentContinuationKind.Lightning || GetJudgmentFrame(pending.Id).Succeeded != true)
            _coreRuntimeOperations.PopResolutionFrame(pending.Id, ResolutionFrameKind.Judgment);
    }

    internal void FinishProgramSkillJudgment(JudgmentFrame pending, Card judgmentCard)
    {
        SetJudgmentFrameStep(pending.Id, ResolutionFrameStep.Completed);
        _coreRuntimeOperations.PopResolutionFrame(pending.Id, ResolutionFrameKind.Judgment);
        var location = _cardZonesOperations._cardZones.GetLocation(judgmentCard.Id);
        if (location == CardLocation.Judgment(pending.TargetSeat) && _coreProgramLifecycleOperations.GetActiveProgramFrame(pending.ParentFrameId).RepeatedJudgment is null)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(judgmentCard, location, CardLocation.Processing, new CardMoveReason("skill-program.judgment.result"));
        }
    }

    internal void CompleteDelayedJudgment(JudgmentFrame pending, bool succeeded)
    {
        var delayedCard = _coreNativeDamageHandlesOperations.Value.GetDelayedJudgmentCard(pending) ?? throw new InvalidOperationException("A delayed judgment has no delayed card.");
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat];
        var delayedLocation = _cardZonesOperations._cardZones.GetLocation(delayedCard.Id);
        if (delayedLocation != CardLocation.Judgment(target.Seat))
        {
            throw new InvalidOperationException("The resolved delayed card is not in its target's judgment zone.");
        }

        var effectiveCardKind = _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(delayedCard);
        var delayedEffectSucceeded = !succeeded;
        var delayedEffects = delayedEffectSucceeded ? CoreActualTurnsRuntime.GetDelayedTurnEffects(effectiveCardKind) : GameEngine.DelayedTurnEffects.None;
        _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= delayedEffects;
        _corePhysicalMovementFacadesOperations.MoveCard(delayedCard, CardLocation.Judgment(target.Seat), CardLocation.DiscardPile, CardMoveReasons.DelayedCardFinish);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DelayedCardResolvedEvent(pending.Id, delayedCard.Id, effectiveCardKind, target.Seat, _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)?.Id, succeeded, SkippedPlayPhase: delayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipPlayPhase)) { SkippedDrawPhase = delayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipDrawPhase) });
        var phaseDescription = delayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipDrawPhase) && delayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipPlayPhase) ? "跳过摸牌和出牌阶段" : delayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipDrawPhase) ? "跳过摸牌阶段" : delayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipPlayPhase) ? "跳过出牌阶段" : "正常进入后续阶段";
        _coreMatchLogOperations.AddLog("DelayedCardResolved", $"{(target.Name)} 的【{(CardCatalog.Get(effectiveCardKind).DisplayName)}】判定为{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(_coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)?.Suit))}，{(phaseDescription)}。", target.Seat);
        _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(target);
    }

    internal void CompleteLightningWithoutHit(long resolutionId, Card lightning, CharacterState target, int? judgmentCardId)
    {
        var nextTargetSeat = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreTableOrderOperations.TableStepSeat(target.Seat, offset)).FirstOrDefault(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreMatchParticipantsPlayersAccess._players[seat].JudgmentAreaAbolished && !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && // A transfer carries the held delayed card's actual color,
 // evaluated for its current holder, never the judgment card.
        !_coreDyingWindowSkillPoliciesOperations.IsBlackTrickTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Lightning, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(target, lightning))) && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Lightning), target.Seat);
        if (nextTargetSeat != target.Seat)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(lightning, CardLocation.Judgment(target.Seat), CardLocation.Judgment(nextTargetSeat), CardMoveReasons.DelayedCardTransfer);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LightningResolvedEvent(resolutionId, lightning.Id, target.Seat, judgmentCardId, Hit: false, NextTargetSeat: nextTargetSeat, DamageAmount: 0));
        _coreMatchLogOperations.AddLog("LightningResolved", nextTargetSeat == target.Seat ? $"{(target.Name)} 的【闪电】未生效，因没有其他合法目标而留在其判定区。" : $"{(target.Name)} 的【闪电】未生效，移至 {(_coreMatchParticipantsPlayersAccess._players[nextTargetSeat].Name)} 的判定区。", target.Seat, nextTargetSeat);
        _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(target, excludedCardId: nextTargetSeat == target.Seat ? lightning.Id : null);
    }

    internal void CompleteLightningJudgment(JudgmentFrame pending, bool hit)
    {
        if (pending.OriginalUseLightning is not null)
        {
            _coreOriginalUseLightningOperations.CompleteOriginalUseLightningJudgment(pending, hit);
            return;
        }

        var lightning = _coreNativeDamageHandlesOperations.Value.GetDelayedJudgmentCard(pending) ?? throw new InvalidOperationException("A Lightning judgment has no delayed card.");
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat];
        var lightningLocation = _cardZonesOperations._cardZones.GetLocation(lightning.Id);
        if (lightningLocation != CardLocation.Judgment(target.Seat))
        {
            throw new InvalidOperationException("The resolved Lightning is not in its target's judgment zone.");
        }

        if (!hit)
        {
            CompleteLightningWithoutHit(pending.Id, lightning, target, _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)?.Id);
            return;
        }

        const int lightningDamage = 3;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LightningResolvedEvent(pending.Id, lightning.Id, target.Seat, _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)?.Id, Hit: true, NextTargetSeat: null, DamageAmount: lightningDamage));
        _coreMatchLogOperations.AddLog("LightningResolved", $"{(target.Name)} 的【闪电】命中，受到 {(lightningDamage)} 点雷电伤害。", target.Seat);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(pending.Id, target.Seat, target.Seat, lightning, damageAmount: lightningDamage, playedCardKind: CardKind.Lightning, isDelayedJudgmentDamage: true, delayedJudgmentSeat: target.Seat);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }

    internal void ResumeAfterLightningDamage(int delayedSeat, int? excludedCardId = null)
    {
        var current = _coreMatchParticipantsPlayersAccess._players[delayedSeat];
        if (!current.IsAlive)
        {
            _coreActualTurnsOperations.EndTurn();
            return;
        }

        _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(current, excludedCardId);
    }

    internal void ResumeCompletedJudgment(GameEngine.JudgmentCompletionReceipt receipt)
    {
        var pending = receipt.Frame;
        var succeeded = receipt.Succeeded;
        if (pending.Continuation == JudgmentContinuationKind.ProgramSkill)
        {
            if (_characterGuoJiaMobileGodOperations.TryReturnShenGuoJiaJudgment(pending))
                return;
            if (_characterZhongHuiAnniversaryGodOperations.TryReturnLishiJudgment(pending))
                return;
            if (_characterZhangFeiAnniversaryGodOperations.TryReturnShenZfJudgment(pending))
                return;
            if (_coreWushuangCombatProgramsOperations.Value.TryReturnWushuangLightningJudgment(pending))
                return;
            if (_characterMaYuanYiOlOperations.Value.TryReturnBinghuoJudgment(pending))
                return;
            if (_characterGuoJiaMouOperations.ReturnMouGjJudgment(pending))
                return;
            if (_coreSourceHuojieOperations.Value.ReturnSourceHuojieJudgment(pending))
                return;
            if (_characterZhouQunOlOperations.Value.TryReturnNingwuJudgment(pending))
                return;
            if (_characterQianZhaoOlOperations.TryReturnWeifuJudgment(pending))
                return;
            if (_characterZhangJiOlOperations.Value.TryReturnZhangJiJudgment(pending))
                return;
            if (_coreDamageJudgmentSuitPaymentsOperations.TryReturnDamageJudgmentSuitPayment(pending))
                return;
            if (_coreSlashSuitDiscardOperations.Value.TryReturnProgramSlashSuitJudgment(pending))
                return;
            if (_coreProgramLifecycleOperations.GetActiveProgramFrame(pending.ParentFrameId).RepeatedJudgment is not null)
            {
                _coreProgramRepeatedJudgmentOperations.ResumeProgramRepeatedJudgment(pending);
                return;
            }

            if (_coreProgramJudgmentOutcomesOperations.TryReturnProgramJudgmentOutcome(pending))
                return;
            var bind = pending.ProgramResultBind ?? throw new InvalidOperationException("A program judgment lost its result binding.");
            var visibility = pending.ProgramResultVisibility ?? throw new InvalidOperationException("A program judgment lost its result visibility.");
            var ownedResultClaimed = _coreOwnedDamagePointJudgmentsOperations.Value.IsOwnedDamagePointJudgmentResultClaimed(pending);
            var cardIds = !ownedResultClaimed && _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)is { } resultCard ? new[]
            {
                resultCard.Id
            }

            : Array.Empty<int>();
            var locations = !ownedResultClaimed && _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)is { } boundCard ? new[]
            {
                _cardZonesOperations._cardZones.GetLocation(boundCard.Id)
            }

            : Array.Empty<CardLocation>();
            _coreProgramLifecycleOperations.SetProgramCardSet(pending.ParentFrameId, bind, cardIds, visibility, locations, _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending)is { } finalCard ? _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(pending, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], finalCard)) : null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(pending.ParentFrameId);
            return;
        }

        if (pending.Continuation == JudgmentContinuationKind.Bagua)
        {
            var attack = _coreNativeDamageHandlesOperations.Value.GetJudgmentAttack(pending) ?? throw new InvalidOperationException("A Bagua judgment has no attack continuation.");
            CompleteBaguaResponse(attack, succeeded);
            return;
        }

        if (pending.Continuation == JudgmentContinuationKind.FactionDefenseBagua)
        {
            var attack = _coreNativeDamageHandlesOperations.Value.GetJudgmentAttack(pending) ?? throw new InvalidOperationException("A FactionDefense Bagua judgment has no attack continuation.");
            _coreNativeDodgeOperations.CompleteFactionDefenseBaguaResponse(attack, succeeded);
            return;
        }

        if (pending.Continuation == JudgmentContinuationKind.Lightning)
        {
            CompleteLightningJudgment(pending, succeeded);
            return;
        }

        if (CoreActualTurnsRuntime.IsDelayedJudgmentContinuation(pending.Continuation))
        {
            CompleteDelayedJudgment(pending, succeeded);
            return;
        }
    }

    internal JudgmentFrame GetJudgmentFrame(long frameId)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault(candidate => candidate.Id == frameId) as JudgmentFrame;
        return frame ?? throw new InvalidOperationException($"Resolution frame {(frameId)} is not a Judgment frame.");
    }

    internal JudgmentTriggerCandidate? CurrentJudgmentCandidate(JudgmentFrame pending)
    {
        var frame = GetJudgmentFrame(pending.Id);
        return frame.ReplacementCandidateIndex < frame.ReplacementCandidates.Count ? frame.ReplacementCandidates[frame.ReplacementCandidateIndex] : null;
    }

    internal void SetJudgmentFrameStep(long frameId, ResolutionFrameStep step)
    {
        var frame = GetJudgmentFrame(frameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[_resolutionStackOperations._resolutionStack.FindLastIndex(candidate => candidate.Id == frameId)].Id, frame with { Step = step });
    }

    internal void ReplaceJudgmentFrame(JudgmentFrame frame)
    {
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(candidate => candidate.Id == frame.Id);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not JudgmentFrame)
        {
            throw new InvalidOperationException($"Resolution frame {(frame.Id)} is not a Judgment frame.");
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, frame);
    }
}
