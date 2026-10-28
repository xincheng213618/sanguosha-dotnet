namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly CardMoveReason ShanheSourceZirunDraw = new("shanhe.source-zirun-1172.draw");
    [ProgramCapability]
    private sealed class ShanheSourceZirunHost(GameEngine engine) : IShanheSourceZirunHost
    { public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => engine.BeginShanheSourceZirun(frame); }

    private SkillProgramStepOutcome BeginShanheSourceZirun(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceZirun is not null || !HalfHandDebtLifecycleMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow))
            throw new InvalidOperationException("Source1172 requires its original own Preparation window.");
        var preparationWindow = f.WindowContext!.ParentFrameId;
        var seats = _players.Where(p => p.IsAlive).OrderBy(p => RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray();
        ReplaceRuntimeTop(f = f with { ShanheSourceZirun = new(preparationWindow, _turnNumber) { Recipients = seats } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceZirunStartedEvent(f.Id, preparationWindow,
            f.OwnerSeat, _turnNumber, seats.Length));
        ContinueShanheSourceZirun(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool ContinueShanheSourceZirun(long id)
    {
        while (_resolutionStack.LastOrDefault() is ProgramSkillFrame { ShanheSourceZirun: { } r } f && f.Id == id)
        {
            if (r.ReturningChildren)
            {
                if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) ||
                    TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
                    TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) ||
                    TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return true;
                ReplaceRuntimeTop(f = f with { ShanheSourceZirun = r = r with
                    { Index = r.Index + 1, RequestedCount = 0, ReturningChildren = false } });
            }
            if (r.Index >= r.Recipients.Count || _winner != Winner.None)
            { ReplaceRuntimeTop(f with { ShanheSourceZirun = null }); return false; }
            var seat = r.Recipients[r.Index];
            if (!_players[seat].IsAlive)
            { ReplaceRuntimeTop(f with { ShanheSourceZirun = r with { Index = r.Index + 1 } }); continue; }
            if (r.RequestedCount != 0 || f.NativeDraw is not null)
                throw new InvalidOperationException("Source1172 cannot pay an already issued recipient twice.");
            var equipment = GetEquipment(_players[seat]).Count;
            var requested = equipment == 0 ? 1 : 2;
            ReplaceRuntimeTop(f = f with { ShanheSourceZirun = r with { RequestedCount = requested } });
            AdvanceEventRulesAndQueueFact(new ShanheSourceZirunRequestedEvent(f.Id, r.Index, seat, equipment, requested));
            BeginOperationProgramDraw(f, seat, requested, ShanheSourceZirunDraw);
            return true;
        }
        return false;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheSourceZirun)]
    private void ReturnShanheSourceZirunDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.ShanheSourceZirun is not { ReturningChildren: false, RequestedCount: > 0 } r ||
            r.Index >= r.Recipients.Count || f.NativeDraw is not null || outcome.RecipientSeat != r.Recipients[r.Index] ||
            outcome.RequestedCount != r.RequestedCount || outcome.ActualDrawIds.Count > outcome.RequestedCount ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            ProgramEventHistory<ShanheSourceZirunReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index))
            throw new InvalidOperationException("Source1172 lost its exact once-issued native draw return.");
        ReplaceRuntimeTop(f with { ShanheSourceZirun = r with { ReturningChildren = true } });
        AdvanceEventRulesAndQueueFact(new ShanheSourceZirunReturnedEvent(f.Id, r.Index, outcome.RecipientSeat,
            outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.Replacement is not null));
        if (!ContinueShanheSourceZirun(f.Id)) AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24922)]
    private bool ResumeShanheSourceZirun(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheSourceZirun: not null } f || f.Id != id) return false;
        if (f.NativeDraw is null && !ContinueShanheSourceZirun(id)) AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 24922)]
    private void AssertShanheSourceZirun(ProgramSkillFrame f)
    {
        if (f.ShanheSourceZirun is not { } r) return;
        if (f.SkillId != ShanheSourceZirunComposition.SkillId || f.InstructionIndex != 1 ||
            f.WindowContext?.ParentFrameId != r.PreparationWindowId || r.ActualTurn != _turnNumber ||
            !HalfHandDebtLifecycleMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) ||
            r.Recipients is not System.Collections.IList { IsReadOnly: true } || r.Recipients.Count is < 1 or > 8 ||
            r.Recipients.Any(s => !IsValidPlayerSeat(s)) || r.Recipients.Distinct().Count() != r.Recipients.Count ||
            !r.Recipients.Contains(f.OwnerSeat) || r.Index < 0 || r.Index >= r.Recipients.Count || r.RequestedCount is < 1 or > 2 ||
            ProgramEventHistory<ShanheSourceZirunStartedEvent>().Count(e => e.FrameId == f.Id && e.PreparationWindowId == r.PreparationWindowId &&
                e.ActualTurn == r.ActualTurn && e.OwnerSeat == f.OwnerSeat && e.RecipientCount == r.Recipients.Count) != 1 ||
            ProgramEventHistory<ShanheSourceZirunRequestedEvent>().Where(e => e.FrameId == f.Id && e.Index == r.Index).ToArray() is not [var request] ||
            request.RecipientSeat != r.Recipients[r.Index] || request.RequestedCount != r.RequestedCount ||
            request.RequestedCount != (request.EquipmentCount == 0 ? 1 : 2))
            throw new InvalidOperationException("Source1172 lost its original Preparation, frozen recipient order or single requested count.");
        var returned = ProgramEventHistory<ShanheSourceZirunReturnedEvent>().Where(e => e.FrameId == f.Id && e.Index == r.Index).ToArray();
        if (r.ReturningChildren ? f.NativeDraw is not null || returned is not [var paid] || paid.RecipientSeat != request.RecipientSeat ||
                paid.RequestedCount != request.RequestedCount || !f.NativeOperationDrawReturns.Any(n => n.Operation == SkillProgramEffectOp.ResolveShanheSourceZirun &&
                    n.InstructionIndex == f.InstructionIndex && n.Reason == ShanheSourceZirunDraw && n.Outcome.RecipientSeat == paid.RecipientSeat &&
                    n.Outcome.RequestedCount == paid.RequestedCount && n.Outcome.SequenceBefore == paid.SequenceBefore &&
                    n.Outcome.SequenceAfter == paid.SequenceAfter && n.Outcome.ActualDrawIds.Count == paid.ActualCount)
            : returned.Length != 0 || f.NativeDraw is not { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation,
                Operation: SkillProgramEffectOp.ResolveShanheSourceZirun } active || active.RequestedCount != request.RequestedCount ||
                !active.Recipients.SequenceEqual([request.RecipientSeat]))
            throw new InvalidOperationException("Source1172 lost its original native draw payment or undrained recipient child.");
    }
}
