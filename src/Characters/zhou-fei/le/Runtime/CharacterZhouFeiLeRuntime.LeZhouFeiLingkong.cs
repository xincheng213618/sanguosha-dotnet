#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiLeRuntime
{
    internal bool HasLingkongEntityMark(int cardId) => _cardEntityMarks.Has(cardId, "lingkong");
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat) && HasLingkongEntityMark(cardId);
    internal bool IsLingkongHandLimitExempt(CharacterState owner, int cardId) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "le:lingkong") && HasLingkongHandEntityMark(owner.Seat, cardId);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28310, SkillProgramEffectOp.MarkLingkongFirstOutsideDrawGain)]
    internal bool CanOfferLingkong(ProgramTriggerLegalityContext gate)
    {
        var gain = gate.Context.MovementBatch?.HandGains?.SingleOrDefault(receipt => receipt.RecipientSeat == gate.OwnerSeat);
        return gain is { FirstOutsideDraw: true, InitiallyAlive: true, Timing: { } timing } &&
            (timing.Phase != TurnPhase.Draw || timing.PhaseActorSeat != gain.RecipientSeat) &&
            gain.Incoming.Any(card => card.IsActualGain);
    }

    SkillProgramStepOutcome ILeZhouFeiLingkongHost.MarkLingkongGain(ProgramSkillFrame frame) => MarkLingkongGain(frame);
    internal SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var batch = frame.WindowContext!.MovementBatch!;
        var gain = batch.HandGains!.Single(receipt => receipt.RecipientSeat == frame.OwnerSeat);
        var cards = gain.Incoming.Where(card => card.IsActualGain).Select(card => card.CardId).ToArray();
        var source = new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LingkongGainMarkedEvent(frame.Id, batch.Id,
            source, frame.GameplayHash, gain.ActualTurn, gain.Timing!.ActualTurnOwnerSeat, cards.Length));
        _cardEntityMarkOperations.Mark(frame.Id, "lingkong", cards);
        return SkillProgramStepOutcome.Continue;
    }
}
