#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal int MouZhitianCount(int seat) => _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouZhitianCounts.GetValueOrDefault(seat, 7);
    internal IReadOnlyList<Card> MouZhitianTop(int seat) => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(MouZhitianCount(seat)).Reverse().ToArray();
    internal IEnumerable<CardSnapshot> MouZhitianPrivateCards(int viewerSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(viewerSeat) && _coreMatchParticipantsPlayersAccess._players[viewerSeat].IsAlive && HasMouOperation(_coreMatchParticipantsPlayersAccess._players[viewerSeat], SkillProgramEffectOp.MouZhitianView) && (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == viewerSeat || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is { SourceSeat: var source, FireAttackSelection.RevealedCardId: not null } && source == viewerSeat) ? MouZhitianTop(viewerSeat).Select(_coreSnapshotValueProjectionOperations.ToSnapshot) : [];
}
