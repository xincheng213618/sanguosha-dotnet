#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    internal (Card Card, CardLocation From)[] WuHfMinimumPool()
    {
        var locations = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => new[] { CardLocation.Equipment(p.Seat), CardLocation.Judgment(p.Seat) }).Append(CardLocation.DrawPile);
        var all = locations.SelectMany(loc => _cardZonesOperations._cardZones.CardsAt(loc).Where(c => c.Rank > 0).Select(c => (Card: c, From: loc))).ToArray();
        if (all.Length == 0)
            return[];
        var rank = all.Min(x => x.Card.Rank);
        return all.Where(x => x.Card.Rank == rank && x.From != CardLocation.DrawPile).Concat(all.Where(x => x.Card.Rank == rank && x.From == CardLocation.DrawPile).TakeLast(1)).ToArray();
    }

    internal bool WuHfChaozhenAvailable(int owner) => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongChaozhenDisabledEvent>().Any(e => e.OwnerSeat == owner && e.Turn == WuHfTurn()) && WuHfMinimumPool().Length > 0;
}
