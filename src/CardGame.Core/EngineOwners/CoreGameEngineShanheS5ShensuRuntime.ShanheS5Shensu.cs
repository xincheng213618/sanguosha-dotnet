#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5ShensuRuntime
{
    internal const string ShanheS5ShensuId = "shanhe:s5-shensu-14144";
    internal bool TracksShanheS5PhaseSkips => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareShanheS5Shensu) == true;

    internal void RecordShanheS5ActualPhaseSkip(int owner, SkillProgramTurnPhase phase, long extraDrawFrameId = 0, long? costProgramFrameId = null)
    {
        if (!TracksShanheS5PhaseSkips)
            return;
        if (_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || owner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || phase is not (SkillProgramTurnPhase.Judgment or SkillProgramTurnPhase.Draw or SkillProgramTurnPhase.Play or SkillProgramTurnPhase.Discard) || extraDrawFrameId < 0 || extraDrawFrameId != 0 && phase != SkillProgramTurnPhase.Draw)
            throw new InvalidOperationException("S5 Shensu phase counting requires an exact actual-turn phase occurrence.");
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5SkippedPhaseRecordedEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Phase == phase && e.ExtraDrawFrameId == extraDrawFrameId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5SkippedPhaseRecordedEvent(owner, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, phase, extraDrawFrameId, costProgramFrameId));
    }

    internal void RecordShanheS5PhaseSkipCost(ProgramSkillFrame f, IReadOnlyList<SkillProgramTurnPhase> phases, ProgramLifecycleTriggerWindowFrame parent)
    {
        if (!TracksShanheS5PhaseSkips)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.WindowContext?.ParentFrameId != parent.Id || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != parent.Id || phases.Count == 0)
            throw new InvalidOperationException("S5 Shensu cannot count an uncommitted or foreign phase cost.");
        var extra = parent.ResumeDrawPhaseObligationFrameId is { } drawId && _resolutionStackOperations._resolutionStack.OfType<DrawPhaseObligationFrame>().SingleOrDefault(d => d.Id == drawId)is { IsExtra: true } draw ? draw.Id : 0;
        foreach (var phase in phases)
        {
            var occurrence = phase == SkillProgramTurnPhase.Draw ? extra : 0;
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5PhaseSkipCostEvent>().Any(e => e.FrameId == f.Id && e.InstructionIndex == f.InstructionIndex && e.Phase == phase))
                throw new InvalidOperationException("A phase cost cannot pay or count twice on the same native instruction.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5PhaseSkipCostEvent(f.Id, f.InstructionIndex, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, phase, occurrence));
            RecordShanheS5ActualPhaseSkip(f.OwnerSeat, phase, occurrence, f.Id);
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ActualPhaseSkipped, 59200)]
    internal bool ObserveShanheS5NativePhaseSkip(ProgramActualPhaseSkipContext context)
    {
        if (!TracksShanheS5PhaseSkips)
            return false;
        if (context.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("A skipped phase crossed its original actual turn.");
        RecordShanheS5ActualPhaseSkip(context.OwnerSeat, context.Phase);
        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 59200)]
    internal void ObserveShanheS5ExtraDrawSkip(IGameEvent payload)
    {
        if (!TracksShanheS5PhaseSkips || payload is not DrawPhaseSkippedEvent e)
            return;
        if (e.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || e.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<DrawPhaseObligationFrame>().SingleOrDefault(d => d.Id == e.FrameId)is not { Skipped: true } native || native.OwnerSeat != e.OwnerSeat || native.IsExtra != e.IsExtra)
            throw new InvalidOperationException("S5 Shensu lost a genuine skipped normal or extra Draw occurrence.");
        RecordShanheS5ActualPhaseSkip(e.OwnerSeat, SkillProgramTurnPhase.Draw, e.IsExtra ? e.FrameId : 0);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 59200, SkillProgramEffectOp.PrepareShanheS5Shensu)]
    internal bool CanOfferShanheS5Shensu(ProgramTriggerLegalityContext gate) => gate.SkillId != ShanheS5ShensuId || gate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players.Any(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat], p, physicalSuit: Suit.None, physicalCardIds: [], ignoreDistance: true, countsTowardLimit: false));
    internal void PrepareShanheS5Shensu(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShanheS5Shensu is not null || f.SkillId != ShanheS5ShensuId || f.WindowContext is not { } context || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.SelectedTargetSeats is not [var target] || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == f.OwnerSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("S5 Shensu requires its original current phase candidate and selected native Slash target.");
        var phases = ShanheS5ShensuContract.Phases(context.Window);
        var costs = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5PhaseSkipCostEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (phases.Length == 0 || costs.Length != phases.Length || !costs.Select(e => e.Phase).SequenceEqual(phases) || costs.Any(e => e.OwnerSeat != f.OwnerSeat || e.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || e.InstructionIndex != 2 || e.ExtraDrawFrameId != 0) || f.InstructionIndex != (context.Window == SkillProgramTriggerWindow.DiscardPhaseStarting ? 4 : 3))
            throw new InvalidOperationException("S5 Shensu must freeze X after this exact actual phase cost was committed.");
        var count = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5SkippedPhaseRecordedEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        var receipt = new ShanheS5ShensuReceipt(new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, parent.Id, context.Window, target, count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Shensu = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5ShensuPreparedEvent(f.Id, receipt));
    }

    internal bool IsShanheS5ShensuFrame(ProgramSkillFrame f) => f.SkillId == ShanheS5ShensuId && f.ShanheS5Shensu is not null;
    internal int ShanheS5ShensuBaseDamageBonus(ProgramSkillFrame active)
    {
        if (!IsShanheS5ShensuFrame(active))
            return 0;
        var root = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == active.Id) ?? throw new InvalidOperationException("S5 Shensu's original issuing frame is missing.");
        var receipt = root.ShanheS5Shensu!;
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != root.Id || _resolutionStackOperations._resolutionStack[^1] is not CardUseFrame { CardId: 0, CardKind: CardKind.Slash, PhysicalCardIds.Count: 0, Action: { PhysicalCards.Count: 0 } action } use || action.ActorSeat != root.OwnerSeat || use.SourceSeat != root.OwnerSeat || action.ConversionChain is not [var source] || source != receipt.Source)
            throw new InvalidOperationException("S5 Shensu base damage belongs only to its exact newly issued native virtual Slash.");
        // This is initial native damage, not an additional modifier. The real
        // incoming prediction, Dodge, later same-use elemental conversion and
        // damage payment all read this one frozen base.
        return receipt.SkippedPhaseCount;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 59200)]
    internal bool ContinueShanheS5Shensu(ProgramSkillFrame f)
    {
        if (f.ShanheS5Shensu is null)
            return IsPaidShanheS5TurnoverReturn(f);
        return true;
    }

    internal bool IsPaidShanheS5TurnoverReturn(ProgramSkillFrame f)
    {
        if (f.SkillId != ShanheS5ShensuId || f.InstructionIndex != 3 || f.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPhaseStarting } context || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.DiscardPhaseStarting } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheS5PhaseSkipCostEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [{ InstructionIndex: 2, Phase: SkillProgramTurnPhase.Discard, ExtraDrawFrameId: 0 } cost] || cost.OwnerSeat != f.OwnerSeat || cost.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return false;
        // Only the actual already-issued turnover on this original frame can
        // bridge source loss before Prepare freezes X. A mere queued phase
        // flag, a different character's turnover or a fresh trigger cannot.
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Count(e => e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == f.OwnerSeat && e.Change.Window == SkillProgramTriggerWindow.CharacterTurnedOver && e.Change.TurnedOver is { } paid && paid.WasFaceDown != paid.IsFaceDown) == 1;
    }

    void IShanheS5ShensuHost.PrepareShanheS5Shensu(ProgramSkillFrame f) => PrepareShanheS5Shensu(f);
}
