#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRongBoundaryMobileRuntime
{
    internal const string MobileZhuRongSkill = "boundary:lieren-mobile", MobileZhuRongExchange = "skill-program.boundary:lieren-mobile.exchange";
    private const string ContestTrigger = "contest-on-slash-target";
    internal static bool IsMobileZhuRongTargetTrigger(string skillId, SkillProgramTrigger trigger) => skillId == MobileZhuRongSkill && trigger.Id == ContestTrigger;

    SkillProgramStepOutcome IMobileZhuRongProgramHost.Exchange(ProgramSkillFrame supplied)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.SkillId != MobileZhuRongSkill || frame.TriggerId != ContestTrigger ||
            frame.InstructionIndex != 3 || frame.PendingMovementContinuation is not null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: { } target } ||
            frame.PindianResultBindings is not [{ Name: "contest", Visibility: SkillProgramCardSetVisibility.Public,
                NativePindianFrameId: > 0, SourceWon: false } result] ||
            result.SourceSeat != frame.OwnerSeat || result.OpponentSeat != target)
            throw new InvalidOperationException("Mobile Lieren exchange lost its non-winning original contest.");

        frames.ReplaceTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        GiveDiscardedCard(result.SourceDiscard, result.OpponentSeat);
        GiveDiscardedCard(result.OpponentDiscard, result.SourceSeat);

        if (movementWindows.Begin(frame.Id))
            return SkillProgramStepOutcome.AwaitChild;
        frames.ReplaceTop(frames.ActiveProgram(frame.Id) with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;

        void GiveDiscardedCard(PindianDiscardEntry? discard, int recipient)
        {
            if (discard is null || !participants.At(recipient).IsAlive ||
                zoneEntries.CurrentEntry(CardLocation.DiscardPile, discard.CardId) != discard.EntrySequence)
                return;

            var card = zones.CardsAt(CardLocation.DiscardPile).First(card => card.Id == discard.CardId);
            movement.Move(card, CardLocation.DiscardPile, CardLocation.Hand(recipient),
                new(MobileZhuRongExchange), disclosure: CardMovementDisclosure.FaceUp);
        }
    }
}
