#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceZirunRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheSourceZirunHost.Begin(ProgramSkillFrame frame) => BeginShanheSourceZirun(frame);
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
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, requested, global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceZirunDefinitions.ShanheSourceZirunDraw);
            return true;
        }

        return false;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheSourceZirun)]
    internal void ReturnShanheSourceZirunDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceZirun is not { ReturningChildren: false, RequestedCount: > 0 } r || r.Index >= r.Recipients.Count || f.NativeDraw is not null || outcome.RecipientSeat != r.Recipients[r.Index] || outcome.RequestedCount != r.RequestedCount || outcome.ActualDrawIds.Count > outcome.RequestedCount || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceZirunReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index))
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

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 24922)]
    internal void AssertShanheSourceZirun(ProgramSkillFrame f)
    {
        if (f.ShanheSourceZirun is not { } r)
            return;
        if (f.SkillId != ShanheSourceZirunComposition.SkillId || f.InstructionIndex != 1 || f.WindowContext?.ParentFrameId != r.PreparationWindowId || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !_coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtLifecycleMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || r.Recipients is not System.Collections.IList { IsReadOnly: true } || r.Recipients.Count is < 1 or > 8 || r.Recipients.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.Recipients.Distinct().Count() != r.Recipients.Count || !r.Recipients.Contains(f.OwnerSeat) || r.Index < 0 || r.Index >= r.Recipients.Count || r.RequestedCount is < 1 or > 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceZirunStartedEvent>().Count(e => e.FrameId == f.Id && e.PreparationWindowId == r.PreparationWindowId && e.ActualTurn == r.ActualTurn && e.OwnerSeat == f.OwnerSeat && e.RecipientCount == r.Recipients.Count) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceZirunRequestedEvent>().Where(e => e.FrameId == f.Id && e.Index == r.Index).ToArray()is not [var request] || request.RecipientSeat != r.Recipients[r.Index] || request.RequestedCount != r.RequestedCount || request.RequestedCount != (request.EquipmentCount == 0 ? 1 : 2))
            throw new InvalidOperationException("Source1172 lost its original Preparation, frozen recipient order or single requested count.");
        var returned = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceZirunReturnedEvent>().Where(e => e.FrameId == f.Id && e.Index == r.Index).ToArray();
        if (r.ReturningChildren ? f.NativeDraw is not null || returned is not [var paid] || paid.RecipientSeat != request.RecipientSeat || paid.RequestedCount != request.RequestedCount || !f.NativeOperationDrawReturns.Any(n => n.Operation == SkillProgramEffectOp.ResolveShanheSourceZirun && n.InstructionIndex == f.InstructionIndex && n.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceZirunDefinitions.ShanheSourceZirunDraw && n.Outcome.RecipientSeat == paid.RecipientSeat && n.Outcome.RequestedCount == paid.RequestedCount && n.Outcome.SequenceBefore == paid.SequenceBefore && n.Outcome.SequenceAfter == paid.SequenceAfter && n.Outcome.ActualDrawIds.Count == paid.ActualCount) : returned.Length != 0 || f.NativeDraw is not { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, Operation: SkillProgramEffectOp.ResolveShanheSourceZirun } active || active.RequestedCount != request.RequestedCount || !active.Recipients.SequenceEqual([request.RecipientSeat]))
            throw new InvalidOperationException("Source1172 lost its original native draw payment or undrained recipient child.");
    }
}
