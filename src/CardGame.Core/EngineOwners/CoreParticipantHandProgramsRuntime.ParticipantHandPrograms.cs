#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreParticipantHandProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IParticipantHandProgramHost.ViewAndTakeHand(ProgramSkillFrame frame, int sourceSeat, int count) => BeginPrivateHandTake(frame.Id, sourceSeat, count);
    SkillProgramStepOutcome global::CardGame.Core.IParticipantHandProgramHost.RequestHandRepayment(ProgramSkillFrame frame, int sourceSeat) => BeginHandRepayment(frame.Id, sourceSeat);
    internal IEnumerable<CardSnapshot> GetParticipantPrivatelyViewedCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewerSeat && f.PrivateHandTake is { Paid: false }).SelectMany(f => f.PrivateHandTake!.ViewedCardIds.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(f.PrivateHandTake.SourceSeat)).Single(c => c.Id == id))).Select(_coreSnapshotValueProjectionOperations.ToSnapshot);
    internal SkillProgramStepOutcome BeginPrivateHandTake(long frameId, int sourceSeat, int count)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.PrivateHandTake is not null || sourceSeat == frame.OwnerSeat || frame.SelectedTargetSeats is not [var selected] || selected != sourceSeat)
            throw new InvalidOperationException("Private hand selection lost its distinct selected source.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[sourceSeat]);
        if (!_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || hand.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PrivateHandTake = new(sourceSeat, Math.Min(count, hand.Count), Array.AsReadOnly(hand.Select(c => c.Id).ToArray()), Array.AsReadOnly(Array.Empty<int>())) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPrivateHandViewedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, sourceSeat, hand.Count));
        PublishParticipantHandChoice(frame, frame.OwnerSeat, PrivateHandTakeChoices(frame), "观看手牌，并选择要获得的牌。", sourceSeat);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PrivateHandTakeChoices(ProgramSkillFrame frame)
    {
        var draft = frame.PrivateHandTake!;
        return Array.AsReadOnly(draft.ViewedCardIds.Except(draft.SelectedCardIds).Select(id =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(draft.SourceSeat)).Single(c => c.Id == id);
            return new PromptChoice(new ChoiceId($"private-hand-take.{(frame.Id)}.{(draft.SelectedCardIds.Count)}.{(id)}"), $"获得【{(card.DisplayName)}】（{(card.Suit)} {(card.RankText)}）", [id], [], new Dictionary<string, string> { ["program-action"] = "private-hand-take", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["source-zone"] = "Hand", ["card-owner-seat"] = draft.SourceSeat.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        }).ToArray());
    }

    internal SkillProgramStepOutcome BeginHandRepayment(long frameId, int sourceSeat)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.HandRepayment is not null || sourceSeat == frame.OwnerSeat || frame.SelectedTargetSeats is not [var selected] || selected != sourceSeat)
            throw new InvalidOperationException("Hand repayment lost its distinct selected source.");
        if (!_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HandRepayment = new(sourceSeat, frame.InstructionIndex) });
        PublishParticipantHandChoice(frame, sourceSeat, HandRepaymentChoices(frame), "交给技能拥有者一张符合花色的手牌，或失去1点体力。", frame.OwnerSeat);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramEffect ParticipantHandPaused(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
    internal IReadOnlyList<PromptChoice> HandRepaymentChoices(ProgramSkillFrame frame)
    {
        var draft = frame.HandRepayment!;
        var effect = ParticipantHandPaused(frame);
        var choices = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.SourceSeat]).Where(c => effect.Suits.Contains(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[draft.SourceSeat], c))).Select(c => new PromptChoice(new ChoiceId($"hand-repay.{(frame.Id)}.card-{(c.Id)}"), $"交出【{(c.DisplayName)}】（{(c.Suit)} {(c.RankText)}）", [c.Id], [], new Dictionary<string, string> { ["program-action"] = "hand-repayment", ["repayment"] = "give", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList();
        choices.Add(new(new ChoiceId($"hand-repay.{(frame.Id)}.lose-hp"), "失去1点体力", [], [], new Dictionary<string, string> { ["program-action"] = "hand-repayment", ["repayment"] = "lose-hp", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishParticipantHandChoice(ProgramSkillFrame frame, int chooser, IReadOnlyList<PromptChoice> choices, string text, int target)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = target,
            Choices = choices,
            RequiredCardCount = frame.PrivateHandTake is { Paid: false } take ? take.RequiredCount - take.SelectedCardIds.Count : 0,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("private-hand-take", "hand-repayment")]
    internal void ResolveParticipantHandChoice(PromptChoice selected)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack.Last().Id);
        var effect = ParticipantHandPaused(frame);
        if (selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The participant hand choice lost its owning frame.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "技能拥有者或技能实例已失效，未支付未完成的成本。");
            return;
        }

        if (effect.Op == SkillProgramEffectOp.ViewAndTakeSelectedTargetHand && frame.PrivateHandTake is { Paid: false } take)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !PrivateHandTakeChoices(frame).Any(c => c.Id == selected.Id) || take.ViewedCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(take.SourceSeat)) || !_coreMatchParticipantsPlayersAccess._players[take.SourceSeat].IsAlive)
                throw new InvalidOperationException("The private hand choice no longer matches its frozen source.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            var ids = take.SelectedCardIds.Append(selected.Cards.Single()).ToArray();
            if (ids.Length < take.RequiredCount)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PrivateHandTake = take with { SelectedCardIds = Array.AsReadOnly(ids) } });
                PublishParticipantHandChoice(frame, frame.OwnerSeat, PrivateHandTakeChoices(frame), "继续选择，选齐后同时获得。", take.SourceSeat);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PrivateHandTake = take with { SelectedCardIds = Array.AsReadOnly(ids), Paid = true } });
            _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(take.SourceSeat)).Single(c => c.Id == id)).ToArray(), CardLocation.Hand(take.SourceSeat), CardLocation.Hand(frame.OwnerSeat), new($"skill-program.{(frame.SkillId)}.private-hand-take"));
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (effect.Op != SkillProgramEffectOp.RequestHandBySuitsOrLoseHp || frame.HandRepayment is not { Paid: false } repayment || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != repayment.SourceSeat || !_coreMatchParticipantsPlayersAccess._players[repayment.SourceSeat].IsAlive || !HandRepaymentChoices(frame).Any(c => c.Id == selected.Id))
            throw new InvalidOperationException("The repayment choice is no longer legal for its actual hand owner.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var give = selected.Parameters.GetValueOrDefault("repayment") == "give";
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HandRepayment = repayment with { Paid = true, PaidCardId = give ? selected.Cards.Single() : null, LostHp = !give } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHandRepaymentResolvedEvent(frame.Id, frame.SkillId, repayment.SourceSeat, frame.OwnerSeat, give, !give));
        if (give)
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[repayment.SourceSeat]).Single(c => c.Id == selected.Cards.Single());
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(repayment.SourceSeat), CardLocation.Hand(frame.OwnerSeat), new($"skill-program.{(frame.SkillId)}.hand-repayment"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, repayment.SourceSeat, frame.OwnerSeat, [card.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence));
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
        else
        {
            var result = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, repayment.SourceSeat, 1);
            if (result != SkillProgramStepOutcome.AwaitChild)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
    }

    internal SkillProgramStepOutcome BeginBoundParticipantGift(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, CardLocation target, CardMoveReason reason)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        ValidateBoundParticipantGiftWindow(frame);
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || target.OwnerSeat is not { } recipient || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && !_corePindianHandReturnsOperations.HasPaidPindianHandReturn(frame) || !ids.All(id => source.CardIds.Contains(id)) || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != source.SourceLocations[source.CardIds.ToList().IndexOf(id)]))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "赠牌参与者或冻结来源已失效，未交出牌且不继续请求杀。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (frame.BoundParticipantGift is not null || target != CardLocation.Hand(frame.SelectedTargetSeats.Single()) || ids.Count == 0 || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != frame.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            throw new InvalidOperationException("An awaited bound gift lost its unpaid owned HE cost.");
        var gift = new ProgramBoundParticipantGiftReceipt(frame.InstructionIndex, bind, target.OwnerSeat!.Value);
        if (frame.WindowContext is null && frame.TriggerId is null && !_corePindianHandReturnsOperations.IsPindianHandReturn(frame) && !_coreMantianGuohaiOperations.IsMantianGuohaiGiftProgram(frame))
            gift = gift with
            {
                RankSum = _coreOwnedHandRankSumGiftOperations.CaptureOwnedHandRankSumGift(frame, ids, reason)
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundParticipantGift = gift });
        if (gift.RankSum is null)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, target, reason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, target.OwnerSeat!.Value, ids, _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, target, reason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, target.OwnerSeat!.Value, ids, _coreCardMovementJournalMovementSequenceAccess._movementSequence, (batchId, records) => _coreOwnedHandRankSumGiftOperations.SealOwnedHandRankSumGift(frame.Id, batchId, records)));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ValidateBoundParticipantGiftWindow(ProgramSkillFrame frame)
    {
        if (frame.MantianGuohaiReturn is not null)
        {
            if (!_coreMantianGuohaiOperations.IsMantianGuohaiGiftProgram(frame) || ParticipantHandPaused(frame)is not { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "given", AwaitMovementTriggers: true, Destination: SkillProgramCardDestination.SelectedTargetHand })
                throw new InvalidOperationException("Mantian Guohai gift lost its exact issued native return.");
            return;
        }

        if (_corePindianHandReturnsOperations.HasPaidPindianHandReturn(frame))
        {
            var paid = frame.PindianHandReturn!;
            if (frame.InstructionIndex != 5 || paid.ReturnCount is not { } count || _coreProgramLifecycleOperations.GetProgramCardSet(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions[4].SourceBind!).CardIds.Count != count)
                throw new InvalidOperationException("Pindian return gift lost its frozen half-hand obligation.");
            return;
        }

        if (frame.WindowContext is null && frame.TriggerId is null)
        {
            _coreOwnedHandRankSumGiftOperations.ValidateOwnedHandRankSumGiftActivation(frame);
            return;
        }

        // The original DrawPhaseEnded producer retains its existing contract.
        // The added action window must keep the exact live parent and candidate
        // throughout all gift movement children; no material use is reissued.
        if (frame.WindowContext?.Window == SkillProgramTriggerWindow.DrawPhaseEnded)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 2 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseBeforeTargetEffects, CardUse: { } cardContext } context || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != context.Window || window.Continuation is not (ProgramCardContinuation.BeforeTargetEffects or ProgramCardContinuation.BeforeTrickTargetEffects) || _resolutionStackOperations._resolutionStack[index - 2] is not CardUseFrame use || use.Id != window.ParentFrameId || window.Action.Type != CardActionType.Use || use.Action is not { Type: CardActionType.Use } action || action.ActionId != window.Action.ActionId || action.ActorSeat != window.Action.ActorSeat || action.ProviderSeat != window.Action.ProviderSeat || action.RequesterSeat != window.Action.RequesterSeat || action.EffectiveKind != window.Action.EffectiveKind || !action.PhysicalCards.SequenceEqual(window.Action.PhysicalCards) || cardContext.CardActionId != action.ActionId || cardContext.ActorSeat != action.ActorSeat || context.SourceSeat != action.ActorSeat || context.OwnerSeat != frame.OwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            throw new InvalidOperationException("An awaited bound gift lost its exact card-use window and owning action.");
        var candidate = window.Candidates[window.CandidateIndex];
        var effect = ParticipantHandPaused(frame);
        if (candidate.OwnerSeat != frame.OwnerSeat || candidate.SkillId != frame.SkillId || candidate.TriggerId != frame.TriggerId || frame.ActivationId != frame.TriggerId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.GameplayHash != frame.GameplayHash || effect is not { Op: SkillProgramEffectOp.MoveBoundCards, AwaitMovementTriggers: true, Destination: SkillProgramCardDestination.SelectedTargetHand })
            throw new InvalidOperationException("An awaited bound gift lost its exact candidate or movement instruction.");
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 750)]
    internal bool ResumeParticipantHandPayment(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || !(frame.BoundParticipantGift is not null || frame.PrivateHandTake is { Paid: true } || frame.HandRepayment is { Paid: true }))
            return false;
        ValidateParticipantHandDrafts(frame, ParticipantHandPaused(frame));
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundParticipantGift = null, PrivateHandTake = null, HandRepayment = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "游戏已结束，已付手牌操作不重复，后续结算取消。");
            return true;
        }

        return false;
    }

    internal void ValidateParticipantHandDrafts(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.PrivateHandTake is { } take && (paused.Op != SkillProgramEffectOp.ViewAndTakeSelectedTargetHand || frame.SelectedTargetSeats is not [var source] || source != take.SourceSeat || source == frame.OwnerSeat || take.RequiredCount is < 1 or > 2 || take.SelectedCardIds.Distinct().Count() != take.SelectedCardIds.Count || take.ViewedCardIds.Distinct().Count() != take.ViewedCardIds.Count || take.SelectedCardIds.Any(id => !take.ViewedCardIds.Contains(id)) || (take.Paid ? take.SelectedCardIds.Count != take.RequiredCount : take.SelectedCardIds.Count >= take.RequiredCount)))
            throw new InvalidOperationException("Private hand selection lost its exact instruction or frozen card receipt.");
        if (frame.HandRepayment is { } repay && (paused.Op != SkillProgramEffectOp.RequestHandBySuitsOrLoseHp || repay.InstructionIndex != frame.InstructionIndex || frame.SelectedTargetSeats is not [var giver] || giver != repay.SourceSeat || giver == frame.OwnerSeat || repay.Paid && (repay.LostHp == (repay.PaidCardId is not null))))
            throw new InvalidOperationException("Hand repayment lost its exact one-card or one-HP paid receipt.");
        if (frame.BoundParticipantGift is { } gift && (paused.Op != SkillProgramEffectOp.MoveBoundCards || !paused.AwaitMovementTriggers || paused.Destination != SkillProgramCardDestination.SelectedTargetHand || gift.InstructionIndex != frame.InstructionIndex || paused.SourceBind != gift.SourceBind || frame.SelectedTargetSeats is not [var recipient] || recipient != gift.RecipientSeat))
            throw new InvalidOperationException("A bound participant gift lost its exact paid instruction.");
        if (frame.BoundParticipantGift is not null)
            ValidateBoundParticipantGiftWindow(frame);
        if (!ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()))
            return;
        if (frame.PrivateHandTake is { Paid: false } view && (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices, PrivateHandTakeChoices(frame)) || view.ViewedCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(view.SourceSeat))))
            throw new InvalidOperationException("The private hand prompt lost its chooser-safe frozen source.");
        if (frame.HandRepayment is { Paid: false } request && (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != request.SourceSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices, HandRepaymentChoices(frame))))
            throw new InvalidOperationException("The hand repayment prompt changed its valid effective suits.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ViewAndTakeSelectedTargetHand, SkillProgramEffectOp.RequestHandBySuitsOrLoseHp)]
    internal PromptChoice SelectAiParticipantHandChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (frame.PrivateHandTake is not null)
            return decision.Choices.OrderByDescending(c => CardCatalog.Get(_cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(frame.PrivateHandTake.SourceSeat)).Single(card => card.Id == c.Cards.Single()).Kind).HandKeepValue).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        var source = _coreMatchParticipantsPlayersAccess._players[frame.HandRepayment!.SourceSeat];
        var gift = decision.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_corePhysicalOwnedCardQueriesOperations.GetHand(source).Single(card => card.Id == c.Cards.Single()), source)).ThenBy(c => c.Id.Value, StringComparer.Ordinal).FirstOrDefault();
        return gift is not null && (source.Hp <= 1 || global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_corePhysicalOwnedCardQueriesOperations.GetHand(source).Single(c => c.Id == gift.Cards.Single()), source) < 50) ? gift : decision.Choices.Single(c => c.Parameters.GetValueOrDefault("repayment") == "lose-hp");
    }
}
