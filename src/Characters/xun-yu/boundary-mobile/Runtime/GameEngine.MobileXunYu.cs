namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileXunYuBonusReason = "skill-program.boundary:jieming-mobile.owner-bonus";
    [ProgramCapability]
    private sealed class MobileXunYuProgramCapability(GameEngine engine) : IMobileXunYuProgramHost
    { public SkillProgramStepOutcome DrawIfRecipientShort(ProgramSkillFrame frame) => engine.DrawMobileXunYuIfRecipientShort(frame); }

    private SkillProgramStepOutcome DrawMobileXunYuIfRecipientShort(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SelectedTargetSeats is not [var target] || !IsValidPlayerSeat(target) || !_players[target].IsAlive ||
            !_players[frame.OwnerSeat].IsAlive || GetHand(_players[target]).Count >= _players[target].MaxHp) return SkillProgramStepOutcome.Continue;
        return BeginOperationProgramDraw(frame, frame.OwnerSeat, 1, new(MobileXunYuBonusReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawMobileJiemingOwnerIfRecipientShort)]
    private void ReturnMobileXunYuBonusDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.SkillId != "boundary:jieming-mobile" || frame.InstructionIndex != 3 || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Mobile Jieming lost its original one-card bonus draw return.");
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 65201)]
    private bool MobileXunYuBonusMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.DrawMobileJiemingOwnerIfRecipientShort && frame.SkillId == "boundary:jieming-mobile" && frame.InstructionIndex == 3 &&
        pending == frame.PendingMovementContinuation && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: var owner } && owner == frame.OwnerSeat &&
        _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch =>
            batch.ParentFrameId == frame.Id && batch.OriginOwnerSeat == frame.OwnerSeat && batch.OriginSkillId == frame.SkillId && batch.OriginSkillInstanceId == frame.SkillInstanceId &&
            batch.Movements is [var move] && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(frame.OwnerSeat) &&
            move.Reason.Value == MobileXunYuBonusReason && _cardMovements.Contains(move));
}
