namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool MobileYoudiPaidBait(ProgramSkillFrame f, out ProgramSkillCardSetBinding? bait,
        out NativeLogicalDiscardInvocationRecordedEvent? invoice)
    {
        bait = null; invoice = null;
        if (f.SkillId != "mobile:youdi" || f.TriggerId != "ending-discard-and-rewards" || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } ||
            f.InstructionIndex < 2 || f.SelectedTargetSeats is not [var chooser] || chooser == f.OwnerSeat || !IsValidPlayerSeat(chooser)) return false;
        bait = f.CardSetBindings.SingleOrDefault(b => b.Name == "mobile-youdi-bait");
        if (bait is not { CardIds: [var card], FrozenRevealedSuit: { } suit } || !Enum.IsDefined(suit) || bait.SourceLocations is not [var destination] || destination != CardLocation.DiscardPile)
            return false;
        invoice = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id &&
            e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == chooser && e.SubjectSeat == f.OwnerSeat && e.ActualCount == 1 && e.HandCount == 1 &&
            e.Reason.Value == "skill-program.mobile:youdi.SelectAndMoveOwnedCard");
        if (invoice is null) return false;
        var invocation = invoice.InvocationId;
        return ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == invocation).ToArray() is [var paid] &&
            paid.CardId == card && paid.From == CardLocation.Hand(f.OwnerSeat) && paid.To == CardLocation.DiscardPile &&
            _cardMovements.Any(m => m.Sequence == paid.CompletionSequence && m.CardId == card && m.From == paid.From && m.To == paid.To &&
                m.Reason.Value == "skill-program.mobile:youdi.SelectAndMoveOwnedCard");
    }
    private void PublishMobileYoudiBait(ProgramSkillFrame supplied, string sourceBind)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 3 || sourceBind != "mobile-youdi-bait" || f.PendingMovementContinuation is not null ||
            !MobileYoudiPaidBait(f, out var binding, out var invoice) || binding is null || invoice is null ||
            ProgramEventHistory<MobileYoudiBaitPublishedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Mobile Youdi requires its exact completed native hand discard and frozen public bait suit.");
        // This entity has already entered the public discard pile. Expose its
        // existing frozen result without creating a second rule-level reveal.
        ReplaceRuntimeTop(f with { CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Select(b => b.Name == sourceBind ? b with { Visibility = SkillProgramCardSetVisibility.Public } : b).ToArray()) });
        AdvanceEventRulesAndQueueFact(new MobileYoudiBaitPublishedEvent(f.Id, MobileDuanfaSource(f), f.GameplayHash, invoice.InvocationId, binding.CardIds.Single(), binding.FrozenRevealedSuit!.Value));
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 82301)]
    private bool KeepPaidMobileYoudi(ProgramSkillFrame f) => MobileYoudiPaidBait(f, out _, out _);
}
