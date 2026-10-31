#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoHappyGodRuntime
{
    internal const string HappyGuixinReason = "skill-program.happy:guixin.obtain";
    internal static CardConversionSource HappyGuixinSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    internal static int HappyGuixinActualObtained(ProgramSkillFrame f) => f.HappyGuixin!.Payments.Sum(p => p.Movement.Movements.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)));
    SkillProgramStepOutcome IHappyGuixinHost.Resolve(ProgramSkillFrame f) => BeginHappyGuixin(f);
    internal SkillProgramStepOutcome BeginHappyGuixin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyGuixin is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) || damage.TargetSeat != f.OwnerSeat || f.WindowContext!.OccurrenceIndex < 0 || f.WindowContext.OccurrenceIndex >= damage.Amount)
            throw new InvalidOperationException("Happy Guixin requires its exact original positive damage-point candidate.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var r = new HappyGuixinReceipt(HappyGuixinSource(f), f.GameplayHash, window.Id, damage.Id, f.WindowContext.OccurrenceIndex, damage.Amount)
        {
            OtherSeats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != f.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuixin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, window.Id, damage.Id, r.OccurrenceIndex, damage.Amount, string.Join(",", r.OtherSeats)));
        ResumeHappyGuixin(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41400)]
    internal bool ResumeHappyGuixin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyGuixin: not null } initial || initial.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            var r = f.HappyGuixin!;
            if (r.Stage is HappyGuixinStage.MovementChildren or HappyGuixinStage.TurnoverChildren)
            {
                if (_characterCaoCaoMobileGodOperations.Value.DrainMobileGuixinChildren(f))
                    return true;
                if (r.Stage == HappyGuixinStage.TurnoverChildren)
                {
                    FinishHappyGuixin(f, false);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, HappyGuixin = r = r with { Stage = HappyGuixinStage.Visiting, Cursor = r.Cursor + 1 } });
            }

            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Payments.Count == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishHappyGuixin(f, true);
                return true;
            }

            if (r.Cursor == r.OtherSeats.Count)
            {
                var obtained = HappyGuixinActualObtained(f);
                var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown;
                if (obtained <= 4 || before)
                {
                    FinishHappyGuixin(f, false);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuixin = r with { Stage = HappyGuixinStage.TurnoverChildren, TurnoverIssued = true, FaceDownBefore = before, FaceDownAfter = _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ? before : !before } });
                _coreProgramLifecycleOperations.TurnOverProgramTarget(id, f.OwnerSeat, f.OwnerSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuixinTurnoverIssuedEvent(id, obtained, before, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown));
                continue;
            }

            var target = r.OtherSeats[r.Cursor];
            // The established HEJ obtain helper enforces current ownership, area access and pending-judgment exclusions.
            var candidates = _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? _characterCaoCaoOlGodOperations.Value.OlGuixinCandidates(f, target) : [];
            if (candidates.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuixin = r with { Cursor = r.Cursor + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuixinSeatSkippedEvent(id, r.Cursor, target));
                continue;
            }

            var randomBefore = _randomOperations._random.State;
            var selected = candidates[_randomOperations._random.Next(candidates.Count)];
            var randomAfter = _randomOperations._random.State;
            var beforeMove = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyGuixin = r with { Stage = HappyGuixinStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([selected.CardId], CardLocation.Hand(f.OwnerSeat), new(HappyGuixinReason), (batch, records) =>
            {
                var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var payment = new HappyGuixinPayment(new(r.Cursor, target, selected.CardId, selected.From, batch, beforeMove, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Movements = records }, randomBefore, randomAfter)
                {
                    Candidates = candidates
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { HappyGuixin = parent.HappyGuixin!with { Payments = parent.HappyGuixin.Payments.Append(payment).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuixinMovementPaidEvent(id, r.Cursor, target, batch, beforeMove, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            }, disclosure: selected.From.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        }

        return true;
    }

    internal void FinishHappyGuixin(ProgramSkillFrame f, bool cancelled)
    {
        var r = f.HappyGuixin!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyGuixin = r with { Stage = HappyGuixinStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyGuixinCompletedEvent(f.Id, r.Payments.Count, HappyGuixinActualObtained(f), r.TurnoverIssued, cancelled));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41400)]
    internal bool ReturnHappyGuixinMovement(ProgramSkillFrame f) => f.HappyGuixin is not null && ResumeHappyGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41400)]
    internal bool RetainHappyGuixinPaid(ProgramSkillFrame f) => f.HappyGuixin is { } r && (r.Payments.Count > 0 || r.TurnoverIssued) && ValidHappyGuixin(f);
}
