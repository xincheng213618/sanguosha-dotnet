#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangQiYingOlRuntime
{
    internal int FaluTokenCount(int owner) => CharacterZhangQiYingOlDefinitions.FaluTokens.Count(m => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(m) > 0);
    internal CardLocation? FaluDiscardOwnedSource(CardMovementRecord movement)
    {
        if (movement.To != CardLocation.DiscardPile)
            return null;
        var origin = _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
        if (origin is { OwnerSeat: not null, Zone: CardZoneKind.Hand or CardZoneKind.Equipment })
            return origin;
        return movement.From is { OwnerSeat: not null, Zone: CardZoneKind.WoodenOxGrain } && CoreGameEngineProgramDiscardTopPlacementRuntime.IsDiscardMovementReason(movement.Reason) ? movement.From : null;
    }

    internal void SetFaluToken(ProgramSkillFrame f, PlayerMarkerKind marker, int value, string reason)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var before = owner.Markers.GetValueOrDefault(marker);
        if (value is < 0 or > 1 || before is < 0 or > 1)
            throw new InvalidOperationException("Falu tokens are binary.");
        if (before == value)
            return;
        if (value == 0)
            owner.Markers.Remove(marker);
        else
            owner.Markers[marker] = value;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, f.OwnerSeat, marker, value - before, value, f.OwnerSeat, "falu." + reason));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FaluTokenChangedEvent(f.Id, f.OwnerSeat, marker, before, value, reason));
    }
}
