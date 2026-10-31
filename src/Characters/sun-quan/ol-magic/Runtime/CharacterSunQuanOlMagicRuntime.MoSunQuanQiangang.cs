#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool HasMoQiangangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMoSunQuanQiangangEntry);

    internal static CardConversionSource MoQiangangSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_sun_quan_ol_magic_MoSunQuanQiangang.MoQiangangSource(f);
    internal bool TryGetMoQiangangOrigin(int ownerSeat, out CardConversionSource source, out string gameplayHash)
    {
        source = null !;
        gameplayHash = "";
        if (!HasMoQiangangCapability || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat))
            return false;
        var entries = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangEnteredEvent>().Where(e => e.Source.OwnerSeat == ownerSeat).ToArray();
        if (entries.Length == 0)
            return false;
        if (entries.Length != 1 || !IsPaidQiangangRumoOrigin(entries[0].FrameId, entries[0].Source, entries[0].GameplayHash))
            throw new InvalidOperationException("Permanent Qiangang entry lost its unique real Tianen-loss payment.");
        source = entries[0].Source;
        gameplayHash = entries[0].GameplayHash;
        return true;
    }

    internal bool MoQiangangEntered(int ownerSeat) => TryGetMoQiangangOrigin(ownerSeat, out _, out _);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 28930, SkillProgramEffectOp.ResolveMoSunQuanQiangangEntry)]
    internal bool CanActivateMoQiangang(ProgramActivationLegalityContext gate) => HasMoQiangangCapability && _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == gate.OwnerSeat && gate.Owner.IsAlive && !MoQiangangEntered(gate.OwnerSeat) && _coreNationalSkillsOperations.HasRuntimeSkill(gate.Owner, "ol:mo-tianen") && gate.Owner.SkillGrants.Grants.Any(g => g.SkillId == "ol:mo-tianen" && !g.IsModeRule) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == gate.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase);
    SkillProgramStepOutcome IMoSunQuanQiangangHost.Resolve(ProgramSkillFrame f) => BeginMoQiangang(f);
    internal SkillProgramStepOutcome BeginMoQiangang(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MoQiangang is not null)
        {
            ContinueMoQiangang(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.SkillId != "ol:mo-qiangang" || f.ActivationId != "lose-tianen-enter-rumo" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId) || !_coreNationalSkillsOperations.HasRuntimeSkill(owner, "ol:mo-tianen") || MoQiangangEntered(f.OwnerSeat) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            throw new InvalidOperationException("Qiangang needs its true current Play, held Tianen and unpaid exact activation.");
        var grants = owner.SkillGrants.Grants.Where(g => g.SkillId == "ol:mo-tianen" && !g.IsModeRule).ToArray();
        if (grants.Length == 0)
            throw new InvalidOperationException("Qiangang cannot pay an absent Tianen skill.");
        var before = owner.SkillGrants.Revision;
        var r = new MoQiangangReceipt(MoQiangangSource(f), f.GameplayHash, phase, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, before)
        {
            Stage = MoQiangangStage.TianenLossChildren,
            RemovedGrants = grants.Select(g => new MoQiangangRemovedGrant(g.GrantId, g.SkillInstanceId, g.SourceId)).ToArray(),
            RevisionAfter = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQiangang = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQiangangStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner, grants.Length, before));
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        // All actual grants are removed before any public payment fact can run observers.
        foreach (var grant in grants)
            if (!owner.SkillGrants.RemoveGrant(grant.GrantId))
                throw new InvalidOperationException("Qiangang lost an actual Tianen grant before paying.");
        r = r with
        {
            RevisionAfter = owner.SkillGrants.Revision
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MoQiangang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQiangangTianenLossPaidEvent(f.Id, r.Source, r.GameplayHash, grants.Length, before, r.RevisionAfter));
        for (var i = 0; i < grants.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQiangangTianenGrantRemovedEvent(f.Id, i, grants[i].GrantId, grants[i].SkillInstanceId, grants[i].SourceId));
        ContinueMoQiangang(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainMoQiangang(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMoQiangang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoQiangang: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == MoQiangangStage.TianenLossChildren)
        {
            if (DrainMoQiangang(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { MoQiangang = r = r with { Stage = MoQiangangStage.EnteringRumo }, PendingMovementContinuation = null });
            _characterSharedGameEngineOlMoRumoOperations.EnterOlMoRumo(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQiangangEnteredEvent(id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { MoQiangang = r = r with { Stage = MoQiangangStage.EntryChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        }

        if (r.Stage != MoQiangangStage.EntryChildren)
            throw new InvalidOperationException("Qiangang entry cannot resume from an unissued stage.");
        if (DrainMoQiangang(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQiangangCompletedEvent(id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool IsPaidQiangangRumoOrigin(long frameId, CardConversionSource source, string gameplayHash)
    {
        if (source.SkillId != "ol:mo-qiangang" || source.BindingId != "lose-tianen-enter-rumo" || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash != gameplayHash || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangStartedEvent>().Where(e => e.FrameId == frameId).ToArray()is not [var start] || start.Source != source || start.GameplayHash != gameplayHash || start.GrantCount < 1 || start.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || start.Phase.ActorSeat != source.OwnerSeat || start.Phase.TurnNumber != start.ActualTurn || start.Phase.ActualTurnOwnerSeat != start.TurnOwner || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(start.Phase) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frameId && e.OwnerSeat == source.OwnerSeat && e.SkillId == source.SkillId && e.ActivationId == source.BindingId) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangTianenLossPaidEvent>().Where(e => e.FrameId == frameId).ToArray()is not [var paid] || paid.Source != source || paid.GameplayHash != gameplayHash || paid.GrantCount != start.GrantCount || paid.RevisionBefore != start.RevisionBefore || paid.RevisionAfter != paid.RevisionBefore + paid.GrantCount)
            return false;
        var removed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangTianenGrantRemovedEvent>().Where(e => e.FrameId == frameId).OrderBy(e => e.Index).ToArray();
        return removed.Length == paid.GrantCount && removed.Select(e => e.GrantId).Distinct(StringComparer.Ordinal).Count() == removed.Length && removed.Where((e, i) => e.Index != i || string.IsNullOrEmpty(e.GrantId) || string.IsNullOrEmpty(e.SkillInstanceId) || string.IsNullOrEmpty(e.SourceId)).Any() == false;
    }

    internal bool IsPaidQiangangRumoEntry(ProgramSkillFrame f) => f.MoQiangang is { Stage: MoQiangangStage.EnteringRumo } r && ValidMoQiangang(f) && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && f.PendingMovementContinuation is null && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangEnteredEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat) && IsPaidQiangangRumoOrigin(f.Id, r.Source, r.GameplayHash);
    internal bool ValidMoQiangang(ProgramSkillFrame f)
    {
        if (f.MoQiangang is not { } r || f.SkillId != "ol:mo-qiangang" || f.ActivationId != "lose-tianen-enter-rumo" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != MoQiangangSource(f) || r.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.RemovedGrants is not System.Collections.IList { IsReadOnly: true } || !IsPaidQiangangRumoOrigin(f.Id, r.Source, r.GameplayHash))
            return false;
        var start = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangStartedEvent>().Single(e => e.FrameId == f.Id);
        var paid = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangTianenLossPaidEvent>().Single(e => e.FrameId == f.Id);
        var removed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangTianenGrantRemovedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (start.Phase != r.Phase || start.ActualTurn != r.ActualTurn || start.TurnOwner != r.TurnOwner || r.RevisionBefore != paid.RevisionBefore || r.RevisionAfter != paid.RevisionAfter || r.RemovedGrants.Count != removed.Length || removed.Where((e, i) => r.RemovedGrants[i] != new MoQiangangRemovedGrant(e.GrantId, e.SkillInstanceId, e.SourceId)).Any())
            return false;
        var entries = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoQiangangEnteredEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return r.Stage == MoQiangangStage.EntryChildren ? entries is [var entered] && entered == new MoQiangangEnteredEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner) : entries.Length == 0;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28930)]
    internal bool ResumeMoQiangang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoQiangang: not null } f || f.Id != id)
            return false;
        ContinueMoQiangang(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28930)]
    internal bool ReturnMoQiangangMovement(ProgramSkillFrame f)
    {
        if (f.MoQiangang is null)
            return false;
        ContinueMoQiangang(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28930)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28930)]
    internal bool CanContinuePaidMoQiangang(ProgramSkillFrame f) => f.MoQiangang is not null && ValidMoQiangang(f);
}
