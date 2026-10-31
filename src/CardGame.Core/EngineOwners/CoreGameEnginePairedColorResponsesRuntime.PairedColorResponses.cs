#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePairedColorResponsesRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 150)]
    internal bool PairedColorResponseFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferPairedColorCardDisposition) && query.Category == SkillProgramCardCategory.Trick;
    internal const string PairedColorDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardOrigins.PairedColorDiscardReason;
    internal const string PairedColorObtainReason = "skill-program.paired-color-response.obtain";
    internal long PairedColorSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal PairedColorResponseLinkedEvent? BuildPairedColorResponseLink(ProgramCardTriggerWindowFrame window)
    {
        if (window.ResponseCompletion is not { } completion || !_coreResponseCompletionOperations.ValidResponseCompletionParent(window) || window.Action is not { Type: CardActionType.Response } response)
            return null;
        var rootId = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == window.ParentFrameId)is NullificationWindowFrame counter ? counter.ParentFrameId : window.ParentFrameId;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(rootId)is not { Action: { Type: CardActionType.Use } original } use || response.ParentActionId != original.ActionId || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, original))
            return null;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var acceptedIndex = Array.FindIndex(history, e => e is CardActionAcceptedEvent a && a.Action.ActionId == response.ActionId);
        if (acceptedIndex < 0)
            return null;
        var paired = original;
        var pairedSeat = original.ActorSeat;
        // Nullification is itself a true Use. Its directly answered counterspell
        // belongs to this exact native chain, rather than every response sharing
        // the original trick's ParentActionId. Duel Slash remains a mere Response
        // and therefore always answers the original Duel Use.
        if (completion.OriginalContinuation == ProgramCardContinuation.NullificationResponse)
        {
            var previous = history.Take(acceptedIndex).OfType<CardResponseCompletedEvent>().LastOrDefault(e => e.ParentFrameId == window.ParentFrameId && e.OriginalContinuation == ProgramCardContinuation.NullificationResponse);
            if (previous is not null)
            {
                var prior = history.Take(acceptedIndex).OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == previous.ActionId).ToArray();
                if (prior is not [var accepted] || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(accepted.Action, ProgramCompletedResponseKind.Nullification) || accepted.Action.ParentActionId != original.ActionId || previous.ActorSeat != accepted.Action.ProviderSeat || previous.NativeActorSeat != accepted.Action.ActorSeat)
                    return null;
                paired = accepted.Action;
                pairedSeat = previous.ActorSeat;
            }
        }

        return new(window.Id, response.ActionId, window.ParentFrameId, rootId, original.ActionId, completion.OriginalContinuation, completion.CompletionActorSeat, response.ActorSeat, paired.ActionId, pairedSeat, response.EffectiveKind, paired.EffectiveKind, response.EffectiveIsRed, paired.EffectiveIsRed);
    }

    internal void CapturePairedColorResponseLink(ProgramCardTriggerWindowFrame window)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferPairedColorCardDisposition))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PairedColorResponseLinkedEvent>().Any(e => e.ResponseActionId == window.Action.ActionId))
            throw new InvalidOperationException("A native response cannot replace its frozen direct color pair.");
        if (BuildPairedColorResponseLink(window)is { } pair)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(pair);
    }

    internal PairedColorResponseLinkedEvent? ExactPairedColorResponseLink(ProgramCardTriggerWindowFrame window)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PairedColorResponseLinkedEvent>().Where(e => e.ResponseWindowFrameId == window.Id).ToArray();
        return facts is [var fact] && BuildPairedColorResponseLink(window) == fact ? fact : null;
    }

    internal bool TryGetPairedColorResponse(ProgramSkillWindowContext context, out ProgramCardTriggerWindowFrame window, out PairedColorResponseLinkedEvent pair, out int counterpart)
    {
        window = null !;
        pair = null !;
        counterpart = -1;
        if (context is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } identity } || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame actual || actual.ResponseCompletion is not { CostsDrained: true } || !_coreResponseCompletionOperations.ValidResponseCompletionWindow(actual) || identity.CardActionId != actual.Action.ActionId || identity.ParentCardUseFrameId != actual.ParentFrameId || ExactPairedColorResponseLink(actual)is not { } link || link.ResponseIsRed is not { } responseRed || link.PairedIsRed is not { } pairedRed || responseRed != pairedRed || link.ResponseSeat == link.PairedSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(link.ResponseSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(link.PairedSeat) || context.OwnerSeat != link.ResponseSeat && context.OwnerSeat != link.PairedSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == actual.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != actual.ParentFrameId)
            return false;
        window = actual;
        pair = link;
        counterpart = context.OwnerSeat == link.ResponseSeat ? link.PairedSeat : link.ResponseSeat;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1020)]
    internal bool CanOfferPairedColorDisposition(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferPairedColorCardDisposition))
            return true;
        if (!TryGetPairedColorResponse(context, out _, out var pair, out var counterpart) || candidate.OwnerSeat != context.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[counterpart].IsAlive || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PairedColorDispositionStartedEvent>().Any(e => e.ResponseActionId == pair.ResponseActionId && e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.StateId == trigger.Effects[0].StateId))
            return false;
        var obtain = _coreDamageCounterpartAcquisitionOperations.HasDamageCounterpartAcquisition(candidate.OwnerSeat, candidate.SkillId, trigger.Effects[0].StateId!, counterpart);
        return PairedColorLegalCards(candidate.OwnerSeat, counterpart, obtain).Any();
    }

    internal IEnumerable<Card> PairedColorLegalCards(int owner, int counterpart, bool obtain) => new[]
    {
        CardLocation.Hand(counterpart),
        CardLocation.Equipment(counterpart)
    }.SelectMany(from => _cardZonesOperations._cardZones.CardsAt(from).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, from, obtain ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Discard)));
    internal bool ExactPairedColorDispositionParent(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window, out PairedColorResponseLinkedEvent pair, out int counterpart)
    {
        window = null !;
        pair = null !;
        counterpart = -1;
        if (f.TriggerId is null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !TryGetPairedColorResponse(context, out window, out pair, out counterpart) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), context, window))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == window.Id && window.Activated && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is { } program && program.GameplayHash == f.GameplayHash && ProgramInstructionResolver.Default.FindTrigger(program, f.TriggerId)is { } trigger && trigger.Effects is [{ Op: SkillProgramEffectOp.OfferPairedColorCardDisposition }];
    }

    internal SkillProgramStepOutcome OfferPairedColorCardDisposition(ProgramSkillFrame supplied, string stateId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.PairedColorDisposition is not null || f.PendingMovementContinuation is not null || !ExactPairedColorDispositionParent(f, out _, out var pair, out var counterpart))
            throw new InvalidOperationException("Paired-color disposition lost its exact completed native response and observer.");
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        PairedColorResponseContract.ValidateTrigger(f.SkillId, trigger);
        if (trigger.Effects[0].StateId != stateId)
            throw new InvalidOperationException("Paired-color disposition changed its declared state.");
        if (!CanOfferPairedColorDisposition(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), trigger, f.WindowContext!))
            return SkillProgramStepOutcome.Continue;
        var obtain = _coreDamageCounterpartAcquisitionOperations.HasDamageCounterpartAcquisition(f.OwnerSeat, f.SkillId, stateId, counterpart);
        var receipt = new ProgramPairedColorDispositionReceipt(1, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, stateId, pair, counterpart, obtain);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedColorDisposition = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedColorDispositionStartedEvent(f.Id, receipt.Source, f.GameplayHash, stateId, pair.ResponseActionId, pair.PairedActionId, counterpart, obtain));
        PublishPairedColorDisposition(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PairedColorDispositionChoices(ProgramSkillFrame f)
    {
        var r = f.PairedColorDisposition!;
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.CounterpartSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], r.Obtain ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Discard).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "paired-color-disposition" } })).ToArray());
    }

    internal void PublishPairedColorDisposition(ProgramSkillFrame f)
    {
        var r = f.PairedColorDisposition!;
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, PairedColorDispositionChoices(f), r.Obtain ? "获得本次同色响应对方的一张手牌或装备。" : "弃置本次同色响应对方的一张手牌或装备。", r.CounterpartSeat);
    }

    [ProgramAnswer("paired-color-disposition")]
    internal void ResolvePairedColorDisposition(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The paired-color chooser lost its owning frame.");
        if (f.PairedColorDisposition is not { Stage: PairedColorDispositionStage.ChoosingCard } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !_corePairedColorResponseObserversOperations.IsPairedColorDispositionChoice(f, prompt) || !PairedColorDispositionChoices(f).Any(c => CoreGameEngineSameNameHandDiscardOrDamageRuntime.SameNameHandChoicesEqual(c, selected)) || !Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), out var slot))
            throw new InvalidOperationException("Paired-color disposition requires its current exact owner-only published card slot.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.CounterpartSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            CompletePairedColorDisposition(f, false);
            return;
        }

        var from = new CardLocation(zone, r.CounterpartSeat);
        var pool = _cardZonesOperations._cardZones.CardsAt(from);
        if (slot < 0 || slot >= pool.Count || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, pool[slot], from, r.Obtain ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("The selected paired-color card is no longer payable.");
        var card = pool[slot];
        var to = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, zone == CardZoneKind.Equipment && card.IsGeneralWeapon ? CardLocation.OutsideGame : r.Obtain ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedColorDisposition = r with { Stage = PairedColorDispositionStage.MovementChildren, PaidCardId = card.Id, PaidKind = card.Kind, PaidFrom = from, PaidGeneralWeapon = card.IsGeneralWeapon, SequenceBefore = PairedColorSequence, SequenceAfter = PairedColorSequence }, PendingMovementContinuation = new(r.CounterpartSeat, 0, null) });
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        var logicalDiscard = r.Obtain ? null : _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [card.Id], new(PairedColorDiscardReason), f.Id);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], to, new(r.Obtain ? PairedColorObtainReason : PairedColorDiscardReason), (batch, records) =>
        {
            if (!r.Obtain)
                foreach (var movement in records)
                    _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id);
            if (records is not [var paid] || paid.CardId != card.Id || paid.CardKind != card.Kind || paid.From != from || paid.To != to)
                throw new InvalidOperationException("Paired-color disposition changed its original real card payment.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = current.PairedColorDisposition!;
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { PairedColorDisposition = receipt with { SequenceAfter = paid.Sequence, BatchId = batch } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedColorDispositionPaidEvent(f.Id, card.Id, card.Kind, from, to, card.IsGeneralWeapon, receipt.SequenceBefore, paid.Sequence, batch));
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainPairedColorDispositionChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 160)]
    internal bool ResumePairedColorDisposition(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PairedColorDisposition: { } r } f || f.Id != frameId)
            return false;
        if (r.Stage == PairedColorDispositionStage.ChoosingCard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return true;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.CounterpartSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || PairedColorDispositionChoices(f).Count == 0)
            {
                CompletePairedColorDisposition(f, false);
                return true;
            }

            PublishPairedColorDisposition(f);
            return true;
        }

        if (DrainPairedColorDispositionChildren(f))
            return true;
        CompletePairedColorDisposition(f, true);
        return true;
    }

    internal void CompletePairedColorDisposition(ProgramSkillFrame f, bool paid)
    {
        var r = f.PairedColorDisposition!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PairedColorDispositionCompletedEvent(f.Id, r.Pair.ResponseActionId, r.CounterpartSeat, r.Obtain, paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PairedColorDisposition = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 140)]
    internal bool ReturnPairedColorDispositionMovement(ProgramSkillFrame f)
    {
        if (f.PairedColorDisposition is not { Stage: PairedColorDispositionStage.MovementChildren })
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 510)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 530)]
    internal bool CanContinuePairedColorDisposition(ProgramSkillFrame f) => f.PairedColorDisposition is { Stage: PairedColorDispositionStage.MovementChildren } && _corePairedColorResponseObserversOperations.ValidPairedColorDispositionReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.OfferPairedColorCardDisposition)]
    internal PromptChoice SelectAiPairedColorDisposition(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderByDescending(c => c.Parameters.GetValueOrDefault("source-zone") == "Equipment" && c.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.PairedColorDisposition!.CounterpartSeat]) : 3).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    SkillProgramStepOutcome IPairedColorResponseHost.OfferPairedColorCardDisposition(ProgramSkillFrame frame, string stateId) => OfferPairedColorCardDisposition(frame, stateId);
}
