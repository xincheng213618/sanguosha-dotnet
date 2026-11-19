#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRongBoundaryMobileRuntime
{
    internal const string MobileZhuRongSkill = "boundary:lieren-mobile", MobileZhuRongSource = "mobile-lieren-source", MobileZhuRongOpponent = "mobile-lieren-opponent", MobileZhuRongExchange = "skill-program.boundary:lieren-mobile.exchange";
    internal static bool IsMobileZhuRongTargetTrigger(string skillId, SkillProgramTrigger trigger) => skillId == MobileZhuRongSkill && trigger.Id == "contest-on-slash-target";
    internal void CaptureMobileZhuRongPindianReturn(ProgramSkillFrame parent, PindianFrame native)
    {
        if (parent.SkillId != MobileZhuRongSkill)
            return;
        if (parent.InstructionIndex != 1 || parent.TriggerId != "contest-on-slash-target" ||
            native.ParentFrameId != parent.Id || native.SkillId != parent.SkillId ||
            native.ProgramResultBind != "contest" || native.Result is not { } result || result.SourceSeat != parent.OwnerSeat ||
            parent.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: var target } ||
            target != result.OpponentSeat ||
            parent.PindianResultBindings is not [{ Name: "contest", Visibility: SkillProgramCardSetVisibility.Public } binding] ||
            binding.NativePindianFrameId != native.Id || binding.SourceSeat != result.SourceSeat ||
            binding.OpponentSeat != result.OpponentSeat || binding.SourceRank != result.SourceRank ||
            binding.OpponentRank != result.OpponentRank || binding.SourceWon != result.SourceWon)
            throw new InvalidOperationException("Mobile Lieren lost its exact public native Pindian return.");
        cardSets.SetProgramCardSet(parent.Id, MobileZhuRongSource, [result.SourceCardId], SkillProgramCardSetVisibility.Public, [CardLocation.DiscardPile]);
        cardSets.SetProgramCardSet(parent.Id, MobileZhuRongOpponent, [result.OpponentCardId], SkillProgramCardSetVisibility.Public, [CardLocation.DiscardPile]);
    }

    SkillProgramStepOutcome IMobileZhuRongProgramHost.Exchange(ProgramSkillFrame supplied)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.SkillId != MobileZhuRongSkill || frame.InstructionIndex != 3 || frame.PendingMovementContinuation is not null || frame.PindianResultBindings is not [{ Name: "contest", SourceSeat: var owner, OpponentSeat: var opponent, SourceWon: false }] || owner != frame.OwnerSeat)
            throw new InvalidOperationException("Mobile Lieren exchange lost its non-winning original contest.");
        var sourceCard = ProgramBoundCardRuntime.GetProgramCardSet(frame, MobileZhuRongSource).CardIds.Single();
        var opponentCard = ProgramBoundCardRuntime.GetProgramCardSet(frame, MobileZhuRongOpponent).CardIds.Single();
        frames.ReplaceTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        GiveDiscardedCard(sourceCard, opponent);
        GiveDiscardedCard(opponentCard, owner);

        if (movementWindows.Begin(frame.Id))
            return SkillProgramStepOutcome.AwaitChild;
        frames.ReplaceTop(frames.ActiveProgram(frame.Id) with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;

        void GiveDiscardedCard(int cardId, int recipient)
        {
            if (!participants.At(recipient).IsAlive ||
                zones.GetLocation(cardId) != CardLocation.DiscardPile)
                return;

            // The zone store indexes each registered physical identity once.
            var card = zones.CardsAt(CardLocation.DiscardPile).First(card => card.Id == cardId);
            movement.Move(card, CardLocation.DiscardPile, CardLocation.Hand(recipient),
                new(MobileZhuRongExchange), disclosure: CardMovementDisclosure.FaceUp);
        }
    }
}
