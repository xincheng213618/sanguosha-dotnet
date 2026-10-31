#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawAdviceProgramsRuntime
{
    internal long DrawAdviceSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static string DrawAdviceReason(string skill, string action) => RuntimeSkills.RuntimeRules_Core_DrawAdvicePrograms.DrawAdviceReason(skill, action);
    internal bool ExactDrawAdviceParent(ProgramSkillFrame frame)
    {
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program;
        var trigger = program?.Triggers.SingleOrDefault(t => t.Id == frame.TriggerId);
        if (program?.GameplayHash != frame.GameplayHash || trigger is null || frame.InstructionIndex != 2 || frame.ActivationId != frame.TriggerId || trigger.Effects is not [_, { Op: SkillProgramEffectOp.DrawTwoThenDiscardTwoIfOverMaxHp }] || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var recipient] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || recipient == frame.OwnerSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Draw || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseEnded } context || context.OwnerSeat != frame.OwnerSeat)
            return false;
        DrawAdviceContract.ValidateTrigger(frame.SkillId, trigger);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != frame.OwnerSeat || parent.Window != context.Window || parent.Continuation != ProgramLifecycleContinuation.CompleteDrawPhaseEnded || parent.DrawPhaseEndedDelayedEffects is not { } delayed || (delayed & (int)GameEngine.DelayedTurnEffects.SkipDrawPhase) != 0 || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            return false;
        var candidate = parent.Candidates[parent.CandidateIndex];
        return candidate.OwnerSeat == frame.OwnerSeat && candidate.SkillId == frame.SkillId && candidate.BindingId == frame.TriggerId && candidate.SkillInstanceId == frame.SkillInstanceId && candidate.GameplayHash == frame.GameplayHash && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.SkillInstanceId == frame.SkillInstanceId && e.Window == context.Window) == 1;
    }

    internal SkillProgramStepOutcome BeginDrawAdvice(ProgramSkillFrame input, int recipient)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!ExactDrawAdviceParent(frame) || frame.SelectedTargetSeats.Single() != recipient || frame.DrawAdviceReceipt is not null || frame.PendingMovementContinuation is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawAdviceStartedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Draw advice requires its exact unpaid own draw-end program and selected recipient.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "弼政来源或受益者失效，未重新支付摸牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var before = DrawAdviceSequence;
        var receipt = new DrawAdviceReceipt(frame.InstructionIndex, new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, frame.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, recipient, 0, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawAdviceStartedEvent(frame.Id, receipt.InstructionIndex, receipt.Source, receipt.GameplayHash, receipt.ParentLifecycleFrameId, receipt.ActualTurnNumber, recipient, before));
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[recipient], 2, true, new(DrawAdviceReason(frame.SkillId, "draw"))).Count;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        receipt = frame.DrawAdviceReceipt!with
        {
            DrawActual = actual,
            DrawAfter = DrawAdviceSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DrawAdviceReceipt = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawAdviceDrawIssuedEvent(frame.Id, recipient, actual, before, receipt.DrawAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainDrawAdviceChildren(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 280)]
    internal bool ResumeDrawAdvice(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.DrawAdviceReceipt is not { } receipt)
            return false;
        if (_corePublicPilePreparationOperations.PreparationGameEnded())
            return true;
        if (DrainDrawAdviceChildren(frame))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.DrawAdviceReceipt!;
        if (receipt.Stage == DrawAdviceStage.Drawing)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var recipient = _coreMatchParticipantsPlayersAccess._players[receipt.RecipientSeat];
            receipt = receipt with
            {
                Stage = DrawAdviceStage.ChoosingDiscard,
                QualificationsFrozen = true,
                OwnerHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count,
                OwnerMaxHp = owner.MaxHp,
                RecipientHandCount = _corePhysicalOwnedCardQueriesOperations.GetHand(recipient).Count,
                RecipientMaxHp = recipient.MaxHp,
                OwnerMustDiscard = owner.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > owner.MaxHp,
                RecipientMustDiscard = recipient.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(recipient).Count > recipient.MaxHp
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = receipt, PendingMovementContinuation = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawAdviceQualifiedEvent(id, frame.OwnerSeat, receipt.RecipientSeat, receipt.OwnerHandCount, receipt.OwnerMaxHp, receipt.RecipientHandCount, receipt.RecipientMaxHp, receipt.OwnerMustDiscard, receipt.RecipientMustDiscard));
        }
        else if (receipt.Stage == DrawAdviceStage.Discarding)
        {
            receipt = receipt with
            {
                Stage = DrawAdviceStage.ChoosingDiscard,
                ParticipantCursor = receipt.ParticipantCursor + 1,
                RequiredDiscardCount = 0,
                SelectedDiscardCardIds = [],
                SelectedSourceLocations = []
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = receipt, PendingMovementContinuation = null });
        }

        while (receipt.ParticipantCursor < 2)
        {
            var seat = receipt.ParticipantCursor == 0 ? frame.OwnerSeat : receipt.RecipientSeat;
            var qualified = receipt.ParticipantCursor == 0 ? receipt.OwnerMustDiscard : receipt.RecipientMustDiscard;
            var available = qualified && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive ? DrawAdviceDiscardableCards(seat).ToArray() : [];
            if (available.Length == 0)
            {
                receipt = receipt with
                {
                    ParticipantCursor = receipt.ParticipantCursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = receipt });
                continue;
            }

            receipt = receipt with
            {
                RequiredDiscardCount = Math.Min(2, available.Length)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = receipt });
            PublishDrawAdviceDiscard(frame);
            return true;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawAdviceResolvedEvent(id, receipt.OwnerDiscardActual, receipt.RecipientDiscardActual));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
        return true;
    }

    internal IEnumerable<Card> DrawAdviceDiscardableCards(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(card => !card.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    internal int DrawAdviceDiscardSeat(ProgramSkillFrame frame) => frame.DrawAdviceReceipt!.ParticipantCursor == 0 ? frame.OwnerSeat : frame.DrawAdviceReceipt.RecipientSeat;
    internal IReadOnlyList<PromptChoice> DrawAdviceChoices(ProgramSkillFrame frame) => Array.AsReadOnly(DrawAdviceDiscardableCards(DrawAdviceDiscardSeat(frame)).Where(c => !frame.DrawAdviceReceipt!.SelectedDiscardCardIds.Contains(c.Id)).Select(card => new PromptChoice(new ChoiceId($"draw-advice.{(frame.Id)}.{(frame.DrawAdviceReceipt!.ParticipantCursor)}.{(frame.DrawAdviceReceipt.SelectedDiscardCardIds.Count)}.{(card.Id)}"), $"弃置【{(card.DisplayName)}】（{(card.Suit)} {(card.RankText)}）", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "draw-advice-discard", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishDrawAdviceDiscard(ProgramSkillFrame frame) => _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(frame, DrawAdviceDiscardSeat(frame), DrawAdviceChoices(frame), $"手牌数超过体力上限，请弃置{(frame.DrawAdviceReceipt!.RequiredDiscardCount)}张手牌或装备牌（选齐后同时弃置）。", DrawAdviceDiscardSeat(frame));
    [ProgramAnswer("draw-advice-discard")]
    internal void ResolveDrawAdviceChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Draw advice lost its owning discard frame.");
        var receipt = frame.DrawAdviceReceipt!;
        var seat = DrawAdviceDiscardSeat(frame);
        if (receipt.Stage != DrawAdviceStage.ChoosingDiscard || receipt.ParticipantCursor >= 2 || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || _corePendingDecisionPolicyPipelineOperations._pendingDecision.IsPrivate != true || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Cards.Count != 1 || selected.Targets.Count != 0 || !DrawAdviceChoices(frame).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards)) || receipt.SelectedDiscardCardIds.Count >= receipt.RequiredDiscardCount || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            throw new InvalidOperationException("Draw advice discard no longer belongs to its actual private HE owner.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var ids = receipt.SelectedDiscardCardIds.Append(selected.Cards.Single()).ToArray();
        var from = receipt.SelectedSourceLocations.Append(_cardZonesOperations._cardZones.GetLocation(selected.Cards.Single())).ToArray();
        receipt = receipt with
        {
            SelectedDiscardCardIds = ids,
            SelectedSourceLocations = from
        };
        if (ids.Length < receipt.RequiredDiscardCount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DrawAdviceReceipt = receipt });
            PublishDrawAdviceDiscard(frame);
            return;
        }

        var before = DrawAdviceSequence;
        receipt = receipt with
        {
            Stage = DrawAdviceStage.Discarding,
            LastDiscardBefore = before,
            LastDiscardAfter = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DrawAdviceReceipt = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, seat, ids, new(DrawAdviceReason(frame.SkillId, "discard")), (batchId, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = active.DrawAdviceReceipt!;
            paid = paid with
            {
                LastDiscardBatchId = batchId,
                LastDiscardAfter = DrawAdviceSequence,
                OwnerDiscardActual = paid.ParticipantCursor == 0 ? records.Count : paid.OwnerDiscardActual,
                RecipientDiscardActual = paid.ParticipantCursor == 1 ? records.Count : paid.RecipientDiscardActual
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { DrawAdviceReceipt = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawAdviceDiscardIssuedEvent(frame.Id, seat, records.Count, batchId, before, paid.LastDiscardAfter, ids));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool ValidDrawAdviceReceipt(ProgramSkillFrame frame)
    {
        if (frame.DrawAdviceReceipt is not { } receipt || !ExactDrawAdviceParent(frame) || receipt.InstructionIndex != frame.InstructionIndex || receipt.Source != new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || receipt.ParentLifecycleFrameId != frame.WindowContext!.ParentFrameId || receipt.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || receipt.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || receipt.RecipientSeat != frame.SelectedTargetSeats.Single() || receipt.DrawActual is < 0 or > 2 || receipt.DrawBefore < 0 || receipt.DrawAfter < receipt.DrawBefore || receipt.DrawAfter > DrawAdviceSequence || receipt.ParticipantCursor is < 0 or > 2 || !Enum.IsDefined(receipt.Stage) || receipt.RequiredDiscardCount is < 0 or > 2 || receipt.OwnerDiscardActual is < 0 or > 2 || receipt.RecipientDiscardActual is < 0 or > 2 || receipt.SelectedDiscardCardIds.Count != receipt.SelectedSourceLocations.Count || receipt.SelectedDiscardCardIds.Distinct().Count() != receipt.SelectedDiscardCardIds.Count || receipt.SelectedDiscardCardIds.Count > receipt.RequiredDiscardCount)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<DrawAdviceStartedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var started] || started != new DrawAdviceStartedEvent(frame.Id, receipt.InstructionIndex, receipt.Source, receipt.GameplayHash, receipt.ParentLifecycleFrameId, receipt.ActualTurnNumber, receipt.RecipientSeat, receipt.DrawBefore) || history.OfType<DrawAdviceDrawIssuedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var draw] || draw != new DrawAdviceDrawIssuedEvent(frame.Id, receipt.RecipientSeat, receipt.DrawActual, receipt.DrawBefore, receipt.DrawAfter) || history.OfType<DrawAdviceResolvedEvent>().Any(e => e.FrameId == frame.Id))
            return false;
        var drawMoves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.DrawBefore && m.Sequence <= receipt.DrawAfter && m.Reason.Value == DrawAdviceReason(frame.SkillId, "draw")).ToArray();
        if (drawMoves.Length != receipt.DrawActual || drawMoves.Select(m => m.CardId).Distinct().Count() != drawMoves.Length || drawMoves.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(receipt.RecipientSeat) || m.TurnNumber != receipt.ActualTurnNumber) || receipt.DrawActual == 0 && receipt.DrawBefore != receipt.DrawAfter)
            return false;
        var qualified = history.OfType<DrawAdviceQualifiedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (receipt.Stage == DrawAdviceStage.Drawing)
        {
            if (receipt.QualificationsFrozen || qualified.Length != 0 || receipt.ParticipantCursor != 0 || receipt.RequiredDiscardCount != 0 || receipt.SelectedDiscardCardIds.Count != 0 || receipt.OwnerDiscardActual != 0 || receipt.RecipientDiscardActual != 0)
                return false;
        }
        else if (!receipt.QualificationsFrozen || qualified is not [var q] || q != new DrawAdviceQualifiedEvent(frame.Id, frame.OwnerSeat, receipt.RecipientSeat, receipt.OwnerHandCount, receipt.OwnerMaxHp, receipt.RecipientHandCount, receipt.RecipientMaxHp, receipt.OwnerMustDiscard, receipt.RecipientMustDiscard) || receipt.OwnerHandCount < 0 || receipt.RecipientHandCount < 0 || receipt.OwnerMaxHp < 0 || receipt.RecipientMaxHp < 0 || receipt.OwnerMustDiscard && receipt.OwnerHandCount <= receipt.OwnerMaxHp || receipt.RecipientMustDiscard && receipt.RecipientHandCount <= receipt.RecipientMaxHp)
            return false;
        var invoices = history.OfType<DrawAdviceDiscardIssuedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (invoices.Length > 2 || invoices.Select(e => e.DiscardSeat).Distinct().Count() != invoices.Length || invoices.Sum(e => e.DiscardSeat == frame.OwnerSeat ? e.ActualCount : 0) != receipt.OwnerDiscardActual || invoices.Sum(e => e.DiscardSeat == receipt.RecipientSeat ? e.ActualCount : 0) != receipt.RecipientDiscardActual)
            return false;
        foreach (var invoice in invoices)
        {
            if (invoice.DiscardSeat != frame.OwnerSeat && invoice.DiscardSeat != receipt.RecipientSeat || invoice.DiscardSeat == frame.OwnerSeat && !receipt.OwnerMustDiscard || invoice.DiscardSeat == receipt.RecipientSeat && !receipt.RecipientMustDiscard || invoice.ActualCount is < 1 or > 2 || invoice.CardIds.Count != invoice.ActualCount || invoice.CardIds.Distinct().Count() != invoice.ActualCount || invoice.BatchId <= frame.Id || invoice.SequenceBefore < receipt.DrawAfter || invoice.SequenceAfter <= invoice.SequenceBefore || invoice.SequenceAfter > DrawAdviceSequence)
                return false;
            var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > invoice.SequenceBefore && m.Sequence <= invoice.SequenceAfter && m.Reason.Value == DrawAdviceReason(frame.SkillId, "discard")).ToArray();
            if (!moves.Select(m => m.CardId).SequenceEqual(invoice.CardIds) || moves.Any(m => m.From.OwnerSeat != invoice.DiscardSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || m.To != CardLocation.DiscardPile || m.TurnNumber != receipt.ActualTurnNumber))
                return false;
        }

        if (receipt.Stage is DrawAdviceStage.Drawing or DrawAdviceStage.Discarding)
        {
            if (frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != frame.OwnerSeat)
                return false;
        }
        else if (frame.PendingMovementContinuation is not null || receipt.SelectedDiscardCardIds.Select((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != receipt.SelectedSourceLocations[i] || receipt.SelectedSourceLocations[i].OwnerSeat != DrawAdviceDiscardSeat(frame) || receipt.SelectedSourceLocations[i].Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(DrawAdviceDiscardSeat(frame), _coreAdvancedLifecycleOperations.GetAdvancedCard(id), receipt.SelectedSourceLocations[i], OwnedCardMoveIntent.Discard)).Any(b => b))
            return false;
        if (receipt.Stage == DrawAdviceStage.Discarding && (receipt.SelectedDiscardCardIds.Count != receipt.RequiredDiscardCount || invoices.LastOrDefault()is not { } last || last.DiscardSeat != DrawAdviceDiscardSeat(frame) || last.BatchId != receipt.LastDiscardBatchId || last.SequenceBefore != receipt.LastDiscardBefore || last.SequenceAfter != receipt.LastDiscardAfter || !last.CardIds.SequenceEqual(receipt.SelectedDiscardCardIds) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > last.SequenceBefore && m.Sequence <= last.SequenceAfter && m.Reason.Value == DrawAdviceReason(frame.SkillId, "discard")).Select(m => m.From).SequenceEqual(receipt.SelectedSourceLocations)))
            return false;
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 230)]
    internal bool ReturnDrawAdviceMovement(ProgramSkillFrame frame)
    {
        if (frame.DrawAdviceReceipt is null || frame.PendingMovementContinuation is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 760)]
    internal bool CanContinueIssuedDrawAdvice(ProgramSkillFrame frame) => frame.DrawAdviceReceipt is not null && ValidDrawAdviceReceipt(frame);
    internal bool DrawAdviceFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.DrawAdviceReceipt is not { } receipt || !ValidDrawAdviceReceipt(frame))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == frame.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == frame.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        var before = receipt.Stage == DrawAdviceStage.Drawing ? receipt.DrawBefore : receipt.LastDiscardBefore;
        var after = receipt.Stage == DrawAdviceStage.Drawing ? receipt.DrawAfter : receipt.LastDiscardAfter;
        var reason = DrawAdviceReason(frame.SkillId, receipt.Stage == DrawAdviceStage.Drawing ? "draw" : "discard");
        var seat = DrawAdviceDiscardSeat(frame);
        bool EquipmentPayment(CardKind kind) => receipt.Stage == DrawAdviceStage.Discarding && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && receipt.SelectedDiscardCardIds.Contains(m.CardId) && m.CardKind == kind && m.From == CardLocation.Equipment(seat) && m.To == CardLocation.DiscardPile && m.Reason.Value == reason);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == frame.Id && moved.Batch.AwaitingProgramFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > before && m.Sequence <= after && m.Reason.Value == reason && (receipt.Stage != DrawAdviceStage.Discarding || moved.Batch.Id == receipt.LastDiscardBatchId) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(seat) && m.To == CardLocation.DiscardPile && EquipmentPayment(CardKind.WoodenOx)));
        if (!EquipmentPayment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == frame.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason;
    }

    internal ProgramSkillFrame? DrawAdviceObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.DrawAdviceFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasPaidDamageObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 280)]
    internal bool IsDrawAdviceProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && DrawAdviceObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    SkillProgramStepOutcome IDrawAdviceProgramHost.BeginDrawAdvice(ProgramSkillFrame frame, int recipientSeat) => BeginDrawAdvice(frame, recipientSeat);
}
