#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoAngFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CharacterCaoAngMouEntries> getCharacterCaoAngMouRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, EngineStates.CoreAiBrainsState aiBrainsState, KernelCreateSnapshotCall createSnapshot) : IMouCaoAngFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwner => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool Parent(ProgramSkillFrame f) => getCharacterCaoAngMouRuntime().MouCaParent(f);
    public int LostEquipmentOwner(ProgramSkillFrame f) => f.WindowContext?.MovementBatch is { } batch && f.WindowContext.MovementIndex is { } index && index >= 0 && index < batch.Movements.Count && batch.Movements[index] is { From.Zone: CardZoneKind.Equipment } m && m.To != m.From && cardMovementJournalState.ReferenceToCardMovements.Contains(m) ? m.From.OwnerSeat ?? -1 : -1;
    public int EmptySlots(int seat) => Enum.GetValues<EquipmentSlot>().Sum(slot => Math.Max(0, matchParticipantsState.ReferenceToPlayers[seat].EquipmentSlotCapacity(slot) - getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Count(c => getEquipmentContentRegistry().Get(c.Kind).Slot == slot)));
    public IReadOnlyList<MouCaEntity> Equipment(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Where(c => !getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(seat, c, CardLocation.Equipment(seat), OwnedCardMoveIntent.Discard)).Select(c => new MouCaEntity(c.Id, c.Kind, CardLocation.Equipment(seat))).ToArray());
    public IReadOnlyList<MouCaPair> Pairs(ProgramSkillFrame f) => getCharacterCaoAngMouRuntime().MouCaPairs(f);
    public IReadOnlyList<LegalAction> Plans(ProgramSkillFrame f) => f.MouCaoAng is { } r ? getCharacterCaoAngMouRuntime().MouCaPlans(f, r.Actor, r.CardId) : [];
    public CardKind Kind(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id).Kind;
    public double TargetValue(int actor, int target) => aiBrainsState.ReferenceToAiBrains[actor].ScoreProgramTarget(createSnapshot(actor), target, new(0, 0, 1, 0, 0, 0, false, false));
}
