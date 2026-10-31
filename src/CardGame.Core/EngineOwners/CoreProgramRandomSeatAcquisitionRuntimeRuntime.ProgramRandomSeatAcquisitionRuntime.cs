#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRandomSeatAcquisitionRuntimeRuntime
{
    internal ProgramRandomSeatAcquisitionRuntime SeatAcquisitions => _programRandomSeatAcquisitionOperations.Get();

    Winner IProgramSeatAcquisitionFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IProgramSeatAcquisitionFacts.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool IProgramSeatAcquisitionFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProgramSeatAcquisitionFacts.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IProgramSeatAcquisitionFacts.IsFaceDown(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsFaceDown;
    bool IProgramSeatAcquisitionFacts.CannotTurnOver(int seat) => _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IProgramSeatAcquisitionFacts.HasSkillInstance(int seat, string skill, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skill, instance);
    uint IProgramSeatAcquisitionFacts.RandomState => _randomOperations._random.State;

    long IProgramSeatAcquisitionFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    ProgramSeatAcquisitionDamagePoint? IProgramSeatAcquisitionFacts.DamagePoint(ProgramSkillFrame f) => _coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) && damage.TargetSeat == f.OwnerSeat && f.WindowContext!.OccurrenceIndex >= 0 && f.WindowContext.OccurrenceIndex < damage.Amount ? new(window.Id, damage.Id, f.WindowContext.OccurrenceIndex, damage.Amount) : null;
    IReadOnlyList<int> IProgramSeatAcquisitionFacts.OtherSeats(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != owner).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(owner, p.Seat)).Select(p => p.Seat).ToArray());
    IReadOnlyList<ProgramSeatAcquisitionCandidate> IProgramSeatAcquisitionFacts.Candidates(ProgramSkillFrame f, int target) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain).Select(c =>
    {
        var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]);
        var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var cards = zone switch
        {
            CardZoneKind.Hand => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]),
            CardZoneKind.Equipment => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]),
            CardZoneKind.Judgment => _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]),
            _ => throw new InvalidOperationException("Unsupported seat-acquisition source.")};
        return new ProgramSeatAcquisitionCandidate(cards[slot].Id, new(zone, target));
    }).Where(c => c.From.Zone != CardZoneKind.Judgment || !_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.TargetSeat == target && j.CardId == c.CardId)).OrderBy(c => c.CardId).ToArray());
    CardLocation? IProgramSeatAcquisitionFacts.DestinationAt(int id, long sequence) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id && m.Sequence <= sequence)?.To;
    CardLocation IProgramSeatAcquisitionFacts.ExpectedDestination(int owner, int id, CardLocation from)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var expected = card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, CardLocation.Hand(owner)) ? CardLocation.OutsideGame : CardLocation.Hand(owner);
        return _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, expected);
    }

    bool IProgramSeatAcquisitionFacts.IsRecorded(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m == movement) == 1;
    SkillProgramEffect? IProgramSeatAcquisitionFacts.PausedEffect(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    int IProgramSeatAcquisitionOperations.NextRandom(int count) => _randomOperations._random.Next(count);
    void IProgramSeatAcquisitionOperations.Obtain(int id, int recipient, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>> paid) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), reason, paid, disclosure: disclosure);
    bool IProgramSeatAcquisitionOperations.DrainChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    void IProgramSeatAcquisitionOperations.TurnOver(ProgramSkillFrame f) => _coreProgramLifecycleOperations.TurnOverProgramTarget(f.Id, f.OwnerSeat, f.OwnerSeat);
    void IProgramSeatAcquisitionOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
}
