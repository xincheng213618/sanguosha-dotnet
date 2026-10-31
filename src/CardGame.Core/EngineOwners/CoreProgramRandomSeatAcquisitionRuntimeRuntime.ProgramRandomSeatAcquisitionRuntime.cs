#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;
using AcquisitionNode = CardGame.Core.ProgramStageNode<CardGame.Core.ProgramSeatAcquisitionCall, CardGame.Core.ProgramSeatAcquisitionReceipt, CardGame.Core.ProgramSeatAcquisitionStage>;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRandomSeatAcquisitionRuntimeRuntime
{
    internal ProgramRandomSeatAcquisitionRuntime SeatAcquisitions => _programRandomSeatAcquisitionOperations.Get();

    Winner global::CardGame.Core.IProgramSeatAcquisitionFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IProgramSeatAcquisitionFacts.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool global::CardGame.Core.IProgramSeatAcquisitionFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IProgramSeatAcquisitionFacts.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IProgramSeatAcquisitionFacts.IsFaceDown(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsFaceDown;
    bool global::CardGame.Core.IProgramSeatAcquisitionFacts.CannotTurnOver(int seat) => _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IProgramSeatAcquisitionFacts.HasSkillInstance(int seat, string skill, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skill, instance);
    uint global::CardGame.Core.IProgramSeatAcquisitionFacts.RandomState => _randomOperations._random.State;

    long global::CardGame.Core.IProgramSeatAcquisitionFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    ProgramSeatAcquisitionDamagePoint? global::CardGame.Core.IProgramSeatAcquisitionFacts.DamagePoint(ProgramSkillFrame f) => _coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) && damage.TargetSeat == f.OwnerSeat && f.WindowContext!.OccurrenceIndex >= 0 && f.WindowContext.OccurrenceIndex < damage.Amount ? new(window.Id, damage.Id, f.WindowContext.OccurrenceIndex, damage.Amount) : null;
    IReadOnlyList<int> global::CardGame.Core.IProgramSeatAcquisitionFacts.OtherSeats(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != owner).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(owner, p.Seat)).Select(p => p.Seat).ToArray());
    IReadOnlyList<ProgramSeatAcquisitionCandidate> global::CardGame.Core.IProgramSeatAcquisitionFacts.Candidates(ProgramSkillFrame f, int target) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain).Select(c =>
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
    CardLocation? global::CardGame.Core.IProgramSeatAcquisitionFacts.DestinationAt(int id, long sequence) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id && m.Sequence <= sequence)?.To;
    CardLocation global::CardGame.Core.IProgramSeatAcquisitionFacts.ExpectedDestination(int owner, int id, CardLocation from)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var expected = card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, CardLocation.Hand(owner)) ? CardLocation.OutsideGame : CardLocation.Hand(owner);
        return _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, expected);
    }

    bool global::CardGame.Core.IProgramSeatAcquisitionFacts.IsRecorded(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m == movement) == 1;
    SkillProgramEffect? global::CardGame.Core.IProgramSeatAcquisitionFacts.PausedEffect(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    int global::CardGame.Core.IProgramSeatAcquisitionOperations.NextRandom(int count) => _randomOperations._random.Next(count);
    void global::CardGame.Core.IProgramSeatAcquisitionOperations.Obtain(int id, int recipient, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>> paid) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), reason, paid, disclosure: disclosure);
    bool global::CardGame.Core.IProgramSeatAcquisitionOperations.DrainChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    void global::CardGame.Core.IProgramSeatAcquisitionOperations.TurnOver(ProgramSkillFrame f) => _coreProgramLifecycleOperations.TurnOverProgramTarget(f.Id, f.OwnerSeat, f.OwnerSeat);
    void global::CardGame.Core.IProgramSeatAcquisitionOperations.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
}
