#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceZirunRuntime
{
    SkillProgramStepOutcome IShanheSourceZirunHost.Begin(ProgramSkillFrame frame) => BeginShanheSourceZirun(frame);
    internal SkillProgramStepOutcome BeginShanheSourceZirun(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceZirun is not null || !_coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtLifecycleMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow))
            throw new InvalidOperationException("Source1172 requires its original own Preparation window.");
        var preparationWindow = f.WindowContext!.ParentFrameId;
        var seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceZirun = new(preparationWindow, _coreActualTurnProgressTurnNumberAccess._turnNumber) { Recipients = seats } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceZirunStartedEvent(f.Id, preparationWindow, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, seats.Length));
        ContinueShanheSourceZirun(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ContinueShanheSourceZirun(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShanheSourceZirun: { } r } f && f.Id == id)
        {
            if (r.ReturningChildren)
            {
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceZirun = r = r with { Index = r.Index + 1, RequestedCount = 0, ReturningChildren = false } });
            }

            if (r.Index >= r.Recipients.Count || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceZirun = null });
                return false;
            }

            var seat = r.Recipients[r.Index];
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceZirun = r with { Index = r.Index + 1 } });
                continue;
            }

            if (r.RequestedCount != 0 || f.NativeDraw is not null)
                throw new InvalidOperationException("Source1172 cannot pay an already issued recipient twice.");
            var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
            var requested = equipment == 0 ? 1 : 2;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceZirun = r with { RequestedCount = requested } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceZirunRequestedEvent(f.Id, r.Index, seat, equipment, requested));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, requested, CoreGameEngineShanheSourceZirunDefinitions.ShanheSourceZirunDraw);
            return true;
        }

        return false;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheSourceZirun)]
    internal void ReturnShanheSourceZirunDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceZirun is not { ReturningChildren: false, RequestedCount: > 0 } r || r.Index >= r.Recipients.Count || f.NativeDraw is not null || outcome.RecipientSeat != r.Recipients[r.Index] || outcome.RequestedCount != r.RequestedCount || outcome.ActualDrawIds.Count > outcome.RequestedCount || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheSourceZirunReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index))
            throw new InvalidOperationException("Source1172 lost its exact once-issued native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceZirun = r with { ReturningChildren = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceZirunReturnedEvent(f.Id, r.Index, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.Replacement is not null));
        if (!ContinueShanheSourceZirun(f.Id))
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24922)]
    internal bool ResumeShanheSourceZirun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceZirun: not null } f || f.Id != id)
            return false;
        if (f.NativeDraw is null && !ContinueShanheSourceZirun(id))
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }
}
