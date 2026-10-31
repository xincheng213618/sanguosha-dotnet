#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;
using AcquisitionNode = CardGame.Core.ProgramStageNode<CardGame.Core.ProgramSeatAcquisitionCall, CardGame.Core.ProgramSeatAcquisitionReceipt, CardGame.Core.ProgramSeatAcquisitionStage>;

namespace CardGame.Core.NativeOwners;
internal sealed class ProgramSeatAcquisitionFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreShenwuNpcProgramsEntries> getCoreShenwuNpcProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries> getCoreGameEngineOrderedPrintedSkillLossObserversRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries> getCoreGameEngineEphemeralCardMaterialsRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry) : IProgramSeatAcquisitionFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int PlayerCount => matchParticipantsState.ReferenceToPlayers.Count;

    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool IsFaceDown(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsFaceDown;
    public bool CannotTurnOver(int seat) => getCoreShenwuNpcProgramsRuntime().ShenwuCannotTurnOver(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool HasSkillInstance(int seat, string skill, string instance) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skill, instance);
    public uint RandomState => getDeterministicRandom().State;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;

    public ProgramSeatAcquisitionDamagePoint? DamagePoint(ProgramSkillFrame f) => getCoreGameEngineOrderedPrintedSkillLossObserversRuntime().ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) && damage.TargetSeat == f.OwnerSeat && f.WindowContext!.OccurrenceIndex >= 0 && f.WindowContext.OccurrenceIndex < damage.Amount ? new(window.Id, damage.Id, f.WindowContext.OccurrenceIndex, damage.Amount) : null;
    public IReadOnlyList<int> OtherSeats(int owner) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.Seat != owner).OrderBy(p => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(owner, p.Seat)).Select(p => p.Seat).ToArray());
    public IReadOnlyList<ProgramSeatAcquisitionCandidate> Candidates(ProgramSkillFrame f, int target) => Array.AsReadOnly(getCoreGameEngineProgramCardPaymentsRuntime().BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain).Select(c =>
    {
        var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]);
        var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var cards = zone switch
        {
            CardZoneKind.Hand => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]),
            CardZoneKind.Equipment => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[target]),
            CardZoneKind.Judgment => getCorePhysicalOwnedCardQueriesRuntime().GetJudgment(matchParticipantsState.ReferenceToPlayers[target]),
            _ => throw new InvalidOperationException("Unsupported seat-acquisition source.")};
        return new ProgramSeatAcquisitionCandidate(cards[slot].Id, new(zone, target));
    }).Where(c => c.From.Zone != CardZoneKind.Judgment || !getFrameStore().OfType<JudgmentFrame>().Any(j => j.TargetSeat == target && j.CardId == c.CardId)).OrderBy(c => c.CardId).ToArray());
    public CardLocation? DestinationAt(int id, long sequence) => cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(m => m.CardId == id && m.Sequence <= sequence)?.To;
    public CardLocation ExpectedDestination(int owner, int id, CardLocation from)
    {
        var card = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
        var expected = card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, CardLocation.Hand(owner)) ? CardLocation.OutsideGame : CardLocation.Hand(owner);
        return getCoreGameEngineEphemeralCardMaterialsRuntime().NormalizeEphemeralCardDestination(card, from, expected);
    }

    public bool IsRecorded(CardMovementRecord movement) => cardMovementJournalState.ReferenceToCardMovements.Count(m => m == movement) == 1;
    public SkillProgramEffect? PausedEffect(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, getContentRegistry().GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
}
