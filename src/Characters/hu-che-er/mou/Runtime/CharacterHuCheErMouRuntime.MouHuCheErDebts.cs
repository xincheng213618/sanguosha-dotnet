#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    internal bool MouHuCheErEquipmentDisabled(int target) =>
        MouHuCheErRuntime.ActiveDebtOrigins(_coreMatchParticipantsPlayersAccess._players[target]).Any();
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Where(card => !_characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id)).ToArray();
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(int seat) => KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    internal IReadOnlyList<Card> KongwuUsableWoodenOxGrain(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(player);
    internal int MouHuCheErDrawPenalty(CharacterState player)
    {
        if (_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat != player.Seat ||
            _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != player.Seat)
            return 0;
        var turn = _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber;
        return MouHuCheErRuntime.ActiveDebtOrigins(player).Count(origin =>
            _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuCheErDebtTurnEvent>()
                .Any(due => due.FrameId == origin.FrameId && due.Target == player.Seat && due.Turn == turn));
    }
}
