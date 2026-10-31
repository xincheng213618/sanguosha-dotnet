#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheYingxiangRuntime
{
    internal const string ShanheYingxiangId = "shanhe:companion-yingxiang";
    internal const string ShanheQingjueDiscardReason = "skill-program.shanhe-yingxiang.qingjue-discard";
    SkillProgramStepOutcome IShanheYingxiangHost.ResolveYingxiang(ProgramSkillFrame frame, bool used) => BeginShanheYingxiang(frame, used);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 5406)]
    internal void RecordShanheYingxiangGift(CardMovedEvent fact)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheYingxiangMovement) || fact.From.OwnerSeat is not { } owner || fact.To.OwnerSeat is not { } recipient || owner == recipient || fact.To.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[owner]).Any(program => program.Id == ShanheYingxiangId))
            return;
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == fact.CardId && m.From == fact.From && m.To == fact.To && m.Reason == fact.Reason);
        if (move is not null)
            TagShanheYingxiang(owner, move);
    }

    internal void TagShanheYingxiang(int owner, CardMovementRecord move)
    {
        if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheYingxiangTaggedEvent>().Any(tag => tag.OwnerSeat == owner && tag.MovementSequence == move.Sequence))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheYingxiangTaggedEvent(owner, move.Sequence));
    }

    internal bool IsShanheYingxiangBefore(int owner, int cardId, int before)
    {
        var tag = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheYingxiangTaggedEvent>().Where(item => item.OwnerSeat == owner && item.MovementSequence < before).OrderByDescending(item => item.MovementSequence).FirstOrDefault(item => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == item.MovementSequence && move.CardId == cardId));
        if (tag is null)
            return false;
        // A transfer retains the original issuer; a settled return to deck, discard or outside ends this tag.
        return !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.CardId == cardId && move.Sequence > tag.MovementSequence && move.Sequence < before && move.To.Zone is CardZoneKind.DrawPile or CardZoneKind.DiscardPile or CardZoneKind.OutsideGame);
    }

    internal bool IsShanheYingxiangTrueUseLoss(CardMovementRecord move) => move.To == CardLocation.Processing && (move.Reason == CardMoveReasons.Use || move.Reason == CardMoveReasons.EquipmentUse || move.Reason == CardMoveReasons.IronChainUse || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeTrueUseProcessingEntryEvent>().Any(item => item.EntrySequence == move.Sequence && item.CardId == move.CardId && item.From == move.From));
    internal bool HasShanheYingxiangNonUseLoss(int owner, CardMovementBatchContext batch) => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheYingxiangQingjueStartedEvent>().Any(item => item.OwnerSeat == owner && item.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber) && batch.Movements.Any(move => move.From.OwnerSeat is { } holder && holder != owner && move.From != move.To && move.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment && IsShanheYingxiangBefore(owner, move.CardId, move.Sequence) && !IsShanheYingxiangTrueUseLoss(move));
    internal bool IsShanheYingxiangMovement(ProgramTriggerCandidate candidate) => candidate.SkillId == ShanheYingxiangId && candidate.BindingId == "marked-non-use-loss";
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 5406)]
    internal void CollectShanheYingxiangMovement(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        if (!_contentRegistryOperations._contentRegistry.Skills.ContainsKey(ShanheYingxiangId))
            return;
        candidates.RemoveAll(IsShanheYingxiangMovement);
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && HasShanheYingxiangNonUseLoss(player.Seat, batch)))
            candidates.AddRange(_coreProgramLifecycleOperations.CollectProgramTriggerCandidates(player, SkillProgramTriggerWindow.CardsMoved).Where(IsShanheYingxiangMovement));
    }

    internal ProgramSkillWindowContext CreateShanheYingxiangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch);
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 5406)]
    internal bool IsIssuedShanheYingxiangMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsShanheYingxiangMovement(candidate) && context is { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } && context.OwnerSeat == candidate.OwnerSeat && context.ParentFrameId == batch.Id && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is { } window && window.Id == batch.Id && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash == candidate.GameplayHash;
    internal CardActionContext? ShanheYingxiangUsedAction(int owner, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } use } || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(window => window.Id == context.ParentFrameId)is not { } parent || parent.Action.ActionId != use.CardActionId || parent.Action.ActorSeat == owner || !(parent.Action.Type == CardActionType.Use || parent.Action.Type == CardActionType.Response && parent.Action.EffectiveKind is CardKind.Dodge or CardKind.Nullification) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheYingxiangUsePaidEvent>().Any(item => item.OwnerSeat == owner && item.ActionId == use.CardActionId))
            return null;
        foreach (var card in parent.Action.PhysicalCards)
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move => move.CardId == card.CardId && move.From == card.From && move.To == CardLocation.Processing);
            if (entry is not null && IsShanheYingxiangBefore(owner, card.CardId, entry.Sequence))
                return parent.Action;
        }

        return null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5406)]
    internal bool CanOfferShanheYingxiang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => candidate.SkillId != ShanheYingxiangId || (trigger.Id == "marked-use" ? ShanheYingxiangUsedAction(candidate.OwnerSeat, context)is not null : context.MovementBatch is { } batch && HasShanheYingxiangNonUseLoss(candidate.OwnerSeat, batch));
    internal SkillProgramStepOutcome BeginShanheYingxiang(ProgramSkillFrame supplied, bool used)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheYingxiang is not null || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Yingxiang requires its original unpaid trigger window.");
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var action = used ? ShanheYingxiangUsedAction(frame.OwnerSeat, context) : null;
        if (used && action is null || !used && (context.MovementBatch is not { } batch || !HasShanheYingxiangNonUseLoss(frame.OwnerSeat, batch)))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = new(source, frame.GameplayHash, context.ParentFrameId, used ? ShanheYingxiangStage.Draw : ShanheYingxiangStage.Cost, action?.ActorSeat ?? -1, action?.ActionId ?? 0) });
        if (used)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheYingxiangUsePaidEvent(frame.Id, frame.OwnerSeat, action!.ActorSeat, action.ActionId));
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheYingxiangQingjueStartedEvent(frame.Id, frame.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, context.MovementBatch!.Id));
        ContinueShanheYingxiang(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card[] ShanheQingjueCards(ProgramSkillFrame frame)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        return hand.Where(card => hand.Count(other => other.Suit == card.Suit) != 1 && !frame.ShanheYingxiang!.Cards.Contains(card.Id) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, CardLocation.Hand(frame.OwnerSeat), OwnedCardMoveIntent.Discard)).ToArray();
    }

    internal IReadOnlyList<PromptChoice> ShanheYingxiangChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ShanheYingxiang!;
        PromptChoice Choice(string branch, int value, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"yingxiang.{(frame.Id)}.{(receipt.Stage)}.{(branch)}.{(value)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "shanhe-yingxiang", ["branch"] = branch, ["value"] = value.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        if (receipt.Stage == ShanheYingxiangStage.Cost)
            return ShanheQingjueCards(frame).Select(card => Choice("card", card.Id, "清绝：选择【" + card.DisplayName + "】", [card.Id])).Append(Choice("pay", 0, receipt.Cards.Count == 0 ? "清绝：不弃牌" : "清绝：弃置所选牌")).ToArray();
        if (receipt.Stage == ShanheYingxiangStage.Option)
            return new[]
            {
                1,
                2
            }.Where(option => (receipt.OptionsChosen & option) == 0).Select(option => Choice("option", option, option == 1 ? "清绝：将弃置牌交给其他角色" : "清绝：获得手牌缺少花色的牌各一张")).ToArray();
        if (receipt.Stage == ShanheYingxiangStage.Recipient)
            return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat).Select(player => Choice("recipient", player.Seat, "交给" + player.Name, targets: [player.Seat])).ToArray();
        return[];
    }

    internal void PublishShanheYingxiang(ProgramSkillFrame frame)
    {
        var choices = ShanheYingxiangChoices(frame);
        if (choices.Count == 0)
        {
            FinishShanheYingxiang(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "萦香：执行清绝", choices.SelectMany(choice => choice.Cards).ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shanhe-yingxiang")]
    internal void AnswerShanheYingxiang(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheYingxiang: { } receipt } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ShanheYingxiangChoices(frame).Where(item => item.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Yingxiang lost its private payer, exact card selection or living recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var value = int.Parse(choice.Parameters["value"], System.Globalization.CultureInfo.InvariantCulture);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters["branch"])
        {
            case "card":
                receipt = receipt with
                {
                    Cards = receipt.Cards.Append(value).ToArray()
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt });
                if (receipt.Cards.Count == 2)
                    PayShanheQingjue(frame);
                break;
            case "pay":
                PayShanheQingjue(frame);
                break;
            case "option":
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { OptionsChosen = receipt.OptionsChosen | value, OptionsRemaining = receipt.OptionsRemaining - 1, Stage = value == 1 ? ShanheYingxiangStage.Recipient : ShanheYingxiangStage.EffectChildren } });
                if (value == 2)
                    GainShanheQingjueMissingSuits(frame);
                break;
            case "recipient":
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.EffectChildren } });
                var cards = receipt.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move => move.CardId == id)?.Reason.Value == ShanheQingjueDiscardReason).ToArray();
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(value), new("skill-program.shanhe-yingxiang.qingjue-give"), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, value, cards, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, moves) =>
                {
                    foreach (var move in moves.Where(move => move.To == CardLocation.Hand(value)))
                        TagShanheYingxiang(frame.OwnerSeat, move);
                }));
                break;
            default:
                throw new InvalidOperationException("Unknown Yingxiang choice.");
        }

        ContinueShanheYingxiang(frame.Id);
    }

    internal void PayShanheQingjue(ProgramSkillFrame frame)
    {
        var receipt = frame.ShanheYingxiang!;
        if (receipt.Cards.Count == 0)
        {
            FinishShanheYingxiang(frame);
            return;
        }

        // The official wording qualifies each card by its suit's current hand multiplicity.
        // It grants at most two listed options, so select up to two such cards; no Qingjue passive is acquired.
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.CostChildren, OptionsRemaining = receipt.Cards.Count } });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, receipt.Cards, new(ShanheQingjueDiscardReason));
    }

    internal void GainShanheQingjueMissingSuits(ProgramSkillFrame frame)
    {
        if (!_corePhysicalDrawFacadesOperations.EnsureDrawPile())
            return;
        var held = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(card => card.Suit).ToHashSet();
        var cards = new[]
        {
            Suit.Spade,
            Suit.Heart,
            Suit.Club,
            Suit.Diamond
        }.Where(suit => !held.Contains(suit)).Select(suit => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(card => card.Suit == suit)).OfType<Card>().Select(card => card.Id).ToArray();
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(frame.OwnerSeat), new("skill-program.shanhe-yingxiang.qingjue-gain"));
    }

    internal void ContinueShanheYingxiang(long frameId)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShanheYingxiang: { } receipt } frame && frame.Id == frameId)
        {
            if (receipt.Source != new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || frame.WindowContext?.ParentFrameId != receipt.ParentWindowId || receipt.Cards is not System.Collections.IList { IsReadOnly: true } || receipt.Cards.Count > 2)
                throw new InvalidOperationException("Yingxiang lost its original frame, content identity or frozen paid selection.");
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program))
                return;
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShanheYingxiang(frame);
                return;
            }

            switch (receipt.Stage)
            {
                case ShanheYingxiangStage.Cost:
                case ShanheYingxiangStage.Option:
                case ShanheYingxiangStage.Recipient:
                    PublishShanheYingxiang(frame);
                    return;
                case ShanheYingxiangStage.CostChildren:
                case ShanheYingxiangStage.EffectChildren:
                    if (receipt.OptionsRemaining == 0)
                    {
                        FinishShanheYingxiang(frame);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.Option } });
                    continue;
                case ShanheYingxiangStage.Draw:
                    if (receipt.DrawIndex >= 2)
                    {
                        FinishShanheYingxiang(frame);
                        return;
                    }

                    var seat = receipt.DrawIndex == 0 ? frame.OwnerSeat : receipt.UserSeat;
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { DrawIndex = receipt.DrawIndex + 1, Stage = _coreMatchParticipantsPlayersAccess._players[seat].IsAlive ? ShanheYingxiangStage.DrawWaiting : ShanheYingxiangStage.Draw, DrawOutcome = null } });
                    if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                        continue;
                    _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, seat, 1, new("skill-program.shanhe-yingxiang.draw"));
                    return;
                case ShanheYingxiangStage.DrawChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.Draw }, PendingMovementContinuation = null });
                    continue;
                case ShanheYingxiangStage.DrawWaiting:
                    throw new InvalidOperationException("Yingxiang must await its exact native draw return.");
                default:
                    FinishShanheYingxiang(frame);
                    return;
            }
        }
    }

    internal void FinishShanheYingxiang(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheYingxiangUse)]
    internal void ReturnShanheYingxiangDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheYingxiang is not { Stage: ShanheYingxiangStage.DrawWaiting, DrawOutcome: null } receipt || outcome.RecipientSeat != (receipt.DrawIndex == 1 ? frame.OwnerSeat : receipt.UserSeat) || outcome.RequestedCount != 1 || frame.NativeDraw is not null)
            throw new InvalidOperationException("Yingxiang lost its once-issued source/holder native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheYingxiang = receipt with { Stage = ShanheYingxiangStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 5406)]
    internal bool ResumeShanheYingxiang(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheYingxiang: not null } frame || frame.Id != frameId)
            return false;
        ContinueShanheYingxiang(frameId);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5406)]
    internal bool ContinuePaidShanheYingxiang(ProgramSkillFrame frame) => frame.ShanheYingxiang is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheYingxiangMovement)]
    internal PromptChoice AiShanheYingxiang(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.OrderByDescending(choice => choice.Parameters["branch"] switch
    {
        "card" => 10 - CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]),
        "pay" => frame.ShanheYingxiang!.Cards.Count == 0 ? -10 : 1,
        "option" => choice.Parameters["value"] == "2" ? 20 : 10,
        "recipient" => _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()]) ? 20 : 0,
        _ => 0
    }).First();
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 200)]
    internal ProgramSkillWindowContext? TryCreateShanheYingxiangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsShanheYingxiangMovement(candidate))
            return CreateShanheYingxiangContext(frame, candidate);
        return null;
    }
}
