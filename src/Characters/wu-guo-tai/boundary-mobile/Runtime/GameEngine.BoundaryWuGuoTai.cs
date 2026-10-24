namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class BoundaryWuGuoTaiQueries(GameEngine engine) : IBoundaryWuGuoTaiQueries
    { public bool CanExchange(int owner, int first, int second) => engine.IsBoundaryWuGuoTaiGanluPair(owner, first, second); }

    private bool IsBoundaryWuGuoTaiGanluPair(int ownerSeat, int firstSeat, int secondSeat)
    {
        if (!IsValidPlayerSeat(ownerSeat) || !IsValidPlayerSeat(firstSeat) || !IsValidPlayerSeat(secondSeat) || firstSeat == secondSeat ||
            !_players[ownerSeat].IsAlive || !_players[firstSeat].IsAlive || !_players[secondSeat].IsAlive) return false;
        return firstSeat == ownerSeat || secondSeat == ownerSeat || Math.Abs(GetEquipment(firstSeat).Count - GetEquipment(secondSeat).Count) <=
            Math.Max(0, _players[ownerSeat].MaxHp - _players[ownerSeat].Hp);
    }

    // The reveal belongs to Wu Guo Tai, but the explicitly instructed discard is
    // performed by the dying holder. Keep this identity on the existing binding.
    private int? BoundaryWuGuoTaiBuyiDiscardActor(ProgramSkillFrame frame, string bind, IReadOnlyList<int> ids, IReadOnlyList<CardLocation> locations)
    {
        if (frame.SkillId != BoundaryWuGuoTaiMobile.Buyi || bind != "nonbasic" || frame.SelectedTargetSeats is not [var holder] ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: var target } || target != holder ||
            ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect is not
                { Op: SkillProgramEffectOp.FilterBoundCards, SourceBind: "revealed", ResultBind: "nonbasic", CardCategories: [SkillProgramCardCategory.Trick, SkillProgramCardCategory.Equipment] } ||
            frame.CardSetBindings.SingleOrDefault(b => b.Name == "revealed") is not { } reveal ||
            ids.Count != locations.Count || ids.Count > 1 ||
            ids.Where((id, index) => locations[index] != CardLocation.Hand(holder) || !reveal.CardIds.Contains(id) ||
                !reveal.SourceLocations.Contains(locations[index]) || _cardZones.GetLocation(id) != locations[index] ||
                CardUseCategoryCatalog.Get(GetAdvancedCard(id).Kind) == CardUseCategories.Basic).Any()) return null;
        return holder;
    }
}
