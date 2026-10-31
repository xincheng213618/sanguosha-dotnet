#nullable enable

using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseHandSeizureRuntime
{
    internal bool PhaseHandSeizureTerminalState => _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameEndedEvent>().Any(e => e.Winner == _coreMatchOutcomeStatusWinnerAccess._winner);
    internal long PhaseHandSeizureSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal CardConversionSource PhaseHandSeizureSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal static string PhaseHandSeizureReason(ProgramSkillFrame frame, string part) => RuntimeSkills.RuntimeRules_Core_PhaseHandSeizure.PhaseHandSeizureReason(frame, part);
    internal static string PhaseHandDebtReturnReason(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_Core_PhaseHandSeizure.PhaseHandDebtReturnReason(frame);
    internal bool IsPhaseHandSeizureCost(int owner, string skill, Card card) => _cardZonesOperations._cardZones.GetLocation(card.Id)is { OwnerSeat: var seat, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } location && seat == owner && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, location, OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, location, OwnedCardMoveIntent.Discard) && !(location.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skill, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[owner], skill), card));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 80)]
    internal bool CanActivatePhaseHandSeizure(CharacterState owner, SkillProgram program) => !program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardTurnOverAndTakeHand)) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandSeizurePaidEvent>().Any(e => e.Source.OwnerSeat == owner.Seat && e.Source.SkillId == program.Id && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal IEnumerable<PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures()
    {
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PhaseHandSeizureIssuedEvent>();
        if (issued.Count == 0)
            return Array.Empty<PhaseHandSeizureIssuedEvent>();
        var settlements = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PhaseHandDebtSettledEvent>();
        if (settlements.Count == 0)
            return issued;
        var settled = settlements.Select(e => e.SeizureFrameId).ToHashSet();
        return issued.Where(e => !settled.Contains(e.FrameId));
    }

    internal PhaseHandSeizureIssuedEvent? CurrentPhaseHandSeizureDebt(int owner, string skill, string instance, string hash, string binding) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat ? UnsettledPhaseHandSeizures().SingleOrDefault(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance && e.GameplayHash == hash && e.ContinuationId == binding && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal bool IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(candidate.SkillId)?.Program is { } program && program.GameplayHash == candidate.GameplayHash && ProgramInstructionResolver.Default.FindTrigger(program, candidate.BindingId)?.Effects is [{ Op: SkillProgramEffectOp.ReturnIssuedPhaseHandDebt }];
    internal IEnumerable<ProgramTriggerCandidate> IssuedPhaseHandDebtCandidates(CharacterState owner, SkillProgramTriggerWindow window, int occurrence)
    {
        if (window != SkillProgramTriggerWindow.PlayEnding || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
            yield break;
        foreach (var due in UnsettledPhaseHandSeizures().Where(e => e.Source.OwnerSeat == owner.Seat && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).OrderBy(e => e.FrameId))
        {
            var program = _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(due.Source.SkillId)?.Program;
            if (program is null || program.GameplayHash != due.GameplayHash || ProgramInstructionResolver.Default.FindTrigger(program, due.ContinuationId)is not { } trigger || trigger.Window != window || trigger.Optional || trigger.Subject != SkillProgramTriggerSubject.Owner || trigger.TurnOwnerScope != SkillProgramTurnOwnerScope.Own || trigger.Effects is not [{ Op: SkillProgramEffectOp.ReturnIssuedPhaseHandDebt }])
                throw new InvalidOperationException("An issued phase-hand obligation lost its exact immutable continuation.");
            yield return new(owner.Seat, due.Source.SkillId, due.ContinuationId, due.Source.SkillInstanceId!, due.GameplayHash, trigger.Priority, occurrence);
        }
    }

    internal bool CanRunIssuedPhaseHandDebt(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsPhaseHandDebtResolver(candidate) && context.Window == SkillProgramTriggerWindow.PlayEnding && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && CurrentPhaseHandSeizureDebt(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.GameplayHash, candidate.BindingId)is not null;
    internal PhaseHandSeizureIssuedEvent RequirePhaseHandDebtParent(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayEnding } context || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play && !PhaseHandSeizureTerminalState || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.FirstOrDefault(f => f.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame parent || parent.OwnerSeat != frame.OwnerSeat || parent.Window != context.Window || parent.Continuation is not (ProgramLifecycleContinuation.CompletePlayPhase or ProgramLifecycleContinuation.CompletePhaseHandDebtForcedEnd) || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("A phase-hand return lost its actual owner PlayEnding parent and candidate.");
        var due = frame.PhaseHandDebtReturn is { } paid ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandSeizureIssuedEvent>().SingleOrDefault(e => e.FrameId == paid.SeizureFrameId) : CurrentPhaseHandSeizureDebt(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, frame.TriggerId!);
        if (due is null || due.Source.OwnerSeat != frame.OwnerSeat || due.Source.SkillId != frame.SkillId || due.Source.SkillInstanceId != frame.SkillInstanceId || due.GameplayHash != frame.GameplayHash || due.ContinuationId != frame.TriggerId || due.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || due.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            throw new InvalidOperationException("A phase-hand return replaced its original issued identity.");
        return due;
    }

    internal SkillProgramStepOutcome BeginPhaseHandSeizure(ProgramSkillFrame supplied, string continuation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.PhaseHandSeizure is not null || frame.PhaseHandDebtReturn is not null || frame.InstructionIndex != 1 || frame.TriggerId is not null || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || frame.SelectedCardIds is not [var cardId] || frame.SelectedTargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchParticipantsPlayersAccess._players[target].Gender != GeneralGender.Male || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || !CanActivatePhaseHandSeizure(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!) || !IsPhaseHandSeizureCost(frame.OwnerSeat, frame.SkillId, _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId)))
            throw new InvalidOperationException("Phase hand seizure requires one payable original HE card and living male target in its actual Play phase.");
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!;
        if (ProgramInstructionResolver.Default.FindTrigger(program, continuation)is not { Window: SkillProgramTriggerWindow.PlayEnding, Optional: false, Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Effects: [{ Op: SkillProgramEffectOp.ReturnIssuedPhaseHandDebt }] })
            throw new InvalidOperationException("Phase hand seizure requires its exact mandatory return continuation.");
        var before = PhaseHandSeizureSequence;
        var receipt = new ProgramPhaseHandSeizureReceipt(1, target, continuation, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, cardId, _cardZonesOperations._cardZones.GetLocation(cardId), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsFaceDown, PhaseHandSeizureStage.PaymentChildren, before, before, []);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandSeizure = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, [cardId], new(PhaseHandSeizureReason(frame, "payment")), (_, records) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var after = records.Max(m => m.Sequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { PhaseHandSeizure = receipt with { CostAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandSeizurePaidEvent(frame.Id, PhaseHandSeizureSource(frame), frame.GameplayHash, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, cardId, receipt.CostFrom, before, after));
        });
        DrainPhaseHandSeizureMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool TryDrainPhaseHandSeizureMovement(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id);
    internal void DrainPhaseHandSeizureMovement(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (PhaseHandSeizureTerminalState)
        {
            _corePhaseHandSeizureReceiptsOperations.AssertPhaseHandSeizure(frame);
            return;
        }

        if (!TryDrainPhaseHandSeizureMovement(frame))
            ReturnPhaseHandSeizureMovement(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 260)]
    internal bool ReturnPhaseHandSeizureMovement(ProgramSkillFrame frame)
    {
        if (frame.PhaseHandSeizure is null && frame.PhaseHandDebtReturn is null)
            return false;
        _corePhaseHandSeizureReceiptsOperations.AssertPhaseHandSeizure(frame);
        if (PhaseHandSeizureTerminalState)
            return true;
        if (frame.PendingMovementContinuation is not { } pending || !_corePhaseHandSeizureReceiptsOperations.IsPhaseHandSeizureMovement(frame, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame), pending))
            throw new InvalidOperationException("The phase-hand movement returned to a different owning instruction.");
        if (TryDrainPhaseHandSeizureMovement(frame))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        ResumePhaseHandSeizure(frame.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 240)]
    internal bool ResumePhaseHandSeizure(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.PhaseHandSeizure is null && frame.PhaseHandDebtReturn is null)
            return false;
        _corePhaseHandSeizureReceiptsOperations.AssertPhaseHandSeizure(frame);
        if (PhaseHandSeizureTerminalState)
            return true;
        if (frame.PendingMovementContinuation is not null)
        {
            DrainPhaseHandSeizureMovement(frame.Id);
            return true;
        }

        if (frame.PhaseHandDebtReturn is { } returned)
        {
            if (returned.Stage == PhaseHandDebtReturnStage.Choosing)
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandDebtReturn = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return true;
        }

        var receipt = frame.PhaseHandSeizure!;
        if (receipt.Stage == PhaseHandSeizureStage.TakeChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandSeizure = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return true;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandSeizure = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, false);
            return true;
        }

        if (receipt.Stage == PhaseHandSeizureStage.PaymentChildren)
        {
            receipt = receipt with
            {
                Stage = PhaseHandSeizureStage.FlipChildren,
                WasFaceDown = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsFaceDown
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandSeizure = receipt });
            _coreProgramLifecycleOperations.TurnOverProgramTarget(frame.Id, frame.OwnerSeat, frame.OwnerSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandSeizureTurnedEvent(frame.Id, frame.OwnerSeat, receipt.WasFaceDown, !receipt.WasFaceDown));
        }

        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        receipt = frame.PhaseHandSeizure!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandSeizure = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, false);
            return true;
        }

        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat]).Select(c => c.Id).ToArray();
        var before = PhaseHandSeizureSequence;
        var same = receipt.TargetSeat == frame.OwnerSeat;
        receipt = receipt with
        {
            Stage = PhaseHandSeizureStage.TakeChildren,
            TakenCardIds = cards,
            TakeBefore = before,
            TakeAfter = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandSeizure = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        void Issued(long after, int count)
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { PhaseHandSeizure = receipt with { TakeAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandSeizureIssuedEvent(frame.Id, PhaseHandSeizureSource(frame), frame.GameplayHash, receipt.ContinuationId, receipt.TargetSeat, receipt.ActualTurnNumber, receipt.PhaseInstanceId, count, same, before, after));
        }

        if (same || cards.Length == 0)
            Issued(before, 0);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(frame.OwnerSeat), new(PhaseHandSeizureReason(frame, "take")), (_, records) => Issued(records.Max(m => m.Sequence), records.Count));
        DrainPhaseHandSeizureMovement(frame.Id);
        return true;
    }

    internal Card[] PhaseHandDebtCards(ProgramSkillFrame frame) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(frame.OwnerSeat)).Where(card => !(card.IsGeneralWeapon && _cardZonesOperations._cardZones.GetLocation(card.Id).Zone == CardZoneKind.Equipment) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Transfer)).ToArray();
    internal SkillProgramStepOutcome BeginPhaseHandDebtReturn(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var due = RequirePhaseHandDebtParent(frame);
        if (frame.InstructionIndex != 1 || frame.PhaseHandDebtReturn is not null || frame.PhaseHandSeizure is not null)
            throw new InvalidOperationException("The phase-hand debt cannot freeze or pay twice.");
        if (!_coreMatchParticipantsPlayersAccess._players[due.TargetSeat].IsAlive)
        {
            SettlePhaseHandDebt(due, frame.Id, "target-death", 0, 0, 0, 0);
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var hp = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[due.TargetSeat].Hp);
        var count = Math.Min(hp, PhaseHandDebtCards(frame).Length);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandDebtReturn = new(1, due.FrameId, due.TargetSeat, hp, count, PhaseHandDebtReturnStage.Choosing, [], []) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandDebtReturnStartedEvent(frame.Id, due.FrameId, frame.OwnerSeat, due.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, hp, count));
        if (count == 0)
        {
            SettlePhaseHandDebt(due, frame.Id, "returned", 0, 0, PhaseHandSeizureSequence, PhaseHandSeizureSequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandDebtReturn = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishPhaseHandDebtChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PhaseHandDebtChoices(ProgramSkillFrame frame) => Array.AsReadOnly(PhaseHandDebtCards(frame).Where(c => !frame.PhaseHandDebtReturn!.CardIds.Contains(c.Id)).Select(c => new PromptChoice(new($"phase-hand-debt.{(frame.Id)}.{(c.Id)}"), $"交给目标【{(c.DisplayName)}】（{(frame.PhaseHandDebtReturn!.CardIds.Count + 1)}/{(frame.PhaseHandDebtReturn.RequiredCount)}）", [c.Id], [], new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { ["program-action"] = "phase-hand-debt-return", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) }))).ToArray());
    internal void PublishPhaseHandDebtChoice(ProgramSkillFrame frame)
    {
        var choices = PhaseHandDebtChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "交给原目标与其体力值等量张牌；不足时交给全部可交的牌。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).ToArray()), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.PhaseHandDebtReturn!.TargetSeat,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("phase-hand-debt-return")]
    internal void ResolvePhaseHandDebtChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing phase-hand return owner.");
        _corePhaseHandSeizureReceiptsOperations.AssertPhaseHandSeizure(frame);
        var receipt = frame.PhaseHandDebtReturn ?? throw new InvalidOperationException("Missing phase-hand return receipt.");
        var due = RequirePhaseHandDebtParent(frame);
        if (receipt.Stage != PhaseHandDebtReturnStage.Choosing || choice.Cards is not [var id] || choice.Targets.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], PhaseHandDebtChoices(frame).Where(c => c.Id == choice.Id).ToArray()) || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive)
            throw new InvalidOperationException("The exact phase-hand return chooser or material changed.");
        var ids = receipt.CardIds.Append(id).ToArray();
        var locations = receipt.Locations.Append(_cardZonesOperations._cardZones.GetLocation(id)).ToArray();
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (ids.Length < receipt.RequiredCount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandDebtReturn = receipt with { CardIds = ids, Locations = locations } });
            PublishPhaseHandDebtChoice(frame);
            return;
        }

        if (ids.Length != receipt.RequiredCount || ids.Where((card, index) => _cardZonesOperations._cardZones.GetLocation(card) != locations[index] || !PhaseHandDebtCards(frame).Any(c => c.Id == card)).Any())
            throw new InvalidOperationException("The complete phase-hand return is no longer owned.");
        var before = PhaseHandSeizureSequence;
        var moving = ids.Where((card, index) => locations[index] != CardLocation.Hand(receipt.TargetSeat)).ToArray();
        receipt = receipt with
        {
            Stage = PhaseHandDebtReturnStage.MovementChildren,
            CardIds = ids,
            Locations = locations,
            SequenceBefore = before,
            SequenceAfter = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandDebtReturn = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (moving.Length == 0)
            SettlePhaseHandDebt(due, frame.Id, "same-hand", receipt.RequiredCount, 0, before, before);
        else
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(moving, CardLocation.Hand(receipt.TargetSeat), new(PhaseHandDebtReturnReason(frame)), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, receipt.TargetSeat, moving, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
                var after = records.Max(m => m.Sequence);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { PhaseHandDebtReturn = receipt with { SequenceAfter = after } });
                SettlePhaseHandDebt(due, frame.Id, "returned", receipt.RequiredCount, records.Count, before, after);
            }));
        DrainPhaseHandSeizureMovement(frame.Id);
    }

    internal void SettlePhaseHandDebt(PhaseHandSeizureIssuedEvent due, long returnFrame, string reason, int required, int actual, long before, long after)
    {
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandDebtSettledEvent>().Any(e => e.SeizureFrameId == due.FrameId))
            throw new InvalidOperationException("One issued phase-hand debt cannot settle twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandDebtSettledEvent(due.FrameId, returnFrame, due.Source.OwnerSeat, due.TargetSeat, reason, required, actual, before, after));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 360)]
    internal void ObservePhaseHandSeizureDeath(IGameEvent payload)
    {
        if (payload is GameEndedEvent ended && _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && ended.Winner == _coreMatchOutcomeStatusWinnerAccess._winner && _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            foreach (var due in UnsettledPhaseHandSeizures().ToArray())
                SettlePhaseHandDebt(due, 0, "game-end", 0, 0, 0, 0);
            return;
        }

        if (payload is not PlayerDiedEvent died)
            return;
        foreach (var due in UnsettledPhaseHandSeizures().Where(e => e.Source.OwnerSeat == died.VictimSeat || e.TargetSeat == died.VictimSeat).ToArray())
            SettlePhaseHandDebt(due, 0, due.Source.OwnerSeat == died.VictimSeat ? "owner-death" : "target-death", 0, 0, 0, 0);
    }

    internal bool HasCurrentIssuedPhaseHandDebt() => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && UnsettledPhaseHandSeizures().Any(e => e.Source.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
    internal bool TryBeginForcedPhaseHandDebtReturn()
    {
        if (!HasCurrentIssuedPhaseHandDebt() || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.Count != 0)
            throw new InvalidOperationException("A forced phase-hand end requires a quiet owning boundary.");
        var owner = _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat];
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        var candidates = Array.AsReadOnly(IssuedPhaseHandDebtCandidates(owner, SkillProgramTriggerWindow.PlayEnding, 0).ToArray());
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, SkillProgramTriggerWindow.PlayEnding, candidates, ProgramLifecycleContinuation.CompletePhaseHandDebtForcedEnd, facts) { ParticipantFacts = new ReadOnlyDictionary<int, SkillProgramTriggerFacts>(new Dictionary<int, SkillProgramTriggerFacts> { [owner.Seat] = facts }) });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ReturnIssuedPhaseHandDebt)]
    internal PromptChoice SelectAiPhaseHandDebt(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    SkillProgramStepOutcome IPhaseHandSeizureProgramHost.DiscardTurnOverAndTakeHand(ProgramSkillFrame frame, string continuation) => BeginPhaseHandSeizure(frame, continuation);
    SkillProgramStepOutcome IPhaseHandSeizureProgramHost.ReturnIssuedPhaseHandDebt(ProgramSkillFrame frame) => BeginPhaseHandDebtReturn(frame);
    [ProgramIssuedTrigger(690, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger690(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsPhaseHandDebtResolver(candidate) ? CanRunIssuedPhaseHandDebt(candidate, context) : null;
}
