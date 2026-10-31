#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal const string WeiwanCostReason = "skill-program.le-xiao-qiao.weiwan.discard";
    internal const string WeiwanObtainReason = "skill-program.le-xiao-qiao.weiwan.obtain";
    internal bool HasLeXiaoQiaoWeiwanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWeiwanQinDiscardAndSuitObtain);

    internal static CardConversionSource WeiwanSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_xiao_qiao_le_LeXiaoQiaoWeiwan.WeiwanSource(f);
    internal ActualDiscardRecoveryPhaseKey? WeiwanPlay(int actor)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        return _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == actor && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } && phase.ActorSeat == actor && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) ? phase : null;
    }

    internal bool CanPayWeiwanQin(int owner, Card card)
    {
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        return from.OwnerSeat == owner && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && HasQiqinEntityMark(card.Id) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, from, OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, from, OwnedCardMoveIntent.Discard);
    }

    SkillProgramStepOutcome ILeXiaoQiaoWeiwanHost.Resolve(ProgramSkillFrame f) => BeginWeiwan(f);
    bool ILeXiaoQiaoWeiwanSelectionQuery.CanDiscard(int owner, Card card) => CanPayWeiwanQin(owner, card);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28620, SkillProgramEffectOp.ResolveWeiwanQinDiscardAndSuitObtain)]
    internal bool CanActivateWeiwan(ProgramActivationLegalityContext gate) => WeiwanPlay(gate.OwnerSeat)is { } phase && !_coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanStartedEvent>().Any(e => e.Source.OwnerSeat == gate.OwnerSeat && e.Phase == phase) && _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(gate.Owner)).Any(c => CanPayWeiwanQin(gate.OwnerSeat, c));
    internal SkillProgramStepOutcome BeginWeiwan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Weiwan is not null)
        {
            ContinueWeiwan(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != "le:weiwan" || f.ActivationId != "discard-qin-obtain-suits" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || WeiwanPlay(f.OwnerSeat)is not { } phase || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanStartedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Phase == phase))
            throw new InvalidOperationException("Weiwan lost its one real Qin discard, other target or actual Play allowance.");
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
        if (!CanPayWeiwanQin(f.OwnerSeat, card))
            throw new InvalidOperationException("Weiwan cannot borrow another zone or a non-Qin discard cost.");
        var r = new WeiwanReceipt(WeiwanSource(f), f.GameplayHash, phase, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, id, card.Kind, from, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), target, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Stage = WeiwanStage.CostChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weiwan = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner, id, card.Kind, from, r.CostSuit, target, r.SequenceBefore));
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [id], new(WeiwanCostReason), (batch, moves) =>
        {
            var paid = moves.Single();
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { Weiwan = current.Weiwan!with { CostBatchId = batch, CostSequence = paid.Sequence, CostTo = paid.To, After = paid.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanCostPaidEvent(f.Id, batch, id, card.Kind, paid.From, paid.To, paid.Sequence));
        });
        ContinueWeiwan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainWeiwan(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.Weiwan!.Branch == WeiwanBranch.LoseHp ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueWeiwan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weiwan: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == WeiwanStage.HealthIssuing)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                CompleteWeiwan(f);
                return;
            }

            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, r.TargetSeat, 1);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.Weiwan!;
            if (r.Stage == WeiwanStage.HealthIssuing)
                throw new InvalidOperationException("Weiwan's one native HP deduction lost its typed result.");
        }

        if (DrainWeiwan(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Weiwan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == WeiwanStage.CostChildren)
        {
            var samples = new List<WeiwanSuitSample>();
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            {
                // Snapshot the entire group before any one selected entity leaves
                // its zone or removal hooks can change another candidate's suit.
                var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
                var candidates = new[]
                {
                    CardLocation.Hand(target.Seat),
                    CardLocation.Equipment(target.Seat),
                    CardLocation.Judgment(target.Seat)
                }.SelectMany(location => _cardZonesOperations._cardZones.CardsAt(location).Select(card => (Card: card, From: location, Suit: _coreNativeCardAppearanceOperations.EffectiveSuit(target, card)))).OrderBy(entry => entry.From.Zone).ThenBy(entry => entry.Card.Id).ToArray();
                foreach (var suit in CharacterXiaoQiaoLeDefinitions.WeiwanSuits.Where(suit => suit != r.CostSuit))
                {
                    var eligible = candidates.Where(entry => entry.Suit == suit).ToArray();
                    var random = _randomOperations._random.State;
                    var selected = eligible.Length == 0 ? ((Card Card, CardLocation From, Suit Suit)? )null : eligible[_randomOperations._random.Next(eligible.Length)];
                    samples.Add(new(samples.Count, suit, selected?.Card.Id, selected?.Card.Kind, selected?.From, eligible.Length, random, _randomOperations._random.State));
                }
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weiwan = r with { Stage = WeiwanStage.ObtainChildren, Samples = samples, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var sample in samples)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanSuitSampledEvent(f.Id, sample.Index, sample.Suit, sample.CardId, sample.PrintedKind, sample.From, sample.CandidateCount, sample.RandomBefore, sample.RandomAfter, before));
            var ids = samples.Where(s => s.CardId is not null).Select(s => s.CardId!.Value).ToArray();
            if (ids.Length == 0)
                FinishWeiwanObtain(f.Id, 0, []);
            else
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(WeiwanObtainReason), (batch, records) => FinishWeiwanObtain(f.Id, batch, records));
            ContinueWeiwan(id);
            return;
        }

        if (r.Stage == WeiwanStage.ObtainChildren)
        {
            var branch = r.ActualCount switch
            {
                1 => WeiwanBranch.LoseHp,
                2 => WeiwanBranch.UnlimitedUse,
                3 => WeiwanBranch.ForbidUse,
                _ => WeiwanBranch.None
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weiwan = r with { Branch = branch, Stage = branch == WeiwanBranch.LoseHp ? WeiwanStage.HealthIssuing : WeiwanStage.HealthChildren }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
            if (branch is WeiwanBranch.UnlimitedUse or WeiwanBranch.ForbidUse)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanTurnLeaseIssuedEvent(new(f.Id, r.Source, r.GameplayHash, f.OwnerSeat, r.TargetSeat, r.ActualTurn, r.TurnOwner, r.ActualCount, r.After)));
            ContinueWeiwan(id);
            return;
        }

        if (r.Stage == WeiwanStage.HealthChildren)
        {
            CompleteWeiwan(f);
            return;
        }

        throw new InvalidOperationException("Weiwan resumed outside its original paid cost/obtain/health cursor.");
    }

    internal void FinishWeiwanObtain(long id, long batch, IReadOnlyList<CardMovementRecord> moves)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.Weiwan!;
        if (r.Stage != WeiwanStage.ObtainChildren || r.ActualCount != -1)
            throw new InvalidOperationException("Weiwan cannot obtain its random group twice.");
        var count = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Weiwan = r with { ObtainBatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        foreach (var sample in r.Samples.Where(s => s.CardId is not null))
        {
            var move = moves.Single(m => m.CardId == sample.CardId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanEntityObtainedEvent(id, sample.Index, move.CardId, move.CardKind, move.From, move.To, move.Sequence, batch));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanObtainPaidEvent(id, batch, moves.Count, count, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal void CompleteWeiwan(ProgramSkillFrame f)
    {
        var r = f.Weiwan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanCompletedEvent(f.Id, r.CardId, r.TargetSeat, Math.Max(0, r.ActualCount), r.Branch ?? WeiwanBranch.None));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weiwan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28620)]
    internal bool ResumeWeiwan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weiwan.CostSequence: > 0 } f || f.Id != id)
            return false;
        ContinueWeiwan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28620)]
    internal bool ReturnWeiwanMovement(ProgramSkillFrame f)
    {
        if (f.Weiwan is null || f.PendingMovementContinuation is null)
            return false;
        ContinueWeiwan(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28620)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28620)]
    internal bool ContinuePaidWeiwan(ProgramSkillFrame f) => HasPaidWeiwanSelection(f);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 28620)]
    internal void ObserveWeiwanHpLoss(IGameEvent payload)
    {
        if (payload is ProgramSkillHpLostEvent fact && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Weiwan: { Stage: WeiwanStage.HealthIssuing, Branch: WeiwanBranch.LoseHp, HpLossCaptured: false } r } f && fact.FrameId == f.Id && fact.SkillId == f.SkillId && fact.TargetSeat == r.TargetSeat && fact.Amount == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Weiwan = r with { Stage = WeiwanStage.HealthChildren, HpLossCaptured = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanHealthAppliedEvent(f.Id, fact.TargetSeat, fact.Amount, fact.RemainingHp, false));
        }
        else if (payload is IdentityHpDeductionPreventedEvent prevented && _resolutionStackOperations._resolutionStack.LastOrDefault()is IdentityAppearanceFrame appearance && appearance.Id == prevented.AppearanceFrameId && appearance.Receipt is { Entry: IdentityAppearanceEntry.ProgramHpLoss, ParentFrameId: { } parent } native && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == parent)is ProgramSkillFrame { Weiwan: { Stage: WeiwanStage.HealthIssuing, Branch: WeiwanBranch.LoseHp, HpLossCaptured: false } r2 } f2 && native.OwnerSeat == r2.TargetSeat && native.OriginalInstructionIndex == f2.InstructionIndex && prevented.RequestedDeduction == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f2.Id, f2 with { Weiwan = r2 with { Stage = WeiwanStage.HealthChildren, HpLossCaptured = true, AppearancePrevented = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeiwanHealthAppliedEvent(f2.Id, r2.TargetSeat, 0, _coreMatchParticipantsPlayersAccess._players[r2.TargetSeat].Hp, true));
        }
    }
}
