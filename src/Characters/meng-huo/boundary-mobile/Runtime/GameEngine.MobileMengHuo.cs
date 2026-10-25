namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static bool IsMobileMengHuoZaiqi(ProgramSkillFrame frame) => frame.SkillId == "boundary:zaiqi-mobile";

    private bool ValidMobileOrOlRedDiscardRecoveryWindow(ProgramSkillFrame frame) =>
        frame.OwnerSeat == _currentSeat && frame.WindowContext?.Window ==
            (IsMobileMengHuoZaiqi(frame) ? SkillProgramTriggerWindow.DiscardPhaseEnded : SkillProgramTriggerWindow.TurnEnding);

    private bool CanMobileOrOlRedDiscardRecoverySelect(ProgramSkillFrame frame, int seat) =>
        IsValidPlayerSeat(seat) && (seat != frame.OwnerSeat || IsMobileMengHuoZaiqi(frame));

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 66101)]
    private bool MobileMengHuoZaiqiDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.OfferRedDiscardRecoveryChoice || !IsMobileMengHuoZaiqi(frame) ||
            frame.TriggerId != "red-discard-phase-end" || frame.InstructionIndex != 1 || !ValidMobileOrOlRedDiscardRecoveryWindow(frame) ||
            pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null ||
            frame.RedDiscardRecoveryDraft is not { Committed: true, AskingSeat: null, Cursor: > 0 } draft || draft.Cursor > draft.Selected.Count ||
            draft.Selected.Count > draft.Budget || draft.Selected.Distinct().Count() != draft.Selected.Count ||
            draft.Selected.Any(seat => !CanMobileOrOlRedDiscardRecoverySelect(frame, seat))) return false;
        var recipient = draft.Selected[draft.Cursor - 1];
        var commitments = ProgramEventHistory<ProgramRedDiscardRecoveryCommittedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        var replies = ProgramEventHistory<ProgramRedDiscardRecoveryChosenEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (commitments is not [var committed] || committed.OwnerSeat != frame.OwnerSeat || committed.Budget != draft.Budget || !committed.Seats.SequenceEqual(draft.Selected) ||
            replies.Length == 0 || replies.Length > draft.Cursor || replies[^1] is not { Option: "draw" } reply || reply.OwnerSeat != frame.OwnerSeat || reply.ChooserSeat != recipient ||
            replies.Count(e => e.ChooserSeat == recipient) != 1 || replies.Any(e => e.OwnerSeat != frame.OwnerSeat || !draft.Selected.Take(draft.Cursor).Contains(e.ChooserSeat))) return false;
        // The original committed reply, not the beneficiary's later hand, proves
        // this one-card payment while its actual gain observer is suspended.
        return _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch =>
            batch.ParentFrameId == frame.Id && batch.OriginOwnerSeat == frame.OwnerSeat && batch.OriginSkillId == frame.SkillId && batch.OriginSkillInstanceId == frame.SkillInstanceId &&
            batch.Movements is [var movement] && movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(recipient) &&
            movement.Reason.Value == "skill-program.red-discard-recovery.draw" && _cardMovements.Contains(movement));
    }
}

internal static class MobileMengHuoComposition
{
    [ProgramCatalogValidation(66109)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "boundary:zaiqi-mobile") return;
        if (program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.CardPolicies.Count != 0 || program.Modifiers.Count != 0 ||
            program.Triggers is not [{ Id: "red-discard-phase-end", Window: SkillProgramTriggerWindow.DiscardPhaseEnded,
                Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, AllowOwnDiscardPhaseEnded: true,
                Optional: false, Effects: [{ Op: SkillProgramEffectOp.OfferRedDiscardRecoveryChoice, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path + ": mobile Zaiqi requires its actual own discard-phase end and one shared red-card-budget choice.");
    }
}
