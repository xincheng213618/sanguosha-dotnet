namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileSunJianPolu = "boundary:polu-mobile";
    private static readonly CardMoveReason MobileSunJianPoluDraw = new("skill-program.boundary:polu-mobile.Draw");
    [ProgramCapability]
    private sealed class MobileSunJianHost(GameEngine engine) : IMobileSunJianPoluHost
    { public SkillProgramStepOutcome Draw(ProgramSkillFrame frame) => engine.BeginMobileSunJianPolu(frame); }

    private int MobileSunJianInvocationCount(ProgramSkillFrame frame) => ProgramEventHistory<ProgramBindingStartedEvent>()
        .Count(e => e.SkillId == MobileSunJianPolu && e.OwnerSeat == frame.OwnerSeat && e.FrameId <= frame.Id);

    private SkillProgramStepOutcome BeginMobileSunJianPolu(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileSunJianPolu || frame.InstructionIndex != 2 || frame.MobileSunJianPolu is not null ||
            frame.WindowContext?.Window is not (SkillProgramTriggerWindow.CharacterDied or SkillProgramTriggerWindow.OwnerDied))
            throw new InvalidOperationException("Polu requires its original accepted death trigger and frozen target set.");
        ReplaceRuntimeTop(frame = frame with { MobileSunJianPolu = new(
            new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, MobileSunJianInvocationCount(frame)) });
        ContinueMobileSunJianPolu(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool ValidMobileSunJianPolu(ProgramSkillFrame frame) => frame.MobileSunJianPolu is { } r &&
        frame.SkillId == MobileSunJianPolu && frame.InstructionIndex == 2 &&
        r.Source == new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId) &&
        r.Hash == frame.GameplayHash && r.Hash == _contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash &&
        r.Count > 0 && r.Count == MobileSunJianInvocationCount(frame) && r.RecipientIndex >= 0 &&
        r.RecipientIndex <= frame.SelectedTargetSeats.Count && frame.SelectedTargetSeats.Distinct().Count() == frame.SelectedTargetSeats.Count;

    private void ContinueMobileSunJianPolu(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileSunJianPolu: { } receipt } frame || frame.Id != id) return;
        if (!ValidMobileSunJianPolu(frame)) throw new InvalidOperationException("Polu lost its original invocation count or recipient cursor.");
        if (receipt.AwaitingMovement)
        {
            if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
                TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(id) ||
                TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(id)) return;
            ReplaceRuntimeTop(frame = GetActiveProgramFrame(id) with { MobileSunJianPolu = receipt = receipt with {
                RecipientIndex = receipt.RecipientIndex + 1, AwaitingMovement = false, Draw = null }, PendingMovementContinuation = null });
        }
        if (receipt.RecipientIndex == frame.SelectedTargetSeats.Count)
        {
            ReplaceRuntimeTop(frame with { MobileSunJianPolu = null });
            AdvanceRuntimeProgram(id);
            return;
        }
        BeginOperationProgramDraw(frame, frame.SelectedTargetSeats[receipt.RecipientIndex], receipt.Count, MobileSunJianPoluDraw);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileSunJianPolu)]
    private void ReturnMobileSunJianPoluDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (!ValidMobileSunJianPolu(frame) || frame.MobileSunJianPolu is not { AwaitingMovement: false } r ||
            r.RecipientIndex >= frame.SelectedTargetSeats.Count || outcome.RecipientSeat != frame.SelectedTargetSeats[r.RecipientIndex] ||
            outcome.RequestedCount != r.Count || outcome.ActualDrawIds.Count > r.Count ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            !_cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter &&
                m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == MobileSunJianPoluDraw)
                .Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Polu cannot accept a foreign or repeated native draw return.");
        ReplaceRuntimeTop(frame = frame with { MobileSunJianPolu = r with { AwaitingMovement = true, Draw = outcome },
            PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        ContinueMobileSunJianPolu(frame.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 65000)]
    private bool ResumeMobileSunJianPolu(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileSunJianPolu.AwaitingMovement: true } frame || frame.Id != id) return false;
        ContinueMobileSunJianPolu(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 65000)]
    private bool ReturnMobileSunJianPoluMovement(ProgramSkillFrame frame)
    {
        if (frame.MobileSunJianPolu is not { AwaitingMovement: true }) return false;
        ContinueMobileSunJianPolu(frame.Id); return true;
    }
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 65000)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 65000)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 65000)]
    private bool ProveMobileSunJianPoluMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.ResolveMobileSunJianPolu && ValidMobileSunJianPolu(frame) &&
        frame.MobileSunJianPolu is { AwaitingMovement: true, Draw: { } draw } r &&
        pending == frame.PendingMovementContinuation && pending.SubjectSeat == draw.RecipientSeat &&
        pending.BeforeCount == 0 && pending.CoverageResultBind is null && draw.RequestedCount == r.Count &&
        r.RecipientIndex < frame.SelectedTargetSeats.Count && draw.RecipientSeat == frame.SelectedTargetSeats[r.RecipientIndex] &&
        draw.SequenceAfter >= draw.SequenceBefore && draw.ActualDrawIds.All(id => _cardMovements.Any(m =>
            m.CardId == id && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter &&
            m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(draw.RecipientSeat) && m.Reason == MobileSunJianPoluDraw));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 65000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 65000)]
    private bool ProveMobileSunJianPoluRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) =>
        frame.MobileSunJianPolu is { AwaitingMovement: true, Draw.Replacement: not null } &&
        ValidMobileSunJianPolu(frame) && RecoveryReplacementFrameRidesOn(recovery, frame) &&
        recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 65000)]
    private void AssertMobileSunJianPolu(ProgramSkillFrame frame)
    { if (frame.MobileSunJianPolu is not null && !ValidMobileSunJianPolu(frame)) throw new InvalidOperationException("Polu changed its original death benefit."); }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 65000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 65000)]
    private bool KeepMobileSunJianPolu(ProgramSkillFrame frame) => frame.MobileSunJianPolu is not null && ValidMobileSunJianPolu(frame);
}
