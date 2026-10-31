#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouJieAnniversaryRuntime
{
    internal bool HasAnniversaryXiahouJie => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXiahouJieZhuangdan);

    internal static CardConversionSource AnniversaryXiahouJieSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static ProgramTriggerCandidate AnniversaryXiahouJieCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
    internal bool IsAnniversaryXiahouJieUniqueHand(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count < _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45500, SkillProgramEffectOp.ResolveAnniversaryXiahouJieLiedan, SkillProgramEffectOp.ResolveAnniversaryXiahouJieZhuangdan)]
    internal bool CanOfferAnniversaryXiahouJie(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || w.OwnerSeat != c.OwnerSeat || w.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat)
            return false;
        return t.Effects.Single().Op == SkillProgramEffectOp.ResolveAnniversaryXiahouJieLiedan ? w.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && (c.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.XiahouJieLie) >= 5) : w.Window == SkillProgramTriggerWindow.AfterTurnEnded && c.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && IsAnniversaryXiahouJieUniqueHand(c.OwnerSeat);
    }

    SkillProgramStepOutcome IAnniversaryXiahouJieHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginAnniversaryXiahouJie(f, op);
    internal SkillProgramStepOutcome BeginAnniversaryXiahouJie(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryXiahouJie is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || !CanOfferAnniversaryXiahouJie(AnniversaryXiahouJieCandidate(f), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Xiahou Jie lost his original unpaid preparation or actual foreign turn end.");
        if (operation == SkillProgramEffectOp.ResolveAnniversaryXiahouJieZhuangdan)
        {
            if (_resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } ended || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended)))
                throw new InvalidOperationException("Zhuangdan lost its actual end-of-turn candidate.");
            _coreProgramSkillChangesOperations.ApplyProgramSkillState(f, f.OwnerSeat,
                new(SkillStateAction.Suppress, SkillStateScope.Named, SkillStateDuration.UntilTurnEndOf)
                { CharacterOnly = false, UntilTurnEndOf = new(ProgramParticipantRef.Owner) }, [AnniversaryXiahouJieComposition.Liedan]);
            return SkillProgramStepOutcome.Continue;
        }

        var own = f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat;
        var r = new AnniversaryXiahouJieReceipt(AnniversaryXiahouJieSource(f), f.GameplayHash, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, own ? AnniversaryXiahouJieStage.DirectDeath : AnniversaryXiahouJieStage.Comparing, DeathMarks: own ? _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.XiahouJieLie) : 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouJie = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXiahouJieStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.ActualTurn, r.TurnOwner, own, r.DeathMarks));
        if (own)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillCauseDeathDeclaredEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.OwnerSeat));
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], null, null, null, causingProgramSkillFrameId: f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueAnniversaryXiahouJie(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainAnniversaryXiahouJie(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueAnniversaryXiahouJie(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryXiahouJie: { } r } f && f.Id == id)
        {
            if (r.Stage == AnniversaryXiahouJieStage.Drawing)
                return;
            if (r.Stage == AnniversaryXiahouJieStage.HpChildren && !r.HpPaid)
            {
                var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1, (before, after, actual) =>
                {
                    var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    var receipt = active.AnniversaryXiahouJie!;
                    _coreRuntimeOperations.ReplaceRuntimeTop(active with { AnniversaryXiahouJie = receipt with { HpPaid = true, HpBefore = before, HpAfter = after, ActualLoss = actual } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXiahouJieHpPaidEvent(id, before, after, actual));
                });
                if (outcome == SkillProgramStepOutcome.AwaitChild)
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.AnniversaryXiahouJie!;
            }

            if (DrainAnniversaryXiahouJie(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.AnniversaryXiahouJie!;
            if (r.Stage == AnniversaryXiahouJieStage.HpChildren)
            {
                if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                    var marker = PlayerMarkerKind.XiahouJieLie;
                    var count = owner.Markers.GetValueOrDefault(marker) + 1;
                    owner.Markers[marker] = count;
                    var key = (marker, f.OwnerSeat);
                    owner.MarkerSourceCounts[key] = owner.MarkerSourceCounts.GetValueOrDefault(key) + 1;
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, marker, 1, count, f.OwnerSeat, "skill-program.anniversary:liedan.lie"));
                }

                FinishAnniversaryXiahouJie(f);
                return;
            }

            if (r.Stage is AnniversaryXiahouJieStage.MaximumChildren or AnniversaryXiahouJieStage.DirectDeath || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                FinishAnniversaryXiahouJie(f);
                return;
            }

            if (r.Stage == AnniversaryXiahouJieStage.DrawChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouJie = r with { Stage = AnniversaryXiahouJieStage.Comparing, Cursor = r.Cursor + 1 } });
                r = f.AnniversaryXiahouJie!;
            }

            if (r.Cursor == 3)
            {
                if (r.Comparisons.All(c => c.Greater == true))
                {
                    var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouJie = r with { Stage = AnniversaryXiahouJieStage.MaximumChildren, MaximumBefore = before, MaximumAfter = before } });
                    _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
                    var after = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXiahouJie = f.AnniversaryXiahouJie!with { MaximumAfter = after } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXiahouJieMaximumEvent(id, before, after));
                    continue;
                }

                if (r.Comparisons.All(c => c.Greater == false))
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXiahouJie = r with { Stage = AnniversaryXiahouJieStage.HpChildren } });
                    continue;
                }

                FinishAnniversaryXiahouJie(f);
                return;
            }

            var ownerNow = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var otherNow = _coreMatchParticipantsPlayersAccess._players[r.TurnOwner];
            int? ownValue = r.Cursor switch
            {
                0 => _corePhysicalOwnedCardQueriesOperations.GetHand(ownerNow).Count,
                1 => _coreHealthQueriesOperations.UnambiguousHp(ownerNow),
                _ => _corePhysicalOwnedCardQueriesOperations.GetEquipment(ownerNow).Count
            };
            int? otherValue = r.Cursor switch
            {
                0 => _corePhysicalOwnedCardQueriesOperations.GetHand(otherNow).Count,
                1 => _coreHealthQueriesOperations.UnambiguousHp(otherNow),
                _ => _corePhysicalOwnedCardQueriesOperations.GetEquipment(otherNow).Count
            };
            var comparison = new AnniversaryXiahouJieComparison(r.Cursor, ownValue, otherValue);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouJie = r with { Comparisons = r.Comparisons.Append(comparison).ToArray(), Stage = comparison.Greater == true ? AnniversaryXiahouJieStage.Drawing : AnniversaryXiahouJieStage.Comparing, Cursor = comparison.Greater == true ? r.Cursor : r.Cursor + 1 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXiahouJieComparedEvent(id, comparison.Index, ownValue, otherValue));
            if (comparison.Greater == true)
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, CharacterXiahouJieAnniversaryDefinitions.AnniversaryXiahouJieDrawReason);
                return;
            }
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryXiahouJieLiedan)]
    internal void ReturnAnniversaryXiahouJieDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryXiahouJie!;
        if (r.Stage != AnniversaryXiahouJieStage.Drawing || f.NativeDraw is not null || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != 1 || r.Draws.Any(d => d.Index == r.Cursor) || draw.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Liedan lost its once-issued native comparison draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouJie = r with { Stage = AnniversaryXiahouJieStage.DrawChildren, Draws = r.Draws.Append(new(r.Cursor, draw)).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXiahouJieDrawReturnedEvent(f.Id, r.Cursor, draw.ActualDrawIds.Count, draw.AttemptFrameId, draw.SequenceBefore, draw.SequenceAfter));
        ContinueAnniversaryXiahouJie(f.Id);
    }

    internal void FinishAnniversaryXiahouJie(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXiahouJieCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXiahouJie = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 45500)]
    internal bool ResumeAnniversaryXiahouJie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXiahouJie: not null } f || f.Id != id)
            return false;
        ContinueAnniversaryXiahouJie(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 45500)]
    internal bool ReturnAnniversaryXiahouJieMovement(ProgramSkillFrame f) => f.AnniversaryXiahouJie is not null && ResumeAnniversaryXiahouJie(f.Id);
}
