#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoSongOlRuntime
{
    internal const string LiluDrawReason = "skill-program.caosong.lilu.draw";
    internal const string LiluGiftReason = "skill-program.caosong.lilu.gift";
    internal CardConversionSource CaoSongSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    SkillProgramStepOutcome ICaoSongProgramHost.ReplaceDrawWithLiluGift(ProgramSkillFrame frame) => BeginCaoSongLilu(frame);
    void ICaoSongProgramHost.GrantYizhengSupport(ProgramSkillFrame frame) => IssueCaoSongYizheng(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26030)]
    internal bool CanOfferCaoSongLilu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ReplaceDrawWithLiluGift) || context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat] is { IsAlive: true, HasHpValue: true } && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat);
    internal SkillProgramStepOutcome BeginCaoSongLilu(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.CaoSongLilu is not null || frame.InstructionIndex != 1 || frame.TriggerId is null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } parent)
            throw new InvalidOperationException("Lilu requires its exact unpaid native draw-phase replacement.");
        var normal = parent.FrozenBaseDrawCount ?? _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { FrozenBaseDrawCount = normal, NormalDrawAdjustment = -normal });
        var limit = Math.Min(5, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongLilu = new(CaoSongSource(frame), frame.GameplayHash, frame.InstructionIndex, parent.Id, limit, CaoSongLiluStage.Drawing, SequenceBefore: before, SequenceAfter: before) });
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], Math.Max(0, limit - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count), true, new(LiluDrawReason));
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CaoSongLilu = frame.CaoSongLilu!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueCaoSongLilu(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainCaoSongLiluChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueCaoSongLilu(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { CaoSongLilu: { } receipt } frame && frame.Id == id)
        {
            if (DrainCaoSongLiluChildren(id))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            receipt = frame.CaoSongLilu!;
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteCaoSongLilu(frame, receipt.GiftCount > 0);
                return;
            }

            if (receipt.GiftCount == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            {
                CompleteCaoSongLilu(frame, false);
                return;
            }

            switch (receipt.Stage)
            {
                case CaoSongLiluStage.Drawing:
                    if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count == 0 || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != frame.OwnerSeat))
                    {
                        CompleteCaoSongLilu(frame, true);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongLilu = receipt with { Stage = CaoSongLiluStage.Recipient } });
                    PublishCaoSongLiluChoice(frame);
                    return;
                case CaoSongLiluStage.Recipient:
                case CaoSongLiluStage.Cards:
                    PublishCaoSongLiluChoice(frame);
                    return;
                case CaoSongLiluStage.GiftChildren:
                    if (!receipt.Grows)
                    {
                        CompleteCaoSongLilu(frame, true);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongLilu = receipt with { Stage = CaoSongLiluStage.MaximumChildren } });
                    _coreProgramLifecycleOperations.ChangeProgramMaximumHp(frame, 1);
                    continue;
                case CaoSongLiluStage.MaximumChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongLilu = receipt with { Stage = CaoSongLiluStage.RecoveryChildren } });
                    _coreProgramLifecycleOperations.RecoverProgramTargetTo(frame.Id, frame.OwnerSeat, frame.OwnerSeat, SkillProgramNumberExpression.IntegerConstant, checked(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp + 1), true);
                    continue;
                case CaoSongLiluStage.RecoveryChildren:
                    CompleteCaoSongLilu(frame, true);
                    return;
                default:
                    throw new InvalidOperationException("Unknown Lilu stage.");
            }
        }
    }

    internal IReadOnlyList<PromptChoice> CaoSongLiluChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.CaoSongLilu!;
        PromptChoice Choice(string action, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"caosong-lilu.{(frame.Id)}.{(receipt.Stage)}.{(action)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "caosong-lilu", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["option"] = action });
        if (receipt.Stage == CaoSongLiluStage.Recipient)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat).Select(p => Choice("recipient", $"交给 {(p.Name)} 手牌", targets: [p.Seat])).ToArray());
        if (receipt.Stage != CaoSongLiluStage.Cards || receipt.RecipientSeat is not { } recipient || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            return[];
        var choices = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(card => Choice("card", $"{((receipt.SelectedCardIds.Contains(card.Id) ? "取消选择" : "选择"))}【{(card.DisplayName)}】 {(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))} {(card.RankText)}", [card.Id])).ToList();
        if (receipt.SelectedCardIds.Count > 0)
            choices.Add(Choice("give", $"交出已选 {(receipt.SelectedCardIds.Count)} 张手牌", receipt.SelectedCardIds, [recipient]));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishCaoSongLiluChoice(ProgramSkillFrame frame)
    {
        var choices = CaoSongLiluChoices(frame);
        if (choices.Count == 0)
        {
            CompleteCaoSongLilu(frame, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, frame.CaoSongLilu!.Stage == CaoSongLiluStage.Recipient ? "礼赂：选择其他角色" : "礼赂：选择至少一张手牌，然后确认交出", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("caosong-lilu")]
    internal void AnswerCaoSongLilu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoSongLilu: { } receipt } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], CaoSongLiluChoices(frame).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Lilu lost its actual private chooser or current hand selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["option"] == "recipient")
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CaoSongLilu = receipt with { Stage = CaoSongLiluStage.Cards, RecipientSeat = choice.Targets.Single() }, Step = ResolutionFrameStep.ResolvingEffect });
        else if (choice.Parameters["option"] == "card")
        {
            var card = choice.Cards.Single();
            var selected = receipt.SelectedCardIds.Contains(card) ? receipt.SelectedCardIds.Where(id => id != card).ToArray() : receipt.SelectedCardIds.Append(card).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CaoSongLilu = receipt with { SelectedCardIds = selected }, Step = ResolutionFrameStep.ResolvingEffect });
        }
        else if (choice.Parameters["option"] == "give")
        {
            var ids = receipt.SelectedCardIds;
            var recipient = receipt.RecipientSeat!.Value;
            if (ids.Count == 0 || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat)) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
                throw new InvalidOperationException("Lilu must commit at least one current physical hand card to the selected other recipient.");
            var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoSongLiluGiftCommittedEvent>().LastOrDefault(e => e.Source.OwnerSeat == frame.OwnerSeat)?.GiftCount;
            var grows = previous is null || ids.Count > previous;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongLilu = receipt with { Stage = CaoSongLiluStage.GiftChildren, GiftCount = ids.Count, PreviousGiftCount = previous, Grows = grows, SequenceBefore = before, SequenceAfter = before }, Step = ResolutionFrameStep.ResolvingEffect });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(LiluGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, recipient, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            receipt = frame.CaoSongLilu!with
            {
                SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CaoSongLilu = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoSongLiluGiftCommittedEvent(frame.Id, receipt.Source, recipient, ids.Count, previous, grows, before, receipt.SequenceAfter));
        }
        else
            throw new InvalidOperationException("Unknown Lilu choice.");
        ContinueCaoSongLilu(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ReplaceDrawWithLiluGift)]
    internal PromptChoice ChooseAiCaoSongLilu(PendingDecision decision, ProgramSkillFrame frame)
    {
        var receipt = frame.CaoSongLilu!;
        if (receipt.Stage == CaoSongLiluStage.Recipient)
            return decision.Choices.First();
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoSongLiluGiftCommittedEvent>().LastOrDefault(e => e.Source.OwnerSeat == frame.OwnerSeat)?.GiftCount ?? 0;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var desired = previous < hand.Count ? previous + 1 : 1;
        if (receipt.SelectedCardIds.Count >= desired)
            return decision.Choices.Single(c => c.Parameters["option"] == "give");
        return decision.Choices.Where(c => c.Parameters["option"] == "card" && !receipt.SelectedCardIds.Contains(c.Cards.Single())).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).ThenBy(c => c.Cards.Single()).First();
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26030)]
    internal bool ResumeCaoSongLilu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoSongLilu: not null } frame || frame.Id != id)
            return false;
        ContinueCaoSongLilu(id);
        return true;
    }

    internal void CompleteCaoSongLilu(ProgramSkillFrame frame, bool completed)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongLilu = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26030)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26030)]
    internal bool CanContinuePaidCaoSongLilu(ProgramSkillFrame frame) => frame.CaoSongLilu is { GiftCount: > 0 } && _coreProgramEventHistoryOperations.ProgramEventHistory<CaoSongLiluGiftCommittedEvent>().Any(e => e.ProgramFrameId == frame.Id && e.Source == frame.CaoSongLilu.Source);
}
