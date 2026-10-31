#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuGuoTaiBoundaryMobileRuntime
{
    bool IBoundaryWuGuoTaiQueries.CanExchange(int owner, int first, int second) => IsBoundaryWuGuoTaiGanluPair(owner, first, second);
    internal bool IsBoundaryWuGuoTaiGanluPair(int ownerSeat, int firstSeat, int secondSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(firstSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(secondSeat) || firstSeat == secondSeat || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[firstSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[secondSeat].IsAlive)
            return false;
        return firstSeat == ownerSeat || secondSeat == ownerSeat || Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(firstSeat).Count - _corePhysicalOwnedCardQueriesOperations.GetEquipment2(secondSeat).Count) <= Math.Max(0, _coreMatchParticipantsPlayersAccess._players[ownerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[ownerSeat].Hp);
    }

    internal int? BoundaryWuGuoTaiBuyiDiscardActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations)
    {
        if (frame.SkillId != BoundaryWuGuoTaiMobile.Buyi || bind != "nonbasic" || frame.SelectedTargetSeats is not [var holder] || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: var target } || target != holder || ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect is not { Op: SkillProgramEffectOp.FilterBoundCards, SourceBind: "revealed", ResultBind: "nonbasic", CardCategories: [SkillProgramCardCategory.Trick, SkillProgramCardCategory.Equipment] } || frame.CardSetBindings.SingleOrDefault(b => b.Name == "revealed")is not { } reveal || ids.Count != locations.Count || ids.Count > 1 || ids.Where((id, index) => locations[index] != CardLocation.Hand(holder) || !reveal.CardIds.Contains(id) || !reveal.SourceLocations.Contains(locations[index]) || _cardZonesOperations._cardZones.GetLocation(id) != locations[index] || CardUseCategoryCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind) == CardUseCategories.Basic).Any())
            return null;
        return holder;
    }
}
