#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeferredHandAlignmentsRuntime
{
    void IDeferredAlignmentProgramHost.ScheduleDeferredHandAlignment(ProgramSkillFrame frame, string continuationId) => ScheduleDeferredHandAlignment(frame, continuationId);
    SkillProgramStepOutcome IDeferredAlignmentProgramHost.ResolveDeferredHandAlignment(ProgramSkillFrame frame) => ResolveDeferredHandAlignment(frame);
    internal void CleanupDeferredHandAlignments()
    {
        foreach (var due in _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments.Where(d => _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == d.Source.SkillId && g.SkillInstanceId == d.Source.SkillInstanceId)).ToArray())
        {
            _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments.Remove(due);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeferredHandAlignmentCancelledEvent(due.Id));
        }
    }

    internal void ScheduleDeferredHandAlignment(ProgramSkillFrame frame, string continuationId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not TurnEndingBoundaryFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != frame.OwnerSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("Deferred alignment requires the actual owner's ending parent and one other living target.");
        var continuation = ProgramInstructionResolver.Default.Resolve(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!, ProgramInstructionSourceKind.Trigger, continuationId).Trigger!;
        var instruction = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetInstruction(active.InstructionIndex - 1).Effect;
        if (instruction is not { Op: SkillProgramEffectOp.ScheduleDeferredHandAlignment } || instruction.StateId != continuationId || context.SourceSeat != parent.OwnerSeat || context.TargetSeat != parent.OwnerSeat)
            throw new InvalidOperationException("Deferred scheduling requires its exact currently executing instruction and ending context.");
        var candidate = parent.Items.ElementAtOrDefault(parent.ItemIndex)?.Candidate;
        if (candidate is null || candidate.OwnerSeat != frame.OwnerSeat || candidate.SkillId != frame.SkillId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.BindingId != frame.TriggerId || context.OwnerSeat != frame.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId!))
            throw new InvalidOperationException("Deferred alignment requires the exact frozen ending candidate and live source grant.");
        if (!continuation.DeferredTurnEndOnly)
            throw new InvalidOperationException("Invalid deferred continuation binding.");
        var source = new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId);
        foreach (var kind in new[]
        {
            DeferredHandAlignmentDueKind.SourceCurrentTurnEnd,
            DeferredHandAlignmentDueKind.TargetNextActualTurnEnd
        }

        )
        {
            var due = new DeferredHandAlignment(++_coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignmentSequence, source, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, kind, continuationId);
            _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments.Add(due);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeferredHandAlignmentScheduledEvent(due));
        }
    }

    internal bool TryBeginDeferredTurnEnd(CharacterState previous, bool skipped)
    {
        CleanupDeferredHandAlignments();
        var dueIds = _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments.Where(d => d.DueKind == DeferredHandAlignmentDueKind.SourceCurrentTurnEnd ? d.Source.OwnerSeat == previous.Seat && d.CreatedTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber : d.TargetSeat == previous.Seat && d.CreatedTurn < _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber).OrderBy(d => d.Id).Select(d => d.Id).ToArray();
        var quarantines = _coreSoleTargetQuarantineOperations.OutstandingSoleTargetQuarantines(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, previous.Seat);
        if (dueIds.Length == 0 && !_corePrivateTurnHoldsOperations.HasActualTurnEndMovementPrelude && quarantines.Length == 0)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Finished)
            throw new InvalidOperationException("Actual end alignment requires a clean finalized turn boundary.");
        _coreRuntimeOperations.PushRuntimeFrame(new DeferredTurnEndFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, previous.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, skipped, Array.AsReadOnly(dueIds)) { Prelude = _corePrivateTurnHoldsOperations.HasActualTurnEndMovementPrelude ? new() : null, SoleTargetQuarantineReturns = quarantines.Length == 0 ? null : new(Array.AsReadOnly(quarantines)) });
        ContinueDeferredTurnEnd();
        return true;
    }

    internal void ContinueDeferredTurnEnd() => _coreRuntimeOperations.AdvanceRuntimeTop<DeferredTurnEndFrame>();
    [NativeFrameRoute(typeof(DeferredTurnEndFrame), 470)]
    internal void ContinueDeferredTurnEndCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is DeferredTurnEndFrame parent)
        {
            if (parent.Prelude is { Completed: false })
            {
                if (_coreDeferredTurnEndPreludesOperations.ContinueDeferredTurnEndPrelude(parent))
                    return;
                parent = (DeferredTurnEndFrame)_resolutionStackOperations._resolutionStack[^1];
            }

            CleanupDeferredHandAlignments();
            if (parent.Current is not null)
                throw new InvalidOperationException("A deferred cursor retained its already finished child.");
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                _coreRuntimeOperations.PopResolutionFrame(parent.Id, ResolutionFrameKind.DeferredTurnEnd);
                CleanupDeferredHandAlignments();
                return;
            }

            if (parent.ItemIndex == parent.DueIds.Count)
            {
                if (_coreSoleTargetQuarantineOperations.ContinueSoleTargetQuarantineReturns(parent))
                    return;
                parent = (DeferredTurnEndFrame)_resolutionStackOperations._resolutionStack[^1];
                if (_coreAfterTurnEndedProgramsOperations.ContinueAfterTurnEndedPrograms(parent))
                    return;
                _characterQinLangOlOperations.ExpireQinLangXianyingTurnEndWine();
                _characterAHuiNanOlOperations.ExpireAHuiNanJuemanTurnEndWine();
                _coreRuntimeOperations.PopResolutionFrame(parent.Id, ResolutionFrameKind.DeferredTurnEnd);
                _coreActualTurnsOperations.AdvanceAfterDeferredTurnEnd(_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat], parent.Skipped);
                return;
            }

            var due = _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments.SingleOrDefault(d => d.Id == parent.DueIds[parent.ItemIndex]);
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { ItemIndex = parent.ItemIndex + 1 });
            if (due is null)
                continue;
            if (due.Id <= 0 || due.Id > _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignmentSequence || due.Source.OwnerSeat == due.TargetSeat || !(due.DueKind == DeferredHandAlignmentDueKind.SourceCurrentTurnEnd ? due.Source.OwnerSeat == parent.OwnerSeat && due.CreatedTurn == parent.TurnNumber : due.DueKind == DeferredHandAlignmentDueKind.TargetNextActualTurnEnd && due.TargetSeat == parent.OwnerSeat && due.CreatedTurn < parent.TurnNumber))
                throw new InvalidOperationException("The deferred cursor contains an invalid or premature due.");
            _coreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments.Remove(due);
            var enabled = _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(_coreMatchParticipantsPlayersAccess._players[due.Source.OwnerSeat], SkillProgramTriggerWindow.TurnEnding).Any(t => t.SkillId == due.Source.SkillId && t.SkillInstanceId == due.Source.SkillInstanceId && t.Trigger.Id == due.Source.BindingId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeferredHandAlignmentConsumedEvent(due, enabled));
            if (!enabled)
                continue;
            var program = _contentRegistryOperations._contentRegistry.GetSkill(due.Source.SkillId).Program!;
            var trigger = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, due.ContinuationId).Trigger!;
            if (!trigger.DeferredTurnEndOnly)
                throw new InvalidOperationException("The exact continuation changed after scheduling.");
            _coreRuntimeOperations.ReplaceRuntimeTop(((DeferredTurnEndFrame)_resolutionStackOperations._resolutionStack[^1])with { Current = due });
            var child = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, due.Source.OwnerSeat, due.Source.SkillId, due.ContinuationId, program.GameplayHash, 0, [], [due.TargetSeat])
            {
                TriggerId = due.ContinuationId,
                SkillInstanceId = due.Source.SkillInstanceId!,
                WindowContext = new(SkillProgramTriggerWindow.TurnEnding, parent.Id, due.Source.OwnerSeat, TargetSeat: due.TargetSeat)
            };
            _coreRuntimeOperations.PushRuntimeFrame(child);
            _coreRuntimeOperations.AdvanceRuntimeProgram(child.Id);
            return;
        }
    }

    internal DeferredHandAlignment RequireDeferredAlignmentParent(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DeferredTurnEndFrame parent || parent.AfterTurnEnded is not null || parent.Prelude is { Completed: false } || parent.Id != context.ParentFrameId || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || parent.Current is not { } due || parent.ItemIndex < 1 || parent.ItemIndex > parent.DueIds.Count || parent.DueIds[parent.ItemIndex - 1] != due.Id || context.OwnerSeat != frame.OwnerSeat || frame.OwnerSeat != due.Source.OwnerSeat || frame.SkillId != due.Source.SkillId || frame.SkillInstanceId != due.Source.SkillInstanceId || frame.TriggerId != due.ContinuationId || frame.SelectedTargetSeats is not [var target] || target != due.TargetSeat || context.TargetSeat != target)
            throw new InvalidOperationException("The deferred resolver lost its exact due, source, grant, turn or parent.");
        return due;
    }

    internal SkillProgramStepOutcome ResolveDeferredHandAlignment(ProgramSkillFrame frame)
    {
        var due = RequireDeferredAlignmentParent(frame);
        var ownerCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count;
        var targetCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[due.TargetSeat]).Count;
        var draw = targetCount < ownerCount ? Math.Max(0, Math.Min(5, ownerCount) - targetCount) : 0;
        var discard = Math.Max(0, targetCount - ownerCount);
        var frozen = new DeferredHandAlignmentResolution(due.Id, due.TargetSeat, ownerCount, targetCount, draw, discard);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { DeferredHandAlignmentResolution = frozen });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeferredHandAlignmentResolvedEvent(due.Id, ownerCount, targetCount, draw, discard));
        if (draw > 0)
        {
            _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, due.TargetSeat, draw, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.deferred-hand.draw"));
            BeginDeferredAlignmentMovement(frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (discard > 0)
        {
            var effect = _coreProgramOwnedCardsOperations.GetOwnedSelectionEffect(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return _coreProgramOwnedCardsOperations.SelectProgramOwnedCards(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), due.TargetSeat, discard, null, [CardZoneKind.Hand], effect.ResultBind!, discard, 0, [], []);
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramEffect DeferredOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect original)
    {
        RequireDeferredAlignmentParent(frame);
        var draft = frame.DeferredHandAlignmentResolution ?? throw new InvalidOperationException("Missing frozen hand alignment delta.");
        if (original.Op != SkillProgramEffectOp.ResolveDeferredHandAlignment || draft.DiscardCount <= 0)
            throw new InvalidOperationException("Only a frozen deferred discard owns this draft.");
        return new(SkillProgramEffectOp.SelectOwnedCards, SkillProgramEffectTarget.SelectedTarget, draft.DiscardCount, original.Condition, resultBind: original.ResultBind, zones: [CardZoneKind.Hand], minimumCards: draft.DiscardCount);
    }

    internal void CompleteDeferredAlignmentDiscard(long frameId, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var due = RequireDeferredAlignmentParent(frame);
        var draft = frame.DeferredHandAlignmentResolution!;
        if (ids.Count != draft.DiscardCount || ids.Distinct().Count() != ids.Count || locations.Any(l => l != CardLocation.Hand(due.TargetSeat)) || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(due.TargetSeat)))
            throw new InvalidOperationException("Deferred discard requires its exact frozen private hand entities.");
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(frame.OwnerSeat, due.TargetSeat, ids.Select(id => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[due.TargetSeat]).Single(c => c.Id == id)).ToArray(), CardLocation.Hand(due.TargetSeat), new("skill-program.deferred-hand.discard"));
        BeginDeferredAlignmentMovement(frameId);
    }

    internal void BeginDeferredAlignmentMovement(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.DeferredHandAlignmentResolution!.TargetSeat, 0, null) });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frameId);
    }

    internal bool CompleteDeferredTurnEndBinding(ProgramSkillFrame frame, ProgramSkillWindowContext context)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DeferredTurnEndFrame parent)
            return false;
        if (context.Window == SkillProgramTriggerWindow.AfterTurnEnded)
            return _coreAfterTurnEndedProgramsOperations.CompleteAfterTurnEndedBinding(parent, frame, context);
        if (parent.Id != context.ParentFrameId || parent.Current is not { } due || parent.ItemIndex < 1 || parent.ItemIndex > parent.DueIds.Count || parent.DueIds[parent.ItemIndex - 1] != due.Id || context.OwnerSeat != frame.OwnerSeat || frame.OwnerSeat != due.Source.OwnerSeat || frame.SkillId != due.Source.SkillId || frame.SkillInstanceId != due.Source.SkillInstanceId || frame.TriggerId != due.ContinuationId)
            throw new InvalidOperationException("A finished deferred binding lost its exact parent cursor.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Current = null });
        ContinueDeferredTurnEnd();
        return true;
    }
}
