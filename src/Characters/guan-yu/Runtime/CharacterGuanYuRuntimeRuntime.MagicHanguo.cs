#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    internal DemonRoundFocusEvent? DemonFocus(int target) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DemonRoundFocusEvent>().LastOrDefault(e => e.TargetSeat == target && e.RoundNumber == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber);
    internal FactionResponsePolicySource? GetDemonDodgeRequest(CharacterState owner, CardKind kind) => kind == CardKind.Dodge && DemonFocus(owner.Seat)is { } focus && _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(owner.Seat, "magic-any").Count > 0 ? new(focus.Source.SkillId, focus.Source.SkillInstanceId, "round-aid", "magic-any", kind, 0, 0) : null;
    internal void ReturnDemonRoundHolds()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            var held = _cardZonesOperations._cardZones.CardsAt(CardLocation.RoundHold(player.Seat)).ToArray();
            if (held.Length > 0)
                _corePhysicalMovementFacadesOperations.MoveCards(held, CardLocation.RoundHold(player.Seat), player.IsAlive ? CardLocation.Hand(player.Seat) : CardLocation.DiscardPile, new("skill-program.hanguo.return"));
        }
    }
}
