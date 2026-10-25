namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileZhuRongSkill = "boundary:lieren-mobile", MobileZhuRongSource = "mobile-lieren-source", MobileZhuRongOpponent = "mobile-lieren-opponent",
        MobileZhuRongExchange = "skill-program.boundary:lieren-mobile.exchange";

    private static bool IsMobileZhuRongTargetTrigger(string skillId, SkillProgramTrigger trigger) => skillId == MobileZhuRongSkill && trigger.Id == "contest-on-slash-target";

    // Called only on the typed return from the original native Pindian child.
    private void CaptureMobileZhuRongPindianReturn(ProgramSkillFrame parent, PindianFrame native)
    {
        if (parent.SkillId != MobileZhuRongSkill) return;
        if (parent.InstructionIndex != 1 || parent.TriggerId != "contest-on-slash-target" || native.ParentFrameId != parent.Id || native.ProgramResultBind != "contest" ||
            native.Result is not { } result || result.SourceSeat != parent.OwnerSeat ||
            parent.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: var target } || target != result.OpponentSeat ||
            parent.PindianResultBindings is not [{ Name: "contest", SourceSeat: var source, OpponentSeat: var opponent }] || source != result.SourceSeat || opponent != result.OpponentSeat ||
            ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == native.Id && e.SkillId == parent.SkillId && e.Result == result) != 1)
            throw new InvalidOperationException("Mobile Lieren lost its exact public native Pindian return.");
        SetProgramCardSet(parent.Id, MobileZhuRongSource, [result.SourceCardId], SkillProgramCardSetVisibility.Public, [CardLocation.DiscardPile]);
        SetProgramCardSet(parent.Id, MobileZhuRongOpponent, [result.OpponentCardId], SkillProgramCardSetVisibility.Public, [CardLocation.DiscardPile]);
    }

    [ProgramCapability]
    private sealed class MobileZhuRongProgramCapability(GameEngine engine) : IMobileZhuRongProgramHost
    { public SkillProgramStepOutcome Exchange(ProgramSkillFrame frame) => engine.ExchangeMobileZhuRongCards(frame); }

    private SkillProgramStepOutcome ExchangeMobileZhuRongCards(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileZhuRongSkill || frame.InstructionIndex != 3 || frame.PendingMovementContinuation is not null ||
            frame.PindianResultBindings is not [{ Name: "contest", SourceSeat: var owner, OpponentSeat: var opponent, SourceWon: false }] || owner != frame.OwnerSeat)
            throw new InvalidOperationException("Mobile Lieren exchange lost its non-winning original contest.");
        var sourceCard = GetProgramCardSet(frame, MobileZhuRongSource).CardIds.Single();
        var opponentCard = GetProgramCardSet(frame, MobileZhuRongOpponent).CardIds.Single();
        ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        foreach (var (id, recipient) in new[] { (sourceCard, opponent), (opponentCard, owner) })
        {
            if (!_players[recipient].IsAlive || _cardZones.GetLocation(id) != CardLocation.DiscardPile) continue;
            var card = _cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == id);
            MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(recipient), new(MobileZhuRongExchange), disclosure: CardMovementDisclosure.FaceUp);
        }
        if (TryBeginCardsMovedProgramWindow(frame.Id)) return SkillProgramStepOutcome.AwaitChild;
        ReplaceRuntimeTop(GetActiveProgramFrame(frame.Id) with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 66201)]
    private bool MobileZhuRongExchangeMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.ExchangeMobileLierenPindianCards || frame.SkillId != MobileZhuRongSkill || frame.InstructionIndex != 3 ||
            pending != frame.PendingMovementContinuation || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null ||
            frame.PindianResultBindings is not [{ Name: "contest", SourceSeat: var owner, OpponentSeat: var opponent, SourceWon: false }] || owner != frame.OwnerSeat ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: var target } || target != opponent) return false;
        var source = frame.CardSetBindings.SingleOrDefault(b => b.Name == MobileZhuRongSource);
        var other = frame.CardSetBindings.SingleOrDefault(b => b.Name == MobileZhuRongOpponent);
        if (source is not { Visibility: SkillProgramCardSetVisibility.Public, CardIds: [var sourceId], SourceLocations: [var sourceZone] } || sourceZone != CardLocation.DiscardPile ||
            other is not { Visibility: SkillProgramCardSetVisibility.Public, CardIds: [var opponentId], SourceLocations: [var opponentZone] } || opponentZone != CardLocation.DiscardPile) return false;
        var disclosure = _modeRules.PublishesCardMovementDisclosure ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.Unspecified;
        return _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch =>
            batch.ParentFrameId == frame.Id && batch.OriginOwnerSeat == owner && batch.OriginSkillId == frame.SkillId && batch.OriginSkillInstanceId == frame.SkillInstanceId &&
            batch.Movements is [var movement] && movement.From == CardLocation.DiscardPile && movement.Reason.Value == MobileZhuRongExchange && movement.Disclosure == disclosure &&
            (movement.CardId == sourceId && movement.To == CardLocation.Hand(opponent) || movement.CardId == opponentId && movement.To == CardLocation.Hand(owner)) && _cardMovements.Contains(movement));
    }
}
