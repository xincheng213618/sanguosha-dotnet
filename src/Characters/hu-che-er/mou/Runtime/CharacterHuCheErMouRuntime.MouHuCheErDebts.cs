#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    internal IEnumerable<MouHuCheErDebtEvent> ActiveMouHuCheErDebts(int target) => !HasMouHuCheEr ? [] : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuCheErDebtEvent>().Where(d => d.Target == target && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuCheErDebtExpiredEvent>().Any(e => e.FrameId == d.FrameId));
    internal bool MouHuCheErEquipmentDisabled(int target) => ActiveMouHuCheErDebts(target).Any();
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Where(card => !_characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id)).ToArray();
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(int seat) => KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    internal IReadOnlyList<Card> KongwuUsableWoodenOxGrain(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(player);
    internal int MouHuCheErDrawPenalty(CharacterState player) => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat == player.Seat && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == player.Seat ? ActiveMouHuCheErDebts(player.Seat).Count(d => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuCheErDebtTurnEvent>().Any(t => t.FrameId == d.FrameId && t.Target == player.Seat && t.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)) : 0;
}
