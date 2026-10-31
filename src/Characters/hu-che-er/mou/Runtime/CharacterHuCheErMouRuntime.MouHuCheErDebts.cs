#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErMouRuntime
{
    internal IEnumerable<MouHuCheErDebtEvent> ActiveMouHuCheErDebts(int target) => !HasMouHuCheEr ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuCheErDebtEvent>().Where(d => d.Target == target && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHuCheErDebtExpiredEvent>().Any(e => e.FrameId == d.FrameId));
    internal bool MouHuCheErEquipmentDisabled(int target) => ActiveMouHuCheErDebts(target).Any();
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Where(card => !_characterWeiYanHappyGodOperations.IsHappyWeiYanBone(card.Id)).ToArray();
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(int seat) => KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[seat]);
    internal IReadOnlyList<Card> KongwuUsableWoodenOxGrain(CharacterState player) => MouHuCheErEquipmentDisabled(player.Seat) ? [] : _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(player);
    internal bool MouHuCheErEquipmentGrantQualified(CharacterState owner, SkillGrant grant) => !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal) || !MouHuCheErEquipmentDisabled(owner.Seat);
    internal long CaptureMouHuCheErEquipmentQualificationStamp(long original)
    {
        if (!HasMouHuCheEr)
            return original;
        var events = _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuCheErDebtEvent>().Count() + _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuCheErDebtExpiredEvent>().Count();
        if (events == 0)
            return original;
        if (_characterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationDependencies.TryGetValue(original, out var cached) && cached.Events == events)
            return cached.Stamp;
        var stamp = long.MinValue + ++_characterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationStamp;
        _characterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationDependencies[original] = (events, stamp);
        return stamp;
    }

    internal int MouHuCheErDrawPenalty(CharacterState player) => _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat == player.Seat && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == player.Seat ? ActiveMouHuCheErDebts(player.Seat).Count(d => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuCheErDebtTurnEvent>().Any(t => t.FrameId == d.FrameId && t.Target == player.Seat && t.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)) : 0;
}
